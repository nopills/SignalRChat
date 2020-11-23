using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SignalRv2.Services.EmailSender
{
    public interface IEmailSender
    {
        Task SendEmailAsync(EmailMessage message);
    }
}
