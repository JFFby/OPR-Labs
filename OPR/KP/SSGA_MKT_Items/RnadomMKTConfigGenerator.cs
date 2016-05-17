using System.Collections.Generic;
using OPR.KP.MKT_Items;
using OPR.KP.Shlp;
using OPR.KP.Shlp.NelderMid;
using OPR.lb2;
using OPR.lb2.Interfaces.Common;
using OPR.SSGA2;
using OPR.SSGA2.Italik;

namespace OPR.KP.SSGA_MKT_Items
{
    public sealed class RnadomMKTConfigGenerator : IArgsGenerator
    {
        public IList<EntityArgs> GenerateEntityArgs(int count)
        {
            var result = new List<EntityArgs>();
            for (int i = 0; i < count; i++)
            {
                result.Add(CreateMktConfig());
            }

            return result;
        }

        private MKT_Config CreateMktConfig()
        {
            var iterationMode = RandomHelper.Random(1, 10) % 2 == 0
                ? MktIterationMode.Full
                : MktIterationMode.Limited;

            return new MKT_Config
            {
                n = RandomHelper.Random(3, 9),
                N = RandomHelper.Random(9, 25),
                Generator = GetGenerator(),
                Separator = new BestSeparator<MKT_Point>(),
                Shlp = GetShlp(iterationMode),
                Lambda = (byte)RandomHelper.Random(3, 9),
                IterationMode = iterationMode
            };
        }

        private IShlpWrapper GetShlp(MktIterationMode iterationMode)
        {
            var type = RandomHelper.Random(1, 10);
            return type % 2 == 0
                 ? (IShlpWrapper)new NelderMidWrapper(GlobalSettings.Fn, GlobalSettings.GetBounds(), iterationMode)
                 : new HyperCubeWrapper(GlobalSettings.Fn, GlobalSettings.GetBounds(), iterationMode);
        }

        private IGenerator<MKT_Point> GetGenerator()
        {
            var type = RandomHelper.Random(1, 10);
            var generator = type % 2 == 0
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
    }
}
