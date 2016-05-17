using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using OPR.KP.MKT_Items;
using OPR.lb1;
using OPR.lb2;
using OPR.lb2.Enums;
using OPR.lb2.Interfaces.Common;
using OPR.SSGA2;
using OPR.SSGA2.Extension;
using OPR.SSGA2.Italik;
using BinaryGeneration = OPR.SSGA2.Generation<OPR.SSGA2.Italik.BinaryValueService, OPR.SSGA2.Italik.BinaryGenom>;
using BinaryGenom = OPR.SSGA2.Italik.BinaryGenom;


namespace Lab2.UI
{
    public partial class Form1 : Form
    {
        private IList<BinaryGeneration> generations;
        private BinarySSGA ssga;
        private int completedIterationCount;

        public Form1()
        {
            InitializeComponent();
            InitValues();
            SetupGrid();
            KeyPreview = true;
            iterationCountTextBox.Text = "10";
            completedIterationCount = 0;
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
        }

        private void StepWithUIUpdate()
        {
            Step();
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
                    CreateRow(parent);
                }

                if (binaryGeneration.Children != null && binaryGeneration.Children.Any())
                {
                    var groups = binaryGeneration.Children.GroupBy(x => x.Type);
                    foreach (var group in groups)
                    {
                        DrawLineChildrenSeparator(GetGroupMsg(group.Key,
                            binaryGeneration.GetChilrensCrossingPoint().ToString()));
                        foreach (var item in group)
                        {
                            CreateRow(item);
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

        private void CreateRow(Entity<BinaryValueService, OPR.SSGA2.Italik.BinaryGenom> entity)
        {
            var row = GetColorizedRow(entity);
            row.Cells.AddRange(Cells(row.DefaultCellStyle.BackColor).ToArray());
            row.Cells[0].Value = string.Format("{0}.{1}", entity.GenerationId, entity.Id);
            row.Cells[1].Value = entity.X();
            row.Cells[2].Value = entity.Y();
            row.Cells[3].Value = entity.Code;
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

        private DataGridViewRow GetColorizedRow(Entity<BinaryValueService, BinaryGenom> entity)
        {
            switch (entity.Function)
            {
                case EntityFunction.BestParent:
                    return ColorizedRow(Color.Green);
                case EntityFunction.WorstParent:
                    return ColorizedRow(Color.Red);
                case EntityFunction.BestChild:
                    return ColorizedRow(Color.Green);
                case EntityFunction.NotValid:
                    return ColorizedRow(Color.BlueViolet);
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
                SetupSSGA();
                ssga = new BinarySSGA(
                    GetFirstSeparator(),
                     new BestSeparator<Entity<BinaryValueService, BinaryGenom>>(),
                     GetGenerator());
                return true;
            }

            return false;
        }

        private void SetupSSGA()
        {
            GlobalSettings.N = int.Parse(TextboxN.Text);
            GlobalSettings.Fn = (x, y) => (float)(Math.Pow(x, 2) + Math.Pow(y, 2));
            GlobalSettings.IsBestFromChildernOnly = true;
            GlobalSettings.IsCrossingFirst = true;
            GlobalSettings.MutationChance = int.Parse(pNuTextbox.Text);
            GlobalSettings.nFromN = int.Parse(nTextBox.Text);
            GlobalSettings.SSGAIterationCount = int.Parse(iterationCountTextBox.Text) + 1;
            GetBounds();
        }

        private IGenerator<MKT_Point> GetGenerator()
        {
            GlobalSettings.isRandomOrGridPoints = randomOrGridPoint.Checked;
            var generator = GlobalSettings.isRandomOrGridPoints
                ? (IGenerator<MKT_Point>)new RandomGenerator(new BinaryArgsConverter())
                : (IGenerator<MKT_Point>)new GridPointsHelper(new BinaryArgsConverter());

            generator.SetupState((dynamic)new
            {
                state = GetState()
            });

            return generator;
        }

        private Genereate_MKT_Point_Arg GetState()
        {
            return new Genereate_MKT_Point_Arg
            {
                Bounds = GlobalSettings.GetBounds(),
                fn = GlobalSettings.Fn
            };
        }

        private void GetBounds()
        {
            GlobalSettings.LeftXBound = BoundsX()[0];
            GlobalSettings.RightXBound = BoundsX()[1];
            GlobalSettings.BottomYBound = BoundsY()[0];
            GlobalSettings.TopYBound = BoundsY()[1];

            GlobalSettings.MaxIntValueFroCrossing = (int)Math.Round(
                Math.Max(
                    Math.Max(Math.Abs(GlobalSettings.LeftXBound), Math.Abs(GlobalSettings.RightXBound)),
                    Math.Max(Math.Abs(GlobalSettings.BottomYBound), Math.Abs(GlobalSettings.TopYBound))));
        }

        private ISeparator<Entity<BinaryValueService, BinaryGenom>> GetFirstSeparator()
        {
            GlobalSettings.firstSelectionVariant = comboBox1.SelectedIndex;
            switch (GlobalSettings.firstSelectionVariant)
            {
                case 0:
                    return new Roulette<BinaryValueService, BinaryGenom>();
                case 1:
                    return new Tournament<BinaryValueService, BinaryGenom>();
                case 2:
                    return new Rang<BinaryValueService, BinaryGenom>();
                default:
                    throw new ArgumentException();
            }
        }

        private void UpdateChart(BinaryGeneration generation)
        {
            chart1.Series[0].Points.Clear();
            chart1.Series[0].SmartLabelStyle.Enabled = true;
            chart1.Series[0].MarkerStep = 1;
            chart1.Series[0].XValueType = ChartValueType.Double;
            foreach (var entity in generation.Entites)
            {
                chart1.Series[0].Points.Add(new DataPoint(entity.X(), entity.Y()));
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
            TextboxN.Text = "20";
            nTextBox.Text = "10";
            tTextBox.Text = "0,1";
            tTextBox.Enabled = false;
            randomOrGridPoint.Checked = true;
        }

        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Space)
            {
                completedIterationCount++;
                StepWithUIUpdate();
            }
            else if (e.KeyCode == Keys.Enter)
            {
                int i = 0;
                do
                {
                    Step();
                    completedIterationCount++;
                } while (++i < GlobalSettings.SSGAIterationCount);

                OnDataArrived();
            }

            if ((e.KeyCode == Keys.Space || e.KeyCode == Keys.Enter)
                && completedIterationCount == GlobalSettings.SSGAIterationCount)
            {
                {
                    MessageBox.Show(completedIterationCount.ToString());
                }
            }
        }

        private void comboBox1_BindingContextChanged(object sender, EventArgs e)
        {
            comboBox1.Items.Add("Roulette");
            comboBox1.Items.Add("Tournament");
            comboBox1.Items.Add("Rang");
            comboBox1.SelectedIndex = 0;
        }
    }
}
