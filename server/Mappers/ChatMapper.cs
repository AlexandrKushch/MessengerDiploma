using OpenAI_API.Chat;
using Server.Data.Models;
using Server.Models.Response;

namespace Server.Mappers
{
    public class ChatMapper
    {
        public static ChatGetDto Map(Chat chat, string userId=null)
        {
            return new ChatGetDto
            {
                Id = chat.Id,
                Name = chat.Name,
                Users = chat.Users.ToList().ConvertAll(u => UserMapper.Map(u)),
                Messages = chat.Messages.ToList().ConvertAll(m => MessageMapper.Map(m)),
                Language = chat.Language.FirstOrDefault(l => l.User.Id == userId)?.Language ?? ""
            };
        }
    }
}