using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using SignalRv2.Abstract;
using SignalRv2.Models;
using SignalRv2.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SignalRv2.Hubs
{
    [Authorize]
    public class ChatHub: Hub
    {
        AppSettings _settings;
        IChatService _chatService;
        IChatRepo _chatRepo;


        public ChatHub(AppSettings settings, IChatService chatService, IChatRepo chatRepo)
        {
            _settings = settings;
            _chatService = chatService;
            _chatRepo = chatRepo;         
        }

        public override async Task OnConnectedAsync()
        {
            User currentUser = _chatRepo.GetUserByName(Context.User.Identity.Name);
            await _chatService.ChangeStatus(currentUser, UserStatus.Recently);
            await base.OnConnectedAsync();         
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            User currentUser = _chatRepo.GetUserByName(Context.User.Identity.Name);
            await _chatService.ChangeStatus(currentUser, UserStatus.Offline);
            await base.OnDisconnectedAsync(exception);
        }

        public async Task MarkAsRead(string[] messageId)
        {
            foreach(var id in messageId)
            {
                _chatRepo.GetMessageById(id).IsRead = true;
            }
            await _chatRepo.SaveChangesAsync();
        }

        public async Task Typing()
        {
            string identityName = Context.User.Identity.Name;
            await Clients.All.SendAsync("Typing", $"{identityName} is typing...");
        }

        public async Task SendPrivateMessage(string recipientName, string message)
        {
            if (message.Length > _settings.MaxMessageLength)
            {
                throw new HubException(String.Format("Message is too long", _settings.MaxMessageLength));
            }

            string identityName = Context.User.Identity.Name;
           
            User currentUser = _chatRepo.GetUserByName(identityName);
            User recipient = _chatRepo.GetUserByName(recipientName);

            string dialogId = _chatRepo.HasDialog(identityName, recipientName);

            if (dialogId == string.Empty)
            {
                dialogId = await _chatService.CreateDialog(currentUser, recipient, DateTimeOffset.Now);
            }

            await _chatService.ChangeStatus(currentUser, UserStatus.Online);
            await _chatService.AddMessage(currentUser, dialogId, message);            
            await Clients.All.SendAsync("RecieveMessage", identityName,  message);
        }

        public async Task<IEnumerable<Message>> GetLastMessages(string dialogId)
        {
            return await _chatRepo.GetLastMessages(dialogId).ToListAsync();
        }

    }
}
