using SignalRv2.Models;
using System;
using System.Threading.Tasks;

namespace SignalRv2.Services.Interfaces
{
    public interface IChatService
    {
        Task ChangeStatus(User user, UserStatus status);
        Task<Message> AddMessage(User user, Dialog dialog, string content);
        Task<Dialog> CreateDialog(User createdBy, User reciever, string message); 
        bool IsValidUserInfo(string FName, string LName);
    }
}
