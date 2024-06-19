using AutoMapper;
using CompanyProject.Data;
using CompanyProject.DTO;
using CompanyProject.Services;
using CompanyProject.Services.Exceptions;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CompanyProject.Controllers
{
    [ApiController]
    [Route("/api/users")]
    public class UserController : Controller
    {

        private readonly IConfiguration _configuration;
        private readonly IApplicationService _applicationService;
        private readonly IMapper? _mapper;

        public UserController(IApplicationService applicationService, IMapper mapper, IConfiguration configuration) : base()
        {
            _applicationService = applicationService;
            _mapper = mapper;
            _configuration = configuration;
        }

        [HttpPost]
        [Route("/api/users/findUser/{username}")]
        public async Task<IActionResult> GetUserByUsernameAsync(string username)
        {
            try
            {
                var user = await _applicationService.UserService.GetUserByUsernameAsync(username);
                return Ok(user);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("/api/users/getUserById/{id}")]
        public async Task<ActionResult<User>> GetUserById(int id)
        {
            var userToReturn = await _applicationService.UserService.GetUserByIdAsync(id);
            if (userToReturn is null)
            {
                throw new UserNotFoundException("UserNotFound");
            }


            return Ok(userToReturn);
        }

        [HttpGet]
        [Route("/api/users/getAllUsers")]
        public async Task<ActionResult<List<User>>> GetAllUsers()
        {
            ICollection<User> allUsers;

            allUsers = await _applicationService.UserService.GetAllUsers();

            if (allUsers is null)
                return NoContent();

            return Ok(allUsers);
        }

        [HttpPost]
        [Route("/api/users/login")]
        public async Task<IActionResult> LoginUser(UserLoginDTO request)
        {
            try
            {
                var user = await _applicationService.UserService.LoginUserAsync(request);
                if (user == null)
                {
                    return NotFound();
                }
                List<Claim> claims = new List<Claim>()
                {
                    new Claim(ClaimTypes.NameIdentifier, request.Username)
                };

                ClaimsIdentity identity = new ClaimsIdentity(claims,
                    CookieAuthenticationDefaults.AuthenticationScheme);

                AuthenticationProperties properties = new()
                {
                    AllowRefresh = true,
                    IsPersistent = request.keepLoggedIn
                };

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(identity), properties);

                return Ok(user);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
