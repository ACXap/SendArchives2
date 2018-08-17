using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using SendArchives.Email;
using SendArchives.ViewModel.InterfacesViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

namespace SendArchives.ViewModel
{
    public class EmailViewModel : ViewModelBase, IEmailViewModel
    {
        private IEmailService _emailService;
        public IEmailService EmailService => _emailService;

        public EmailViewModel(IEmailService emailService)
        {
            _emailService = emailService;
        }

        private ObservableCollection<EmailMessage> _collectionMessage;
        public ObservableCollection<EmailMessage> CollectionMessage
        {
            get => _collectionMessage;
            set => Set(ref _collectionMessage, value);
        }

        private string _recipients;
        public string Recipients
        {
            get => _recipients;
            set => Set(ref _recipients, value);
        }
        private string _subjectFirstEmail;
        public string SubjectFirstEmail
        {
            get => _subjectFirstEmail;
            set => Set(ref _subjectFirstEmail, value);
        }
        private string _subjectNextEmail;
        public string SubjectNextEmail
        {
            get => _subjectNextEmail;
            set => Set(ref _subjectNextEmail, value);
        }
        private string _textFistEmail;
        public string TextFirstEmail
        {
            get => _textFistEmail;
            set => Set(ref _textFistEmail, value);
        }
        private string _textNextEmail;
        public string TextNextEmail
        {
            get => _textNextEmail;
            set => Set(ref _textNextEmail, value);
        }
        private string _signature;
        public string Signature
        {
            get => _signature;
            set => Set(ref _signature, value);
        }
        private bool _isStartedSending;
        public bool IsStartedSending
        {
            get => _isStartedSending;
            set => Set(ref _isStartedSending, value);
        }

        private int _idProcessed;
        public int IdProcessed
        {
            get => _idProcessed;
            set => Set("IdProcessed", ref _idProcessed, value);
        }


        private ICommand _commandCancelSend;
        public ICommand CommandCancelSend
        {
            get
            {
                return _commandCancelSend
                    ?? (_commandCancelSend = new RelayCommand(
                    () =>
                    {
                        _emailService.CancelSend(e =>
                        {
                        });
                    },
                    () =>
                    {
                        return IsStartedSending;
                    }));
            }
        }

        private ICommand _commandResending;
        public ICommand CommandResending

        {
            get
            {
                return _commandResending
                    ?? (_commandResending = new RelayCommand<EmailMessage>(
                    e =>
                    {
                        IsStartedSending = true;
                        _emailService.SendOneItem += _emailService_SendOneItem;
                        _emailService.SendEmailAsync(error =>
                        {
                            _emailService.SendOneItem -= _emailService_SendOneItem;
                            IsStartedSending = false;
                        }, e);
                    }));
            }

        }

        public void Send(Action<Exception> callback, EmailSettings es, List<EmailFiles> listFiles, bool isGrouped)
        {
            CreateCollectionMessage(es, listFiles, isGrouped);
            _emailService.SendOneItem += _emailService_SendOneItem;
            _emailService.SendAsync(e =>
            {
                _emailService.SendOneItem -= _emailService_SendOneItem;
                IsStartedSending = false;
            }, CollectionMessage, es.TimeDelaySending);
        }

        private void _emailService_SendOneItem(object sender, SendEmailEventArgs e)
        {
            var m = CollectionMessage.FirstOrDefault(s => s.IDEmail == e.IdEmail);
            m.DateSend = e.SendDate;
            m.StatusMessage = e.StatusMessage;
            m.SendMessage = e.Message;
            IdProcessed = e.IdEmail;
        }

        private void CreateCollectionMessage(EmailSettings es, List<EmailFiles> listFiles, bool isGrouped)
        {

            CollectionMessage = new ObservableCollection<EmailMessage>();
            var recipients = _recipients.Trim().Split(new char[] { ';' });
            var indexEmail = 1;

            if (isGrouped)
            {
                var a = listFiles.GroupBy(s => s.Group);

                foreach (var b in a)
                {
                    CollectionMessage.Add(CreateEmail(es, b.Select(s => s.PathFile).ToArray(), recipients, indexEmail++));
                }
            }
            else
            {
                foreach (var b in listFiles)
                {
                    CollectionMessage.Add(CreateEmail(es, new string[1] { b.PathFile }, recipients, indexEmail++));
                }
            }
        }
        private EmailMessage CreateEmail(EmailSettings es, string[] files, string[] recipients, int id)
        {
            var m = new EmailMessage
            {
                IDEmail = id,
                Recipients = recipients,
                Attachments = files,
                CanRequestDelivery = es.CanRequestDeliveryReport,
                CanRequestRead = es.CanRequestReadReport,
                Subject = SubjectFirstEmail,
                Text = TextFirstEmail,
                DateSend = DateTime.Now,
                StatusMessage = Email.Enumerations.StatusMessage.ReadyToSend
            };
            return m;
        }
    }
}
