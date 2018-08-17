using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;

namespace SendArchives.EmailSignature.Test
{
    [TestFixture]
    public class EmailSignatureTest
    {
        private const string _emailSignatureTextTest = "Test signature";
        private IEmailSignatureService _emailSignatureService;
        // private EmailSignature _emailSignature;
        private EmailSignature _emailSignatureTest;
        private Exception _error;

        [SetUp]
        public void Init()
        {
            _emailSignatureService = new EmailSignatureService(TestContext.CurrentContext.TestDirectory + @"\");
            _emailSignatureTest = new EmailSignature() { Name = "test.txt", TypeSignature = Enumerations.TypeSignature.Their };
            //  _emailSignature = null;
            _error = null;
        }

        [Test]
        public void SaveEmailSignature_NotErrorRequired()
        {
            _emailSignatureService.SaveEmailSignature((e) =>
            {
                _error = e;
            }, _emailSignatureTest, _emailSignatureTextTest);

            Assert.That(_error, Is.Null);
        }

        [Test]
        public void SaveEmailSignature_ErrorRequiredErrorNameFile()
        {
            _emailSignatureTest.Name = "?.txt";
            _emailSignatureService.SaveEmailSignature((e) =>
            {
                _error = e;
            }, _emailSignatureTest, _emailSignatureTextTest);

            Assert.That(_error, Is.Not.Null);
        }

        [Test]
        public void SaveEmailSignature_ErrorRequiredEmailSignatureNull()
        {
            EmailSignature es = null;
            _emailSignatureService.SaveEmailSignature((e) =>
            {
                _error = e;
            }, es, _emailSignatureTextTest);

            Assert.That(_error, Is.Not.Null);
        }

        [Test]
        public void RemoveEmailSignature_NotErrorRequired()
        {
            EmailSignature es = new EmailSignature()
            {
                Name = "TestRemove.txt",
                TypeSignature = Enumerations.TypeSignature.Their
            };
            _emailSignatureService.SaveEmailSignature((e) => { }, es, _emailSignatureTextTest);

            _emailSignatureService.RemoveEmailSignature((e) =>
            {
                _error = e;
            }, es);

            Assert.That(_error, Is.Null);
        }

        [Test]
        public void RemoveEmailSignature_ErrorRequiredFileNotFound()
        {
            EmailSignature es = new EmailSignature()
            {
                Name = "TestRemoveTestTestTest.txt",
                TypeSignature = Enumerations.TypeSignature.Their
            };

            _emailSignatureService.RemoveEmailSignature((e) =>
            {
                _error = e;
            }, es);

            Assert.That(_error, Is.Not.Null);
        }

        [Test]
        public void RemoveEmailSignature_ErrorRequiredTypeSignatureNotCorect()
        {
            EmailSignature es = new EmailSignature()
            {
                Name = "TestRemoveTestTestTest.txt",
                TypeSignature = Enumerations.TypeSignature.Outlook
            };

            _emailSignatureService.RemoveEmailSignature((e) =>
            {
                _error = e;
            }, es);

            Assert.That(_error, Is.Not.Null);
        }

        [Test]
        public void RemoveEmailSignature_ErrorRequiredEmailSignatureNameNotCorrect()
        {
            EmailSignature es = new EmailSignature()
            {
                Name = "?.txt",
                TypeSignature = Enumerations.TypeSignature.Outlook
            };

            _emailSignatureService.RemoveEmailSignature((e) =>
            {
                _error = e;
            }, es);

            Assert.That(_error, Is.Not.Null);
        }

        [Test]
        public void GetEmailSignatureText_ErrorNotRequiredGetByPath()
        {
            var path = _emailSignatureService.PathSignatureTheir + _emailSignatureTest.Name;
            _emailSignatureService.GetEmailSignatureText((str, e) =>
            {
                _error = e;
            }, path);

            Assert.That(_error, Is.Null);
        }

        [Test]
        public void GetEmailSignatureText_ErrorRequiredGetByPathNotCorrect()
        {
            var path = "testtesttest?";
            _emailSignatureService.GetEmailSignatureText((str, e) =>
            {
                _error = e;
            }, path);

            Assert.That(_error, Is.Not.Null);
        }

        [Test]
        public void GetEmailSignatureText_ErrorNotRequiredGetByEmailSignatur()
        {
            _emailSignatureService.GetEmailSignatureText((str, e) =>
            {
                _error = e;
            }, _emailSignatureTest);

            Assert.That(_error, Is.Null);
        }

        [Test]
        public void GetEmailSignatureText_ErrorRequiredGetByEmailSignaturNotCorrect()
        {
            EmailSignature es = new EmailSignature()
            {
                Name = "",
                TypeSignature = Enumerations.TypeSignature.Outlook
            };

            _emailSignatureService.GetEmailSignatureText((str, e) =>
            {
                _error = e;
            }, es);

            Assert.That(_error, Is.Not.Null);
        }

        [Test]
        public void GetEmailSignatureText_ErrorRequiredGetByEmailSignaturNull()
        {
            EmailSignature es = null;
            _emailSignatureService.GetEmailSignatureText((str, e) =>
            {
                _error = e;
            }, es);

            Assert.That(_error, Is.Not.Null);
        }

        [Test]
        public void GetEmailSignatureText_EqualsRequiredSaveEmailSignature()
        {
            string text = null;

            _emailSignatureService.SaveEmailSignature((e) => { }, _emailSignatureTest, _emailSignatureTextTest);
            _emailSignatureService.GetEmailSignatureText((str, e) =>
            {
                text = str;
            }, _emailSignatureTest);

            Assert.That(text, Is.EqualTo(_emailSignatureTextTest));
        }

        [Test]
        public void GetEmailSignatures_EmailSignaturesNotNullRequired()
        {
            var countSignatureInTestFolder = 10;
            var pathTestFolder = _emailSignatureService.PathSignatureTheir + @"\TestFolder1";
            CreateTestFolderAndEmailSignature(pathTestFolder, countSignatureInTestFolder);
          
            var emailSignatureService = new EmailSignatureService(pathTestFolder);
            var emailSignatures = new List<EmailSignature>();

            emailSignatureService.GetEmailSignatures((es, e) =>
            {
                emailSignatures = es;
            }, pathTestFolder);

            var countSignature = emailSignatures.Count;

            Assert.That(countSignatureInTestFolder, Is.EqualTo(countSignature));
        }

        [Test]
        public void GetEmailSignatures_ErrorNotRequired()
        {
            var countSignatureInTestFolder = 10;
            var pathTestFolder = _emailSignatureService.PathSignatureTheir + @"\TestFolder2";
            CreateTestFolderAndEmailSignature(pathTestFolder, countSignatureInTestFolder);
            var emailSignatureService = new EmailSignatureService(pathTestFolder);
            var errors = new List<Exception>();

            emailSignatureService.GetEmailSignatures((es, e) =>
            {
                errors = e;
            }, pathTestFolder);

            var countErrors = errors.Count;

            Assert.That(countErrors, Is.EqualTo(0));
        }

        private void CreateTestFolderAndEmailSignature(string pathFolder, int countSignatureInTestFolder)
        {
            if (Directory.Exists(pathFolder))
            {
                Directory.Delete(pathFolder, true);
            }
            Directory.CreateDirectory(pathFolder);
            for (int i = 0; i < countSignatureInTestFolder; i++)
            {
                File.Create(pathFolder + @"\" + i + ".txt");
            }
        }
    }
}