using JwtPractice.Domain.DTOs;
using JwtPractice.Domain.Entities;
using JwtPractice.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JwtPractice.Infrastructure.Repositories
{
    public class CandidateRepository : ICandidateRepository
    {

        private readonly ApplicationDbContext _context;

        public CandidateRepository(ApplicationDbContext context)
        {
            _context = context;
        }


        public async Task<List<Candidate>> GetAllAsync()
        {
            return await _context.Candidates.ToListAsync();
        }


        public async Task CreateAsync(Register registrationInfo, ApplicationUser userInfo)
        {
            _context.Candidates.Add(
                new Candidate
                {
                    FirstName = registrationInfo.FirstName,
                    LastName = registrationInfo.LastName,
                    Email = registrationInfo.EmailAddress,
                    DateRegistered = DateTime.Now,
                    User = userInfo
                }
            );
            await _context.SaveChangesAsync();
        }

    }
}
