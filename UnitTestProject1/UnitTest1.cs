using System;
using System.Diagnostics;
using System.Linq.Expressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OPR.KP.MKT_Items;
using OPR.KP.Shlp.NelderMid;
using OPR.lb1;

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
            var hc = new ShlpHyperCube(startPoint, 1, 1.1f, 15, 10, bounds, MultiplicationCoord);
            var nm = new NelderMid(new MKT_Point(startPoint.x, startPoint.y, MultiplicationCoord), MultiplicationCoord, bounds);
            var watch1 = Stopwatch.StartNew();
            var hsResult = hc.Calculate();
            watch1.Stop();
            var watch2 = Stopwatch.StartNew();
            var nmResult = nm.Calculate();
            watch2.Stop();
            var nm_r = MultiplicationCoord(nmResult.x, nmResult.y);
            var hs_r = MultiplicationCoord(hsResult.x, hsResult.y);
            Assert.IsTrue(Math.Abs(nm_r - hs_r) < 0.01);
        }

        [TestMethod]
        public void TestMethod2()
        {
            Expression<Func<float, float, float>> fn_exp = (x, y) => (float)(Math.Pow((y - Math.Pow(x, 2)), 2) + Math.Pow((1 - x), 2));
            var fn = fn_exp.Compile();
            var startPoint = new SquarePoint(1, 1);
            var bounds = new[] { new SquarePoint(0, 0), new SquarePoint(4.2f, 6.4f), };
            var hc = new ShlpHyperCube(startPoint, 1, 1.1f, 25, 10, bounds, fn);
            var nm = new NelderMid(new MKT_Point(startPoint.x, startPoint.y, fn), fn, bounds);
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

        protected virtual float MultiplicationCoord(float x, float y)
        {
            return (float)((1 + 8 * x - 7 * Math.Pow(x, 2) + 7 * Math.Pow(x, 3) / 3 - Math.Pow(x, 4) / 4) * (Math.Pow(y, 2) * Math.Pow(Math.E, -1 * y)));

        }
    }
}
