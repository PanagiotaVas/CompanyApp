using CompanyProject.Models;
using System.ComponentModel.DataAnnotations;

namespace CompanyProject.DTO
{
    public class UserDTO
    {
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Username must be between 2 and 50 characters.")]
        public string? Username { get; set; }

        [StringLength(100, MinimumLength = 8, ErrorMessage = "Password must be between 8 and 100 character")]
        [RegularExpression(@"^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?\d)(?=.*?\W).{8,}$", ErrorMessage = "Password must contain " +
            "at least one uppercase letter, one lowercase letter, one digit and one special character.")]
        public string? Password { get; set; }

        [StringLength(100, ErrorMessage = "Email should not exceed 100 characters.")]
        [EmailAddress(ErrorMessage = "Invalid email address.")]
        public string? Email { get; set; }

        [StringLength(50, MinimumLength = 2, ErrorMessage = "Firstname  must be between 2 and 50 characters.")]
        public string? Firstname { get; set; }

        [StringLength(50, MinimumLength = 2, ErrorMessage = "Lastname  must be between 2 and 50 characters.")]
        public string? Lastname { get; set; }

        [StringLength(10, ErrorMessage = "Phonenumber must be 10 characters.")]
        public string? Phonenumber { get; set; }

    }
}
