using AutoMapper;
using AutoMapper.QueryableExtensions;
using Azure.Core;
using CompanyProject.Data;
using CompanyProject.DTO;
using CompanyProject.Models;
using CompanyProject.Security;
using Microsoft.EntityFrameworkCore;
using EmpTasks = CompanyProject.Data.Task;
using Task = System.Threading.Tasks.Task;

namespace CompanyProject.Repositories
{
    public class UserRepository : BaseRepository<User>, IUserRepository

    {
        private readonly IMapper _mapper;
        public UserRepository(CompanyProjectDbContext context, IMapper mapper)
            : base(context)
        {
            _mapper = mapper;
        }

        public async Task<bool> DeleteUserAsync(int id)
        {
            var userToDelete = await _context.Users.FindAsync(id);

            if (userToDelete is null) return false;

            _context.Users.Remove(userToDelete);
        

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> SignUpUserAsync(UserDTO userDTO)
        {
            var existingUser = await _context.Users.FirstOrDefaultAsync(x => x.Username == userDTO.Username);
            if (existingUser != null) return false;

            var user = new User()
            {
                Username = userDTO.Username,
                Password = EncryptionUtil.Encrypt(userDTO.Password)
            };

            await _context.Users.AddAsync(user);
            return true;
        }

        public async Task<User?> GetByUsernameAsync(string username)
        {
            var userToReturn = await _context.Users.Where(u => u.Username == username).FirstOrDefaultAsync();

            return userToReturn;
        }

        public async Task<User?> GetByIdAsync(int id)
        {
            var userToReturn = await _context.Users.Where(u => u.Id == id).FirstOrDefaultAsync();

            return userToReturn;
        }

        public async Task<User?> GetUserAsync(string username, string password)
        {
            var user = await _context.Users.Where(u => u.Username == username).FirstOrDefaultAsync();

            if (user is null)
            {
                return null;
            }

            if (!EncryptionUtil.IsValidPassword(password, user.Password))
            {
                return null;
            }
            return user;
        }

        public async Task<ICollection<User>> GetAllUsersAsync()
        {
            return await _context.Users
                .ToArrayAsync();
        }

        public async Task<User?> UpdateUserAsync(int userId, UserPatchDTO userDTO)
        {
            var user = await _context.Users.Where(x => x.Id == userId).FirstAsync();
            user.Username = userDTO.Username;

            user.Password = EncryptionUtil.Encrypt(userDTO.Password);

            _context.Users.Update(user);
            return user;


        }
    }
}
