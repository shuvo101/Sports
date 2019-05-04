using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Sports.API.Model;
using Sports.API.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Sports.API.Repository;
using Sports.API.Data;
using Sports.API.Helper.JWT;
using Microsoft.AspNetCore.Authorization;

namespace Sports.API.Controllers
{
    public class AuthController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IConfiguration _configuration;
        private readonly UnitOfWork _repo;
        private ApplicationDbContext _context;
        private JWT Jwt { get; }

        public AuthController(UserManager<ApplicationUser> userManager, IConfiguration configuration, JWT jwt, IHttpContextAccessor httpContextAccessor, ApplicationDbContext context)
        {
            _userManager = userManager;
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
            Jwt = jwt;
            _context = context;
            _repo = new UnitOfWork(_context);
        }

        [Route("login")]
        [HttpPost]
        [ProducesResponseType(typeof(SuccessfulLoginResponse), 200)]
        [ProducesResponseType(typeof(FailedLoginResponse), 401)]
        public async Task<IActionResult> Login([FromBody] LoginViewModel info)
        {
            //info.userName = System.Net.WebUtility.UrlDecode(info.userName);
            //info.password = System.Net.WebUtility.UrlDecode(info.password);

            var user = await _userManager.FindByNameAsync(info.userName);
            if (user != null && await _userManager.CheckPasswordAsync(user, info.password))
            {
                var roles = await _userManager.GetRolesAsync(user);
                LoggedInUserInfo userInfo = new LoggedInUserInfo();
                userInfo.UserID = user.Id;
                userInfo.FirstName = user.FirstName;
                userInfo.LastName = user.LastName;
                userInfo.UserName = user.UserName;
                userInfo.Role = roles.FirstOrDefault();
                var success = new SuccessfulLoginResponse { Token = Jwt.GetTokenFor(user.Id.ToString(), roles.FirstOrDefault()), User = userInfo };
                var response = new LoginResponseViewModel();
                response.successResonse = success;
                return Ok(response);
            }
            else
            {
                var response = new LoginResponseViewModel();
                response.failedResponse = new FailedLoginResponse { Error = 1007 };
                return UnauthorizedError(response);

            }

            

        }
        
        [HttpPost]
        [Route("[action]")]
        private IActionResult UnauthorizedError(object obj)
        {
            this.HttpContext.Response.StatusCode = 401;
            return new JsonResult(obj);
        }

        [HttpPost]
        [Authorize]
        [Route("[action]")]
        public IActionResult KeepAlive()
        {
            var name = User.Identity.Name;
            var role = User.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value).FirstOrDefault();
            return Ok(Jwt.GetTokenFor(name, role));
        }

        [HttpPost]
        [Authorize]
        [Route("[action]")]
        public ActionResult<PayloadResponse> IsAlive()
        {
            PayloadResponse response = new PayloadResponse();
            response.Message = "200";
            response.Payload = null;
            response.PayloadType = "CreateTest";
            response.ResponseTime = DateTime.Now.ToString();
            response.Success = true;
            return Ok(response);
        }
        
    }
}