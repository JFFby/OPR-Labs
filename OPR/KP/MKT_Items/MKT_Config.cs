using System;
using OPR.KP.Shlp;
using OPR.KP.Shlp.NelderMid;
using OPR.KP.SSGA_MKT_Items;
using OPR.KP.SSGA_MKT_Items.Enums;
using OPR.lb2;
using OPR.lb2.Interfaces.Common;
using OPR.SSGA2;

namespace OPR.KP.MKT_Items
{
    public sealed class MKT_Config : EntityArgs
    {
        public IShlpWrapper Shlp { get; set; }

        public IGenerator<MKT_Point> Generator { get; set; }

        public ISeparator<MKT_Point> FirstSeparator { get; set; }

        public ISeparator<MKT_Point> SecondSeparator { get; set; }

        public int N { get; set; }

        public int n { get; set; }

        public byte Lambda { get; set; }

        public MktIterationMode IterationMode { get; set; }

        public static MKT_Config ParseFromArray(int[] code)
        {
            var iterationMode = (MktIterationMode)code[5];
            var generator = GetGenerator(code[2]);
            var shlp = GetShlp(code[4], iterationMode);
            var secondSeparator = GetSeparator(code[6]);
            var firstSeparator = GetSeparator(code[3]);

            if (generator != null && shlp != null
                && firstSeparator != null && secondSeparator != null)
            {
                return new MKT_Config
                {
                    N = code[0],
                    n = code[1],
                    Lambda = (byte)code[7],
                    IterationMode = iterationMode,
                    Generator = generator,
                    Shlp = shlp,
                    SecondSeparator = secondSeparator,
                    FirstSeparator = firstSeparator
                };
            }

            return null;
        }

        public int[] ToArray()
        {
            var resutl = new int[8];
            resutl[0] = N;
            resutl[1] = n;
            resutl[2] = GetGeneratorInteger();
            resutl[3] = GetSeparatorInteger(FirstSeparator);
            resutl[4] = (int)Shlp.ShlpType;
            resutl[5] = (int)IterationMode;
            resutl[6] = GetSeparatorInteger(SecondSeparator);
            resutl[7] = Lambda;

            return resutl;
        }

        public override string ToString()
        {
            return string.Join("_",
                GetBoundString(GlobalSettings.LeftXBound, GlobalSettings.RightXBound),
                GetBoundString(GlobalSettings.BottomYBound, GlobalSettings.TopYBound),
                Shlp.ShlpType, Generator.GetType().Name, FirstSeparator.GetType().Name,
                SecondSeparator.GetType().Name, N, n, Lambda, IterationMode);
        }

        public string GetBoundString(float from, float to)
        {
            return string.Format("{0}-{1}", from, to);
        }

        private static ISeparator<MKT_Point> GetSeparator(int code)
        {
            var type = (SeparatorType)code;
            switch (type)
            {
                case SeparatorType.Best:
                    return new BestSeparator<MKT_Point>();
                case SeparatorType.Rang:
                    return new Rang<MKT_Point>();
                case SeparatorType.Roulette:
                    return new Roulette<MKT_Point>();
                case SeparatorType.Tournament:
                    return new Tournament<MKT_Point>();
                default:
                    return null;
            }
        }

        private static IShlpWrapper GetShlp(int code, MktIterationMode iterationMode)
        {
            var type = (ShlpType)code;
            switch (type)
            {
                case ShlpType.HyperCube:
                    return new HyperCubeWrapper(GlobalSettings.Fn, GlobalSettings.GetBounds(), iterationMode);
                case ShlpType.NelderMid:
                    return new NelderMidWrapper(GlobalSettings.Fn, GlobalSettings.GetBounds(), iterationMode);
                default:
                    return null;
            }
        }

        private static IGenerator<MKT_Point> GetGenerator(int code)
        {
            var type = (GeneratorType)code;
            switch (type)
            {
                case GeneratorType.Grid:
                    return new GridPointsHelper(null);
                case GeneratorType.Random:
                    return new RandomGenerator(null);
                default:
                    return null;
            }
        }

        private int GetSeparatorInteger(ISeparator<MKT_Point> separator)
        {
            if (separator is BestSeparator<MKT_Point>)
            {
                return (int)SeparatorType.Best;
            }

            if (separator is Roulette<MKT_Point>)
            {
                return (int)SeparatorType.Roulette;
            }

            if (separator is Tournament<MKT_Point>)
            {
                return (int)SeparatorType.Tournament;
            }

            if (separator is Rang<MKT_Point>)
            {
                return (int)SeparatorType.Rang;
            }

            throw new ArgumentOutOfRangeException();
        }

        private int GetGeneratorInteger()
        {
            if (Generator is RandomGenerator)
            {
                return (int)GeneratorType.Random;
            }
            else if (Generator is GridPointsHelper)
            {
                return (int)GeneratorType.Grid;
            }

            throw new ArgumentOutOfRangeException();
        }
    }
}
