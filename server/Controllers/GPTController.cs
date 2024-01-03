using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Server.Models.Request;
using Server.Services.Interfaces;

namespace Server.Controllers
{
    [ApiController]
    [Route("gpt")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class GPTController : ControllerBase
    {
        private readonly IGPTService _gptService;

        public GPTController(IGPTService gptService)
        {
            _gptService = gptService;
        }

        [HttpGet]
        public async Task<IActionResult> GetHistory()
        {
            var userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;
            var messages = await _gptService.GetHistory(userId);
            return Ok(messages);
        }

        [HttpPost("info")]
        public IActionResult SetInfo([FromBody] Text info)
        {
            _gptService.SetInfo(info.Content);
            return Ok();
        }

        [HttpGet("info")]
        public IActionResult GetInfo()
        {
            var info = _gptService.GetInfo();
            return Ok(info);
        }

        [HttpPost("ask")]
        public async Task<IActionResult> Ask([FromBody] Text question)
        {
            var userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;
            var answer = await _gptService.Ask(userId, question.Content);
            return Ok();
        }
    }
}