using System;

namespace OPR.lb2
{
    public  static class RandomHelper
    {
        private static Random rnd = new Random(DateTime.Now.Millisecond);

        public static float RandomFloat(float min, float max)
        {
            return (float) (min + rnd.NextDouble()*(max - min));
        }

        public static int Random(int min, int max)
        {
            return rnd.Next(min, max);
        }
    }
}
