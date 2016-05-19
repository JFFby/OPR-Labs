using System;

namespace OPR.lb2
{
    public  static class RandomHelper
    {
        private static Random rnd = new Random(DateTime.Now.Millisecond);
        private static int jffCount = 0;

        public static float RandomFloat(float min, float max)
        {
            ++jffCount;
            return (float) (min + rnd.NextDouble()*(max - min));
        }

        public static int Random(int min, int max)
        {
            ++jffCount;
            return rnd.Next(min, max);
        }
    }
}
