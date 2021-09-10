using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace JwtPractice.Domain.Entities
{
    public class Register
    {
        [Required(ErrorMessage = "First Name is required.")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last Name is required.")]
        public string LastName { get; set; }

        [EmailAddress(ErrorMessage = "This is not a valid email.")]
        [Required(ErrorMessage = "Email Address is required.")]
        public string EmailAddress { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        public string Password { get; set; }
    }
}
