using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OPR.lb2
{
    class GridPointsHelper
    {
        public static float[] getStepOfGrid(float width, float height, int number)
        {
            float widthLineCount = 0, heightLineCount = 0;
            float[] arrayStep = new float[2];
            bool stop = false;
            
            for (widthLineCount = 0; widthLineCount <= 10;)
            {
                if (stop)
                {
                    break;
                }
                ++widthLineCount;
                for (heightLineCount = 1; heightLineCount <= widthLineCount; ++heightLineCount)
                {
                    if (widthLineCount * heightLineCount == number)
                    {
                        stop = true;
                        break;
                    }
                }
            }

            arrayStep[0] = width / widthLineCount;
            arrayStep[1] = height / heightLineCount;

            return arrayStep;
        }
    }
}
