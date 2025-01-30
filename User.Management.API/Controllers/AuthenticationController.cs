using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NETCore.MailKit.Core;
using System.Data;
using User.Management.API.Models;
using User.Management.API.Models.Authentication.SignUp;
using User.Management.Service.Models;
using User.Management.Service.Services;


namespace User.Management.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {

        private readonly UserManager<IdentityUser> _usermanager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;
        private readonly IEmailServices _emailService;

        public AuthenticationController(UserManager<IdentityUser> usermanager, 
            RoleManager<IdentityRole> roleManager,
            IConfiguration configuration,
            IEmailServices emailService)
        {
            _usermanager = usermanager;
            _roleManager = roleManager;
            _configuration = configuration;
            _emailService = emailService;
        }
        [HttpPost]
        public async Task<IActionResult> Register([FromBody]RegisterUser registerUser, string role)
        {
            //check user exist by email
            var userexist = await _usermanager.FindByEmailAsync(registerUser.Email);
            if (userexist != null) {
                return StatusCode(StatusCodes.Status403Forbidden, 
                    new Response { Status ="Error", Message="User Already Exist"});
            }

            //Add user to Database
            IdentityUser user = new()
            {
                UserName = registerUser.Username,
                SecurityStamp = Guid.NewGuid().ToString(),
                Email = registerUser.Email,
            };

            //Check Role is exist or not
            if(await _roleManager.RoleExistsAsync(role))
            {
                var result = await _usermanager.CreateAsync(user, registerUser.Password);
                if (!result.Succeeded)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError,
                      new Response { Status = "Error", Message = "User Failed to Created!" });
                }
                _usermanager.AddToRoleAsync(user, role);
                return StatusCode(StatusCodes.Status200OK,
                new Response { Status = "Success", Message = "User Created Successfully!" });
            }
            else
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                      new Response { Status = "Error", Message = "Role is not Exist." });
            }
        }

        [HttpGet]
        public IActionResult TestEmail()
        {
            var message = new Message(new string[] { "uniquextech7@gmail.com" }, "Testing Email", "Testing Email");
            _emailService.SendEmails(message);
            return StatusCode(StatusCodes.Status200OK,
                new Response { Status = "Success", Message = "User Created Successfully!" });
        }

        [HttpGet("ConfirmEmail")]
        public async Task<IActionResult> ConfirmEmail(string code, string email)
        {
            var user = await _usermanager.FindByEmailAsync(email);
            if(user != null)
            {
                var result = await _usermanager.ConfirmEmailAsync(user, code);
                if (!result.Succeeded)
                {
                    return StatusCode(StatusCodes.Status200OK,
                        new Response { Status = "Success", Message = "Email Verified Successfully!" });
                }
            }
            return StatusCode(StatusCodes.Status500InternalServerError,
                      new Response { Status = "Error", Message = "Email Verified Failed!" });
        }
    }
}
