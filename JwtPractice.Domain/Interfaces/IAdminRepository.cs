using JwtPractice.Domain.DTOs;
using JwtPractice.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JwtPractice.Domain.Interfaces
{
    public interface IAdminRepository
    {
        public Task<List<Administrator>> GetAllAsync();

        public Task CreateAsync(Register registrationInfo, ApplicationUser userInfo);
    }
}
