using SendArchives.Email;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace SendArchives.ViewModel.InterfacesViewModel
{
    public interface IEmailViewModel
    {
        IEmailService EmailService { get; }

        bool IsStartedSending { get; set; }
        ObservableCollection<EmailMessage> CollectionMessage { get; set; }
        string Recipients { get; set; }
        string SubjectFirstEmail { get; set; }
        string SubjectNextEmail { get; set; }
        string TextFirstEmail { get; set; }
        string TextNextEmail { get; set; }
        string Signature { get; set; }

        ICommand CommandCancelSend { get; }
        ICommand CommandResending { get; }
        void Send(Action<Exception> callback, EmailSettings es, List<EmailFiles> listFiles, bool isGrouped);

    }
}
