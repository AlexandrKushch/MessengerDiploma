using System.ComponentModel.DataAnnotations.Schema;

namespace Server.Data.Models
{
    public class Chat
    {
        public Guid Id { get; set; }

        public string Name { get; set; }
        
        public ICollection<User> Users { get; set; } = new List<User>();

        public ICollection<Message> Messages { get; set; } = new List<Message>();

        public ICollection<ChatUserLanguage> Language { get; set; } = new List<ChatUserLanguage>();
    }
}
