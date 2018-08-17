using SendArchives.EmailSignature.Enumerations;
using SendArchives.Logger;
using System;
using System.Collections.Generic;
using System.IO;

namespace SendArchives.EmailSignature
{
    public class EmailSignatureService : IEmailSignatureService
    {
        private string _pathSignatureTheir = Directory.GetCurrentDirectory() + @"\Signatures\";
        private string _pathSignatureOutlook = Environment.ExpandEnvironmentVariables(@"%AppData%\Microsoft\Signatures\");

        public string PathSignatureOutlook => _pathSignatureOutlook;
        public string PathSignatureTheir => _pathSignatureTheir;
        public int MaxSizeFileSignature => 1024;
        public string ExtensionFileSignature => ".txt";

        private ILoggerService _loggerService;
        public ILoggerService LoggerService => _loggerService;

        public void GetAllEmailSignature(Action<List<EmailSignature>, List<Exception>> callback)
        {
            List<EmailSignature> listEmailSignature = new List<EmailSignature>();
            List<Exception> errors = new List<Exception>();

            GetEmailSignatures((es, er) =>
            {
                listEmailSignature.AddRange(es);
                errors.AddRange(er);
            }, PathSignatureOutlook);
            GetEmailSignatures((es, er) =>
            {
                listEmailSignature.AddRange(es);
                errors.AddRange(er);
            }, PathSignatureTheir);

            callback(listEmailSignature, errors);
        }

        public void GetEmailSignatureText(Action<string, Exception> callback, EmailSignature emailSignature)
        {
            string text = string.Empty;
            Exception error = null;

            if (emailSignature == null)
            {
                error = new ArgumentNullException("EmailSignature", "EmailSignature == null");
            }
            else if (string.IsNullOrEmpty(emailSignature.Name))
            {
                error = new ArgumentException(typeof(EmailSignature).FullName + ".Name is Empty");
            }
            else
            {
                var path = emailSignature.Path;
                if (string.IsNullOrEmpty(path))
                {
                    path = (emailSignature.TypeSignature == TypeSignature.Outlook ? PathSignatureOutlook : PathSignatureTheir) + emailSignature.Name;
                }
                GetEmailSignatureText((t, e) =>
                {
                    text = t;
                    error = e;
                }, path);
            }

            callback(text, error);
        }

        public void GetEmailSignatureText(Action<string, Exception> callback, string path)
        {
            Exception error = null;
            string text = string.Empty;

            if (File.Exists(path))
            {
                try
                {
                    text = File.ReadAllText(path);
                }
                catch (Exception ex)
                {
                    error = ex;
                }
            }
            else
            {
                error = new FileNotFoundException("File not found", path);
            }

            callback(text, error);
        }

        public void RemoveEmailSignature(Action<Exception> callback, EmailSignature emailSignature)
        {
            Exception error = null;

            if (emailSignature == null)
            {
                error = new ArgumentNullException(typeof(EmailSignature).FullName, typeof(EmailSignature).FullName + " == null");
            }
            else if (string.IsNullOrEmpty(emailSignature.Name))
            {
                error = new ArgumentException(typeof(EmailSignature).FullName + ".Name is Empty");
            }
            else if (emailSignature.TypeSignature == TypeSignature.Outlook)
            {
                error = new ArgumentException("Argument error", typeof(TypeSignature).Name);
            }
            else
            {
                var path = emailSignature.Path;
                if (string.IsNullOrEmpty(path))
                {
                    path = _pathSignatureTheir + emailSignature.Name;
                }
                if (File.Exists(path))
                {
                    try
                    {
                        File.Delete(path);
                    }
                    catch (Exception ex)
                    {
                        error = ex;
                    }
                }
                else
                {
                    error = new FileNotFoundException("File not found", emailSignature.Path);
                }
            }

            callback(error);
        }

        public void SaveEmailSignature(Action<Exception> callback, EmailSignature emailSignature, string signatureText)
        {
            Exception error = null;

            if (emailSignature == null)
            {
                error = new ArgumentNullException(typeof(EmailSignature).FullName, typeof(EmailSignature).FullName + " == null");
            }
            else if (string.IsNullOrEmpty(emailSignature.Name))
            {
                error = new ArgumentNullException(typeof(EmailSignature).FullName + ".Name is Empty");
            }
            else
            {
                if (!Directory.Exists(PathSignatureTheir))
                {
                    try
                    {
                        Directory.CreateDirectory(PathSignatureTheir);
                    }
                    catch (Exception ex)
                    {
                        error = ex;
                    }
                }
                if (Directory.Exists(PathSignatureTheir))
                {
                    string path = PathSignatureTheir + emailSignature.Name;
                    try
                    {
                        File.WriteAllText(path, signatureText);
                    }
                    catch (Exception ex)
                    {
                        error = ex;
                    }
                }
            }

            callback(error);
        }

        public void GetEmailSignatures(Action<List<EmailSignature>, List<Exception>> callback, string pathSignature)
        {
            List<Exception> errors = new List<Exception>();
            List<EmailSignature> listEmailSignature = null;

            if (!Directory.Exists(pathSignature))
            {
                errors.Add(new DirectoryNotFoundException($"Directory {pathSignature} not found"));
            }
            else
            {
                try
                {
                    var files = Directory.GetFiles(pathSignature, "*.txt");
                    var countFiles = files.Length;
                    if (countFiles == 0)
                    {
                        errors.Add(new Exception($"Folder {pathSignature} empty"));
                    }
                    else
                    {
                        listEmailSignature = new List<EmailSignature>(countFiles);
                        foreach (var file in files)
                        {
                            if (!File.Exists(file))
                            {
                                errors.Add(new FileNotFoundException($"File {file} not found"));
                                continue;
                            }
                            try
                            {
                                FileInfo fi = new FileInfo(file);
                                if (fi.Length > MaxSizeFileSignature)
                                {
                                    errors.Add(new Exception("The file is larger than the allowed size"));
                                    continue;
                                }
                                listEmailSignature.Add(new EmailSignature()
                                {
                                    Name = fi.Name,
                                    TypeSignature = GetTypeSignature(pathSignature),
                                    Path = fi.FullName
                                });
                            }
                            catch (Exception ex)
                            {
                                errors.Add(ex);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    errors.Add(ex);
                }
            }

            callback(listEmailSignature, errors);
        }

        private TypeSignature GetTypeSignature(string pathSignature)
        {
            if (pathSignature == PathSignatureOutlook)
            {
                return TypeSignature.Outlook;
            }
            else if (pathSignature == PathSignatureTheir)
            {
                return TypeSignature.Their;
            }
            return TypeSignature.Other;
        }

        public EmailSignatureService() { }
        public EmailSignatureService(string pathSignature)
        {
            _pathSignatureTheir = pathSignature;
        }

        public EmailSignatureService(ILoggerService loggerService)
        {
            _loggerService = loggerService;
        }
    }
}