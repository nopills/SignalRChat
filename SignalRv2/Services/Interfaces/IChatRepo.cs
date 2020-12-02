using SignalRv2.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SignalRv2.Services.Interfaces
{
    public interface IChatRepo: IDisposable
    {
        Task ChangeUserInfo(User user, string FName, string LName, string AvatarUrl);
        User GetUserById(string userId);
        User GetUserByName(string name);
        //IQueryable<User> GetUsersByStatus(string status);
        IQueryable<Message> GetLastMessages(string dialogId);
        IQueryable<Message> GetUnreadMessages(string dialogId);
        long GetCountUnreadMessages(string dialogId, string senderId);
        Message GetMessageById(string messageId);
        IQueryable<User> GetAllUsers();
        IQueryable<Dialog> GetAllDialogs(User user);
        IQueryable<Dialog> GetLastDialogs(User user);
        Dialog GetDialogById(string Id);
        Dialog HasDialog(string owner, string recipeint);
        Task AddUser(User user);
        Task AddMessage(Message message);
        Task AddDialog(Dialog dialog);

        void SaveChanges();
        Task SaveChangesAsync();
    }
}
