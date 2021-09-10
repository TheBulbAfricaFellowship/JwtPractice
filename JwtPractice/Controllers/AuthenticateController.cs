using JwtPractice.Domain.Entities;
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
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;

        public AuthenticateController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
        }


        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] Login model)
        {
            Response responseBody = new Response();
            
            ApplicationUser user = await _userManager.FindByEmailAsync(model.EmailAddress);
            if (user != null && await _userManager.CheckPasswordAsync(user, model.Password))
            {
                IList<string> userRoles = await _userManager.GetRolesAsync(user);

                List<Claim> authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                };

                foreach (string userRole in userRoles)
                {
                    authClaims.Add(new Claim(ClaimTypes.Role, userRole));
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
            responseBody.Message = "Login attempt was unsuccessful.";
            responseBody.Status = "Failed";
            responseBody.Payload = null;

            return Unauthorized(responseBody);
        }


        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody] Register model)
        {
            Response responseBody = new Response();

            var userExists = await _userManager.FindByEmailAsync(model.EmailAddress);
            if (userExists != null)
            {
                responseBody.Message = "A user with this email address already exists";
                responseBody.Status = "Failed";
                responseBody.Payload = null;
                return Conflict(responseBody);
            }

            ApplicationUser user = new ApplicationUser()
            {
                Email = model.EmailAddress,
                Candidate = new Candidate {
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Email = model.EmailAddress,
                    DateRegistered = DateTime.Now
                }
            };

            var result = await _userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
            {
                responseBody.Message = "Registration was not successful. Please try again.";
                responseBody.Status = "Failed";
                responseBody.Payload = null;
                return BadRequest(responseBody);
            }

            responseBody.Message = "Registration completed successfully.";
            responseBody.Status = "Success";
            responseBody.Payload = null;
            return Created("", responseBody);
        }

    }
}
