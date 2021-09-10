using System;
using System.ComponentModel.DataAnnotations;

namespace JwtPractice.Domain.Entities
{
    public class Administrator
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
        public DateTime DateProvisioned { get; set; }

        [Required]
        public long UserId { get; set; }

        public ApplicationUser User { get; set; }
    }
}
