using SendArchives.Logger;
using System;
using System.Collections.Generic;

namespace SendArvhives.Settings
{
    /// <summary>
    /// Describes the interface with working with the settings
    /// </summary>
    public interface ISettingsService
    {

        ILoggerService LoggerService { get; } ///TODO add description
        /// <summary>
        /// Settings file name. </summary>
        /// <value>
        /// The property for the configuration file name.
        /// </value>
        string NameFileSettings { get; }
        /// <summary>
        /// Loading settings from a file.
        /// With the ability to load default settings for errors.
        /// </summary>
        /// <param name="callback">Delegate with two parameters: settings and runtime error</param>
        void LoadSettings(Action<Settings, List<Exception>> callback);
        /// <summary>
        /// Save to file settings.
        /// </summary>
        /// <param name="callback">Delegate with one parameter: runtime error</param>
        /// <param name="settings">Settings to save</param>
        void SaveSettings(Action<Exception> callback, Settings settings);
        /// <summary>
        /// Returns default settings.
        /// The default settings are set manually in the method.
        /// </summary>
        /// <param name="callback">Delegate with two parameters: settings and runtime error</param>
        void GetDefaultSettings(Action<Settings, Exception> callback);
    }
}