using Microsoft.AspNetCore.Identity;
using OpenAI_API;
using OpenAI_API.Chat;
using Server.Data.Models;
using Server.Data.Models.Repository;
using Server.Mappers;
using Server.Models.Response;
using Server.Services.Interfaces;

namespace Server.Services.Implementations
{
    public class GPTService : IGPTService
    {
        private readonly OpenAIAPI _api;

        private readonly IUserRepository _userRepository;

        private readonly IChatRepository _chatRepository;

        public GPTService(IUserRepository userRepository, IChatRepository chatRepository)
        {
            var token = "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";
            _api = new OpenAIAPI(token);
            _userRepository = userRepository;
            _chatRepository = chatRepository;
        }

        public async Task<List<MessageGetDto>> GetHistory(string userId)
        {
            var user = await _userRepository.GetById(userId);
            var assistantChat = await _userRepository.GetAssistant(user);

            return assistantChat?.Messages?.ToList().ConvertAll(m => MessageMapper.Map(m)) ?? new List<MessageGetDto>();
        }

        public void SetInfo(string info)
        {
            File.WriteAllText("company-info.txt", info);
        }

        public string GetInfo()
        {
            return File.ReadAllText("company-info.txt");
        }

        public async Task<MessageGetDto> Ask(string userId, string question)
        {
            var user = await _userRepository.GetById(userId);
            var assistantChat = await _userRepository.GetAssistant(user);

            if (user == null)
            {
                throw new Exception("User is not found.");
            }

            if (assistantChat == null)
            {
                assistantChat = await _chatRepository.Create(new Chat { Name = "Assistant", Users = new List<User> { user } });
            }

            var chat = _api.Chat.CreateConversation();

            var comanyInfo = GetInfo();
            chat.AppendSystemMessage($"You are assistant of company. Your task is to help workers with their questions. Answer please in 255 symbols. There is information about company:\n{comanyInfo}");
            var messages = assistantChat.Messages.ToList();
            for (int i = 0; i < messages.Count; i += 2)
            {
                chat.AppendUserInput(messages[i].Content);
                chat.AppendExampleChatbotOutput(messages[i + 1].Content);
            }

            chat.AppendUserInput(question);
            var answer = await MakeRequest(chat);

            assistantChat.Messages.Add(new Message { User = user, Content = question });
            assistantChat.Messages.Add(new Message { Content = answer });
            await _chatRepository.Update(assistantChat);

            return MessageMapper.Map(assistantChat.Messages.Last());
        }

        public async Task Translate(string currentUserId, Chat chat, string language)
        {
            var messagesToTranslate = chat.Messages.Where(m => m.User.Id != currentUserId).ToList();

            if (messagesToTranslate.Count > 0)
            {
                var chatGPT = _api.Chat.CreateConversation();
                chatGPT.AppendSystemMessage($"Translate all these lines to language: {language}. There may be different languages, but they separated by '\n' symbol.");
                var message = "";

                foreach (var toTranslate in messagesToTranslate)
                {
                    message += $"{toTranslate.Content}\n";
                }

                chatGPT.AppendUserInput(message);
                var response = await MakeRequest(chatGPT);

                var responses = response.Split("\n");

                for (int i = 0; i < messagesToTranslate.Count; i++)
                {
                    messagesToTranslate[i].Content = responses[i];
                }
            }
        }

        public async Task Translate(Message message, string language)
        {
            var chatGPT = _api.Chat.CreateConversation();
            var msg = $"Translate this line to language: {language}";

            msg += $"\n{message.Content}";

            chatGPT.AppendUserInput(msg);
            var response = await MakeRequest(chatGPT);

            message.Content = response;
        }

        private async Task<string> MakeRequest(Conversation chatGPT)
        {
            try
            {
                var response = await chatGPT.GetResponseFromChatbotAsync();
                return response;
            }
            catch (Exception)
            {
                Console.WriteLine("***Rerequested***");
                await Task.Delay(20000);
                var response = await MakeRequest(chatGPT);
                return response;
            }
        }
    }
}