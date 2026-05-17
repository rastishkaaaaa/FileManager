using System;
using System.Collections;
using System.Windows.Forms;

namespace FileManager
{
    public class ListViewItemComparer : IComparer
    {
        private readonly int _column;
        private readonly SortOrder _order;

        public ListViewItemComparer(int column, SortOrder order)
        {
            _column = column;
            _order = order;
        }

        public int Compare(object x, object y)
        {
            var itemX = (ListViewItem)x;
            var itemY = (ListViewItem)y;

            int result;
            if (_column == 1)
            {
                if (DateTime.TryParse(itemX.SubItems[_column].Text, out var dateX) &&
                    DateTime.TryParse(itemY.SubItems[_column].Text, out var dateY))
                {
                    result = dateX.CompareTo(dateY);
                }
                else
                {
                    result = string.Compare(itemX.SubItems[_column].Text, itemY.SubItems[_column].Text);
                }
            }
            else
            {
                result = string.Compare(itemX.SubItems[_column].Text, itemY.SubItems[_column].Text);
            }

            return _order == SortOrder.Descending ? -result : result;
        }
    }
}