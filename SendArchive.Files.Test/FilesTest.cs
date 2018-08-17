using NUnit.Framework;
using SendArchives.Files;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Windows;

namespace SendArchive.Files.Test
{
    [TestFixture]
    public class FilesTest
    {
        private IFilesService _filesService;
        private Exception _error;
        private List<Exception> _errors;
        private List<FileSpecification> _files;
        private string _folderTest = TestContext.CurrentContext.TestDirectory + @"\";
        private string _fileTest;

        [SetUp]
        public void Init()
        {
            _error = null;
            _errors = new List<Exception>();
            _files = new List<FileSpecification>();
            _filesService = new FilesService();
            _fileTest = Path.GetFullPath(Assembly.GetExecutingAssembly().Location);
        }

        [Test]
        public void OpenRepositoryFiles_ErrorNotRequired()
        {
            _filesService.OpenRepositoryFile((e) =>
            {
                _error = e;
            }, _fileTest);

            Assert.That(_error, Is.Null);
        }

        [Test]
        public void OpenRepositoryFiles_ErrorNotNullFileNotFoundRequired()
        {
            _filesService.OpenRepositoryFile((e) =>
            {
                _error = e;
            }, "testtest");

            Assert.That(_error, Is.Not.Null);
        }

        [Test]
        public void OpenRepositoryFiles_ErrorFileNotFoundRequired()
        {
            string msg = "File no found";

            _filesService.OpenRepositoryFile((e) =>
            {
                _error = e;
            }, "testtest");

            Assert.That(_error.Message, Is.EqualTo(msg));
        }

        [Test]
        public void OpenFolder_ErrorNotRequired()
        {
            _filesService.OpenFolder((e) =>
            {
                _error = e;
            }, _folderTest);

            Assert.That(_error, Is.Null);
        }

        [Test]
        public void OpenFolder_ErrorNotNullDirectoryNotFoundRequired()
        {
            _filesService.OpenFolder((e) =>
            {
                _error = e;
            }, "testtest");

            Assert.That(_error, Is.Not.Null);
        }

        [Test]
        public void OpenFolder_ErrorDirectoryNotFoundRequired()
        {
            string testfolder = "testtest";
            string msg = $"Directory {testfolder} not found";

            _filesService.OpenFolder((e) =>
            {
                _error = e;
            }, testfolder);

            Assert.That(_error.Message, Is.EqualTo(msg));
        }

        [Test]
        public void GetFilesFromFolder_ListFilesNotNullRequired()
        {
            var files = Directory.GetFiles(_folderTest);

            _filesService.GetFilesFromFolder((l, e) =>
            {
                _files = l;
            }, _folderTest);

            Assert.That(_files.Count, Is.EqualTo(files.Length));
        }

        [Test]
        public void GetFilesFromFolder_ErrorNotRequired()
        {
            _filesService.GetFilesFromFolder((l, e) =>
            {
                
            }, _folderTest);

            Assert.That(_errors.Count, Is.EqualTo(0));
        }

        [Test]
        public void GetFilesFromFolder_ErrorNotNullDirectoryNotFoundRequired()
        {
            string testfolder = "testtest";
            string msg = $"Directory {testfolder} not found";

            _filesService.GetFilesFromFolder((l, e) =>
            {
                _errors = e;
            }, testfolder);

            Assert.That(_errors[0].Message, Is.EqualTo(msg));
        }

        [Test]
        public void GetFilesFromFolder_ErrorNotNullArgumentPathFolderNullRequired()
        {
            string testfolder = null;
            string msg = $"Name folder is empty";

            _filesService.GetFilesFromFolder((l, e) =>
            {
                _errors = e;
            }, testfolder);

            Assert.That(_errors[0].Message, Is.EqualTo(msg));
        }

        [Test]
        public void GetFilesFromFolder_FileSpecificationNameNotNullRequired()
        {
            _filesService.GetFilesFromFolder((l, e) =>
            {
                _files = l;
            }, _folderTest);

            Assert.That(_files[0].Name, Is.Not.Null);
        }

        [Test]
        public void GetFilesFromFolder_FileSpecificationFullNameNotNullRequired()
        {
            _filesService.GetFilesFromFolder((l, e) =>
            {
                _files = l;
            }, _folderTest);

            Assert.That(_files[0].FullName, Is.Not.Null);
        }

        [Test]
        public void GetFilesFromFolder_FileSpecificationSizeNotNullRequired()
        {
            _filesService.GetFilesFromFolder((l, e) =>
            {
                _files = l;
            }, _folderTest);

            Assert.That(_files[0].Size, Is.Not.Null);
        }


    }
}