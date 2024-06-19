using Azure.Core;
using CompanyProject.Data;
using CompanyProject.DTO;
using Microsoft.EntityFrameworkCore;
using EmplTask = CompanyProject.Data.Task;
using Task = System.Threading.Tasks.Task;

namespace CompanyProject.Repositories
{
    public class TaskRepository : BaseRepository<EmplTask>, ITaskRepository
    {
        public TaskRepository(CompanyProjectDbContext context)
            : base(context)
        { }

        public async Task<bool> DeleteTaskAsync(int id)
        {
            try
            {
                var task = await _context.Tasks.FindAsync(id);
                if (task == null)
                {
                    return false;
                }

                _context.Tasks.Remove(task);
                int affectedRows = await _context.SaveChangesAsync();

                return affectedRows > 0;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<ICollection<EmplTask>> GetAllTasksAsync()
        {
            ICollection<EmplTask> tasks = await _context.Tasks.ToArrayAsync();
            return tasks;
        }

        public async Task<ICollection<EmplTask>> GetAllTasksOfUserAsync(int userId)
        {
            var tasks = await _context.Employees
                .Where(e => e.UserId == userId)
                .SelectMany(e => e.EmployeeTasks)
                .Select(et => et.Task)
                .Include(t => t.Employees)
                .ToListAsync();

            return tasks;
        }

        public async Task<EmplTask?> GetTaskAsync(int id)
        {
            return await _context.Tasks.Where(x => x.Id == id).FirstOrDefaultAsync();
        }

        public async Task<EmplTask?> GetTaskByTitle(string title)
        {
            EmplTask? task = await _context.Tasks.Where(x => x.Title == title).FirstOrDefaultAsync();
            return task;
        }

        public async Task<List<Employee>> GetTaskEmployeesAsync(int taskId)
        {
            List<Employee> employees;
                                                    
            employees = await _context.Tasks.Where(emp => emp.Id == taskId)
                    .SelectMany(em => em.Employees).ToListAsync();
           
            return employees;
        }

        public async Task<bool> InsertTaskAsync(TaskDTO taskDTO)
        {
            try
            {
                var task = new EmplTask()
                {
                    Title = taskDTO.Title,
                    UserId = taskDTO.UserId,
                    Description = taskDTO.Description,
                    Deadline = taskDTO.Deadline
                };

                await _context.Tasks.AddAsync(task);
                int affectedRows = await _context.SaveChangesAsync();

                return affectedRows > 0; // Return true if the project was successfully inserted
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<EmplTask> UpdateTaskAsync(int id, TaskUpdateDTO taskUpdateDTO)
        {
            var task = await _context.Tasks.FindAsync(id);
            if (task is null) return null;

            task.Title = taskUpdateDTO.Title;
            task.UserId = taskUpdateDTO.UserId;
            task.Description = taskUpdateDTO.Description;
            task.Deadline = taskUpdateDTO.Deadline;

            try
            {
                _context.Tasks.Update(task);
                int affectedRows = await _context.SaveChangesAsync();

                return affectedRows > 0 ? task : null;
            }
            catch (DbUpdateConcurrencyException)
            {
                return null;
            }
            catch (DbUpdateException)
            {
                return null;
            }
        }
    }
}
