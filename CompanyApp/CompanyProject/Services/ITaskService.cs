using CompanyProject.Data;
using CompanyProject.DTO;
using EmplTask = CompanyProject.Data.Task;
using Task = System.Threading.Tasks.Task;

namespace CompanyProject.Services
{
    public interface ITaskService
    {
        Task InsertTaskAsync(TaskDTO taskDTO);
        Task<EmplTask>? UpdateTaskAsync(int id, TaskUpdateDTO taskDTO);
        Task<bool> DeleteTaskAsync(int id);
        Task<EmplTask?> GetTaskAsync(int id);
        Task<ICollection<EmplTask>> GetAllTasksAsync();
        Task<ICollection<EmplTask>> GetAllTasksOfUserAsync(int userId);
        Task<EmplTask?> GetTaskByTitle(string title);
    }
}
