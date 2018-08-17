using SendArchives.Logger;
using System;
using System.Collections.Generic;
using System.Windows;

namespace SendArchives.Language
{
    /// <summary>
    /// Describes the interface with working with the languages
    /// </summary>
    public interface ILanguageService
    {
        ILoggerService LoggerService { get; } ///TODO add description

        /// <summary>
        /// Language Change Event
        /// </summary>
        event EventHandler<ChangeLanguageEventArgs> LanguageChangedEvent;
        /// <summary>
        /// Contains information about the default language
        /// </summary>
        /// <returns>Default language</returns>
        string KeyLanguageDefoult { get; }
        /// <summary>
        /// Returns the dictionary of a language with a given language key
        /// </summary>
        /// <param name="callback">Delegate with two parameters: dictionary and runtime error</param>
        /// <param name="keyLanguage">Dictionary language code</param>
        void GetDictionaryLang(Action<ResourceDictionary, Exception> callback, string keyLanguage);
        /// <summary>
        /// Returns all dictionaries of languages
        /// </summary>
        /// <param name="callback">Delegate with two parameters: List dictionary and runtime error</param>
        void GetCollectionDictionaryLang(Action<List<LanguageInfo>, List<Exception>> callback);
        /// <summary>
        /// Method for invoking a language change event
        /// </summary>
        /// <param name="keyLanguage">Language code</param>
        void LanguageChanged(string keyLanguage);
    }
}