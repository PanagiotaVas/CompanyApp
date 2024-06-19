using AutoMapper;
using CompanyProject.DTO;
using CompanyProject.Services;
using CompanyProject.Services.Exceptions;
using Microsoft.AspNetCore.Mvc;
using EmplTask = CompanyProject.Data.Task;
using Task = System.Threading.Tasks.Task;
using CompanyProject.Data;

namespace CompanyProject.Controllers
{
    [ApiController]
    [Route("/api/tasks")]
    public class TaskController : BaseController
    {
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;

        public TaskController(IApplicationService applicationService, IConfiguration configuration, IMapper mapper)
            : base(applicationService)
        {
            _configuration = configuration;
            _mapper = mapper;
        }
  
       [HttpPost]
        [Route("/api/tasks/insertTask")]
        public async Task<ActionResult<TaskReadOnlyDTO>> InsertTask(TaskDTO taskDTO)
       {
            if (!ModelState.IsValid)
            {
                var errors = ModelState
                    .Where(e => e.Value!.Errors.Any())
                    .Select(e => new { Field = e.Key, Errors = e.Value!.Errors.Select(err => err.ErrorMessage).ToArray() });

                throw new InvalidTaskException("ErrorInInsertingTask " + errors);
            }

            if (_applicationService == null)
            {
                throw new ServerGenericException("ApplicationServiceNull");
            }

            EmplTask? task = await _applicationService.TaskService.GetTaskByTitle(taskDTO.Title!);

            if (task is not null)
            {
                throw new TaskAlreadyExistsException("Task with title " + taskDTO.Title + " already exists.");
            }

            await _applicationService.TaskService.InsertTaskAsync(taskDTO);

            EmplTask? taskToReturn = await _applicationService.TaskService.GetTaskByTitle(taskDTO.Title!);

            if (taskToReturn is null)
            {
                throw new InsertTaskException("Error in inserting task");
            }

            return Ok(taskToReturn);
        }

        [HttpGet]
        [Route("/api/tasks/getTaskById/{id}")]
        public async Task<ActionResult<EmplTask>> GetTaskById(int id)
        {
            var taskToReturn = await _applicationService.TaskService.GetTaskAsync(id);

            if (taskToReturn is null)
            {
                throw new TaskNotFoundException("Task with id " + id + " was not Found.");
            }

            return Ok(taskToReturn);
        }

        [HttpGet]
        [Route("/api/tasks/getTaskByTitle/{title}")]
        public async Task<ActionResult<EmplTask>> GetTaskByTitle(string title)
        {
            var taskToReturn = await _applicationService.TaskService.GetTaskByTitle(title);

            if (taskToReturn is null)
            {
                throw new TaskNotFoundException("Task with title " + title + " was not Found.");
            }

            return Ok(taskToReturn);
        }


        [HttpPut]
        [Route("/api/tasks/updateTask")]
        public async Task<ActionResult<TaskDTO>> UpdateTask(TaskUpdateDTO dto)
        {
            EmplTask? searchTask = await _applicationService.TaskService.GetTaskAsync(dto.Id);

            if (searchTask is null)
            {
                throw new TaskNotFoundException("Task with id " + dto.Id + " could not be found to be updated.");
            }       

            EmplTask? task = await _applicationService.TaskService.UpdateTaskAsync(dto.Id, dto)!;
            TaskDTO taskToReturn = _mapper.Map<TaskDTO>(task);
            return Ok(taskToReturn);
        }


        [HttpDelete]
        [Route("/api/tasks/deleteTask/{id}")]
        public async Task<IActionResult> DeleteTask(int id)
        {
            ICollection<EmployeeTask> allAssignments;

            var taskToDelete = await _applicationService.TaskService.GetTaskAsync(id);

            if (taskToDelete is null) throw new TaskNotFoundException("Task with id " + id + " could not be found.");

            // need to delete the assignments first
            allAssignments = await _applicationService.EmployeesXTasksService.GetAllAssignmentsOfTaskAsync(id);

            if (allAssignments is not null)
            {
                foreach (var assignment in allAssignments)
                {
                    await _applicationService.EmployeesXTasksService.DeleteAllAssignmentsOfTask(assignment.TaskId);

                }
            }

            bool deleted = await _applicationService.TaskService.DeleteTaskAsync(id);

            return Ok(deleted);
        }


        [HttpGet]
        [Route("/api/tasks/getAllTasks")]
        public async Task<ActionResult<List<EmplTask>>> GetAllTasks()
        {
            ICollection<EmplTask> allTasks;

            allTasks = await _applicationService.TaskService.GetAllTasksAsync();

            if (allTasks is null)
                return NoContent();

            return Ok(allTasks);
        }

    }
}
