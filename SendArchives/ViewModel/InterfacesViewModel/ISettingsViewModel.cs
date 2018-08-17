using SendArvhives.Settings;
using System;

namespace SendArchives.ViewModel.InterfacesViewModel
{
    public interface ISettingsViewModel
    {
        ISettingsService SettingsService { get; }
        Settings Settings { get; set; }

        void LoadDefaultSettings(Action<Exception> callback);
        void SaveSettings(Action<Exception> callbak);
    }
}