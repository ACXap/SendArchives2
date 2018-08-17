using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using SendArchives.Files;
using SendArchives.ViewModel.InterfacesViewModel;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Data;
using System.Windows.Input;

namespace SendArchives.ViewModel
{
    public class FilesViewModel : ViewModelBase, IFilesViewModel
    {
        private IFilesService _filesService;
        public IFilesService FilesService => _filesService;



        private ObservableCollection<FileSpecification> _collectionFiles;
        public ObservableCollection<FileSpecification> CollectionFiles
        {
            get => _collectionFiles;
            set => Set(ref _collectionFiles, value);
        }

        private int _maxSize;
        public int MaxSize
        {
            get => _maxSize;
            set => Set("MaxSize", ref _maxSize, value);
        }

        private int _countGroup;
        public int CountGroup
        {
            get => _countGroup;
            set => Set("CountGroup", ref _countGroup, value);
        }

        private bool _isGrouped = false;
        public bool IsGrouped
        {
            get => _isGrouped;
            set
            {
                Set("IsGrouped", ref _isGrouped, value);
                if(value)
                {
                    CVFiles.GroupDescriptions.Clear();
                    CVFiles.GroupDescriptions.Add(new PropertyGroupDescription("Group"));
                    CVFiles.SortDescriptions.Add(new SortDescription("Group", ListSortDirection.Ascending));
                    CVFiles.SortDescriptions.Add(new SortDescription("Size", ListSortDirection.Descending));
                    CountGroup = CVFiles.Groups.Count;
                }
                else
                {
                    CVFiles.GroupDescriptions.Clear();
                    foreach(var i in CollectionFiles)
                    {
                        i.Group = 0;
                    }
                }
            }
        }

        private ICollectionView _cvFiles;
        public ICollectionView CVFiles
        {
            get => _cvFiles;
            set => Set("CVFiles", ref _cvFiles, value);
        }

        private ICommand _commandAddFilesToCollection;
        public ICommand CommandAddFilesToCollection
        {
            get
            {
                return _commandAddFilesToCollection
                    ?? (_commandAddFilesToCollection = new RelayCommand(
                    () =>
                    {
                        FilesService.GetFiles((f, e) =>
                        {
                            if (CollectionFiles == null)
                            {
                                CollectionFiles = new ObservableCollection<FileSpecification>(f);
                                CVFiles = CollectionViewSource.GetDefaultView(CollectionFiles);
                            }
                            else
                            {
                                foreach (var item in f)
                                {
                                    CollectionFiles.Add(item);
                                }
                            }
                        });
                    }));
            }
        }

        private ICommand _commandClearCollection;
        public ICommand CommandClearCollection
        {
            get
            {
                return _commandClearCollection
                    ?? (_commandClearCollection = new RelayCommand(
                    () =>
                    {
                        CollectionFiles?.Clear();
                        IsGrouped = false;
                    }, () =>
                     {
                         if (CollectionFiles != null && CollectionFiles.Count > 0)
                         {
                             return true;
                         }
                         return false;
                     }));
            }
        }

        private ICommand _commandRemoveFileFromCollection;
        public ICommand CommandRemoveFileFromCollection
        {
            get
            {
                return _commandRemoveFileFromCollection
                    ?? (_commandRemoveFileFromCollection = new RelayCommand<FileSpecification>(
                    f =>
                    {
                        CollectionFiles.Remove(f);
                        if(!CollectionFiles.Any())
                        {
                            IsGrouped = false;
                        }
                    }));
            }
        }

        private ICommand _commandOpenFolderFile;
        public ICommand CommandOpenFolderFile
        {
            get
            {
                return _commandOpenFolderFile
                    ?? (_commandOpenFolderFile = new RelayCommand<string>(
                    f =>
                    {
                        FilesService.OpenRepositoryFile((e) =>
                        {

                        }, f);
                    }));
            }
        }

        private ICommand _commandOpenFolder;
        public ICommand CommandOpenFolder
        {
            get
            {
                return _commandOpenFolder
                    ?? (_commandOpenFolder = new RelayCommand<string>(
                    f =>
                    {
                        FilesService.OpenFolder((e) =>
                        {

                        }, f);
                    }));
            }
        }

        private ICommand _commandGroupFiles;
        public ICommand CommandGroupFiles
        {
            get
            {
                return _commandGroupFiles
                    ?? (_commandGroupFiles = new RelayCommand(
                    () =>
                    {
                        var maxsize = _maxSize * 1024 * 1024;
                        var listFile = CollectionFiles.Where(s => s.Group == 0).OrderByDescending(s => s.Size).ToList();
                        if(listFile.Max(s=>s.Size) >maxsize)
                        {
                            return;
                        }
                        var group = 1;
                        long sum = 0;
                        var countList = listFile.Count;
                        while (countList > 0)
                        {
                            sum = 0;
                            for (int i = 0; i < countList; i++)
                            {
                                var sump = sum + listFile[i].Size;
                                if (sump < maxsize)
                                {
                                    listFile[i].Group = group;
                                    sum += listFile[i].Size;
                                }
                                else if (sump == maxsize)
                                {
                                    listFile[i].Group = group;
                                    break;
                                }
                            }
                            group++;
                            listFile = CollectionFiles.Where(s => s.Group == 0).OrderByDescending(s => s.Size).ToList();
                            countList = listFile.Count;
                        }
                        IsGrouped = true;
                    }, 
                    ()=>
                    {
                        return CollectionFiles == null ? false : !IsGrouped && CollectionFiles.Any();
                    }
                    ));
            }
        }

        private ICommand _commandUnGroup;
        public ICommand CommandUnGroup
        {
            get
            {
                return _commandUnGroup
                    ?? (_commandUnGroup = new RelayCommand<string>(
                    f =>
                    {
                        IsGrouped = false;
                    }));
            }
        }

        public FilesViewModel(IFilesService filesService)
        {
            _filesService = filesService;
        }

        public void ReplaceFileOnArchve(string path)
        {
            _filesService.GetFilesFromFolder((list, error) =>
            {
                CollectionFiles.Clear();
                foreach (var f in list)
                {
                    CollectionFiles.Add(f);
                }
            }, path);
            IsGrouped = false;
        }
    }
}