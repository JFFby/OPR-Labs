using System;

namespace OPR.SSGA2
{
    public static class GlobalSettings
    {
        public static bool IsBestFromChildernOnly { get; set; }

        public static int nFromN { get; set; }

        public static int MutationChance { get; set; }

        public static bool IsCrossingFirst { get; set; }

        public static Func<float, float, float> Fn { get; set; } 
    }
}
