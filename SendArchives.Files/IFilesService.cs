using SendArchives.Logger;
using System;
using System.Collections.Generic;

namespace SendArchives.Files
{
    public interface IFilesService
    {
        ILoggerService LoggerService { get; }

        void GetFiles(Action<List<FileSpecification>, List<Exception>> callback);
        void GroupFiles(Action<Exception> callback, List<FileSpecification> listFiles, int maxSizeGroup);
        void GetFilesFromFolder(Action<List<FileSpecification>, List<Exception>> callback, string pathFolder);
        void OpenRepositoryFile(Action<Exception> callback, string pathFile);
        void OpenFolder(Action<Exception> callback, string pathFolder);
    }
}