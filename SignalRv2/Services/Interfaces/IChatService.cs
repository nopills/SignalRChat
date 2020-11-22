using SignalRv2.Models;
using System;
using System.Threading.Tasks;

namespace SignalRv2.Services
{
    public interface IChatService
    {
        Task<Message> AddMessage(ChatClient chatClient, string content, long dialogId);
        Task<string> CreateDialog(User cretedBy, User reciever, DateTimeOffset dateTime);
    }
}
