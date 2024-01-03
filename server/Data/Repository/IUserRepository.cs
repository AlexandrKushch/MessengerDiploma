using Microsoft.AspNetCore.Identity;

namespace Server.Data.Models.Repository
{
    public interface IUserRepository
    {
        List<User> GetAll();

        ValueTask<User> GetById(string id);

        ValueTask<User> GetByEmail(string email);

        ValueTask<User> GetByUserName(string userName);

        ValueTask<bool> CheckPassword(User user, string password);

        Task<IdentityResult> Create(User user, string password);

        Task Update(User user);

        Task Delete(User user);

        List<Chat> GetAllChats(string userId);

        Task<Chat> GetAssistant(User user);
    }
}