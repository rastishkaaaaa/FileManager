using System.Windows.Forms;

namespace FileManager
{
    public static class UIManager
    {
        public static void ShowError(string message) =>
            MessageBox.Show(message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);

        public static void ShowSuccess(string message) =>
            MessageBox.Show(message, "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);

        public static bool ShowConfirmation(string message) =>
            MessageBox.Show(message, "Подтверждение", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes;

        public static bool IsItemSelected(ListView listView) =>
            listView.SelectedItems.Count > 0;

        public static ListViewItem GetSelectedItem(ListView listView) =>
            IsItemSelected(listView) ? listView.SelectedItems[0] : null;
    }
}