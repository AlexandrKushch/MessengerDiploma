namespace Server.Models.Response
{
    public class LoginGetDto
    {
        public string Id { get; set; }

        public string UserName { get; set; }
        
        public string Token { get; set; }
    }
}