using CompanyProject.Data;
using Microsoft.EntityFrameworkCore;

namespace CompanyProject.Repositories
{
    public class EmployeesXTasksRepository : BaseRepository<EmployeeTask>, IEmployeesXTasksRepository
    {
        public EmployeesXTasksRepository(CompanyProjectDbContext context) : base(context) { }

        public async Task<bool> AssignTaskToEmployee(int emplId, int tId, int uId)
        {
            var employeeId = emplId;
            var taskId = tId;
            var userId = uId;

            var employee = _context.Employees.Where(x => x.Id == employeeId).FirstOrDefault();
            var task = _context.Tasks.Where(x => x.Id == taskId).FirstOrDefault();
            if (employee is null || task is null) return false;

            var newTask = new EmployeeTask()
            {
                EmployeeId = employeeId,
                TaskId = taskId,
                UserId = userId
            };
            _context.EmployeeTasks.Add(newTask); 

            await _context.SaveChangesAsync();
            return true;

        }

        public async Task<ICollection<EmployeeTask>> GetAllAssignmentsAsync()
        {
            List<EmployeeTask> assignments = await _context.EmployeeTasks.ToListAsync();
            return assignments;
        }

        public async Task<ICollection<EmployeeTask>> GetAllAssignmentsOfTaskAsync(int taskId)
        {
            List<EmployeeTask> assignments = await _context.EmployeeTasks.Where(x => x.TaskId == taskId).ToListAsync();
            return assignments;
        }

        public async Task<ICollection<EmployeeTask>> GetAllAssignmentsOfUserAsync(int userId)
        {
            List<EmployeeTask> assignments = await _context.EmployeeTasks.Where(x => x.UserId == userId).ToListAsync();
            return assignments;
        }

        public async Task<ICollection<EmployeeTask>> GetAllAssignmentsOfUserAsync(string username)
        {
            List<EmployeeTask> assignments = await _context.EmployeeTasks.Where(x => x.User.Username == username).ToListAsync();
            return assignments;
        }

        public async Task<bool> RemoveAssignment(int emplId, int tId, int uId)
        {

            var employee = await _context.Employees.Where(x => x.Id == emplId).FirstOrDefaultAsync();
            var task = await _context.Tasks.Where(x => x.Id == tId).FirstOrDefaultAsync();
            var user = await _context.Users.Where(x => x.Id == uId).FirstOrDefaultAsync();

            
            if (employee is null || task is null || user is null) return false;

            var assignment = await _context.EmployeeTasks.Where(
                x => x.EmployeeId == employee.Id && x.TaskId == task.Id && x.UserId == user.Id).FirstOrDefaultAsync();
            
            _context.EmployeeTasks.Remove(assignment!);

            _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> RemoveAssignmentByTask(int taskId)
        {

            var task = await _context.Tasks.Where(x => x.Id == taskId).FirstOrDefaultAsync();

            if ( task is null ) return false;
                                                                                // whatever the rest may be
            var assignment = await _context.EmployeeTasks.Where( x => x.TaskId == task.Id).FirstOrDefaultAsync();

            _context.EmployeeTasks.Remove(assignment!);

            _context.SaveChangesAsync();

            return true;
        }
    }
}
