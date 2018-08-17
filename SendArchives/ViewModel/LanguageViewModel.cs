using GalaSoft.MvvmLight;
using SendArchives.Language;
using SendArchives.ViewModel.InterfacesViewModel;
using System.Collections.ObjectModel;
using System.Linq;

namespace SendArchives.ViewModel
{
    public class LanguageViewModel : ViewModelBase, ILanguageViewModel
    {
        private ILanguageService _languageService;
        public ILanguageService LanguageService => _languageService;

        private LanguageInfo _currentLanguage;
        public LanguageInfo CurrentLanguage
        {
            get => _currentLanguage;
            set
            {
                if (_currentLanguage != null && _currentLanguage != value)
                {
                    LanguageService.LanguageChanged(value.KeyLanguage);
                }
                Set(ref _currentLanguage, value, true);
            }
        }

        private ObservableCollection<LanguageInfo> _collectionLanguage;
        public ObservableCollection<LanguageInfo> CollectionLanguage
        {
            get => _collectionLanguage;
            set => Set(ref _collectionLanguage, value);
        }

        public LanguageViewModel(ILanguageService languageService, string keyLanguage = null)
        {
            _languageService = languageService;
            if(keyLanguage!= null)
            {
                LanguageService.LanguageChanged(keyLanguage);
            }
        }

        public void InitializeComponent(string keyLanguage)
        {
            if (CollectionLanguage == null)
            {
                _languageService.GetCollectionDictionaryLang((list, error) =>
                {
                    CollectionLanguage = new ObservableCollection<LanguageInfo>(list);
                });
                SetLanguage(keyLanguage);
            }
        }

        public void SetLanguage(string keyLanguage)
        {
            CurrentLanguage = CollectionLanguage.FirstOrDefault(s => s.KeyLanguage == keyLanguage);
        }
    }
}