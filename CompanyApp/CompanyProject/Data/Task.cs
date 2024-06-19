using System.Text.Json.Serialization;

namespace CompanyProject.Data
{
    public class Task
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public string? Title { get; set; }

        public string? Description { get; set; }

        public DateTime? Deadline { get; set; }

        [JsonIgnore]
        public virtual ICollection<Employee> Employees { get; }

        public virtual User User { get; set; } = null!;

        [JsonIgnore]
        public ICollection<EmployeeTask>? EmployeeTasks { get; set; }


    }
}
