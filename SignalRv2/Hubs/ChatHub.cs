using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using SignalRv2.Abstract;
using SignalRv2.Models;
using SignalRv2.Services.Interfaces;
using System;
using System.Threading.Tasks;

namespace SignalRv2.Hubs
{
    public class ChatHub: Hub
    {
        AppSettings _settings;

        IChatService _chatService;
        IChatRepo _chatRepo { get; }


        public ChatHub(AppSettings settings, IChatService chatService, IChatRepo chatRepo)
        {
            _settings = settings;
            _chatService = chatService;
            _chatRepo = chatRepo;
        }

        [Authorize]
        public async Task SendPrivateMessage(string recipientName, string message)
        {
            if (message.Length > _settings.MaxMessageLength)
            {
                throw new HubException(String.Format("Message is too long", _settings.MaxMessageLength));
            }

            string identityName = Context.User.Identity.Name;
            User user = _chatRepo.GetUserByName(identityName);
            User recipient = _chatRepo.GetUserByName(recipientName);
            string dialogId = _chatRepo.HasDialog(Context.User.Identity.Name, recipientName);

            if (dialogId == string.Empty)
            {
                dialogId = await _chatService.CreateDialog(user, recipient, DateTimeOffset.Now);
            }



            await _chatService.AddMessage(user, dialogId, message);


            await Clients.All.SendAsync("RecieveMessage", user.UserName, message);

        }
    }
}
