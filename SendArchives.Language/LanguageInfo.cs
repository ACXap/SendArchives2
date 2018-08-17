namespace SendArchives.Language
{
    /// <summary>
    /// Class for storing language data for a collection of languages
    /// </summary>
    public class LanguageInfo
    {
        /// <summary>
        /// Full name of the language
        /// </summary>
        public string NameLanguage { get; set; }
        /// <summary>
        /// Language code
        /// </summary>
        public string KeyLanguage { get; set; }
        /// <summary>
        /// Path to the language flag
        /// </summary>
        public string FlagLanguage { get; set; }
    }
}