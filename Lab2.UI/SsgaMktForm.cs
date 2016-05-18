using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using MKT.UI;
using OPR.KP.MKT_Items;
using OPR.KP.Shlp;
using OPR.KP.Shlp.NelderMid;
using OPR.KP.SSGA_MKT_Items;
using OPR.lb2;
using OPR.lb2.Interfaces.Common;
using OPR.SSGA2;
using OPR.SSGA2.Italik;
using MktGeneration = OPR.SSGA2.Generation<OPR.KP.SSGA_MKT_Items.MktValueService, OPR.KP.SSGA_MKT_Items.MktGenom>;

namespace Lab2.UI
{
    public partial class SsgaMktForm : Form
    {
        private readonly MktSsga ssga;
        private IList<MktGeneration> generations;
        private int completedIterationCount = 0;
        private bool isFirstStep = true;
        private Dictionary<string, IGenerator<MKT_Point>> generators;
        private Dictionary<string, ISeparator<MKT_Point>> separators;
        private Dictionary<string, Func<MktIterationMode, IShlpWrapper>> shlpWrappers;

        public SsgaMktForm(MktSsga ssga)
        {
            InitializeComponent();
            this.ssga = ssga;
            SetupGrid();
            KeyPreview = true;
            WindowState = FormWindowState.Maximized;
            InitializeComponents();
        }

        private void InitializeComponents()
        {
            InitGeneratorDictionary();
            InitShlpWrappers();
            InitializeSeparatorLins();
        }

        private void InitShlpWrappers()
        {
            shlpWrappers = new Dictionary<string, Func<MktIterationMode, IShlpWrapper>>
            {
                {typeof(NelderMidWrapper).Name, mode => new NelderMidWrapper(GlobalSettings.Fn, GlobalSettings.GetBounds(), mode)  },
                {typeof(HyperCubeWrapper).Name, mode => new HyperCubeWrapper(GlobalSettings.Fn, GlobalSettings.GetBounds(), mode)  }
            };
        }

        private void InitializeSeparatorLins()
        {
            var bestSeparator = new BestSeparator<MKT_Point>();
            var roulette = new Roulette<MKT_Point>();
            var tournament = new Tournament<MKT_Point>();
            var rang = new Rang<MKT_Point>();
            separators = new Dictionary<string, ISeparator<MKT_Point>>
            {
                {bestSeparator.GetType().Name, bestSeparator},
                {roulette.GetType().Name, roulette},
                {tournament.GetType().Name, tournament},
                {rang.GetType().Name, rang}
            };
        }

        private void InitGeneratorDictionary()
        {
            var randomGenerator = new RandomGenerator(new BinaryArgsConverter());
            var gridGenerator = new GridPointsHelper(new BinaryArgsConverter());
            generators = new Dictionary<string, IGenerator<MKT_Point>>
            {
                { randomGenerator.GetType().Name, randomGenerator},
                { gridGenerator.GetType().Name, gridGenerator }
            };
        }

        private void SetupGrid()
        {
            dataGridView1.Columns.Add("1", "#");
            dataGridView1.Columns.Add("2", "N");
            dataGridView1.Columns.Add("3", "n");
            dataGridView1.Columns.Add("4", "Generator");
            dataGridView1.Columns.Add("5", "Separator 1");
            dataGridView1.Columns.Add("6", "Shlp");
            dataGridView1.Columns.Add("7", "Run Type");
            dataGridView1.Columns.Add("8", "Separator 2");
            dataGridView1.Columns.Add("9", "Lambda");
            dataGridView1.Columns.Add("10", "Code");
            dataGridView1.Columns.Add("11", "Value");
        }

        public void Step()
        {
            generations = isFirstStep ? ssga.Start() : ssga.EvalutionStep();
            isFirstStep = false;
        }

        private void OnDataArrived()
        {
            GridHandler.UpdateGrid(dataGridView1, generations, CreateRow);
        }

        private void CreateRow(Entity<MktValueService, MktGenom> entity)
        {
            var config = entity.Args as MKT_Config;
            var row = GridHandler.GetColorizedRow(entity);
            row.Cells.AddRange(GridHandler.Cells(row.DefaultCellStyle.BackColor, dataGridView1).ToArray());
            row.Cells[0].Value = string.Format("{0}.{1}", entity.GenerationId, entity.Id);
            row.Cells[1].Value = config.N;
            row.Cells[2].Value = config.n;
            row.Cells[3].Value = config.Generator.GetType().Name;
            row.Cells[4].Value = config.FirstSeparator.GetType().Name;
            row.Cells[5].Value = config.Shlp.GetType().Name;
            row.Cells[6].Value = config.IterationMode.ToString();
            row.Cells[7].Value = config.SecondSeparator.GetType().Name;
            row.Cells[8].Value = config.Lambda;
            row.Cells[9].Value = entity.Code;
            row.Cells[10].Value = entity.Value;
            dataGridView1.Rows.Add(row);
        }

        private void dataGridView1_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            var row = dataGridView1.Rows[e.RowIndex];
            var iterationMode = (MktIterationMode)Enum.Parse(typeof (MktIterationMode), row.Cells[6].Value.ToString());
            var config = new MKT_Config
            {
                N = int.Parse(row.Cells[1].Value.ToString()),
                n = int.Parse(row.Cells[2].Value.ToString()),
                Generator = generators[row.Cells[3].Value.ToString()],
                FirstSeparator = separators[row.Cells[4].Value.ToString()],
                Shlp = shlpWrappers[row.Cells[5].Value.ToString()](iterationMode),
                IterationMode = iterationMode,
                Lambda = byte.Parse(row.Cells[8].Value.ToString()),
                SecondSeparator = separators[row.Cells[7].Value.ToString()]
            };

            var mktForm = new MKT_Form(config);
            mktForm.Show();
        }

        private void StepWithUIUpdate()
        {
            Step();
            OnDataArrived();
        }

        private void SsgaMktForm_KeyUp(object sender, KeyEventArgs e)
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
    }
}
