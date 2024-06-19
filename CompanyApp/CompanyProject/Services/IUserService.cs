using CompanyProject.Data;
using CompanyProject.DTO;
using CompanyProject.Models;
using EmplTask = CompanyProject.Data.Task;
using Task = System.Threading.Tasks.Task;

namespace CompanyProject.Services
{
    public interface IUserService
    {
        Task SignUpUserAsync(UserSignUpDTO userDTO);
        Task<User?> LoginUserAsync(UserLoginDTO loginDTO);
        Task<User?> UpdateUserAccountAsync(int userId, UserPatchDTO userDTO);
        Task<User?> GetUserByUsernameAsync(string username);
        Task<User?> GetUserByIdAsync(int id);
        Task<ICollection<User>> GetAllUsers();
        Task<User?> VerifyAndGetUser(UserLoginDTO credentials);
        string CreateUserToken(int userId, string username, string? appSecurityKey);
        Task<bool> DeleteUser(int id);

    }
}
