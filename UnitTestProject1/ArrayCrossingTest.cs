using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OPR.SSGA2;
using OPR.SSGA2.Italik;

namespace UnitTestProject1
{
    [TestClass]
    public class ArrayCrossingTest
    {
        [TestMethod]
        public void Test()
        {
            var secondCode = new int[] { 1, 5, 88, 3, 5, 23, 85 };
            var _code = new[] { 7, 3, 0, 12, 55, 2, 8 };

            var crossingPoint = 3;
            var firstChild = CrossCode(_code, secondCode, crossingPoint);
            var secondChild = CrossCode(secondCode, _code, crossingPoint);

            Assert.AreEqual(firstChild[3], 3);
            Assert.AreEqual(secondChild[3], 12);
        }

        [TestMethod]
        public void Test2()
        {
            var codeTransformer = new BinaryCromosome();
            var args = new BinaryEntityArgs {X = 2.535749f, Y = -3.845346f};
            var code = codeTransformer.EntityArgsToCode(args);
            var args2 = codeTransformer.CodeToEntityArgs(code) as ValidationResult;

            Assert.AreEqual(2.5f, args2.X);
            Assert.AreEqual(-3.8f, args2.Y);
        }

        private int[] CrossCode(int[] first, int[] second, int crossingPoint)
        {
            var result = new int[second.Length];
            Array.Copy(first, result, crossingPoint);
            Array.Copy(second, crossingPoint, result, crossingPoint, second.Length - crossingPoint);

            return result;
        }
    }
}
