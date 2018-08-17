using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using SendArchives.Logger;

namespace SendArvhives.Settings
{
    public class SettingsService : ISettingsService
    {
        private string _nameFolderTemp = @"\Temp";
        private byte _pathFolderTempLength = 200;

        private ILoggerService _loggerService;
        public ILoggerService LoggerService => _loggerService;

        private string _nameFileSettings = "Settings.ini";
        public string NameFileSettings => _nameFileSettings;

        public void GetDefaultSettings(Action<Settings, Exception> callback)
        {
            Exception error = null;

            _loggerService.Debug("Start get default settings");

            Settings set = new Settings()
            {
                KeyLanguage = "ru",
                CanShowTabHelp = false,
                CanChangeTextSubjectNextEmail = false,
                CanUseSignature = false,
                CanUseSignatureOnlyFirstEmail = true,
                SignaturePath = null,
                CanUseIntegralArchiver = false,
                NameArchiveDefault = "Archive",
                SizePartArchive = 5,
                TimeDelaySending = 10,
                CanRequestDeliveryReport = true,
                CanRequestDeliveryReportOnlyFirstEmail = true,
                CanRequestReadReport = true,
                CanRequestReadReportOnlyFirstEmail = true
            };

            string path = string.Empty;

            try
            {
                path = Path.GetDirectoryName(AppDomain.CurrentDomain.BaseDirectory) + _nameFolderTemp;
            }
            catch (Exception ex)
            {
                error = ex;
                _loggerService.Error("", ex);
            }

            if (!string.IsNullOrEmpty(path) && path.Length < _pathFolderTempLength)
            {
                set.ArchiveTempFolder = path;
            }
            else
            {
                set.ArchiveTempFolder = Path.GetTempPath();
                _loggerService.Warn($" Path folder temp for archive installed temp folder current user", null);
            }

            _loggerService.Debug("End get default settings");

            callback(set, error);
        }

        public void LoadSettings(Action<Settings, List<Exception>> callback)
        {
            List<Exception> errors = new List<Exception>();
            Settings set = null;

            _loggerService.Debug("Start load settings");

            if (!File.Exists(NameFileSettings))
            {
                _loggerService.Warn($"File {NameFileSettings} not found", new FileNotFoundException("File not found", NameFileSettings));
            }
            else
            {
                try
                {
                    using (StreamReader reader = new StreamReader(NameFileSettings))
                    {
                        XmlSerializer serializer = new XmlSerializer(typeof(Settings));
                        set = (Settings)serializer.Deserialize(reader);
                    }
                }
                catch (Exception ex)
                {
                    errors.Add(ex);
                    _loggerService.Error("", ex);
                }
            }
            if (set == null)
            {
                GetDefaultSettings((s, e) =>
                {
                    set = s;
                    if (e != null)
                    {
                        errors.Add(e);
                    }
                });
            }

            _loggerService.Debug("End load settings");

            callback(set, errors);
        }

        public void SaveSettings(Action<Exception> callback, Settings settings)
        {
            Exception error = null;

            _loggerService.Debug("Start save settings");

            if (settings != null)
            {
                try
                {
                    using (StreamWriter writer = new StreamWriter(NameFileSettings))
                    {
                        XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
                        ns.Add("", "");
                        XmlSerializer serializer = new XmlSerializer(settings.GetType());
                        serializer.Serialize(writer, settings, ns);
                    }
                }
                catch (Exception ex)
                {
                    error = ex;
                    _loggerService.Error("", ex);
                }
            }
            else
            {
                _loggerService.Error("", new ArgumentNullException(typeof(Settings).FullName, typeof(Settings).FullName + " == null"));
            }

            _loggerService.Debug("End save settings");

            callback(error);
        }

        public SettingsService() { }

        public SettingsService(string pathFileSettings)
        {
            _nameFileSettings = pathFileSettings;
        }

        public SettingsService(ILoggerService loggerService)
        {
            _loggerService = loggerService;
        }
    }
}