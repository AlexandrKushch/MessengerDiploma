namespace Server.Models.Response
{
    public class MessageGetDto
    {
        public Guid Id { get; set; }

        public String Content { get; set; }

        public DateTime PostedTime { get; set; }

        public string UserName { get; set; }

        public Guid ChatId { get; set; }
    }
}