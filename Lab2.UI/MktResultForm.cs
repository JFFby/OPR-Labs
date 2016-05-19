using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using OPR.KP.Logger;
using OPR.lb2;

namespace Lab2.UI
{
    public partial class MktResultForm : Form
    {
        private readonly Dictionary<string, int> results; 

        public MktResultForm(Dictionary<string, List<LogValue>> results)
        {
            InitializeComponent();
            InitGrid();
            this.results = results
                .OrderBy(x => x.Value.Count)
                .ThenBy(x => x.Key)
                .ToDictionary(x => x.Key, y => y.Value.Count);
            ViewResults();
            RandomHelper.Random(1, 2);
        }

        private void ViewResults()
        {
            for (int i = 0; i < results.Keys.Count; i++)
            {
                var row = new DataGridViewRow();
                row.Cells.AddRange(GridHandler.Cells(Color.White, dataGridView1).ToArray());
                row.Cells[0].Value = i + 1;
                row.Cells[1].Value = results.Keys.ElementAt(i);
                row.Cells[2].Value = results[results.Keys.ElementAt(i)];

                dataGridView1.Rows.Add(row);
            }
        }

        private void InitGrid()
        {
            dataGridView1.Columns.Add("1", "#");
            var col = new DataGridViewColumn();
            col.HeaderText = "config";
            col.Width = 450;
            dataGridView1.Columns.Add(col);
            dataGridView1.Columns.Add("3", "count");
        }
    }
}
