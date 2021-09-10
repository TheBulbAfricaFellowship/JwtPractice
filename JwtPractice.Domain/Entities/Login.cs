using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JwtPractice.Domain.Entities
{
    public class Login
    {
        [EmailAddress(ErrorMessage = "This is not a valid email.")]
        [Required(ErrorMessage = "Email Address is required.")]
        public string EmailAddress { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        public string Password { get; set; }
    }
}
