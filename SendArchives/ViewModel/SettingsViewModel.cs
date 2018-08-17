using GalaSoft.MvvmLight;
using SendArchives.ViewModel.InterfacesViewModel;
using SendArvhives.Settings;
using System;

namespace SendArchives.ViewModel
{
    public class SettingsViewModel : ViewModelBase, ISettingsViewModel
    {
        private ISettingsService _settingsService;
        public ISettingsService SettingsService => _settingsService;

        private Settings _settings;
        public Settings Settings
        {
            get => _settings;
            set => Set(ref _settings, value);
        }

        public SettingsViewModel(ISettingsService settingsService, Settings set = null)
        {
            _settingsService = settingsService;
            if(set == null)
            {
                _settingsService.LoadSettings((s, error) =>
                {
                    Settings = s;
                });
            }
            else
            {
                Settings = set;
            }
        }

        public void SaveSettings(Action<Exception> callbak)
        {
            Exception error = null;
            _settingsService.SaveSettings((e) =>
            {
                error = e;
            }, _settings);
            callbak(error);
        }

        public void LoadDefaultSettings(Action<Exception> callback)
        {
            Exception error = null;
            _settingsService.GetDefaultSettings((set, e) =>
            {
                Settings = set;
                error = e;
            });
            callback(error);
        }
    }
}