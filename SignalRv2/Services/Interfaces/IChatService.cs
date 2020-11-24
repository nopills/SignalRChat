using SignalRv2.Models;
using System;
using System.Threading.Tasks;

namespace SignalRv2.Services.Interfaces
{
    public interface IChatService
    {
        Task ChangeStatus(User user, UserStatus status);
        Task<Message> AddMessage(User user, string dialogId, string content);
        Task<string> CreateDialog(User cretedBy, User reciever, DateTimeOffset dateTime);

    }
}
