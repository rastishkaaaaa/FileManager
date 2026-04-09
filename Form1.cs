using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FileManager
{
    public partial class Form1 : Form
    {
        private FileManager fileManager;

        public Form1()
        {
            InitializeComponent();
            fileManager = new FileManager(listView1);
        }

        private void buttonCreate_Click(object sender, EventArgs e)
        {
            fileManager.CreateFile();
        }

        private void buttonDelete_Click(object sender, EventArgs e)
        {
            fileManager.DeleteSelected();
        }

        private void buttonRename_Click(object sender, EventArgs e)
        {
            fileManager.RenameSelected();
        }

        private void buttonSortDate_Click(object sender, EventArgs e)
        {
            fileManager.SortByDate();
        }

        private void buttonSortName_Click(object sender, EventArgs e)
        {
            fileManager.SortByName();
        }
    }
}
