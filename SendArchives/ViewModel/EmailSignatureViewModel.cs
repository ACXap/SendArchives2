using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using SendArchives.EmailSignature;
using SendArchives.ViewModel.InterfacesViewModel;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

namespace SendArchives.ViewModel
{
    public class EmailSignatureViewModel : ViewModelBase, IEmailSignatureViewModel
    {
        private IEmailSignatureService _emailSignatureService;
        public IEmailSignatureService EmailSignatureService => _emailSignatureService;

        private string _currentEmailSignatureText;
        public string CurrentEmailSignatureText
        {
            get => _currentEmailSignatureText;
            set
            {
                Set(ref _currentEmailSignatureText, value);
            }
        }

        private EmailSignature.EmailSignature _currentEmailSignature;
        public EmailSignature.EmailSignature CurrentEmailSignature
        {
            get => _currentEmailSignature;
            set
            {
                if (value != null)
                {
                    _emailSignatureService.GetEmailSignatureText((str, error) =>
                    {
                        CurrentEmailSignatureText = str;
                    }, value.Path);
                }
                else
                {
                    CurrentEmailSignatureText = null;
                }
                Set(ref _currentEmailSignature, value, true);
            }
        }

        private string _newEmailSignatureText;
        public string NewEmailSignatureText
        {
            get => _newEmailSignatureText;
            set
            {
                Set(ref _newEmailSignatureText, value);
            }
        }

        private EmailSignature.EmailSignature _newEmailSignature;
        public EmailSignature.EmailSignature NewEmailSignature
        {
            get => _newEmailSignature;
            set
            {
                Set(ref _newEmailSignature, value);
                NewEmailSignatureText = null;
            }
        }

        private ObservableCollection<EmailSignature.EmailSignature> _collectionEmailSignature;
        public ObservableCollection<EmailSignature.EmailSignature> CollectionEmailSignature
        {
            get => _collectionEmailSignature;
            set
            {
                Set(ref _collectionEmailSignature, value);
            }
        }

        private ICommand _createEmailSignature;
        public ICommand CommandCreateEmailSignature
        {
            get
            {
                return _createEmailSignature
                    ?? (_createEmailSignature = new RelayCommand(
                    () =>
                    {
                        NewEmailSignature = new EmailSignature.EmailSignature();
                    }));
            }
        }

        private ICommand _saveEmailSignature;
        public ICommand CommandSaveEmailSignature
        {
            get
            {
                return _saveEmailSignature
                    ?? (_saveEmailSignature = new RelayCommand(
                    () =>
                    {
                        if (string.IsNullOrEmpty(NewEmailSignature.Name))
                        {
                            return;
                        }
                        NewEmailSignature.Name += _emailSignatureService.ExtensionFileSignature;
                        NewEmailSignature.Path = _emailSignatureService.PathSignatureTheir + NewEmailSignature.Name;
                        NewEmailSignature.TypeSignature = EmailSignature.Enumerations.TypeSignature.Their;

                        _emailSignatureService.SaveEmailSignature((error) =>
                        {

                        }, NewEmailSignature, NewEmailSignatureText);
                        CollectionEmailSignature.Add(NewEmailSignature);
                        NewEmailSignature = null;
                    }, () =>
                    {
                        return !string.IsNullOrEmpty(NewEmailSignature?.Name);
                    }));
            }
        }

        private ICommand _closeEmailSignature;
        public ICommand CommandCloseEmailSignature
        {
            get
            {
                return _closeEmailSignature
                    ?? (_closeEmailSignature = new RelayCommand(
                    () =>
                    {
                        NewEmailSignature = null;
                    }));
            }
        }

        private ICommand _removeEmailSignature;
        public ICommand CommandRemoveEmailSignature
        {
            get
            {
                return _removeEmailSignature
                    ?? (_removeEmailSignature = new RelayCommand(
                    () =>
                    {
                        Exception error = null;
                        _emailSignatureService.RemoveEmailSignature((e) =>
                        {
                            error = e;
                        }, CurrentEmailSignature);
                        if (error == null)
                        {
                            CollectionEmailSignature.Remove(CurrentEmailSignature);
                            if (CollectionEmailSignature.Any())
                            {
                                CurrentEmailSignature = CollectionEmailSignature.First();
                            }
                        }
                    }, () =>
                     {
                         return CurrentEmailSignature?.TypeSignature != EmailSignature.Enumerations.TypeSignature.Outlook;
                     }));
            }
        }

        public EmailSignatureViewModel(IEmailSignatureService emailSignatureService, string signaturePath)
        {
            _emailSignatureService = emailSignatureService;
            if (!string.IsNullOrEmpty(signaturePath))
            {
                _emailSignatureService.GetEmailSignatureText((str, error) =>
                {
                    CurrentEmailSignatureText = str;
                }, signaturePath);
            }
        }

        public void InitializeComponent(string signaturePath)
        {
            if (CollectionEmailSignature == null)
            {
                _emailSignatureService.GetAllEmailSignature((list, error) =>
                {
                    CollectionEmailSignature = new ObservableCollection<EmailSignature.EmailSignature>(list);
                });
                CurrentEmailSignature = CollectionEmailSignature.FirstOrDefault(s => s.Path == signaturePath);
            }
        }
    }
}