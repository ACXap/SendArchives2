using NUnit.Framework;
using SendArvhives.Settings;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;

namespace SendArchives.Settings.Test
{
    [TestFixture]
    public class SettingsServiceTest
    {
        private ISettingsService _settingsServise;
        private ISettingsService _settingsServiseWithOutFile;
        private ISettingsService _settingsServiseWithFileNameError;
        private ISettingsService _settingsServiseFileError;
        private ISettingsService _settingsServiseFileTestIni;
        private SendArvhives.Settings.Settings _settings;
        private Exception _error;
        private List<Exception> _errors;

        [SetUp]
        public void Init()
        {
            _settingsServise = new SettingsService(TestContext.CurrentContext.TestDirectory + @"\Settings.ini");
            _settingsServiseWithOutFile = new SettingsService(TestContext.CurrentContext.TestDirectory + @"\testtest");
            _settingsServiseWithFileNameError = new SettingsService(TestContext.CurrentContext.TestDirectory + @"\?testtest");
            _settingsServiseFileError = new SettingsService(TestContext.CurrentContext.TestDirectory + @"\SettingsError.ini");
            _settingsServiseFileTestIni = new SettingsService(TestContext.CurrentContext.TestDirectory + @"\SettingsTest.ini");
            _settings = null;
            _error = null;
            _errors = null;
        }

        [Test]
        public void LoadSettings_SettingsRequired()
        {
            _settingsServise.LoadSettings((s, e) =>
            {
                _settings = s;
            });

            Assert.That(_settings, Is.Not.Null);
        }

        [Test]
        public void LoadSettings_NotErrorRequired()
        {
            _settingsServise.LoadSettings((s, e) =>
             {
                 _errors = e;
             });

            Assert.That(_errors.Count, Is.EqualTo(0));
        }

        [Test]
        public void LoadSettings_SettingsRequiredWhihOutFile()
        {
            _settingsServiseWithOutFile.LoadSettings((s, e) =>
            {
                _settings = s;
            });

            Assert.That(_settings, Is.Not.Null);
        }

        [Test]
        public void LoadSettings_ErrorRequiredWhihOutFile()
        {
            _settingsServiseWithOutFile.LoadSettings((s, e) =>
            {
                _errors = e;
            });

            Assert.That(_errors.Count, Is.EqualTo(1));
        }

        [Test]
        public void LoadSettings_ErrorFileNotFoundRequiredWhihOutFile()
        {
            _settingsServiseWithOutFile.LoadSettings((s, e) =>
            {
                _errors = e;
            });

            Assert.That(_errors[0].Message, Is.EqualTo("File not found"));
        }

        [Test]
        public void LoadSettings_SettingsRequiredFileError()
        {
            _settingsServiseFileError.LoadSettings((s, e) =>
            {
                _settings = s;
            });

            Assert.That(_settings, Is.Not.Null);
        }

        [Test]
        public void LoadSettings_ErrorRequiredFileError()
        {
            _settingsServiseFileError.LoadSettings((s, e) =>
            {
                _errors = e;
            });

            Assert.That(_errors.Count, Is.EqualTo(1));
        }

        [Test]
        public void GetDefaultSettings_SettingsRequired()
        {
            _settingsServise.GetDefaultSettings((s, e) =>
            {
                _settings = s;
            });

            Assert.That(EqualsSettings(_settings));
        }

        [Test]
        public void SaveSettings_ErrorNotRequired()
        {
            _settingsServiseFileTestIni.GetDefaultSettings((s, e) =>
            {
                _settings = s;
            });
            _settingsServiseFileTestIni.SaveSettings((e) => 
            {
                _error = e;
            }, _settings);

            Assert.That(_error, Is.Null);
        }

        [Test]
        public void SaveSettings_ErrorRequiredWithSettingNull()
        {
            SendArvhives.Settings.Settings set = null;
            _settingsServiseFileTestIni.SaveSettings((e) => 
            {
                _error = e;
            }, set);

            Assert.That(_error, Is.Not.Null);
        }

        [Test]
        public void SaveSettings_ErrorRequiredWithFileNameError()
        {
            _settingsServiseFileTestIni.GetDefaultSettings((s, e) =>
            {
                _settings = s;
            });
            _settingsServiseWithFileNameError.SaveSettings((e) => 
            {
                _error = e;
            }, _settings);

            Assert.That(_error, Is.Not.Null);
        }

        //Testing of all methods together and required settings not null
        [Test]
        public void AllMethodsClassSettingsService_EqualsRequiredSettings()
        {
            _settingsServiseFileTestIni.GetDefaultSettings((s, e) =>
            {
                _settings = s;
            });
            _settingsServiseFileTestIni.SaveSettings((e) => 
            {
                _error = e;
            }, _settings);
            SendArvhives.Settings.Settings settings = null;
            _settingsServiseFileTestIni.LoadSettings((s, e) =>
            {
                settings = s;
            });

            Assert.That(EqualsSettings(settings));
        }

        //Testing of all methods together and required error null
        [Test]
        public void SaveSettings_EqualsRequiredLoadSettings()
        {
            Exception defSetError = null;
            Exception saveDefSetError = null;
            List<Exception> loadDefSetError = null;
            _settingsServiseFileTestIni.GetDefaultSettings((s, e) =>
            {
                _settings = s;
                defSetError = e;
            });
            _settingsServiseFileTestIni.SaveSettings((e) =>
            {
                saveDefSetError = e;
            }, _settings);
            _settingsServiseFileTestIni.LoadSettings((s, e) =>
            {
                loadDefSetError = e;
            });

            Assert.That(defSetError == null && loadDefSetError.Count == 0 && saveDefSetError == null);
        }

        private bool EqualsSettings(SendArvhives.Settings.Settings settings)
        {
            if (settings == null)
            {
                return false;
            }

            SendArvhives.Settings.Settings set = new SendArvhives.Settings.Settings()
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
                ArchiveTempFolder = Path.GetDirectoryName(AppDomain.CurrentDomain.BaseDirectory) + @"\Temp",
                TimeDelaySending = 10,
                CanRequestDeliveryReport = true,
                CanRequestDeliveryReportOnlyFirstEmail = true,
                CanRequestReadReport = true,
                CanRequestReadReportOnlyFirstEmail = true
            };
            if (settings.GetType() != set.GetType())
            {
                return false;
            }

            return settings.KeyLanguage == set.KeyLanguage &&
            settings.CanShowTabHelp == set.CanShowTabHelp &&
            settings.CanChangeTextSubjectNextEmail == set.CanChangeTextSubjectNextEmail &&
            settings.CanUseSignature == set.CanUseSignature &&
            settings.CanUseSignatureOnlyFirstEmail == set.CanUseSignatureOnlyFirstEmail &&
            settings.SignaturePath == set.SignaturePath &&
            settings.CanUseIntegralArchiver == set.CanUseIntegralArchiver &&
            settings.NameArchiveDefault == set.NameArchiveDefault &&
            settings.SizePartArchive == set.SizePartArchive &&
            settings.ArchiveTempFolder == set.ArchiveTempFolder &&
            settings.TimeDelaySending == set.TimeDelaySending &&
            settings.CanRequestDeliveryReport == set.CanRequestDeliveryReport &&
            settings.CanRequestDeliveryReportOnlyFirstEmail == set.CanRequestDeliveryReportOnlyFirstEmail &&
            settings.CanRequestReadReport == set.CanRequestReadReport &&
            settings.CanRequestReadReportOnlyFirstEmail == set.CanRequestReadReportOnlyFirstEmail;
        }
    }
}