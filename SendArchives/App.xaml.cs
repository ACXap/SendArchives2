using GalaSoft.MvvmLight.Threading;
using SendArchives.Email;
using SendArchives.EmailSignature;
using SendArchives.Files;
using SendArchives.Language;
using SendArchives.Logger;
using SendArchives.ViewModel;
using SendArchives.Zip;
using SendArvhives.Settings;
using System;
using System.Linq;
using System.Windows;

namespace SendArchives
{
    public partial class App : Application
    {
        #region Service

        private readonly ILoggerService _loggerService;
        private readonly ISettingsService _settingsService;
        private readonly ILanguageService _languageService;
        private readonly IFilesService _filesService;
        private readonly IZipService _zipService;
        private readonly IEmailSignatureService _emailSignatureService;
        private readonly IEmailService _emailService;

        #endregion Service initialization

        static App()
        {
            DispatcherHelper.Initialize();
        }

        private App()
        {
            _loggerService = new LoggerService();  ///TODO add variable level of logging

            _loggerService.Info($"+++++ Start application {DateTime.Now} +++++");
            _loggerService.Debug("Start service initialization");

            _settingsService = new SettingsService(_loggerService);
            _languageService = new LanguageService(_loggerService);
            _filesService = new FilesService(_loggerService);
            _zipService = new ZipService(_loggerService);
            _emailSignatureService = new EmailSignatureService(_loggerService);
            _emailService = new EmailService(_loggerService);
            

            _loggerService.Debug("End service initialization");

            _languageService.LanguageChangedEvent += ((sen, ev) => SetLanguage(ev.KeyLanguage));
        }

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            MainWindow mw = CreateMainWindow();
            mw.Show();
        }

        private MainWindow CreateMainWindow()
        {
            Settings set = null;
            string pathSignature = string.Empty;

            _settingsService.LoadSettings((s, e) =>
            {
                set = s;
            });
            pathSignature = set.CanUseSignature ? set.SignaturePath : null;

            MainWindow mw = new MainWindow()
            {
                DataContext = new MainViewModel()
                {
                    LanguageViewModel = new LanguageViewModel(_languageService, set.KeyLanguage),
                    SettingsViewModel = new SettingsViewModel(_settingsService, set),
                    EmailSignatureViewModel = new EmailSignatureViewModel(_emailSignatureService, pathSignature),
                    FilesViewModel = new FilesViewModel(_filesService),
                    ZipViewModel = new ZipViewModel(_zipService),
                    EmailViewModel = new EmailViewModel(_emailService)
                }
            };
            return mw;
        }

        private void SetLanguage(string keyLanguage)
        {
            var dict = Current.Resources.MergedDictionaries.FirstOrDefault(s => s.Source.OriginalString.Contains("LanguagesDictionaries.xaml"));
            if (dict == null)
            {
                throw new Exception("Not found LanguagesDictionaries.xaml in project SendArchive");
            }
            if (dict.MergedDictionaries.Any())
            {
                dict.MergedDictionaries.Clear();
            }
            _languageService.GetDictionaryLang((rd, error) =>
            {
                dict.MergedDictionaries.Add(rd);
            }, keyLanguage);
        }

        private void Application_Exit(object sender, ExitEventArgs e)
        {
            _loggerService.Info($"+++++ End application {DateTime.Now} +++++");
        }
    }
}