using System.ComponentModel.DataAnnotations;

namespace JwtPractice.Domain.DTOs
{
    public class Login
    {
        [EmailAddress(ErrorMessage = "This is not a valid email address.")]
        [Required(ErrorMessage = "Email Address is required.")]
        public string EmailAddress { get; set; }


        [Required(ErrorMessage = "Password is required.")]
        public string Password { get; set; }
    }
}
