using CompanyProject.Models;
using CompanyProject.Services;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CompanyProject.Controllers
{
    [Produces("application/json")]  
    [Route("/api/[controller]/[action]")]
    [ApiController]
    public class BaseController : ControllerBase
    {
        public readonly IApplicationService _applicationService;

        protected BaseController(IApplicationService applicationService)
        {
            _applicationService = applicationService;
        }

        private ApplicationUser? _appUser;   // is the logged in user

        // prop of Base Controller
        protected ApplicationUser? AppUser
        {  
            get
            {
                if(User != null && User.Claims != null && User.Claims.Any())
                {
                    var claimsTypes = User.Claims.Select(t => t.Type);
                    if (!claimsTypes.Contains(ClaimTypes.NameIdentifier))
                    {
                        return null;
                    }

                    var userClaimsId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                    _ = int.TryParse(userClaimsId, out int id);

                    _appUser = new ApplicationUser
                    {
                        Id = id
                    };

                    var userClaimsName = User.FindFirst(ClaimTypes.Name)?.Value;

                    _appUser.Username = userClaimsName;
                    return _appUser;
                }
                return null;
            }
            // we return null if the id is not found
            // also if the username is not found
        }
    }
}
