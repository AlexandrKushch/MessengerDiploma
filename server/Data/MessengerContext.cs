using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Server.Data.Models;

namespace Server.Data
{
    public class MessengerContext : IdentityDbContext<User>
    {
        public MessengerContext(DbContextOptions options) : base(options)
        {

        }

        public DbSet<Message> Messages { get; set; }

        public DbSet<Chat> Chats { get; set; }

        public DbSet<ChatUserLanguage> Languages { get; set; }
    }
}
