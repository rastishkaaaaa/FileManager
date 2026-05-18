using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FileManagerTests 
{
    [TestClass]
    public class FileManagerSearchTests
    {
        //  для имитации элементов файловой системы в тестах
        public class FileSystemItem
        {
            public string Name { get; set; }
            public bool IsDirectory { get; set; }
        }

        public List<FileSystemItem> SearchFilesAndFolders(List<FileSystemItem> allItems, string query)
        {
            if (string.IsNullOrWhiteSpace(query))
                return allItems;

            string lowerQuery = query.ToLowerInvariant();
            return allItems.Where(item =>
                item.Name.ToLowerInvariant().Contains(lowerQuery)).ToList();
        }

        [TestMethod]
        public void SearchByExactName_ShouldReturnSingleMatch()
        {
            var items = new List<FileSystemItem>
            {
                new FileSystemItem { Name = "report.txt", IsDirectory = false },
                new FileSystemItem { Name = "image.png", IsDirectory = false },
                new FileSystemItem { Name = "notes.txt", IsDirectory = false }
            };
            string query = "report.txt";

            var result = SearchFilesAndFolders(items, query);

            Assert.AreEqual(1, result.Count, "Должен найтись ровно один файл");
            Assert.AreEqual("report.txt", result[0].Name, "Имя найденного файла не совпадает");
        }

        [TestMethod]
        public void SearchByPartialName_ShouldReturnMultipleMatches()
        {
            var items = new List<FileSystemItem>
            {
                new FileSystemItem { Name = "annual_report.pdf", IsDirectory = false },
                new FileSystemItem { Name = "monthly_report.docx", IsDirectory = false },
                new FileSystemItem { Name = "data.csv", IsDirectory = false }
            };
            string query = "report";

            var result = SearchFilesAndFolders(items, query);

            Assert.AreEqual(2, result.Count, "Должны найтись два файла");
            Assert.IsTrue(result.Any(r => r.Name == "annual_report.pdf"));
            Assert.IsTrue(result.Any(r => r.Name == "monthly_report.docx"));
        }

        [TestMethod]
        public void SearchWithNoMatch_ShouldReturnEmptyList()
        {
            var items = new List<FileSystemItem>
            {
                new FileSystemItem { Name = "file1.txt", IsDirectory = false },
                new FileSystemItem { Name = "folder", IsDirectory = true }
            };
            string query = "zzz_nonexistent";

            var result = SearchFilesAndFolders(items, query);

            Assert.AreEqual(0, result.Count, "Список результатов должен быть пустым");
        }

        [TestMethod]
        public void SearchWithEmptyQuery_ShouldReturnAllItems()
        {
            var items = new List<FileSystemItem>
            {
                new FileSystemItem { Name = "a.txt", IsDirectory = false },
                new FileSystemItem { Name = "b.txt", IsDirectory = false }
            };
            string query = "";

            var result = SearchFilesAndFolders(items, query);

            Assert.AreEqual(items.Count, result.Count, "При пустом запросе должен вернуться полный список");
        }

        [TestMethod]
        public void Search_ShouldBeCaseInsensitive()
        {
            var items = new List<FileSystemItem>
            {
                new FileSystemItem { Name = "Document.TXT", IsDirectory = false },
                new FileSystemItem { Name = "readme.md", IsDirectory = false }
            };
            string query = "document";

            var result = SearchFilesAndFolders(items, query);

            Assert.AreEqual(1, result.Count, "Поиск должен игнорировать регистр");
            Assert.AreEqual("Document.TXT", result[0].Name);
        }
    }
}