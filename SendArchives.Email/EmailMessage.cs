using SendArchives.Email.Enumerations;
using System;
using System.ComponentModel;

namespace SendArchives.Email
{

    public class EmailMessage: INotifyPropertyChanged
    {
        public string[] Recipients { get; set; }
        public string Subject { get; set; }
        public string Text { get; set; }
        public string[] Attachments { get; set; }
        public bool CanRequestDelivery { get; set; }
        public bool CanRequestRead { get; set; }
        public int IDEmail { get; set; }
        public DateTime DateSend { get; set; }
        public StatusMessage StatusMessage { get; set; }
        public string SendMessage { get; set; }
        public bool IsSuccessfulSend { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}