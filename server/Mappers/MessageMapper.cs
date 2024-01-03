using OpenAI_API.Chat;
using Server.Data.Models;
using Server.Models.Request;
using Server.Models.Response;

namespace Server.Mappers
{
    public class MessageMapper
    {
        public static Message Map(MessageCreateDto dto)
        {
            return new Message
            {
                PostedTime = DateTime.UtcNow,
                Content = dto.Content
            };
        }

        public static MessageGetDto Map(Message message)
        {
            return new MessageGetDto
            {
                Id = message.Id,
                Content = message.Content,
                PostedTime = message.PostedTime,
                UserName = message.User?.UserName ?? "GPT",
                ChatId = message.Chat.Id
            };
        }
    }
}