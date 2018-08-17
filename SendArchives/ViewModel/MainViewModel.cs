using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using SendArchives.EmailSignature;
using SendArchives.Enumerations;
using SendArchives.Files;
using SendArchives.Language;
using SendArchives.ViewModel.InterfacesViewModel;
using SendArchives.Zip;
using SendArvhives.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SendArchives.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        private ISettingsViewModel _settingsViewModel;
        public ISettingsViewModel SettingsViewModel
        {
            get => _settingsViewModel;
            set => Set(ref _settingsViewModel, value);
        }

        private ILanguageViewModel _languageViewModel;
        public ILanguageViewModel LanguageViewModel
        {
            get => _languageViewModel;
            set => Set(ref _languageViewModel, value);
        }

        private IEmailSignatureViewModel _emailSignatureViewModel;
        public IEmailSignatureViewModel EmailSignatureViewModel
        {
            get => _emailSignatureViewModel;
            set => Set(ref _emailSignatureViewModel, value);
        }

        private IFilesViewModel _filesViewModel;
        public IFilesViewModel FilesViewModel
        {
            get
            {
                return _filesViewModel;
            }
            set
            {
                Set(ref _filesViewModel, value);
            }
        }

        private IZipViewModel _zipViewModel;
        public IZipViewModel ZipViewModel
        {
            get => _zipViewModel;
            set => Set("ZipViewModel", ref _zipViewModel, value);
        }

        private IEmailViewModel _emailViewModel;
        public IEmailViewModel EmailViewModel
        {
            get => _emailViewModel;
            set => Set("EmailViewModel", ref _emailViewModel, value);
        }

        private TabMainWindow _tabIndexSelected;
        public TabMainWindow TabIndexSelected
        {
            get => _tabIndexSelected;
            set
            {
                Set(ref _tabIndexSelected, value);
                switch (value)
                {
                    case TabMainWindow.TabFormEmails:
                        break;
                    case TabMainWindow.TabFormFiles:
                        TabFormFilesOpen();
                        break;
                    case TabMainWindow.TabResult:
                        break;
                    case TabMainWindow.TabSettings:
                        TabSettingsOpen();
                        break;
                    case TabMainWindow.TabHelp:
                        break;
                    default:
                        break;
                }
            }
        }

        private RelayCommand _commandSaveSettings;
        public RelayCommand CommandSaveSettings
        {
            get
            {
                return _commandSaveSettings
                    ?? (_commandSaveSettings = new RelayCommand(
                    () =>
                    {
                        SettingsViewModel.SaveSettings((e) =>
                        {

                        });
                    }));
            }
        }

        private RelayCommand _commandloadDefaultSettings;
        public RelayCommand CommandLoadDefaultSettings
        {
            get
            {
                return _commandloadDefaultSettings
                    ?? (_commandloadDefaultSettings = new RelayCommand(
                    () =>
                    {
                        SettingsViewModel.LoadDefaultSettings((e) =>
                        {
                            if (e == null)
                            {
                                LanguageViewModel.SetLanguage(SettingsViewModel.Settings.KeyLanguage);
                            }
                        });
                    }));
            }
        }

        private RelayCommand _commandGetArchive;
        public RelayCommand CommandGetArchive
        {
            get
            {
                return _commandGetArchive
                    ?? (_commandGetArchive = new RelayCommand(
                    () =>
                    {
                        ZipViewModel.GetArchive((pathArchive, er) =>
                        {
                            if (er == null && !string.IsNullOrEmpty(pathArchive))
                            {
                                FilesViewModel.ReplaceFileOnArchve(pathArchive);
                            }
                        }, FilesViewModel.CollectionFiles.Select(s => s.FullName).ToList());
                    }));
            }
        }

        private RelayCommand _commandSendEmail;
        public RelayCommand CommandSendEmail
        {
            get
            {
                return _commandSendEmail
                    ?? (_commandSendEmail = new RelayCommand(
                    () =>
                    {
                        TabIndexSelected = TabMainWindow.TabResult;
                        _emailViewModel.IsStartedSending = true;

                        var es = new EmailSettings()
                        {
                            CanUseSignature = _settingsViewModel.Settings.CanUseSignature,
                            CanUseSignatureOnlyFirstEmail = _settingsViewModel.Settings.CanUseSignatureOnlyFirstEmail,
                            CanChangeTextSubjectNextEmail = _settingsViewModel.Settings.CanChangeTextSubjectNextEmail,
                            CanRequestDeliveryReport = _settingsViewModel.Settings.CanRequestDeliveryReport,
                            CanRequestDeliveryReportOnlyFirstEmail = _settingsViewModel.Settings.CanRequestDeliveryReportOnlyFirstEmail,
                            CanRequestReadReport = _settingsViewModel.Settings.CanRequestReadReport,
                            CanRequestReadReportOnlyFirstEmail = _settingsViewModel.Settings.CanRequestReadReportOnlyFirstEmail,
                            TimeDelaySending = _settingsViewModel.Settings.TimeDelaySending
                        };

                        List<EmailFiles> listFiles = _filesViewModel.CollectionFiles.Select(p => new EmailFiles()
                        {
                            PathFile = p.FullName,
                            Group = p.Group
                        }).ToList();

                        _emailViewModel.Send(e =>
                        {

                        }, es, listFiles, _filesViewModel.IsGrouped);
                    },
                    () =>
                    {
                        return _filesViewModel.CollectionFiles != null && _filesViewModel.CollectionFiles.Any() && !string.IsNullOrEmpty(_emailViewModel.Recipients);
                    }));
            }
        }

        private void TabSettingsOpen()
        {
            var set = _settingsViewModel.Settings;
            LanguageViewModel.InitializeComponent(set.KeyLanguage);
            EmailSignatureViewModel.InitializeComponent(set.SignaturePath);
        }

        private void TabFormFilesOpen()
        {
            var set = _settingsViewModel.Settings;
            ZipViewModel.InitializeComponent(set.ArchiveTempFolder, set.NameArchiveDefault, set.SizePartArchive);
        }

        public MainViewModel()
        {
            Messenger.Default.Register<PropertyChangedMessage<LanguageInfo>>(this, ActChangedLanguage);
            Messenger.Default.Register<PropertyChangedMessage<EmailSignature.EmailSignature>>(this, ActChangedEmailSignature);
        }

        private void ActChangedEmailSignature(PropertyChangedMessage<EmailSignature.EmailSignature> obj)
        {
            SettingsViewModel.Settings.SignaturePath = EmailSignatureViewModel.CurrentEmailSignature.Path;
        }

        private void ActChangedLanguage(PropertyChangedMessage<LanguageInfo> obj)
        {
            SettingsViewModel.Settings.KeyLanguage = LanguageViewModel.CurrentLanguage.KeyLanguage;
        }

    }
}