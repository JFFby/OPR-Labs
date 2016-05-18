using System;
using System.Diagnostics;
using System.Linq.Expressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OPR.KP.Logger;
using OPR.KP.MKT_Items;
using OPR.KP.Shlp;
using OPR.KP.Shlp.NelderMid;
using OPR.KP.SSGA_MKT_Items;
using OPR.lb1;
using OPR.SSGA2;

namespace UnitTestProject1
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            var startPoint = new SquarePoint(1, 1);
            var bounds = new[] { new SquarePoint(0, 0), new SquarePoint(4.2f, 6.4f), };
            var hc = new ShlpHyperCube(startPoint, 1, 1.1f, 15, 10, bounds, MultiplicationCoord, MktIterationMode.Limited);
            var nm = new NelderMid(new MKT_Point(startPoint.x, startPoint.y, MultiplicationCoord), MultiplicationCoord, bounds, MktIterationMode.Limited);
            var watch1 = Stopwatch.StartNew();
            var hsResult = hc.Calculate();
            watch1.Stop();
            var watch2 = Stopwatch.StartNew();
            var nmResult = nm.Calculate();
            watch2.Stop();
            var nm_r = MultiplicationCoord(nmResult.x, nmResult.y);
            var hs_r = MultiplicationCoord(hsResult.x, hsResult.y);
            Assert.IsTrue(Math.Abs(nm_r - hs_r) <1);
        }

        [TestMethod]
        public void TestMethod2()
        {
            Expression<Func<float, float, float>> fn_exp = (x, y) => (float)(Math.Pow((y - Math.Pow(x, 2)), 2) + Math.Pow((1 - x), 2));
            var fn = fn_exp.Compile();
            var startPoint = new SquarePoint(1, 1);
            var bounds = new[] { new SquarePoint(0, 0), new SquarePoint(4.2f, 6.4f), };
            var hc = new ShlpHyperCube(startPoint, 1, 1.1f, 25, 10, bounds, fn, MktIterationMode.Limited);
            var nm = new NelderMid(new MKT_Point(startPoint.x, startPoint.y, fn), fn, bounds, MktIterationMode.Limited);
            var watch1 = Stopwatch.StartNew();
            var hsResult = hc.Calculate();
            watch1.Stop();
            var watch2 = Stopwatch.StartNew();
            var nmResult = nm.Calculate();
            watch2.Stop();
            var nm_r = fn(nmResult.x, nmResult.y);
            var hs_r = fn(hsResult.x, hsResult.y);
            Assert.IsTrue(Math.Abs(nm_r - hs_r) < 0.01);
        }

        [TestMethod]
        public void LogTest()
        {
            var config = CreateConfig(4);
            var logger = new Logger();
            logger.Log(config, new LogValue { Value = 1, X = 1, Y = 3 });
        }

        private MKT_Config CreateConfig(int lambda)
        {

            var bounds = new[] { new SquarePoint(0, 0), new SquarePoint(4.2f, 6.4f), };
            var shlp = new HyperCubeWrapper(MultiplicationCoord, bounds, MktIterationMode.Full);
            GlobalSettings.LeftXBound = 0;
            GlobalSettings.RightXBound = 4.2f;
            GlobalSettings.BottomYBound = 0;
            GlobalSettings.TopYBound = 6.4f;
            return new MKT_Config
            {
                n = lambda,// RandomHelper.Random(n_Bounds[0], n_Bounds[1]),
                N = 3,//RandomHelper.Random(N_Bounds[0], N_Bounds[1]),
                Shlp = shlp,
                Generator = new RandomGenerator(null),
                Lambda = 4,
                FirstSeparator = new BestSeparator<MKT_Point>(),
                SecondSeparator = new BestSeparator<MKT_Point>(),
                IterationMode = MktIterationMode.Full
            };
        }

        protected virtual float MultiplicationCoord(float x, float y)
        {
            return (float)((1 + 8 * x - 7 * Math.Pow(x, 2) + 7 * Math.Pow(x, 3) / 3 - Math.Pow(x, 4) / 4) * (Math.Pow(y, 2) * Math.Pow(Math.E, -1 * y)));

        }
    }
}
