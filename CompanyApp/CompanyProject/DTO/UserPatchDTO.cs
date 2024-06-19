using System.ComponentModel.DataAnnotations;

namespace CompanyProject.DTO
{
    public class UserPatchDTO
    {
        [Required]
        public int Id { get; set; }

        [StringLength(50, MinimumLength = 2, ErrorMessage = "Username must be between 2 and 50 character")]
        public string? Username { get; set; }

        [StringLength(100, MinimumLength = 8, ErrorMessage = "Password must be between 8 and 100 character")]
        [RegularExpression(@"^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?\d)(?=.*?\W).{8,}$", ErrorMessage = "Password must contain " +
            "at least one uppercase letter, one lowercase letter, one digit and one special character.")]
        public string? Password { get; set; }
    }
}
