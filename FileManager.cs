using System;
using System.Collections;
using System.IO;
using System.Windows.Forms;
using System.Drawing;

namespace FileManager
{
    public class FileManager
    {
        private ListView listView;
        private string currentDirectory;

        public FileManager(ListView listView)
        {
            this.listView = listView;
            this.currentDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
        }

        public string CurrentDirectory
        {
            get => currentDirectory;
            set
            {
                if (Directory.Exists(value))
                {
                    currentDirectory = value;
                    LoadFilesAndFolders();
                }
            }
        }

        public void GoToParentDirectory()
        {
            var parentDir = Directory.GetParent(currentDirectory);
            if (parentDir != null)
            {
                currentDirectory = parentDir.FullName;
                LoadFilesAndFolders();
            }
        }

        public bool CanGoUp()
        {
            return Directory.GetParent(currentDirectory) != null;
        }

        public void LoadFilesAndFolders()
        {
            listView.Items.Clear();

            if (!Directory.Exists(currentDirectory))
                return;

            // Добавляем ".." для перехода в родительскую папку
            var parentDir = Directory.GetParent(currentDirectory);
            if (parentDir != null)
            {
                var upItem = new ListViewItem("..")
                {
                    ForeColor = Color.Gray,
                    Font = new Font(listView.Font, FontStyle.Italic)
                };
                upItem.SubItems.Add("-");
                upItem.SubItems.Add("<НАЗАД>");
                upItem.Tag = "PARENT_DIRECTORY";
                listView.Items.Add(upItem);
            }

            try
            {
                // Загрузка папок (синим цветом)
                foreach (var dir in Directory.GetDirectories(currentDirectory))
                {
                    var info = new DirectoryInfo(dir);
                    var item = new ListViewItem(Path.GetFileName(dir)) { ForeColor = Color.Blue };
                    item.SubItems.Add(info.LastWriteTime.ToString("dd.MM.yyyy HH:mm"));
                    item.SubItems.Add("<DIR>");
                    item.Tag = info;
                    listView.Items.Add(item);
                }

                // Загрузка файлов
                foreach (var file in Directory.GetFiles(currentDirectory))
                {
                    var info = new FileInfo(file);
                    var item = new ListViewItem(Path.GetFileName(file));
                    item.SubItems.Add(info.LastWriteTime.ToString("dd.MM.yyyy HH:mm"));
                    item.SubItems.Add($"{info.Length:N0} байт");
                    item.Tag = info;
                    listView.Items.Add(item);
                }
            }
            catch (Exception)
            {
                // Игнорируем ошибки доступа
            }
        }

        public void CreateFile()
        {
            using (var dialog = new FileNameDialog("Создание файла"))
            {
                if (dialog.ShowDialog() == DialogResult.OK && dialog.IsSaveConfirmed)
                {
                    var result = FileNameValidator.Validate(dialog.FileName, currentDirectory);

                    if (!result.IsValid)
                    {
                        if (result.IsConflict)
                        {
                            if (UIManager.ShowConfirmation(result.Message))
                            {
                                // Пользователь согласился на замену
                            }
                            else
                            {
                                return;
                            }
                        }
                        else
                        {
                            UIManager.ShowError(result.Message);
                            return;
                        }
                    }

                    try
                    {
                        var path = Path.Combine(currentDirectory, dialog.FileName);
                        File.Create(path).Close();
                        LoadFilesAndFolders();
                        UIManager.ShowSuccess("Файл успешно создан");
                    }
                    catch (Exception ex)
                    {
                        UIManager.ShowError($"Ошибка создания файла: {ex.Message}");
                    }
                }
            }
        }

        public void DeleteFile()
        {
            if (!UIManager.IsItemSelected(listView))
            {
                UIManager.ShowError("Выберите элемент для удаления");
                return;
            }

            var selectedItem = UIManager.GetSelectedItem(listView);

            // Проверка на элемент ".."
            if (selectedItem.Tag?.ToString() == "PARENT_DIRECTORY")
            {
                UIManager.ShowError("Выберите элемент для удаления");
                return;
            }

            var itemName = selectedItem.Text;
            var itemPath = Path.Combine(currentDirectory, itemName);

            if (!UIManager.ShowConfirmation("Вы действительно хотите удалить выбранный элемент?"))
            {
                return;
            }

            try
            {
                if (File.Exists(itemPath))
                {
                    File.Delete(itemPath);
                }
                else if (Directory.Exists(itemPath))
                {
                    Directory.Delete(itemPath, true);
                }

                LoadFilesAndFolders();
                UIManager.ShowSuccess("Файл успешно удалён");
            }
            catch (Exception ex)
            {
                UIManager.ShowError($"Ошибка удаления: {ex.Message}");
            }
        }

        public void RenameFile()
        {
            if (!UIManager.IsItemSelected(listView))
            {
                UIManager.ShowError("Выберите элемент для переименования!");
                return;
            }

            var selectedItem = UIManager.GetSelectedItem(listView);

            // Проверка на элемент ".."
            if (selectedItem.Tag?.ToString() == "PARENT_DIRECTORY")
            {
                UIManager.ShowError("Выберите элемент для переименования!");
                return;
            }

            var oldName = selectedItem.Text;
            var oldPath = Path.Combine(currentDirectory, oldName);

            using (var dialog = new FileNameDialog("Переименование файла", oldName))
            {
                if (dialog.ShowDialog() == DialogResult.OK && dialog.IsSaveConfirmed)
                {
                    var result = FileNameValidator.Validate(dialog.FileName, currentDirectory);

                    if (!result.IsValid)
                    {
                        if (result.IsConflict)
                        {
                            if (!UIManager.ShowConfirmation(result.Message))
                            {
                                return;
                            }
                        }
                        else
                        {
                            UIManager.ShowError(result.Message);
                            return;
                        }
                    }

                    try
                    {
                        var newPath = Path.Combine(currentDirectory, dialog.FileName);
                        File.Move(oldPath, newPath);
                        LoadFilesAndFolders();
                        UIManager.ShowSuccess("Файл успешно переименован");
                    }
                    catch (Exception ex)
                    {
                        UIManager.ShowError($"Ошибка переименования: {ex.Message}");
                    }
                }
            }
        }

        public void SortByName()
        {
            listView.ListViewItemSorter = new ListViewItemComparer(0, SortOrder.Ascending);
            listView.Sort();
        }

        public void SortByDate()
        {
            listView.ListViewItemSorter = new ListViewItemComparer(1, SortOrder.Ascending);
            listView.Sort();
        }

        public void SelectDirectory()
        {
            using (var dialog = new FolderBrowserDialog())
            {
                dialog.SelectedPath = currentDirectory;
                dialog.Description = "Выберите папку для просмотра";
                dialog.ShowNewFolderButton = false;

                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    currentDirectory = dialog.SelectedPath;
                    LoadFilesAndFolders();
                }
            }
        }
    }
}