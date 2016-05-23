using System;
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
        public static int[] nBounds = new[] {3, 9};
        public static int[] NBounds = new[] {9, 25};

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
                n = RandomHelper.Random(nBounds[0], nBounds[1]),
                N = RandomHelper.Random(NBounds[0], NBounds[1]),
                Generator = GetGenerator(),
                FirstSeparator = GetSeparator(),
                SecondSeparator = GetSeparator(),
                Shlp = GetShlp(iterationMode),
                Lambda = (byte)RandomHelper.Random(nBounds[0], nBounds[1]),
                IterationMode = iterationMode
            };
        }

        private ISeparator<MKT_Point> GetSeparator()
        {
            var separatorType = RandomHelper.Random(0, 100) % 4;
            switch (separatorType)
            {
                case 0:
                    return new Roulette<MKT_Point>();//new Roulette<MKT_Point>();
                case 1:
                    return new Tournament<MKT_Point>();
                case 2:
                    return new Rang<MKT_Point>();
                case 3:
                    return new BestSeparator<MKT_Point>();
                default:
                    throw new ArgumentException();
            }
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
