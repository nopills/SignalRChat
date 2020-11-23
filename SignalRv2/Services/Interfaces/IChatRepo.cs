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
        Message GetMessageById(string messageId);
        IQueryable<User> GetAllUsers();
        string HasDialog(string owner, string recipeint);
        Task AddUser(User user);
        Task AddMessage(Message message);
        Task<string> AddDialog(Dialog dialog);

        void SaveChanges();
        Task SaveChangesAsync();
    }
}
