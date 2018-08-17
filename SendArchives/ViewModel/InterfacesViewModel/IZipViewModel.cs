using SendArchives.Zip;
using System;
using System.Collections.Generic;
using System.Windows.Input;

namespace SendArchives.ViewModel.InterfacesViewModel
{
    public interface IZipViewModel
    {
        IZipService ZipSrvice { get; }

        bool IsCreatedArchive { get; set; }
        string NameArchive { get; set; }
        string PathArchive { get; set; }
        string Password { get; set; }
        byte SizePart { get; set; }

        void GetArchive(Action<string, Exception> callback, List<string> files);
        void GetRandomPassword();
        void CancelSaveArchive();

        void InitializeComponent(string archivePath, string archiveName, byte sizePart);

        ICommand CommandGetRandomPassword { get; }
        ICommand CommandCancelSaveArchive { get; }
    }
}
