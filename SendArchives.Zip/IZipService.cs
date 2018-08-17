using SendArchives.Logger;
using System;

namespace SendArchives.Zip
{
    public interface IZipService
    {
        ILoggerService LoggerSerrvice { get; }

        event EventHandler ArchiveStartCreate;
        event EventHandler ArchiveStopCreate;
        event EventHandler ArchiveFinishedArchiveOneItem;

        void CreateArchiveAsync(Action<Exception> callback, ZipParametres zipParametres);
        void CancelCreateArchive(Action<Exception> calback);
    }
}