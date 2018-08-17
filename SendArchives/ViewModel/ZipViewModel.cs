using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using SendArchives.ViewModel.InterfacesViewModel;
using SendArchives.Zip;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Input;

namespace SendArchives.ViewModel
{
    public class ZipViewModel : ViewModelBase, IZipViewModel
    {
        private const int _maxCharsInPassword = 12;
        private const string _charsRandomPassword = "$%#@!*abcdefghijklmnopqrstuvwxyz1234567890?;:ABCDEFGHIJKLMNOPQRSTUVWXYZ^&";
        private const string _extensionArchive = ".zip";

        private IZipService _zipService;
        public IZipService ZipSrvice => _zipService;

        private bool _isCreatedArchive = false;
        public bool IsCreatedArchive
        {
            get => _isCreatedArchive;
            set => Set("IsCreatedArchive", ref _isCreatedArchive, value);
        }

        private string _nameArchive;
        public string NameArchive
        {
            get => _nameArchive;
            set => Set("NameArchive", ref _nameArchive, value);
        }

        private string _pathArchive;
        public string PathArchive
        {
            get => _pathArchive;
            set => Set("PathArchive", ref _pathArchive, value);
        }

        private byte _sizePart;
        public byte SizePart
        {
            get => _sizePart;
            set
            {
                Set("SizePart", ref _sizePart, value);
            }
        }

        private string _password;
        public string Password
        {
            get => _password;
            set => Set("Password", ref _password, value);
        }

        private ICommand _commandGetRandomPassword;
        public ICommand CommandGetRandomPassword
        {
            get => _commandGetRandomPassword ??
               (_commandGetRandomPassword = new RelayCommand(() =>
                 {
                     GetRandomPassword();
                 }));
        }

        private ICommand _commandCancelSaveArchive;
        public ICommand CommandCancelSaveArchive
        {
            get => _commandCancelSaveArchive ??
               (_commandCancelSaveArchive = new RelayCommand(() =>
               {
                   CancelSaveArchive();
               }));
        }

        public void CancelSaveArchive()
        {
            _zipService.CancelCreateArchive((er) =>
            {

            });
        }

        public void GetArchive(Action<string, Exception> callback, List<string> files)
        {
            Exception error = null;
            DateTime dateTimeCreateZip = DateTime.Now;
            string path = PathArchive + "\\" + dateTimeCreateZip.ToShortDateString() + "_" + dateTimeCreateZip.ToLongTimeString().Replace(":", "");
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            var zippar = new ZipParametres
            {
                SizePart = SizePart,
                Password = Password,
                CollectionFiles = files,
                NameArchive = NameArchive + _extensionArchive,
                PathFolderForArchive = path
            };
            _zipService.CreateArchiveAsync((er) =>
            {
                error = er;
                if (er != null)
                {
                    IsCreatedArchive = false;
                }
                callback(path, error);
            }, zippar);
        }

        public ZipViewModel(IZipService zipService)
        {
            _zipService = zipService;
        }

        public void GetRandomPassword()
        {
            Random random = new Random();
            Password = new string(Enumerable.Repeat(_charsRandomPassword, _maxCharsInPassword).Select(s => s[random.Next(s.Length)]).ToArray());
        }

        public void InitializeComponent(string archivePath, string archiveName, byte sizePart)
        {
            NameArchive = archiveName;
            PathArchive = archivePath;
            SizePart = sizePart;
            _zipService.ArchiveStartCreate += ((s, e) => { IsCreatedArchive = true; });
            _zipService.ArchiveStopCreate += ((s, e) => { IsCreatedArchive = false; });
        }
    }
}