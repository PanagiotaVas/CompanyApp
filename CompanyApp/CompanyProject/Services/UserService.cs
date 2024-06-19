using AutoMapper;
using Azure.Core;
using CompanyProject.Data;
using CompanyProject.DTO;
using CompanyProject.Models;
using CompanyProject.Repositories;
using CompanyProject.Security;
using CompanyProject.Services.Exceptions;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Task = System.Threading.Tasks.Task;

namespace CompanyProject.Services
{
    public class UserService : IUserService
    {

        private readonly IUnitOfWork? _unitOfWork;
        private readonly ILogger<UserService>? _logger;
        private readonly IMapper? _mapper;

        public UserService(IUnitOfWork? unitOfWork, ILogger<UserService>? logger, IMapper? mapper)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<bool> DeleteUser(int id)
        {
            bool userDeleted = false;
            try
            {
                User? user = await _unitOfWork!.UserRepository.GetByIdAsync(id);
                if (user is null) throw new UserNotFoundException("User with id " + id + " was not found");

                var deleted = await _unitOfWork.UserRepository.DeleteAsync(user.Id);

                await _unitOfWork.SaveAsync();

                userDeleted = true;
                _logger!.LogInformation("{Message}", "Employee with id: " + id + " was deleted successfully");
                return userDeleted;
            }
            catch (Exception ex)
            {
                _logger!.LogError("{Message}{Exception}", ex.Message, ex.StackTrace);
            }
            return userDeleted;
        }

        public async Task<User?> GetUserByUsernameAsync(string username)
        {
            User? user = null;

            try
            {
                user = await _unitOfWork!.UserRepository.GetByUsernameAsync(username);
                _logger!.LogInformation("{Message}", "User: " + user + " was found and returned successfully.");
            }
            catch (Exception ex)
            {
                _logger!.LogError("{Message}{Exception}", ex.Message, ex.StackTrace);
            }
            return user;
        }

        public async Task<User?> LoginUserAsync(UserLoginDTO loginDTO)
        {
            User? user = null;

            try
            {
                user = await _unitOfWork!.UserRepository.GetUserAsync(loginDTO.Username!, loginDTO.Password!);
                if (user == null) return null;
                _logger!.LogInformation("{Message}", "User: " + user + " logged in successfully.");
            }
            catch(Exception ex)
            {
                _logger!.LogError("{Message}{Exception}", ex.Message, ex.StackTrace);
            }
            return user;
        }

        public async Task<User?> UpdateUserAccountAsync(int userId, UserPatchDTO userDTO)
        {
            User user  = null;
            try
            {
                user = await _unitOfWork.UserRepository.UpdateUserAsync(userId, userDTO);
                await _unitOfWork.SaveAsync();
                _logger!.LogInformation("{Message}", "User: " + user + " was updated successfully.");

            }
            catch (Exception ex)
            {
                _logger!.LogError("{Message}{Exception}", ex.Message, ex.StackTrace);

            }
            return user;
        }

        public async Task SignUpUserAsync(UserSignUpDTO signUpDTO)
        {
            Employee? employee;
            User? user = null;

            try
            {
                user = ExtractUser(signUpDTO);
                User? userAlreadyExists = await _unitOfWork!.UserRepository.GetByUsernameAsync(user.Username!);

                if (userAlreadyExists is not null)
                {
                    throw new UserAlreadyExistsException("User with username " + user.Username + " already exists.");
                }

                user.Password = EncryptionUtil.Encrypt(user.Password!);
                await _unitOfWork.UserRepository!.AddAsync(user);
                await _unitOfWork!.SaveAsync();

                var insertedUser = await _unitOfWork.UserRepository.GetByUsernameAsync(signUpDTO.Username);

                employee = ExtractEmployee(signUpDTO);

                if (await _unitOfWork!.EmployeeRepository.GetEmployeeByPhonenumber(employee.PhoneNumber) is not null)
                    {
                        throw new EmployeeAlreadyExistsException("Employee already exists.");
                    }

                employee.UserId = insertedUser.Id;

                await _unitOfWork!.EmployeeRepository.AddAsync(employee);

                await _unitOfWork!.SaveAsync();

                _logger!.LogInformation("{Message}", "User " + user + " signed up successfully.");
            }
            catch (Exception ex)
            {
                _logger!.LogError("{Message}{Exception}", ex.Message, ex.StackTrace);
            }
        }

        public async Task<User?> VerifyAndGetUser(UserLoginDTO credentials)
        {
            User? user = null;

            try
            {
                user = await _unitOfWork!.UserRepository.GetUserAsync(credentials.Username!, credentials.Password!);
                _logger!.LogInformation("{Message}", "User: " + user + " was found and returned successfully.");
            }
            catch (Exception ex)
            {
                _logger!.LogInformation("{Message}{Exception}", ex.Message, ex.StackTrace);
            }
            return user;
        }


        public string CreateUserToken(int userId, string username, string? appSecurityKey)
        {
            var sercurityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(appSecurityKey!));
            var signingCredentials = new SigningCredentials(sercurityKey, SecurityAlgorithms.HmacSha256);

            var claimsInfo = new List<Claim>
                            {
                                new Claim(ClaimTypes.Name, username),
                                new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
                            };

            var jwtSecurityToken = new JwtSecurityToken(null, null, claimsInfo, DateTime.UtcNow,
                DateTime.UtcNow.AddHours(2), signingCredentials);

            var userToken = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
            return userToken;

        }

        private User ExtractUser(UserSignUpDTO signUpDTO)
        {
            return new User()
            {
                Username = signUpDTO.Username,
                Password = signUpDTO.Password,
            };
        }

        private Employee ExtractEmployee(UserSignUpDTO userSignUpDTO)
        {
            return new Employee()
            {
                Firstname = userSignUpDTO.Firstname,
                Lastname = userSignUpDTO.Lastname,
                Email = userSignUpDTO.Email,
                PhoneNumber = userSignUpDTO.PhoneNumber,
            };
        }

        public async Task<User?> GetUserByIdAsync(int id)
        {
            User? user = null;

            try
            {
                user = await _unitOfWork!.UserRepository.GetByIdAsync(id);
                _logger!.LogInformation("{Message}", "User with id: " + id + " was found successfully.");
            }
            catch (Exception ex)
            {
                _logger!.LogError("{Message}{Exception}", ex.Message, ex.StackTrace);
                throw;
            }
            return user;
        }

        public async Task<ICollection<User>> GetAllUsers()
        {
            ICollection<User> allUsers = null;
            try
            {
                allUsers = await _unitOfWork!.UserRepository.GetAllUsersAsync();
                _logger!.LogInformation("{Message}", "All Employees were returned successfully");
                return allUsers;

            }
            catch (Exception ex)
            {
                _logger!.LogError("{Message}{Exception}", ex.Message, ex.StackTrace);
            }
            return allUsers;
        }
    }
}
