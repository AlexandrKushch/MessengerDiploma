using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Server.Models.Request;
using Server.Models.Response;
using Server.Services.Interfaces;

namespace Server.Controllers
{
    [ApiController]
    [Route("chats")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class ChatController : ControllerBase
    {
        private readonly IChatService _chatService;

        public ChatController(IChatService chatService)
        {
            _chatService = chatService;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;
            var chats = _chatService.GetAll(userId);
            return Ok(chats);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ChatCreateDto chat)
        {
            var c = await _chatService.CreateAsync(chat.Name);
            return Ok(c);
        }

        [HttpPost("{chatId}/users/{userId}")]
        public async Task<IActionResult> AddUser([FromRoute] Guid chatId, [FromRoute] string userId)
        {
            var chat = await _chatService.AddUserAsync(chatId, userId);
            return Ok(chat);
        }

        [HttpGet("{chatId}")]
        public async Task<IActionResult> GetById([FromRoute] Guid chatId, [FromQuery] string language = "")
        {
            var userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;
            var chat = await _chatService.GetById(userId, chatId, language);
            return Ok(chat);
        }

        [HttpPost("{chatId}/messages")]
        public async Task<IActionResult> SendMessage([FromRoute] Guid chatId, [FromBody] MessageCreateDto messageDto)
        {
            var userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;
            var message = await _chatService.SendMessage(userId, chatId, messageDto);
            return Ok(message);
        }

        [HttpGet("{chatId}/translate/{messageId}")]
        public async Task<IActionResult> TranslateMessage([FromRoute] Guid chatId, [FromRoute] Guid messageId, [FromQuery] string language)
        {
            var watch = new System.Diagnostics.Stopwatch();
            watch.Start();

            var message = await _chatService.TranslateMessage(chatId, messageId, language);

            watch.Stop();
            Console.WriteLine($"Execution Time Translate Message: {watch.ElapsedMilliseconds} ms");

            return Ok(message);
        }
    }
}