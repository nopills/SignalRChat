using System;
using SignalRv2.Models;
using SignalRv2.Services.Interfaces;
using System.Threading.Tasks;

namespace SignalRv2.Services
{
    public class ChatService : IChatService
    {
        IChatRepo _chatRepo;
        public ChatService(IChatRepo chatRepo)
        {
            _chatRepo = chatRepo;
        }
        public async Task<Message> AddMessage(User user, string dialogId, string content)
        {
            var message = new Message
            {
                Content = content,
                When = DateTimeOffset.Now,
                User = user,
                DialogId = dialogId,
                IsRead = false
            };

            await _chatRepo.AddMessage(message);

            return message;
        }

        public async Task ChangeStatus(User user, UserStatus status)
        {
            user.Status = status;
            user.LastActivity = DateTimeOffset.UtcNow;
            await _chatRepo.SaveChangesAsync();
        }

        public async Task<string> CreateDialog(User cretedBy, User reciever, DateTimeOffset dateTime)
        {
            string dialogId = await _chatRepo.AddDialog(new Dialog
            {
                CreatedBy = cretedBy,
                Reciever = reciever,
                CreatedDate = dateTime
            });
            return dialogId;
        }

        public bool IsValidUserInfo(string FName, string LName)
        {
            return (!String.IsNullOrEmpty(FName) && !String.IsNullOrEmpty(LName) && LName.Length > 1 && FName.Length > 1);
        }
    }
}
