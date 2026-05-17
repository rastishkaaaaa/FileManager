using System;
using System.Windows.Forms;
using System.Drawing;

namespace FileManager
{
    public partial class FileNameDialog : Form
    {
        private TextBox txtFileName;
        private Button btnSave;
        private Button btnCancel;

        public string FileName { get; private set; }
        public bool IsSaveConfirmed { get; private set; }

        public FileNameDialog(string title, string initialName = "")
        {
            InitializeComponent();
            this.Text = title;
            txtFileName.Text = initialName;
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();

            txtFileName = new TextBox
            {
                Location = new Point(12, 12),
                Size = new Size(260, 20),
                TabIndex = 0
            };
            txtFileName.TextChanged += TxtFileName_TextChanged;

            btnSave = new Button
            {
                Text = "Сохранить",
                Location = new Point(116, 45),
                Size = new Size(75, 23),
                DialogResult = DialogResult.OK,
                TabIndex = 1
            };
            btnSave.Click += BtnSave_Click;

            btnCancel = new Button
            {
                Text = "Отмена",
                Location = new Point(197, 45),
                Size = new Size(75, 23),
                DialogResult = DialogResult.Cancel,
                TabIndex = 2
            };

            this.AcceptButton = btnSave;
            this.CancelButton = btnCancel;
            this.ClientSize = new Size(284, 81);
            this.Controls.Add(txtFileName);
            this.Controls.Add(btnSave);
            this.Controls.Add(btnCancel);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.StartPosition = FormStartPosition.CenterParent;
            this.Text = "Ввод имени файла";

            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private void TxtFileName_TextChanged(object sender, EventArgs e)
        {
            string cleaned = FileNameValidator.RemoveInvalidCharacters(txtFileName.Text);
            if (cleaned != txtFileName.Text)
            {
                int selectionStart = txtFileName.SelectionStart;
                txtFileName.Text = cleaned;
                txtFileName.SelectionStart = Math.Max(0, selectionStart - 1);
            }
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            FileName = txtFileName.Text.Trim();
            IsSaveConfirmed = true;
        }
    }
}