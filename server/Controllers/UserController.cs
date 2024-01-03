using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Server.Models.Request;
using Server.Services.Interfaces;

namespace Server.Controllers
{
    [ApiController]
    [Route("users")]
    [EnableCors]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> LoginAsync([FromBody] UserLoginDto dto)
        {
            try
            {
                var loginDto = await _userService.LoginAsync(dto);
                return Ok(loginDto);
            }
            catch(Exception e) 
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public IActionResult GetAll()
        {
            var users = _userService.GetAll();
            return Ok(users);
        }

        [HttpGet("current")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> GetById()
        {
            var userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;
            var user = await _userService.GetByIdAsync(Guid.Parse(userId));
            return Ok(user);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] UserCreateDto dto)
        {
            var user = await _userService.CreateAsync(dto);
            return Ok(user);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            await _userService.DeleteAsync(id);
            return Ok();
        }
    }
}
