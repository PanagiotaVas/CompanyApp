using CompanyProject.Data;
using CompanyProject.DTO;
using EmpTasks = CompanyProject.Data.Task;
using Task = System.Threading.Tasks.Task;

namespace CompanyProject.Repositories
{
    public interface IUserRepository : IBaseRepository<User>
    {
        Task<bool> SignUpUserAsync(UserDTO userDTO);
        Task<User?> GetUserAsync(string username, string password);
        Task<User?> UpdateUserAsync(int userId, UserPatchDTO userDTO);
        Task<User?> GetByUsernameAsync(string username);
        Task<User?> GetByIdAsync(int id);
        Task<ICollection<User>> GetAllUsersAsync();
        Task<bool> DeleteUserAsync(int id);
    }
}
