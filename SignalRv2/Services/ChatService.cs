using System;
using SignalRv2.Models;
using SignalRv2.Services.Interfaces;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace SignalRv2.Services
{
    public class ChatService : IChatService
    {
        IChatRepo _chatRepo;
        public ChatService(IChatRepo chatRepo)
        {
            _chatRepo = chatRepo;
        }
        public async Task<Message> AddMessage(User user, Dialog dialog, string content)
        {
            var message = new Message
            {
                Content = content,
                When = DateTimeOffset.Now,
                User = user,
                Dialog = dialog,
                IsRead = false
            };
           
            dialog.LastMessage = content;

            await _chatRepo.AddMessage(message);

            return message;
        }

        public async Task ChangeStatus(User user, UserStatus status)
        {
            user.Status = status;
            user.LastActivity = DateTimeOffset.UtcNow;
            await _chatRepo.SaveChangesAsync();
        }

        public async Task<Dialog> CreateDialog(User createdBy, User reciever, string message)
        {
            Dialog dialog = new Dialog
            {
                CreatedBy = createdBy,
                Reciever = reciever,
                CreatedDate = DateTimeOffset.Now,
                LastActivity = DateTimeOffset.Now,
                LastMessage = message
            };
            await _chatRepo.AddDialog(dialog);
            return dialog;
        }

      

        public bool IsValidUserInfo(string FName, string LName)
        {
            return (!String.IsNullOrEmpty(FName) && !String.IsNullOrEmpty(LName) && LName.Length > 1 && FName.Length > 1);
        }
    }
}
