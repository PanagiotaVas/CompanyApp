using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using EmpTasks = CompanyProject.Data.Task;


namespace CompanyProject.Data
{
    public class Employee
    {
        public int Id { get; set; }

        public string? Firstname { get; set; }

        public string? Lastname { get; set; }

        public string? Email { get; set; }

        public string? PhoneNumber { get; set; }

        public int? UserId { get; set; }

        [JsonIgnore]
        public virtual ICollection<EmpTasks>? Tasks { get; set; }
        [JsonIgnore]
        public virtual ICollection<EmployeeTask>? EmployeeTasks { get; set; }
        
    }
}


