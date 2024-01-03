namespace Server.Data.Models
{
    public class ChatUserLanguage
    {
        public Guid Id { get; set; }

        public User User { get; set; }

        public Chat Chat { get; set; }

        public string Language { get; set; }
    }
}