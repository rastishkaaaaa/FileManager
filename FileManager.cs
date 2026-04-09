using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FileManager
{
    public class FileManager
    {
        private string currentDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
        private ListView listView;

        public FileManager(ListView listView)
        {
            this.listView = listView;
            LoadFilesAndFolders();
        }

        public void LoadFilesAndFolders()
        {
            listView.Items.Clear();
            try
            {
                var files = Directory.GetFiles(currentDirectory);
                var dirs = Directory.GetDirectories(currentDirectory);

                foreach (var dir in dirs)
                {
                    var dirInfo = new DirectoryInfo(dir);
                    listView.Items.Add(new ListViewItem(new[] { dirInfo.Name, dirInfo.LastWriteTime.ToString(), "Папка" })
                    { ForeColor = System.Drawing.Color.Blue });
                }
                foreach (var file in files)
                {
                    var fileInfo = new FileInfo(file);
                    listView.Items.Add(new ListViewItem(new[] { fileInfo.Name, fileInfo.LastWriteTime.ToString(), fileInfo.Length.ToString() }));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки: {ex.Message}");
            }
        }

        public void CreateFile()
        {
            using (var dialog = new SaveFileDialog())
            {
                dialog.InitialDirectory = currentDirectory;
                dialog.Title = "Создать файл";
                dialog.Filter = "Текстовые файлы (*.txt)|*.txt";
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    File.Create(dialog.FileName).Dispose();
                    LoadFilesAndFolders();
                }
            }
        }

        public void DeleteSelected()
        {
            if (listView.SelectedItems.Count == 0)
            {
                MessageBox.Show("Выберите файл или папку для удаления.");
                return;
            }
            string name = listView.SelectedItems[0].Text;
            string fullPath = Path.Combine(currentDirectory, name);
            try
            {
                if (File.Exists(fullPath))
                    File.Delete(fullPath);
                else if (Directory.Exists(fullPath))
                    Directory.Delete(fullPath, true);
                LoadFilesAndFolders();
            }
            catch (Exception ex) { MessageBox.Show($"Ошибка: {ex.Message}"); }
        }

        public void RenameSelected()
        {
            if (listView.SelectedItems.Count == 0) return;
            string oldName = listView.SelectedItems[0].Text;
            string oldPath = Path.Combine(currentDirectory, oldName);
            using (var dialog = new SaveFileDialog())
            {
                dialog.InitialDirectory = currentDirectory;
                dialog.Title = "Переименовать";
                dialog.FileName = oldName;
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    string newPath = dialog.FileName;
                    if (File.Exists(oldPath))
                        File.Move(oldPath, newPath);
                    else if (Directory.Exists(oldPath))
                        Directory.Move(oldPath, newPath);
                    LoadFilesAndFolders();
                }
            }
        }

        public void SortByDate()
        {
            // Реализация сортировки по дате (оставлена как в примере)
            var items = listView.Items.Cast<ListViewItem>()
                .OrderBy(item => DateTime.Parse(item.SubItems[1].Text)).ToList();
            listView.Items.Clear();
            listView.Items.AddRange(items.ToArray());
        }

        public void SortByName()
        {
            var items = listView.Items.Cast<ListViewItem>()
                .OrderBy(item => item.Text).ToList();
            listView.Items.Clear();
            listView.Items.AddRange(items.ToArray());
        }
    }
}
