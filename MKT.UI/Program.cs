using System;
using System.Linq;
using System.Windows.Forms;
using OPR.KP.MKT_Items;
using OPR.KP.SSGA_MKT_Items;
using OPR.lb2;
using OPR.SSGA2;

namespace MKT.UI
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            GlobalSettings.LeftXBound = 0;
            GlobalSettings.RightXBound = 4.2f;
            GlobalSettings.BottomYBound = 0;
            GlobalSettings.TopYBound = 6.4f;
            GlobalSettings.Fn = MultiplicationCoord;
            var config = (MKT_Config)new RnadomMKTConfigGenerator()
                .GenerateEntityArgs(10)
                .ElementAt(RandomHelper.Random(1, 9));

            Application.Run(new MKT_Form(config));
        }
            
        private static float MultiplicationCoord(float x, float y)
        {
            return (float)((1 + 8 * x - 7 * Math.Pow(x, 2) + 7 * Math.Pow(x, 3) / 3 - Math.Pow(x, 4) / 4) * (Math.Pow(y, 2) * Math.Pow(Math.E, -1 * y)));
            //return (float)(Math.Pow((y - Math.Pow(x, 2)), 2) + Math.Pow((1 - x), 2));
        }
    }
}
