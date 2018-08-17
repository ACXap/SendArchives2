using Microsoft.Win32;
using SendArchives.Logger;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace SendArchives.Files
{
    public class FilesService : IFilesService
    {
        private ILoggerService _loggerService;
        public ILoggerService LoggerService => _loggerService;

        public void GetFiles(Action<List<FileSpecification>, List<Exception>> callback)
        {
            OpenFileDialog myDialog = new OpenFileDialog
            {
                Multiselect = true,
                DereferenceLinks = false
            };

            if (myDialog.ShowDialog() != true || myDialog.FileNames.Length == 0)
            {
                return;
            }
            var countFiles = myDialog.FileNames.Length;

            List<Exception> errors = new List<Exception>();
            List<FileSpecification> listFiles = new List<FileSpecification>(countFiles);

            GetFiles(errors, listFiles, myDialog.FileNames);

            callback(listFiles, errors);
        }

        public void GetFilesFromFolder(Action<List<FileSpecification>, List<Exception>> callback, string pathFolder)
        {
            List<Exception> errors = new List<Exception>();
            List<FileSpecification> listFiles = null;

            if (string.IsNullOrEmpty(pathFolder))
            {
                errors.Add(new ArgumentException("Name folder is empty"));
            }
            else if (!Directory.Exists(pathFolder))
            {
                errors.Add(new DirectoryNotFoundException($"Directory {pathFolder} not found"));
            }
            else
            {
                try
                {
                    var files = Directory.GetFiles(pathFolder);
                    var countFiles = files.Length;
                    if (countFiles == 0)
                    {
                        errors.Add(new Exception("Folder empty"));
                    }
                    else
                    {
                        listFiles = new List<FileSpecification>(countFiles);
                        GetFiles(errors, listFiles, files);
                    }
                }
                catch (Exception ex)
                {
                    errors.Add(ex);
                }
            }

            callback(listFiles, errors);
        }

        public void OpenFolder(Action<Exception> callback, string pathFolder)
        {
            Exception error = null;

            if (!Directory.Exists(pathFolder))
            {
                error = new DirectoryNotFoundException($"Directory {pathFolder} not found");
            }
            else
            {
                try
                {
                    Process.Start(new ProcessStartInfo("explorer", string.Format("/e, \"{0}\"", pathFolder)));
                }
                catch (Exception ex)
                {
                    error = ex;
                }
            }

            callback(error);
        }

        public void OpenRepositoryFile(Action<Exception> callback, string pathFile)
        {
            Exception error = null;

            if (!File.Exists(pathFile))
            {
                error = new FileNotFoundException("File no found", pathFile);
            }
            else
            {
                try
                {
                    Process.Start(new ProcessStartInfo("explorer", string.Format("/e, /select, \"{0}\"", pathFile)));
                }
                catch (Exception ex)
                {
                    error = ex;
                }
            }

            callback(error);
        }

        public void GroupFiles(Action<Exception> callback, List<FileSpecification> listFiles, int maxSizeGroup)
        {
            Exception error = null;


        }

        private FileSpecification GetFileSpecification(string file)
        {
            FileInfo fi = new FileInfo(file);
            var f = new FileSpecification
            {
                Name = fi.Name,
                FullName = fi.FullName,
                Size = fi.Length
            };
            return f;
        }

        private void GetFiles(List<Exception> errors, List<FileSpecification> listFiles, string[] files)
        {
            foreach (var file in files)
            {
                if (File.Exists(file))
                {
                    try
                    {
                        listFiles.Add(GetFileSpecification(file));
                    }
                    catch (Exception ex)
                    {
                        errors.Add(ex);
                    }
                }
            }
        }

        public FilesService(ILoggerService loggerService)
        {
            _loggerService = loggerService;
        }
        public FilesService()
        {

        }
    }
}