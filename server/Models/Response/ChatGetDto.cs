namespace Server.Models.Response
{
    public class ChatGetDto
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public ICollection<UserGetDto> Users { get; set; } 

        public ICollection<MessageGetDto> Messages { get; set; }

        public string Language { get; set; }
    }
}