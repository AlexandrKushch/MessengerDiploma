namespace Server.Data.Models.Repository
{
    public interface IChatRepository
    {
        ValueTask<Chat> Create(Chat chat);

        ValueTask<List<Chat>> GetAll();

        Chat GetById(Guid id);

        Task Update(Chat chat);

        Task Delete(Chat chat);
    }
}