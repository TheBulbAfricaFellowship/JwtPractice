﻿using Microsoft.AspNetCore.Identity;

namespace JwtPractice.Domain.Entities
{
    public class ApplicationUser : IdentityUser<long>
    {
        public Candidate Candidate { get; set; }
    }
}
