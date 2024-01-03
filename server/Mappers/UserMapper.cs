using Server.Data.Models;
using Server.Models.Request;
using Server.Models.Response;

namespace Server.Mappers
{
    public class UserMapper
    {
        public static User Map(UserCreateDto dto)
        {
            return new User
            {
                Email = dto.Email,
                UserName = dto.UserName,
                Chats = new List<Chat>(),
                Messages = new List<Message>()
            };
        }

        public static UserGetDto Map(User user)
        {
            return new UserGetDto
            {
                Id = user.Id,
                UserName = user.UserName
            };
        }
    }
}