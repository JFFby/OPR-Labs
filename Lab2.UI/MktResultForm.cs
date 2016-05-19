using System.Collections.Generic;
using System.Windows.Forms;
using OPR.KP.Logger;

namespace Lab2.UI
{
    public partial class MktResultForm : Form
    {
        public MktResultForm(Dictionary<string, List<LogValue>> results)
        {
            InitializeComponent();
            InitGrid();
        }

        private void InitGrid()
        {
            dataGridView1.Columns.Add("1", "3");
            dataGridView1.Columns.Add("2", "config");
            dataGridView1.Columns.Add("3", "count");
        }
    }
}
