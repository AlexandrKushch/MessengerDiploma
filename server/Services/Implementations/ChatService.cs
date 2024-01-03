using Server.Data.Models;
using Server.Data.Models.Repository;
using Server.Mappers;
using Server.Models.Request;
using Server.Models.Response;
using Server.Services.Interfaces;

namespace Server.Services.Implementations
{
    public class ChatService : IChatService
    {
        private readonly IChatRepository _chatRepository;

        private readonly IUserRepository _userRepository;

        private readonly IGPTService _gptService;

        public ChatService(IChatRepository chatRepository, IUserRepository userRepository, IGPTService gptService)
        {
            _chatRepository = chatRepository;
            _userRepository = userRepository;
            _gptService = gptService;
        }

        public async Task<ChatGetDto> AddUserAsync(Guid chatId, string userId)
        {
            var user = await _userRepository.GetById(userId);
            var chat = _chatRepository.GetById(chatId);

            chat.Users.Add(user);
            // user.Chats.Add(chat);

            // await _userRepository.Update(user);

            await _chatRepository.Update(chat);
            var chatResponse = ChatMapper.Map(chat);

            return chatResponse;
        }

        public async Task<ChatGetDto> CreateAsync(string name)
        {
            var chat = new Chat()
            {
                Name = name
            };

            chat = await _chatRepository.Create(chat);
            var chatResponse = ChatMapper.Map(chat);

            return chatResponse;
        }

        public async Task<MessageGetDto> SendMessage(string authorId, Guid chatId, MessageCreateDto messageDto)
        {
            var user = await _userRepository.GetById(authorId);
            var chat = _chatRepository.GetById(chatId);

            var message = MessageMapper.Map(messageDto);
            message.User = user;

            chat.Messages.Add(message);
            await _chatRepository.Update(chat);

            return MessageMapper.Map(message);
        }

        public async Task<MessageGetDto> TranslateMessage(Guid chatId, Guid messageId, string language)
        {
            var chat = _chatRepository.GetById(chatId);
            var message = chat.Messages.FirstOrDefault(m => m.Id == messageId);

            await _gptService.Translate(message, language);

            var responseMessage = MessageMapper.Map(message);

            return responseMessage;
        }

        public async Task DeleteAsync(Guid id)
        {
            var chat = _chatRepository.GetById(id);
            await _chatRepository.Delete(chat);
        }

        public List<ChatGetDto> GetAll(string userId)
        {
            var chats = _userRepository.GetAllChats(userId);

            var responses = chats.ConvertAll(c => ChatMapper.Map(c, userId));

            return responses;
        }

        public async Task<ChatGetDto> GetById(string userId, Guid chatId, string language)
        {
            var watch = new System.Diagnostics.Stopwatch();
            watch.Start();
            var chat = _chatRepository.GetById(chatId);

            if (!string.IsNullOrEmpty(language))
            {
                if (!chat.Language.Any(c => c.Language == language))
                {
                    chat.Language.Add(new ChatUserLanguage
                    {
                        User = await _userRepository.GetById(userId),
                        Language = language
                    });
                    await _chatRepository.Update(chat);
                }
                await _gptService.Translate(userId, chat, language);
                watch.Stop();

                Console.WriteLine($"Execution Time Translate Chat ({chat.Messages.Count}): {watch.ElapsedMilliseconds} ms");
            }

            return ChatMapper.Map(chat, userId);
        }
    }
}