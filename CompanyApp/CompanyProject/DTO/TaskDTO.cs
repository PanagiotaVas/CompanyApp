using System.ComponentModel.DataAnnotations;

namespace CompanyProject.DTO
{
    public class TaskDTO
    {
        [Required]
        public int UserId { get; set; }

        [StringLength(255, MinimumLength = 10, ErrorMessage = "Title must be between 10 and 255 characters")]
        public string? Title { get; set; }

        [StringLength(255, MinimumLength = 10, ErrorMessage = "Description must be between 10 and 255 characters")]
        public string? Description { get; set; }

        public DateTime? Deadline { get; set; }

    }
}
