using JwtPractice.Domain.DTOs;
using JwtPractice.Domain.Entities;
using JwtPractice.Domain.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace JwtPractice.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticateController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly IConfiguration _configuration;
        private readonly ICandidateRepository _candidateRepository;
        private readonly IAdminRepository _adminRepository;

        public AuthenticateController(UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager, 
            IConfiguration configuration, ICandidateRepository candidateRepository, IAdminRepository adminRepository)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
            _candidateRepository = candidateRepository;
            _adminRepository = adminRepository;
        }


        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] Login model)
        {
            Response responseBody = new Response();
            
            ApplicationUser user = await _userManager.FindByEmailAsync(model.EmailAddress);
            if (user != null && await _userManager.CheckPasswordAsync(user, model.Password))
            {
                IList<string> assignedRoles = await _userManager.GetRolesAsync(user);

                List<Claim> authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                };

                foreach (string role in assignedRoles)
                {
                    authClaims.Add(new Claim(ClaimTypes.Role, role));
                }

                SymmetricSecurityKey authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));

                JwtSecurityToken token = new JwtSecurityToken(
                    issuer: _configuration["JWT:ValidIssuer"],
                    audience: _configuration["JWT:ValidAudience"],
                    expires: DateTime.Now.AddHours(3),
                    claims: authClaims,
                    signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                    );

                // Assign response body properties for successful login
                responseBody.Message = "Logged in successfully";
                responseBody.Status = "Success";
                responseBody.Payload = new {
                    token = new JwtSecurityTokenHandler().WriteToken(token),
                    expiration = token.ValidTo
                };
                return Ok(responseBody);
            }

            // Assign response body properties for unsuccessful login
            responseBody.Message = "Login attempt was unsuccessful. Invalid email address or password.";
            responseBody.Status = "Failed";
            responseBody.Payload = null;
            return Unauthorized(responseBody);
        }


        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody] Register model)
        {
            Response responseBody = new Response();

            ApplicationUser userWithConflictingEmail = await _userManager.FindByEmailAsync(model.EmailAddress);
            if (userWithConflictingEmail != null)
            {
                responseBody.Message = "A user with this email address already exists";
                responseBody.Status = "Failed";
                responseBody.Payload = null;
                return Conflict(responseBody);
            }

            ApplicationUser user = new ApplicationUser()
            {
                Email = model.EmailAddress,
                UserName = $"{model.FirstName}.{model.LastName}",
                SecurityStamp = Guid.NewGuid().ToString()
            };

            IdentityResult result = await _userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
            {
                responseBody.Message = "Registration was not successful. Please try again.";
                responseBody.Status = "Failed";
                responseBody.Payload = null;
                return BadRequest(responseBody);
            }

            await _candidateRepository.CreateAsync(model, user);

            responseBody.Message = "Registration completed successfully.";
            responseBody.Status = "Success";
            responseBody.Payload = null;
            return Created($"/users/{user.Id}", responseBody);
        }


        [HttpPost]
        [Route("register-admin")]
        public async Task<IActionResult> RegisterAdmin([FromBody] Register model)
        {
            Response responseBody = new Response();

            ApplicationUser userWithConflictingEmail = await _userManager.FindByEmailAsync(model.EmailAddress);
            if (userWithConflictingEmail != null)
            {
                responseBody.Message = "A user with this email address already exists";
                responseBody.Status = "Failed";
                responseBody.Payload = null;
                return Conflict(responseBody);
            }

            ApplicationUser user = new ApplicationUser()
            {
                Email = model.EmailAddress,
                UserName = $"{model.FirstName}.{model.LastName}",
                SecurityStamp = Guid.NewGuid().ToString()
            };

            IdentityResult result = await _userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
            {
                responseBody.Message = "Admin registration was not successful. Please try again.";
                responseBody.Status = "Failed";
                responseBody.Payload = null;
                return BadRequest(responseBody);
            }

            await _adminRepository.CreateAsync(model, user);

            if (!await _roleManager.RoleExistsAsync("Admin"))
                await _roleManager.CreateAsync(new ApplicationRole() { Name = "Admin" });

            if (await _roleManager.RoleExistsAsync("Admin"))
                await _userManager.AddToRoleAsync(user, "Admin");

            responseBody.Message = "Admin registration completed successfully.";
            responseBody.Status = "Success";
            responseBody.Payload = null;
            return Created($"/users/{user.Id}", responseBody);
        }
    }
}
