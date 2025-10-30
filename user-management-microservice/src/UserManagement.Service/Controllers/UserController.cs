using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using UserManagement.Service.DTOs;
using UserManagement.Service.Services;

namespace UserManagement.Service.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly UserService _userService;

        public UserController(UserService userService)
        {
            _userService = userService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserRegistrationDto registrationDto)
        {
            var result = await _userService.RegisterUserAsync(registrationDto);
            if (result.IsSuccess)
            {
                return CreatedAtAction(nameof(GetUser), new { id = result.UserId }, result);
            }
            return BadRequest(result.Errors);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserLoginDto loginDto)
        {
            var result = await _userService.AuthenticateUserAsync(loginDto);
            if (result.IsSuccess)
            {
                return Ok(result);
            }
            return Unauthorized(result.Errors);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUser(int id)
        {
            var user = await _userService.GetUserByIdAsync(id);
            if (user != null)
            {
                return Ok(user);
            }
            return NotFound();
        }
    }
}