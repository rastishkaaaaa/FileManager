using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using FileManager;

namespace FileManagerTests
{
    [TestClass]
    public class FileManagerLoadTests
    {
        private string testDirectory;
        private ListView listView;
        private FileManager.FileManager fileManager;

        [TestInitialize]
        public void Init()
        {
            testDirectory = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
            Directory.CreateDirectory(testDirectory);

            listView = new ListView();
            listView.View = View.Details;
            listView.Columns.Add("Name", 100);
            listView.Columns.Add("Date", 100);
            listView.Columns.Add("Size", 100);

            fileManager = new FileManager.FileManager(listView);

            var field = typeof(FileManager.FileManager).GetField("currentDirectory", BindingFlags.NonPublic | BindingFlags.Instance);
            field.SetValue(fileManager, testDirectory);
        }

        [TestCleanup]
        public void Cleanup()
        {
            if (Directory.Exists(testDirectory))
                Directory.Delete(testDirectory, true);
            listView?.Dispose();
            fileManager = null;
        }

        [TestMethod]
        public void LoadFilesAndFolders_ShouldShowFilesAndDirectories()
        {
            string file1 = Path.Combine(testDirectory, "test.txt");
            string subDir = Path.Combine(testDirectory, "SubDir");
            File.WriteAllText(file1, "content");
            Directory.CreateDirectory(subDir);

            fileManager.LoadFilesAndFolders();

            // Ожидаем 3 элемента: ".." (родительская папка), "SubDir", "test.txt"
            Assert.AreEqual(3, listView.Items.Count);

            // Проверяем, что ".." есть первым элементом
            Assert.AreEqual("..", listView.Items[0].Text);

            // Проверяем наличие созданных элементов (игнорируя "..")
            Assert.IsTrue(listView.Items.Cast<ListViewItem>().Any(item => item.Text == "SubDir"));
            Assert.IsTrue(listView.Items.Cast<ListViewItem>().Any(item => item.Text == "test.txt"));
        }

        [TestMethod]
        public void LoadFilesAndFolders_WhenDirectoryEmpty_ShouldShowNothing()
        {
            fileManager.LoadFilesAndFolders();

            // В пустой директории остаётся только элемент ".."
            Assert.AreEqual(1, listView.Items.Count);
            Assert.AreEqual("..", listView.Items[0].Text);
        }
    }
}