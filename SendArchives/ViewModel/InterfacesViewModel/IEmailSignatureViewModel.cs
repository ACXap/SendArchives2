using System.Collections.ObjectModel;
using System.Windows.Input;

namespace SendArchives.ViewModel.InterfacesViewModel
{
    public interface IEmailSignatureViewModel
    {
        EmailSignature.IEmailSignatureService EmailSignatureService { get; }

        string CurrentEmailSignatureText { get; set; }
        EmailSignature.EmailSignature CurrentEmailSignature { get; set; }
        ObservableCollection<EmailSignature.EmailSignature> CollectionEmailSignature { get; set; }
        void InitializeComponent(string signaturePath);
        ICommand CommandCreateEmailSignature { get; }
        ICommand CommandSaveEmailSignature { get; }
        ICommand CommandCloseEmailSignature { get; }
        ICommand CommandRemoveEmailSignature { get; }
    }
}