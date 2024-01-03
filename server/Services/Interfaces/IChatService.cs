using Server.Models.Request;
using Server.Models.Response;

namespace Server.Services.Interfaces
{
    public interface IChatService
    {
        Task<ChatGetDto> CreateAsync(string name);

        List<ChatGetDto> GetAll(string userId);

        Task<ChatGetDto> GetById(string userId, Guid chatId, string language);

        Task<ChatGetDto> AddUserAsync(Guid chatId, string userId);

        Task<MessageGetDto> SendMessage(string authorId, Guid chatId, MessageCreateDto message);

        Task<MessageGetDto> TranslateMessage(Guid chatId, Guid messageId, string language);
        
        Task DeleteAsync(Guid id);
    }
}