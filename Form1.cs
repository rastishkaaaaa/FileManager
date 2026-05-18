using System;
using System.Windows.Forms;

namespace FileManager
{
    public partial class Form1 : Form
    {
        private FileManager fileManager;

        public Form1()
        {
            InitializeComponent();
            InitializeFileManager();
        }

        private void InitializeFileManager()
        {
            fileManager = new FileManager(listViewFiles);
            fileManager.LoadFilesAndFolders();
        }

        private void btnCreateFile_Click(object sender, EventArgs e)
        {
            fileManager.CreateFile();
        }

        private void btnRename_Click(object sender, EventArgs e)
        {
            fileManager.RenameFile();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            fileManager.DeleteFile();
        }

        private void btnSortByDate_Click(object sender, EventArgs e)
        {
            fileManager.SortByDate();
        }

        private void btnSortByName_Click(object sender, EventArgs e)
        {
            fileManager.SortByName();
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            fileManager.SelectDirectory();
        }

        private void listViewFiles_DoubleClick(object sender, EventArgs e)
        {
            if (listViewFiles.SelectedItems.Count == 0)
                return;

            var selectedItem = listViewFiles.SelectedItems[0];

            // Переход в родительскую папку
            if (selectedItem.Tag?.ToString() == "PARENT_DIRECTORY")
            {
                fileManager.GoToParentDirectory();
                return;
            }

            var itemName = selectedItem.Text;
            var itemPath = System.IO.Path.Combine(fileManager.CurrentDirectory, itemName);

            if (System.IO.Directory.Exists(itemPath))
            {
                fileManager.CurrentDirectory = itemPath;
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            fileManager.SearchFilesAndFolders(txtSearch.Text);
        }

        private void btnResetSearch_Click(object sender, EventArgs e)
        {
            txtSearch.Text = "";
            fileManager.LoadFilesAndFolders();
        }
<<<<<<< HEAD
=======

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {

        }
>>>>>>> bd49e75c75965ee2eaeb048acb8367823370e973
    }
}