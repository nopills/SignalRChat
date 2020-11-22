﻿using System;
using SignalRv2.Models;
using SignalRv2.Services.Interfaces;
using System.Threading.Tasks;

namespace SignalRv2.Services
{
    public class ChatService : IChatService
    {
        IChatRepo _chatRepo;
        public ChatService(IChatRepo chatRepo)
        {
            _chatRepo = chatRepo;
        }
        public async Task<Message> AddMessage(ChatClient chatClient, string content, long dialogId)
        {
            var message = new Message
            {
                Content = content,
                When = DateTimeOffset.Now,
                ChatClient = chatClient,       
                DialogId = dialogId
            };

            await _chatRepo.AddMessage(message);

            return message;
        }

        public async Task<string> CreateDialog(User cretedBy, User reciever, DateTimeOffset dateTime)
        {
            string dialogId = await _chatRepo.AddDialog(new Dialog
            {
                CreatedBy = cretedBy,
                Reciever = reciever,
                CreatedDate = dateTime
            });
            return dialogId;
        }
    }
}
