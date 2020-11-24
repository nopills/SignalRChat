using Microsoft.EntityFrameworkCore;
using SignalRv2.Models;
using SignalRv2.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SignalRv2.Services
{
    public class ChatRepo: IChatRepo
    {
        DatabaseContext _db;
        public ChatRepo(DatabaseContext db)
        {
            _db = db;
        }       

        public async Task<string> AddDialog(Dialog dialog)
        {
            await _db.Dialogs.AddAsync(dialog);
            await _db.SaveChangesAsync();
            return dialog.Id;
        }

        public async Task AddMessage(Message message)
        {
            await _db.Messages.AddAsync(message);
            await _db.SaveChangesAsync();
        }

        public async Task AddUser(User user)
        {
            await _db.Users.AddAsync(user);
            await _db.SaveChangesAsync();
        }

        public void Dispose()
        {
            _db.Dispose();
        }

        public IQueryable<User> GetAllUsers()
        {
            return _db.Users;
        }

        public Message GetMessageById(string messageId)
        {
            return _db.Messages.FirstOrDefault(m => m.Id == messageId);
        }

        public User GetUserById(string userId)
        {
            return _db.Users.FirstOrDefault(u => u.Id == userId);
        }

        public User GetUserByName(string name)
        {
            return _db.Users.FirstOrDefault(n => n.UserName == name);
        }

        public void SaveChanges()
        {
            _db.SaveChanges();
        }

        public async Task SaveChangesAsync()
        {
            await _db.SaveChangesAsync();
        }      

        public string HasDialog(string owner, string recipeint)
        {
            Dialog dialog = _db.Dialogs.FirstOrDefault(x => x.CreatedBy.UserName == owner && x.Reciever.UserName == recipeint);
            if (dialog == null)
            {
                dialog = _db.Dialogs.FirstOrDefault(x => x.CreatedBy.UserName == recipeint && x.Reciever.UserName == owner);
                if (dialog == null)
                {
                    return String.Empty;
                }
            }
            return dialog.Id;
        }

        public IQueryable<Message> GetLastMessages(string dialogId)
        {
            return _db.Messages.Where(d => d.DialogId == dialogId).OrderBy(x => x.When).Take(100);
        }
    }
}