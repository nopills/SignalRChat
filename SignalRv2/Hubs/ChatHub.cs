using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using SignalRv2.Abstract;
using SignalRv2.Models;
using SignalRv2.Models.ViewModels;
using SignalRv2.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
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
        IEnumerable<DialogViewModel> dialogs;


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
            if (dialogs != null)
            {
                await CheckUserStatus(dialogs, currentUser);
            }          
            await base.OnDisconnectedAsync(exception);
        }

        public async Task SendStatus(UserStatus status)
        {           
            User currentUser = _chatRepo.GetUserByName(Context.User.Identity.Name);               
            await _chatService.ChangeStatus(currentUser, status);        
        }

        private async Task CheckUserStatus(IEnumerable<DialogViewModel> dialogs, User user)
        {
            foreach (var u in dialogs)
            {
                if (u.Status == UserStatus.Online.ToString() || u.Status == UserStatus.Recently.ToString())
                {
                    await Clients.User(u.RecieverUsername).SendAsync("CheckUserStatus", Context.User.Identity.Name, user.Status.ToString());

                }
            }

        }

        public async Task MarkAsRead(string[] messageId)
        {
            if(messageId != null)
            {
                foreach (var id in messageId)
                {
                    _chatRepo.GetMessageById(id).IsRead = true;
                }
                await _chatRepo.SaveChangesAsync();
            }
        }

        public async Task Typing(string recipient)
        {
            string identityName = Context.User.Identity.Name;          
            await Clients.User(recipient).SendAsync("Typing", $"{identityName} is typing...");
        }

        public async Task<bool> SendPrivateMessage(string recipientName, string message)
        {
            string identityName = Context.User.Identity.Name;
           
            if (recipientName != identityName && !String.IsNullOrEmpty(recipientName))
            {              
                if (message.Length > _settings.MaxMessageLength)
                {
                    throw new HubException(String.Format("Message is too long", _settings.MaxMessageLength));
                }

                if (message.Length <= 0)
                {
                    throw new HubException(String.Format("Message is too low", _settings.MaxMessageLength));
                }


                User currentUser = _chatRepo.GetUserByName(identityName);                
                User recipient = _chatRepo.GetUserByName(recipientName);

                if(recipient != null)
                {
                    Dialog dialog = _chatRepo.HasDialog(identityName, recipientName);

                    if (dialog == null)
                    {
                        dialog = await _chatService.CreateDialog(currentUser, recipient, message);
                    }
                 
                    
                    if (currentUser.Status != UserStatus.Online)
                    {
                        await SendStatus(UserStatus.Online);
                    }

                    
                    await Clients.User(recipientName).SendAsync("RecieveMessage", Context.User.Identity.FullName(), identityName, message);
                    await _chatService.AddMessage(currentUser, dialog, message);
                    return true;
                }
                return false;
            }
            return false;
        }

        public async Task<IEnumerable<Message>> GetLastMessages(string dialogId)
        {
            return await _chatRepo.GetLastMessages(dialogId).ToListAsync();
        }


        public async Task<IEnumerable<DialogViewModel>> GetDialogs()
        {
            string identityName = Context.User.Identity.Name;
            User user = _chatRepo.GetUserByName(identityName);
            if (user != null)
            {
                List<Dialog> lastDialogs = await _chatRepo.GetLastDialogs(user).ToListAsync();
                List<DialogViewModel> dialogViewModels = lastDialogs.Select(c => c.CreatedBy == user ?
                     new DialogViewModel
                     {
                         DialogId = c.Id,
                         LastActivity = c.LastActivity.ToString("hh:mm tt", new CultureInfo("en-US")),
                         LastMessage = c.LastMessage,
                         SenderId = c.CreatedById,
                         RecieverName = String.Format("{0} {1}", c.Reciever.FirstName, c.Reciever.LastName),    
                         RecieverUsername = c.Reciever.UserName,
                         UnreadMessage = _chatRepo.GetCountUnreadMessages(c.Id, user.Id),
                         Status = c.Reciever.Status.ToString()
                     } :
                    new DialogViewModel
                    {
                        DialogId = c.Id,
                        LastActivity = c.LastActivity.ToString("hh:mm tt", new CultureInfo("en-US")),
                        LastMessage = c.LastMessage,
                        SenderId = c.RecieverId,
                        RecieverName = String.Format("{0} {1}", c.CreatedBy.FirstName, c.CreatedBy.LastName),
                        RecieverUsername = c.CreatedBy.UserName,
                        UnreadMessage = _chatRepo.GetCountUnreadMessages(c.Id, user.Id),
                        Status = c.CreatedBy.Status.ToString()
                    }
                    ).ToList();
       

                if(dialogViewModels != null)
                {
                    dialogs = dialogViewModels;
                    await CheckUserStatus(dialogViewModels, user);
                    return dialogViewModels;
                }

            }
            return null;
        }


        public async Task<IEnumerable<Message>> GetMessages(string dialogId)
        {
            
            return await _chatRepo.GetLastMessages(dialogId).ToListAsync();
        }
    }
}
