using Ionic.Zip;
using SendArchives.Logger;
using System;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SendArchives.Zip
{
    public class ZipService : IZipService
    {
        private CancellationTokenSource cts;

        private ILoggerService _loggerService;
        public ILoggerService LoggerSerrvice => _loggerService;

        public event EventHandler ArchiveStartCreate;
        public event EventHandler ArchiveStopCreate;
        public event EventHandler ArchiveFinishedArchiveOneItem;

        public void CancelCreateArchive(Action<Exception> calback)
        {
            cts.Cancel();
        }

        public async void CreateArchiveAsync(Action<Exception> callback, ZipParametres zipParametres)
        {
            Exception error = null;
            if (zipParametres == null)
            {
                error = new ArgumentNullException(typeof(ZipParametres).FullName, typeof(ZipParametres).FullName + " == null");
            }
            else if (!Directory.Exists(zipParametres.PathFolderForArchive))
            {
                error = new DirectoryNotFoundException($"Directory {zipParametres.PathFolderForArchive} not found");
            }
            else
            {
                error = await CreateArchive(zipParametres);
            }
            callback(error);
        }

        private Task<Exception> CreateArchive(ZipParametres zipParametres)
        {
            cts = new CancellationTokenSource();
            Task<Exception> task = new Task<Exception>(() =>
            {
                Exception error = null;
                var saveProggress = SaveProgress(cts);
                var zipName = zipParametres.PathFolderForArchive + "\\" + zipParametres.NameArchive;
                ZipFile zip = new ZipFile(zipName, Encoding.GetEncoding(866));
                try
                {
                    zip.SaveProgress += saveProggress;
                    zip.CompressionMethod = CompressionMethod.Deflate;
                    zip.CompressionLevel = Ionic.Zlib.CompressionLevel.BestCompression;
                    if (!string.IsNullOrEmpty(zipParametres.Password))
                    {
                        zip.Encryption = EncryptionAlgorithm.PkzipWeak;
                        zip.Password = zipParametres.Password;
                    }
                    zip.MaxOutputSegmentSize = 1024 * 1024 * zipParametres.SizePart;
                    zip.AddFiles(zipParametres.CollectionFiles, "");
                    zip.Save();
                    cts.Token.ThrowIfCancellationRequested();
                }
                catch (OperationCanceledException ex)
                {
                    error = ex;
                }
                catch (Exception ex)
                {
                    error = ex;
                }
                finally
                {
                    if (zip != null)
                    {
                        zip.SaveProgress -= saveProggress;
                        zip.Dispose();
                    }
                }
                return error;
            }, cts.Token);
            task.Start();
            return task;
        }

        private EventHandler<SaveProgressEventArgs> SaveProgress(CancellationTokenSource cts)
        {
            return new EventHandler<SaveProgressEventArgs>((senser, e) =>
            {
                if (cts.IsCancellationRequested)
                {
                    e.Cancel = true;
                }
                if(e.EventType==ZipProgressEventType.Saving_Started)
                {
                    ArchiveStartCreate?.Invoke(this, null);
                }
                if(e.EventType == ZipProgressEventType.Saving_Completed)
                {
                    ArchiveStopCreate?.Invoke(this, null);
                }
                if(e.EventType == ZipProgressEventType.Saving_AfterWriteEntry)
                {
                    ArchiveFinishedArchiveOneItem?.Invoke(this, null);
                }
            });
        }

        public ZipService(ILoggerService loggerService)
        {
            _loggerService = loggerService;
        }
    }
}