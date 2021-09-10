using System;
using System.ComponentModel.DataAnnotations;

namespace JwtPractice.Domain.Entities
{
    public class Candidate
    {
        [Required]
        public long Id { get; set; }

        [Required(ErrorMessage = "First Name is required.")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last Name is required.")]
        public string LastName { get; set; }

        [EmailAddress(ErrorMessage = "This is not a valid email.")]
        [Required(ErrorMessage = "Email Address is required.")]
        public string Email { get; set; }

        [Required]
        public DateTime DateRegistered { get; set; }

        [Required]
        public long UserId { get; set; }

        public ApplicationUser User { get; set; }
    }
}




/*
 ASP.NET CORE IDENTITY

Entities                        Database Tables
---------                       ---------------

- IdentityUser               - AspNetUsers
- IdentityRole               - AspNetRoles               
- UserRole                   - AspNetUserRoles
 
 
USERS: (Id, FName, LName)
1       Philip      Amadi
2       Mustapha    Rufai


ROLES: (Id, Name)
1       DotNet Trainer
2       JavaScript Trainer
3       Learning Lead
 

USERROLES: (UserId, RoleId)
2       2
1       1
1       3


 
 */