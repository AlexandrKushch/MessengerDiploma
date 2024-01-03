using System.ComponentModel.DataAnnotations.Schema;
namespace Server.Data.Models
{
    public class Message
    {
        public Guid Id { get; set; }

        public string Content { get; set; }

        public DateTime PostedTime { get; set; }

        public User User { get; set; }
        
        public Chat Chat { get; set; }
    }    
}
