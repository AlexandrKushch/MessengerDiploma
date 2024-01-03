using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Server.Data.Models.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly UserManager<User> _userManager;

        private readonly MessengerContext _context;

        public UserRepository(UserManager<User> userManager, MessengerContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        public async Task<IdentityResult> Create(User user, string password)
        {
            return await _userManager.CreateAsync(user, password);
        }

        public async Task Delete(User user)
        {
            await _userManager.DeleteAsync(user);
        }

        public List<User> GetAll()
        {
            return _userManager.Users.ToList();
        }

        public async ValueTask<User> GetById(string id)
        {
            return await _userManager.FindByIdAsync(id);
        }

        public async Task Update(User user)
        {
            await _userManager.UpdateAsync(user);
        }

        public async ValueTask<User> GetByEmail(string email)
        {
            return await _userManager.FindByEmailAsync(email);
        }

        public async ValueTask<User> GetByUserName(string userName)
        {
            return await _userManager.FindByNameAsync(userName);
        }

        public async ValueTask<bool> CheckPassword(User user, string password)
        {
            return await _userManager.CheckPasswordAsync(user, password);
        }

        public async Task<Chat> GetAssistant(User user)
        {
            await _context.Entry(user).Collection(u => u.Chats).LoadAsync();
            var assistant = user.Chats.FirstOrDefault(c => c.Name == "Assistant");

            if (assistant == null)
            {
                return null;
            }

            await _context.Entry(assistant).Collection(c => c.Messages).LoadAsync();
            return assistant;
        }

        public List<Chat> GetAllChats(string userId)
        {
            return _context.Users
                .Include(u => u.Chats)
                    .ThenInclude(c => c.Users)
                .Include(u => u.Chats)
                    .ThenInclude(c => c.Messages)
                .Include(u => u.Chats)
                    .ThenInclude(c => c.Language)
                .FirstOrDefault(u => u.Id == userId)
                .Chats
                .Where(c => c.Name != "Assistant")
                .ToList();
        }
    }
}