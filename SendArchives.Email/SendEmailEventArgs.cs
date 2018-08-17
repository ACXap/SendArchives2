using SendArchives.Email.Enumerations;
using System;

namespace SendArchives.Email
{
    public class SendEmailEventArgs : EventArgs
    {
        public int IdEmail { get; set; }
        public DateTime SendDate { get; set; }
        public StatusMessage StatusMessage { get; set; }
        public string Message { get; set; }
    }
}