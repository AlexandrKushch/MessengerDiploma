using Microsoft.EntityFrameworkCore;

namespace Server.Data.Models.Repository
{
    public class ChatRepository : IChatRepository
    {
        private readonly MessengerContext _context;

        private readonly DbSet<Chat> _model;

        public ChatRepository(MessengerContext context)
        {
            _context = context;
            _model = _context.Chats;
        }

        public async ValueTask<Chat> Create(Chat chat)
        {
            await _model.AddAsync(chat);
            await _context.SaveChangesAsync();
            return chat;
        }

        public async Task Delete(Chat chat)
        {
            _model.Remove(chat);
            await _context.SaveChangesAsync();
        }

        public async ValueTask<List<Chat>> GetAll()
        {
            return await _model
                .Where(c => c.Name != "Assistant")
                .ToListAsync();
        }

        public Chat GetById(Guid id)
        {
            return _model
                .Include(c => c.Users)
                .Include(c => c.Messages)
                .Include(c => c.Language)
                .FirstOrDefault(c => c.Id == id);
        }

        public async Task Update(Chat chat)
        {
            _model.Update(chat);
            await _context.SaveChangesAsync();
        }
    }
}