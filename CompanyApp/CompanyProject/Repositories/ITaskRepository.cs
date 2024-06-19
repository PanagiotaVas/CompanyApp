using CompanyProject.Data;
using CompanyProject.DTO;
using EmplTask = CompanyProject.Data.Task;

namespace CompanyProject.Repositories
{
    public interface ITaskRepository
    {
        Task<bool> InsertTaskAsync(TaskDTO taskDTO);
        Task<EmplTask> UpdateTaskAsync(int id, TaskUpdateDTO taskUpdateDTO);
        Task<EmplTask?> GetTaskAsync(int id);
        Task<EmplTask?> GetTaskByTitle(string title);
        Task<bool> DeleteTaskAsync(int id);
        Task<ICollection<EmplTask>> GetAllTasksAsync();
        Task<ICollection<EmplTask>> GetAllTasksOfUserAsync(int userId);
        Task<List<Employee>> GetTaskEmployeesAsync(int taskId);
    }
}
