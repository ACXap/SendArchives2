using SendArchives.Files;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace SendArchives.ViewModel.InterfacesViewModel
{
    public interface IFilesViewModel
    {
        IFilesService FilesService { get; }

        bool IsGrouped { get; set; }
        ObservableCollection<FileSpecification> CollectionFiles { get; set; }

        void ReplaceFileOnArchve(string path);
        ICommand CommandAddFilesToCollection { get; }
        ICommand CommandRemoveFileFromCollection { get; }
        ICommand CommandOpenFolderFile { get; }
        ICommand CommandOpenFolder { get; }
        ICommand CommandClearCollection { get; }
        ICommand CommandGroupFiles { get; }
        ICommand CommandUnGroup { get; }
    }
}