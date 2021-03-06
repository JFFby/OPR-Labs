﻿using System;
using OPR.lb1;

namespace OPR.SSGA2
{
    public static class GlobalSettings
    {
        #region Common Settings
        public static bool isRandomOrGridPoints { get; set; }

        public static int firstSelectionVariant { get; set; }

        public static int N { get; set; }

        public static int nFromN { get; set; }

        public static int MutationChance { get; set; }

        public static bool IsCrossingFirst { get; set; }

        public static bool IsBestFromChildernOnly { get; set; }

        public static Func<float, float, float> Fn { get; set; }

        public static float LeftXBound { get; set; }

        public static float RightXBound { get; set; }

        public static float TopYBound { get; set; }

        public static float BottomYBound { get; set; }

        public static int SSGAIterationCount { get; set; }

        public static bool IsTwoCrossingPoints { get; set; }

        public static SquarePoint[] GetBounds()
        {
            return new SquarePoint[]
            {
                new SquarePoint(LeftXBound, BottomYBound),
                new SquarePoint(RightXBound, TopYBound)
            };
        }
        #endregion

        #region BinaryRegion
        public static int MaxIntValueFroCrossing { get; set; }
        #endregion
    }
}
