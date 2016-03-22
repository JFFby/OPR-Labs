using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using OPR.lb2;
using OPR.lb2.Enums;

namespace Lab2.UI
{
    public partial class Form1 : Form
    {
        private IList<BinaryGeneration> generations;
        private SSGA ssga;
        private byte N;

        public Form1()
        {
            InitializeComponent();
            InitValues();
            SetupGrid();
            KeyPreview = true;
        }

        private void SetupGrid()
        {
            dataGridView1.Columns.Add("1", "N");
            dataGridView1.Columns.Add("2", "x");
            dataGridView1.Columns.Add("2", "y");
            dataGridView1.Columns.Add("2", "X");
            dataGridView1.Columns.Add("2", "f(x, y)");
        }

        private void Step()
        {
            var isFirstStep = InitializeSSGA();
            generations = isFirstStep
                 ? ssga.Start()
                 : ssga.EvalutionStep();
            OnDataArrived();
        }

        private void OnDataArrived()
        {
            UpdateChart(generations.Last());
            UpdateGrid();
        }

        private void UpdateGrid()
        {
            dataGridView1.Rows.Clear();
            foreach (var binaryGeneration in generations)
            {
                Drawline();
                for (var i = 0; i < binaryGeneration.Parents.Count; ++i)
                {
                    var parent = binaryGeneration.Parents[i];
                    CreateRow(parent, binaryGeneration.Id.ToString());
                }

                if (binaryGeneration.Children != null && binaryGeneration.Children.Any())
                {
                    var groups = binaryGeneration.Children.OfType<BinaryEntity>().GroupBy(x => x.Genom.Type);
                    foreach (var group in groups)
                    {
                        DrawLineChildrenSeparator(GetGroupMsg(group.Key, binaryGeneration.GrossPoint.ToString()));
                        foreach (var item in group)
                        {
                            CreateRow(item, binaryGeneration.Id.ToString());
                        }
                    }
                }
            }
        }

        private string GetGroupMsg(EntityType type, string crossPoit)
        {
            if (type == EntityType.Child)
            {
                return string.Format("{0} - {1}", type.ToString(), crossPoit);
            }

            return type.ToString();
        }

        private void CreateRow(BinaryEntity entity, string geneId)
        {
            var row = GetColorizedRow(entity.Function);
            row.Cells.AddRange(Cells(row.DefaultCellStyle.BackColor).ToArray());
            row.Cells[0].Value = string.Format("{0}.{1}", geneId, entity.Id);
            row.Cells[1].Value = entity.Genom.X.Value;
            row.Cells[2].Value = entity.Genom.Y.Value;
            row.Cells[3].Value = entity.Genom.Code;
            row.Cells[4].Value = entity.Value;
            dataGridView1.Rows.Add(row);
        }

        private void DrawLineChildrenSeparator(string msg)
        {
            var color = Color.Yellow;
            var row = ColorizedRow(color);
            row.Cells.AddRange(Cells(Color.Yellow).ToArray());
            row.Cells[3].Value = "Потомки";
            row.Cells[4].Value = msg;
            dataGridView1.Rows.Add(row);
        }

        private IList<DataGridViewCell> Cells(Color color)
        {
            var cells = new List<DataGridViewCell>();
            for (int i = 0; i < dataGridView1.ColumnCount; i++)
            {
                var cell = new DataGridViewTextBoxCell();
                cell.Style.BackColor = color;
                cells.Add(cell);
            }

            return cells;
        }

        private DataGridViewRow GetColorizedRow(EntityFunction function)
        {
            switch (function)
            {
                case EntityFunction.BestParent:
                    return ColorizedRow(Color.Green);
                case EntityFunction.WorstParent:
                    return ColorizedRow(Color.Red);
                case EntityFunction.BestChild:
                    return ColorizedRow(Color.Green);
                case EntityFunction.None:
                default:
                    return ColorizedRow(Color.White);
            }
        }

        private void Drawline()
        {
            if (dataGridView1.Rows.Count > 0)
            {
                var row = ColorizedRow(Color.Black);
                dataGridView1.Rows.Add(row);
            }
        }

        private DataGridViewRow ColorizedRow(Color color)
        {
            var datagridRow = new DataGridViewRow();
            datagridRow.DefaultCellStyle.BackColor = color;
            return datagridRow;
        }

        private bool InitializeSSGA()
        {
            if (ssga == null)
            {
                var pNu = byte.Parse(pNuTextbox.Text);
                N = byte.Parse(TextboxN.Text);
                var n = byte.Parse(nTextBox.Text);
                BinaryСhromosome.SetUp(GetBinaryViewBoundLength(), mutationChance: pNu);
                ssga = new SSGA(BoundsX(), BoundsY(), N, n);
                return true;
            }

            return false;
        }

        private void UpdateChart(BinaryGeneration generation)
        {
            chart1.Series[0].Points.Clear();
            chart1.Series[0].SmartLabelStyle.Enabled = true;
            chart1.Series[0].MarkerStep = 1;
            chart1.Series[0].XValueType = ChartValueType.Double;
            foreach (var entity in generation.Entities)
            {
                chart1.Series[0].Points.Add(new DataPoint(entity.Genom.X.Value, entity.Genom.Y.Value));
            }
        }

        private int GetBinaryViewBoundLength()
        {
            var bounds = new List<float>(BoundsY());
            bounds.AddRange(BoundsX());
            return bounds.Select(x => (int)Math.Floor(x)).OrderByDescending(x => x)
                .First();

        }

        private float[] BoundsX()
        {
            return new float[]
            {
                (float) double.Parse(xs.Text),
                (float) double.Parse(xb.Text)
            };
        }

        private float[] BoundsY()
        {
            return new float[]
            {
                (float) double.Parse(ys.Text),
                (float) double.Parse(yb.Text)
            };
        }

        private void InitValues()
        {
            xs.Text = "-4";
            xb.Text = "4";
            ys.Text = "-4";
            yb.Text = "4";
            pNuTextbox.Text = "7";
            TextboxN.Text = "5";
            nTextBox.Text = "10";
            tTextBox.Text = "0,1";
            tTextBox.Enabled = false;
        }

        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Space)
            {
                Step();
            }
        }
    }
}
