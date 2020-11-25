using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using SignalRv2.Abstract;
using SignalRv2.Models;
using SignalRv2.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;

namespace SignalRv2.Hubs
{
    [Authorize]
    public class ChatHub: Hub
    {
        AppSettings _settings;
        IChatService _chatService;
        IChatRepo _chatRepo;
        private readonly IUserIdProvider _userProvider;

        public ChatHub(AppSettings settings, IChatService chatService, IChatRepo chatRepo)
        {
            _settings = settings;
            _chatService = chatService;
            _chatRepo = chatRepo;    
        }

        public override async Task OnConnectedAsync()
        {
            User currentUser = _chatRepo.GetUserByName(Context.User.Identity.Name);
            await SendStatus(UserStatus.Recently);
            await base.OnConnectedAsync();         
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            User currentUser = _chatRepo.GetUserByName(Context.User.Identity.Name);
            await SendStatus(UserStatus.Offline);
            await base.OnDisconnectedAsync(exception);
        }

        public async Task SendStatus(UserStatus status)
        {
            User currentUser = _chatRepo.GetUserByName(Context.User.Identity.Name);          
            //int minFromLastActivity = Convert.ToInt32((DateTimeOffset.UtcNow - currentUser.LastActivity).ToString("mm"));
            //if(minFromLastActivity > 5)
            //{

            //}
            await _chatService.ChangeStatus(currentUser, status);
            await Clients.All.SendAsync("CheckStatus", Context.User.Identity.Name, status.ToString());
        }

        //public async Task CheckUserStatus()
        //{
        //    User currentUser = _chatRepo.GetUserByName(Context.User.Identity.Name);
        //    IEnumerable<Dialog> dialogs = await _chatRepo.GetAllDialogs(currentUser).ToListAsync();
       
        //}

        public async Task MarkAsRead(string[] messageId)
        {
            foreach(var id in messageId)
            {
                _chatRepo.GetMessageById(id).IsRead = true;
            }
            await _chatRepo.SaveChangesAsync();
        }

        public async Task Typing(string recipient)
        {
            string identityName = Context.User.Identity.Name;          
            await Clients.User(recipient).SendAsync("Typing", $"{identityName} is typing...");
        }

        public async Task SendPrivateMessage(string recipientName, string message)
        {
            string identityName = Context.User.Identity.Name;
            if (recipientName != identityName)
            {
                
                if (message.Length > _settings.MaxMessageLength)
                {
                    throw new HubException(String.Format("Message is too long", _settings.MaxMessageLength));
                }



                User currentUser = _chatRepo.GetUserByName(identityName);
                User recipient = _chatRepo.GetUserByName(recipientName);

                string dialogId = _chatRepo.HasDialog(identityName, recipientName);

                if (dialogId == string.Empty)
                {
                    dialogId = await _chatService.CreateDialog(currentUser, recipient, DateTimeOffset.Now);
                }

                if (currentUser.Status != UserStatus.Online)
                {
                    await SendStatus(UserStatus.Online);
                }

                await Clients.User(recipientName).SendAsync("RecieveMessage", identityName, message);
                await _chatService.AddMessage(currentUser, dialogId, message);



                // await Clients.Client(Context.ConnectionId).SendAsync("RecieveMessage", message);
                //  await Clients.All.SendAsync("RecieveMessage", Context.UserIdentifier,  message);
            }
        }

        public async Task<IEnumerable<Message>> GetLastMessages(string dialogId)
        {
            return await _chatRepo.GetLastMessages(dialogId).ToListAsync();
        }

    }
}
