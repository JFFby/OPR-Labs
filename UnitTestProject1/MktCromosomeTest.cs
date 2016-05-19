using System.Linq;
using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OPR.KP.MKT_Items;
using OPR.KP.Shlp;
using OPR.KP.SSGA_MKT_Items;
using OPR.KP.SSGA_MKT_Items.Enums;
using OPR.lb2;

namespace UnitTestProject1
{
    [TestClass]
    public class MktCromosomeTest
    {
        [TestMethod]
        public void TestMethod1()
        {
            var config = new MKT_Config { Generator = new GridPointsHelper(null) };
            var method = typeof(MKT_Config).GetMethod("GetGeneratorInteger",
                BindingFlags.NonPublic | BindingFlags.Instance);
            var value = method.Invoke(config, new object[] { });
            Assert.AreEqual((int)GeneratorType.Grid, value);
        }

        public void TestMethod2()
        {
            var config = new MKT_Config { Generator = new RandomGenerator(null) };
            var method = typeof(MKT_Config).GetMethod("GetGeneratorInteger",
                BindingFlags.NonPublic | BindingFlags.Instance);
            var value = method.Invoke(config, new object[] { });
            Assert.AreEqual((int)GeneratorType.Random, value);
        }

        [TestMethod]
        public void TestMethod3()
        {
            var config = new MKT_Config();
            var method = typeof(MKT_Config).GetMethod("GetSeparatorInteger",
                BindingFlags.NonPublic | BindingFlags.Instance);
            var value = method.Invoke(config, new object[] { new Roulette<MKT_Point>() });
            Assert.AreEqual((int)SeparatorType.Roulette, value);
        }

        [TestMethod]
        public void TestMethod4()
        {
            var config = new MKT_Config();
            var method = typeof(MKT_Config).GetMethod("GetSeparatorInteger",
                BindingFlags.NonPublic | BindingFlags.Instance);
            var value = method.Invoke(config, new object[] { new Rang<MKT_Point>(), });
            Assert.AreEqual((int)SeparatorType.Rang, value);
        }

        [TestMethod]
        public void Test4()
        {
            SeparatorType? type = (SeparatorType)17;
            Assert.IsNotNull(type);
        }

        [TestMethod]
        public void Test5()
        {
            var mktChromosome = new MktChromosome();
            for (int i = 0; i < 10; i++)
            {
                var config = new RnadomMKTConfigGenerator()
                    .GenerateEntityArgs(1)
                    .Cast<MKT_Config>()
                    .First();
                var code = mktChromosome.EntityArgsToCode(config);
                var config2 = mktChromosome.CodeToEntityArgs(code);
                Assert.AreEqual(config.ToString(), config2.Args.ToString());
            }
        }

        [TestMethod]
        public void Test6()
        {
            var code = new int[] { 22, 9, 0, 2, 1, 1, 0, 6 };
            var code2 = new int[] { 13, 5, 1, 1, 0, 0, 1, 4 };
            var genom = new MktGenom();
            var method = genom.GetType().GetMethod("TpCroosCode",
                BindingFlags.NonPublic | BindingFlags.Instance);

            var result = (int[]) method.Invoke(genom, new[] {code, code2, (object) 1, 7 });
        }
    }
}
