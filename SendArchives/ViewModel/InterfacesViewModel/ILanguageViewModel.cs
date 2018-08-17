using System.Collections.ObjectModel;

namespace SendArchives.ViewModel.InterfacesViewModel
{
    public interface ILanguageViewModel
    {
        Language.ILanguageService LanguageService { get; }

        Language.LanguageInfo CurrentLanguage { get; set; }
        ObservableCollection<Language.LanguageInfo> CollectionLanguage { get; set; }
        void InitializeComponent(string keyLanguage);
        void SetLanguage(string keyLanguage);
    }
}