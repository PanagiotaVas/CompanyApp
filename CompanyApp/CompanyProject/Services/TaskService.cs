using AutoMapper;
using CompanyProject.Data;
using CompanyProject.DTO;
using CompanyProject.Repositories;
using CompanyProject.Services.Exceptions;
using EmplTask = CompanyProject.Data.Task;
using Task = System.Threading.Tasks.Task;

namespace CompanyProject.Services
{
    public class TaskService : ITaskService
    {
        private readonly IUnitOfWork? _unitOfWork;
        private readonly IMapper? _mapper;
        private readonly ILogger<UserService>? _logger;

        public TaskService(IUnitOfWork? unitOfWork, ILogger<UserService>? logger, IMapper? mapper)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<bool> DeleteTaskAsync(int id)
        {
            bool taskDeleted = false;
            try
            {
                var taskToDelete = await _unitOfWork!.TaskRepository.GetTaskAsync(id);
                if (taskToDelete is null) throw new TaskNotFoundException("Task with id " + id + " was not found");

                taskDeleted = await _unitOfWork.TaskRepository.DeleteTaskAsync(id);

                await _unitOfWork.SaveAsync();

                _logger!.LogInformation("{Message}", "Task with id:  " + id + " was deleted successfully.");
                return taskDeleted;
            }
            catch (Exception ex)
            {
                _logger!.LogError("{Message}{Exception}", ex.Message, ex.StackTrace);
            }
            return taskDeleted;
        }

        public async Task<ICollection<EmplTask>> GetAllTasksAsync()
        {
            ICollection<EmplTask>? tasks = null;

            try
            {
                tasks = await _unitOfWork!.TaskRepository.GetAllTasksAsync();
                _logger!.LogInformation("{Message}", "All tasks were returned successfully.");
                return tasks;
            }
            catch (Exception ex)
            {
                _logger!.LogError("{Messaage}{Exception}", ex.Message, ex.StackTrace);
            }
            return tasks;
        }

        public async Task<EmplTask?> GetTaskByTitle(string title)
        {
            EmplTask? taskToReturn = null;

            try
            {
                taskToReturn = await _unitOfWork!.TaskRepository.GetTaskByTitle(title);
                _logger!.LogInformation("{Message}", "Task with title " + title + " was returned successfully");
            }
            catch (Exception ex)
            {
                _logger!.LogError("{Message}{Error}", ex.Message, ex.StackTrace);
            }
            return taskToReturn;
        }

        public async Task<ICollection<EmplTask>> GetAllTasksOfUserAsync(int userId)
        {
            ICollection<EmplTask>? tasks = null;

            try
            {
                tasks = await _unitOfWork!.TaskRepository.GetAllTasksOfUserAsync(userId);
                _logger!.LogInformation("{Message}", "All tasks of user with id " + userId + " were returned successfully.");
                return tasks;
            }
            catch (Exception ex)
            {
                _logger!.LogError("{Messaage}{Exception}", ex.Message, ex.StackTrace);
            }
            return tasks;
        }

        public async Task<EmplTask?> GetTaskAsync(int id)
        {
            EmplTask? task = null;
            try
            {
                task = await _unitOfWork!.TaskRepository.GetTaskAsync(id);

                _logger!.LogInformation("{Message}", "Task with id: " + id + " was found successfully");
                return task;
            }
            catch (Exception ex)
            {
                _logger!.LogError("{Message}{Exception}", ex.Message, ex.StackTrace);
            }
            return task;
        }

        public async Task InsertTaskAsync(TaskDTO taskDTO)
        {
            try
            {
                bool inserted = await _unitOfWork!.TaskRepository.InsertTaskAsync(taskDTO);
                await _unitOfWork.SaveAsync();
                _logger!.LogInformation("{Message}", "Task was updated successfully");
            }
            catch (Exception ex)
            {
                _logger!.LogError("{Message}{Error}", ex.Message, ex.StackTrace);

            }
        }

        public async Task<EmplTask>? UpdateTaskAsync(int id, TaskUpdateDTO taskDTO)
        {
            EmplTask taskToReturn = null;

            try
            {
                taskToReturn = await _unitOfWork!.TaskRepository.UpdateTaskAsync(id, taskDTO);
                _logger!.LogInformation("{Message}", "Task with id " + id + " was updated successfully");
            }
            catch (Exception ex)
            {
                _logger!.LogError("{Message}{Error}", ex.Message, ex.StackTrace);

            }
            return taskToReturn;
        }
    }
}

