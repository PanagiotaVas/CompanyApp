using CompanyProject.Data;
using EmplTask = CompanyProject.Data.Task;
using Task = System.Threading.Tasks.Task;

namespace CompanyProject.Services
{
    public interface IEmployeesXTasksService
    {
        Task<bool> AssignTaskAsync(string username, int taskId);
        Task<bool> DeleteAssignmentAsync(string username, int taskId);
        Task<ICollection<EmployeeTask>> GetAllAssignmentsOfUserAsync(int userId);
        Task<ICollection<EmployeeTask>> GetAllAssignmentsOfTaskAsync(int taskId);
        Task<ICollection<EmployeeTask>> GetAllAssignmentsOfUserAsync(string username);
        Task<ICollection<EmployeeTask>> GetAllAssignmentsAsync();
        Task<EmplTask> GetUserTaskAsync(string username, int taskId);
        Task<bool> DeleteEmployeeAndAssignments(int id, string username);
        Task<bool> DeleteAllAssignmentsOfTask(int taskId);
    }
}
