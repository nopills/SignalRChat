using SignalRv2.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SignalRv2.Services.Interfaces
{
    public interface IChatRepo: IDisposable
    {
        User GetUserById(string userId);
        User GetUserByName(string name);
        IQueryable<Message> GetLastMessages(string dialogId);
        Message GetMessageById(string messageId);
        IQueryable<User> GetAllUsers();
        IQueryable<Dialog> GetAllDialogs(User user);
        string HasDialog(string owner, string recipeint);
        Task AddUser(User user);
        Task AddMessage(Message message);
        Task<string> AddDialog(Dialog dialog);

        void SaveChanges();
        Task SaveChangesAsync();
    }
}
