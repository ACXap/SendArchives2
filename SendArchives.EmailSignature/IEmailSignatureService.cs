using SendArchives.Logger;
using System;
using System.Collections.Generic;

namespace SendArchives.EmailSignature
{
    public interface IEmailSignatureService
    {
        ILoggerService LoggerService { get; }

        string PathSignatureOutlook { get; }
        string PathSignatureTheir { get; }
        int MaxSizeFileSignature { get; }
        string ExtensionFileSignature { get; }
        void GetEmailSignatureText(Action<string, Exception> callback, EmailSignature emailSignature);
        void GetEmailSignatureText(Action<string, Exception> callback, string path);
        void GetAllEmailSignature(Action<List<EmailSignature>, List<Exception>> callback);
        void SaveEmailSignature(Action<Exception> callback, EmailSignature emailSignature, string signatureText);
        void RemoveEmailSignature(Action<Exception> callback, EmailSignature emailSignature);
        void GetEmailSignatures(Action<List<EmailSignature>, List<Exception>> callback, string pathSignature);
    }
}