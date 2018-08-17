using Microsoft.Exchange.WebServices.Data;
using SendArchives.Email.Enumerations;
using SendArchives.Logger;
using System;
using System.Collections.Generic;
using System.Threading;

namespace SendArchives.Email
{
    public class EmailService : IEmailService
    {
        private const string emailDomen = "@*****.**";     \\тут ваш почтовый домен

        private CancellationTokenSource cts;

        private ILoggerService _loggerService;
        private ExchangeService service;

        public event EventHandler<SendEmailEventArgs> SendOneItem;

        public ILoggerService LoggerService => _loggerService;

        public EmailService(ILoggerService loggerService)
        {
            _loggerService = loggerService;
        }


        public void CancelSend(Action<Exception> callback)
        {
            cts?.Cancel();
        }

        public void Connect(Action<Exception> callback)
        {
            service = new ExchangeService(ExchangeVersion.Exchange2007_SP1)
            {
                UseDefaultCredentials = true
            };
            service.AutodiscoverUrl(Environment.UserName + emailDomen, RedirectionUrlValidationCallback);

        }

        private static bool RedirectionUrlValidationCallback(string redirectionUrl)
        {
            // The default for the validation callback is to reject the URL.
            bool result = false;

            Uri redirectionUri = new Uri(redirectionUrl);

            // Validate the contents of the redirection URL. In this simple validation
            // callback, the redirection URL is considered valid if it is using HTTPS
            // to encrypt the authentication credentials. 
            if (redirectionUri.Scheme == "https")
            {
                result = true;
            }
            return result;
        }


        public async void SendAsync(Action<Exception> callback, IEnumerable<EmailMessage> collectionMessages, int timePauseSend)
        {
            Exception error = null;
            cts = new CancellationTokenSource();
            await System.Threading.Tasks.Task.Factory.StartNew(()=> Connect(e => { }));
            try
            {
                foreach (var m in collectionMessages)
                {
                    try
                    {
                        cts.Token.ThrowIfCancellationRequested();
                        await SendEmailAsync(e=>
                        {

                        }, m);
                        //Microsoft.Exchange.WebServices.Data.EmailMessage email = new Microsoft.Exchange.WebServices.Data.EmailMessage(service);
                        //email.ToRecipients.AddRange(m.Recipients);
                        //email.Subject = m.Subject;
                        //email.Body = new MessageBody(m.Text);
                        //foreach (var file in m.Attachments)
                        //{
                        //    email.Attachments.AddFileAttachment(file);
                        //}
                        //email.IsDeliveryReceiptRequested = m.CanRequestDelivery;
                        //email.IsReadReceiptRequested = m.CanRequestRead;
                        //await email.SendAndSaveCopy();
                        //SendOneItem?.Invoke(this, new SendEmailEventArgs() { IdEmail = m.IDEmail, StatusMessage = StatusMessage.Send, SendDate = DateTime.Now });
                        await System.Threading.Tasks.Task.Delay(timePauseSend * 1000, cts.Token);
                    }
                    catch (OperationCanceledException ex)
                    {
                        if(m.StatusMessage != StatusMessage.Send && m.StatusMessage != StatusMessage.Error)
                        {
                            SendOneItem?.Invoke(this, new SendEmailEventArgs() { IdEmail = m.IDEmail, StatusMessage = StatusMessage.Cancel, SendDate = DateTime.Now, Message = ex.Message });
                        }
                    }
                    catch (Exception ex)
                    {
                        SendOneItem?.Invoke(this, new SendEmailEventArgs() { IdEmail = m.IDEmail, StatusMessage = StatusMessage.Error, SendDate = DateTime.Now, Message = ex.Message });
                    }
                }
            }
            catch (Exception ex)
            {
                error = ex;
            }
            callback(error);
        }

        public async System.Threading.Tasks.Task SendEmailAsync(Action<Exception> callback, EmailMessage message)
        {
            Microsoft.Exchange.WebServices.Data.EmailMessage email = new Microsoft.Exchange.WebServices.Data.EmailMessage(service);
            email.ToRecipients.AddRange(message.Recipients);
            email.Subject = message.Subject;
            email.Body = new MessageBody(message.Text);
            foreach (var file in message.Attachments)
            {
                email.Attachments.AddFileAttachment(file);
            }
            email.IsDeliveryReceiptRequested = message.CanRequestDelivery;
            email.IsReadReceiptRequested = message.CanRequestRead;
            await email.SendAndSaveCopy();
            SendOneItem?.Invoke(this, new SendEmailEventArgs() { IdEmail = message.IDEmail, StatusMessage = StatusMessage.Send, SendDate = DateTime.Now });
            callback(null);
        }

    }
}
