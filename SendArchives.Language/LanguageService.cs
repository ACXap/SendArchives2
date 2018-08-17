using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows;
using SendArchives.Logger;

namespace SendArchives.Language
{
    public class LanguageService : ILanguageService
    {
        private string _folderLanguages = Directory.GetCurrentDirectory() + @"\Language\";

        private ILoggerService _loggerService;
        public ILoggerService LoggerService => _loggerService;

        public event EventHandler<ChangeLanguageEventArgs> LanguageChangedEvent;
        public string KeyLanguageDefoult => "ru";

        public void GetCollectionDictionaryLang(Action<List<LanguageInfo>, List<Exception>> callback)
        {
            List<Exception> errors = new List<Exception>();
            List<LanguageInfo> collectionLang = null;

            if (!Directory.Exists(_folderLanguages))
            {
                errors.Add(new DirectoryNotFoundException($"Directory {_folderLanguages} not found"));
            }
            else
            {
                try
                {
                    var filesLanguage = Directory.GetFiles(_folderLanguages, "*.lng");
                    var countFilesLanguage = filesLanguage.Length;
                    if (countFilesLanguage == 0)
                    {
                        errors.Add(new Exception("Folder Languages is empty"));
                    }
                    else
                    {
                        collectionLang = new List<LanguageInfo>(countFilesLanguage);
                        foreach (var file in filesLanguage)
                        {
                            if (File.Exists(file))
                            {
                                try
                                {
                                    using (StreamReader sr = new StreamReader(file, Encoding.Default))
                                    {
                                        string line;
                                        bool isNameLanguage = false;
                                        bool isKeyLanguage = false;
                                        LanguageInfo languageInfo = new LanguageInfo();
                                        while ((line = sr.ReadLine()) != null)
                                        {
                                            string[] keyValue = line.Split(new char[] { '=' }, 2);
                                            if (keyValue[0] == "NameLanguage")
                                            {
                                                languageInfo.NameLanguage = keyValue[1];
                                                isNameLanguage = true;
                                                continue;
                                            }
                                            if (keyValue[0] == "KeyLanguage")
                                            {
                                                languageInfo.KeyLanguage = keyValue[1];
                                                isKeyLanguage = true;
                                                continue;
                                            }
                                            if (isNameLanguage && isKeyLanguage)
                                            {
                                                break;
                                            }
                                        }
                                        if (!string.IsNullOrEmpty(languageInfo.KeyLanguage))
                                        {
                                            var pathFlag = _folderLanguages + @"Flag\" + languageInfo.KeyLanguage + ".png";
                                            if (File.Exists(pathFlag))
                                            {
                                                languageInfo.FlagLanguage = pathFlag;
                                            }
                                        }
                                        collectionLang.Add(languageInfo);
                                    }
                                }
                                catch (Exception ex)
                                {
                                    errors.Add(ex);
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    errors.Add(ex);
                }
            }

            callback(collectionLang, errors);
        }
        public void GetDictionaryLang(Action<ResourceDictionary, Exception> callback, string keyLanguage)
        {
            Exception error = null;
            ResourceDictionary rd = new ResourceDictionary();

            if (string.IsNullOrEmpty(keyLanguage))
            {
                keyLanguage = KeyLanguageDefoult;
            }
            string pathLanguage = _folderLanguages + keyLanguage + ".lng";
            if (File.Exists(pathLanguage))
            {
                try
                {
                    using (StreamReader sr = new StreamReader(pathLanguage, Encoding.Default))
                    {
                        string line;
                        while ((line = sr.ReadLine()) != null)
                        {
                            string[] keyValue = line.Split(new char[] { '=' }, 2);
                            rd.Add(keyValue[0], keyValue[1].Replace("\\r\\n", Environment.NewLine));
                        }
                    }
                }
                catch (Exception ex)
                {
                    error = ex;
                }
            }
            else
            {
                error = new FileNotFoundException("File not found", pathLanguage);
            }

            callback(rd, error);
        }
        public void LanguageChanged(string keyLanguage)
        {
            LanguageChangedEvent?.Invoke(this, new ChangeLanguageEventArgs(keyLanguage, null));
        }

        public LanguageService(ILoggerService loggerService)
        {
            _loggerService = loggerService;
        }
    }
}