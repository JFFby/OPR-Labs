using System;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using OPR.KP.MKT_Items;

namespace MKT.UI
{
    public partial class MKT_Form : Form
    {
        private bool IsEnd = false;
        private readonly OPR.KP.MKT mkt;

        public MKT_Form(MKT_Config config)
        {
            InitializeComponent();
            KeyPreview = true;
            mkt = new OPR.KP.MKT(config);
            mkt.OnEnd += OnMKTEnd;
            Step();
        }

        private void OnStep(object sender, EventArgs eventArgs)
        {
            chart1.Series[0].Points.Clear();
            var entities = eventArgs as MktStepEntities;
            if (entities != null)
            {
                foreach (var startPoint in entities.StartPoints)
                {
                    chart1.Series[0].Points.Add(new DataPoint(startPoint.x, startPoint.y)
                    {
                        Color = Color.Gray,
                        Label = string.Format("s{0}", startPoint.Id.ToString()),
                        MarkerSize = 10,
                        LabelToolTip = startPoint.Value.ToString()
                    });
                }

                foreach (var startPoint in entities.BestFromStart)
                {
                    chart1.Series[0].Points.Add(new DataPoint(startPoint.x, startPoint.y)
                    {
                        Color = Color.LawnGreen,
                        Label = string.Format("b{0}", startPoint.Id.ToString()),
                        MarkerSize = 10,
                        LabelToolTip = startPoint.Value.ToString()
                    });
                }

                foreach (var lambdaPoint in entities.LambdaPoints)
                {
                    chart1.Series[0].Points.Add(new DataPoint(lambdaPoint.x, lambdaPoint.y)
                    {
                        Color = Color.DarkGray,
                        Label = string.Format("L{0}", lambdaPoint.Id.ToString()),
                        MarkerStyle = MarkerStyle.Cross,
                        MarkerSize = 10,
                        LabelToolTip = lambdaPoint.Value.ToString()
                    });
                }

                foreach (var lambdaPoint in entities.ShlpFromLambda)
                {
                    chart1.Series[0].Points.Add(new DataPoint(lambdaPoint.x, lambdaPoint.y)
                    {
                        Color = Color.Yellow,
                        Label = string.Format("shlp_L{0}", lambdaPoint.ShlpFrom.Id.ToString()),
                        MarkerStyle = MarkerStyle.Cross,
                        MarkerSize = 10,
                        LabelToolTip = lambdaPoint.Value.ToString()
                    });
                }

                foreach (var lambdaPoint in entities.ShlpFromBest)
                {
                    chart1.Series[0].Points.Add(new DataPoint(lambdaPoint.x, lambdaPoint.y)
                    {
                        Color = Color.Yellow,
                        Label = string.Format("shlp_b{0}", lambdaPoint.ShlpFrom.Id.ToString()),
                        MarkerSize = 10,
                        LabelToolTip = lambdaPoint.Value.ToString()
                    });
                }

                chart1.Series[0].Points.Add(new DataPoint(entities.WorstPoint.x, entities.WorstPoint.y)
                {
                    Color = Color.Red,
                    Label = string.Format("w{0}", entities.WorstPoint.Id.ToString()),
                    MarkerSize = 10,
                    LabelToolTip = entities.WorstPoint.Value.ToString()
                });
            }
        }

        private void OnMKTEnd(object sender, EventArgs e)
        {
            IsEnd = true;
            var rezult = sender as MKT_Point;
            if (rezult != null)
            {
                MessageBox.Show(rezult.Value.ToString());
            }
        }

        private void Step()
        {
            var stepEntities = mkt.Step();
            OnStep(mkt, stepEntities);
        }

        private void MKT_Form_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Space)
            {
                if (!IsEnd)
                {
                    Step();
                }
            }
        }
    }
}
