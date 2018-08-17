using SendArchives.Logger;
using System;
using System.Collections.Generic;

namespace SendArchives.Email
{
    public interface IEmailService
    {
        ILoggerService LoggerService { get; }

        event EventHandler<SendEmailEventArgs> SendOneItem;

        void Connect(Action<Exception> callback);

        void SendAsync(Action<Exception> callback, IEnumerable<EmailMessage> collectionMessages, int timePauseSend);
        System.Threading.Tasks.Task SendEmailAsync(Action<Exception> callback, EmailMessage message);
        void CancelSend(Action<Exception> callback);
    }
}