using System;

namespace SendArchives.Language
{
    public class ChangeLanguageEventArgs : EventArgs
    {
        public string KeyLanguage { get; private set; }
        public string CultureLanguage { get; private set; }
        public ChangeLanguageEventArgs(string keyLanguage, string cultureLanguage)
        {
            KeyLanguage = keyLanguage;
            CultureLanguage = cultureLanguage;
        }
    }
}