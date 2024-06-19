using CompanyProject.Data;

namespace CompanyProject.Repositories
{
    public interface IEmployeesXTasksRepository
    {
        Task<bool> AssignTaskToEmployee(int emplId, int tId, int uId);
        Task<bool> RemoveAssignment(int emplId, int tId, int uId);
        Task<bool> RemoveAssignmentByTask(int taskId);
        Task<ICollection<EmployeeTask>> GetAllAssignmentsOfUserAsync(int userId);
        Task<ICollection<EmployeeTask>> GetAllAssignmentsOfTaskAsync(int taskId);
        Task<ICollection<EmployeeTask>> GetAllAssignmentsOfUserAsync(string username);
        Task<ICollection<EmployeeTask>> GetAllAssignmentsAsync();
    }
}
