using JwtPractice.Domain.DTOs;
using JwtPractice.Domain.Entities;
using JwtPractice.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JwtPractice.Infrastructure.Repositories
{
    public class AdminRepository : IAdminRepository
    {

        private readonly ApplicationDbContext _context;

        public AdminRepository(ApplicationDbContext context)
        {
            _context = context;
        }


        public async Task<List<Administrator>> GetAllAsync()
        {
            return await _context.Administrators.ToListAsync();
        }

        
        public async Task CreateAsync(Register registrationInfo, ApplicationUser userInfo)
        {
            _context.Administrators.Add(
                new Administrator
                {
                    FirstName = registrationInfo.FirstName,
                    LastName = registrationInfo.LastName,
                    Email = registrationInfo.EmailAddress,
                    DateProvisioned = DateTime.Now,
                    User = userInfo
                }
            );
            await _context.SaveChangesAsync();
        } 
    }
}
