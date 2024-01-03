using Microsoft.AspNetCore.Identity;

namespace Server.Data.Models
{
    public class User : IdentityUser
    {
        public ICollection<Message> Messages { get; set; }
        
        public ICollection<Chat> Chats { get; set; }
    }
}
