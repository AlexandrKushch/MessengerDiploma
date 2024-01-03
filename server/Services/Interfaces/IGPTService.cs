using Server.Data.Models;
using Server.Models.Response;

namespace Server.Services.Interfaces
{
    public interface IGPTService
    {
        Task<List<MessageGetDto>> GetHistory(string userId);

        void SetInfo(string info);

        string GetInfo();

        Task<MessageGetDto> Ask(string userId, string question);

        Task Translate(string currentUserId, Chat chat, string language);

        Task Translate(Message message, string language);
    }
}