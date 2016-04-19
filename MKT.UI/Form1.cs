using System;
using System.Windows.Forms;
using OPR.KP.MKT_Items;
using OPR.lb1;

namespace MKT.UI
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            var mkt = new OPR.KP.MKT(new ShlpHyperCubeConfig
            {
                Fn = MultiplicationCoord,
                DeltaSideLenth = 1.1f,
                InnerPointsCount = 5,
                SideLength = 1,
                IterationCount = 15
            },
                new BestSeparator(),
                new RandomGenerator(),
                10,
                new[] { new SquarePoint(0, 0), new SquarePoint(4.2f, 6.4f), },
                new[] { 5, 10 },
                new int[] { 3, 5 },
                4);
            var rezult = mkt.Solve();
            var val = MultiplicationCoord(1.83f, 6.38f);
            MessageBox.Show(rezult.Value.ToString());
        }

        protected virtual float MultiplicationCoord(float x, float y)
        {
            return (float)((1 + 8 * x - 7 * Math.Pow(x, 2) + 7 * Math.Pow(x, 3) / 3 - Math.Pow(x, 4) / 4) * (Math.Pow(y, 2) * Math.Pow(Math.E, -1 * y)));
            //return (float)(Math.Pow((y - Math.Pow(x, 2)), 2) + Math.Pow((1 - x), 2));
        }
    }
}
