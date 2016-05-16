using OPR.KP.MKT_Items;
using OPR.lb2.Interfaces.Common;
using OPR.SSGA2;
using OPR.SSGA2.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OPR.lb2
{
    public sealed class GridPointsHelper : IGenerator<MKT_Point>
    {
        public IConverter converter;
        public object state;

        public void RandomGenerator(IConverter converter)
        {
            this.converter = converter;
        }

        public IList<EntityArgs> GenerateEntityArgs(int count)
        {
            return Generate(count).Select(converter.Convert).ToList();
        }

        public void SetupState(dynamic state)
        {
            this.state = state.state;
        }

        private Genereate_MKT_Point_Arg GetBounds()
        {
            var bounds = state as Genereate_MKT_Point_Arg;
            if (bounds == null || bounds.Bounds.Length != 2)
            {
                throw new ArgumentException("state");
            }

            return bounds;
        }

        public IList<MKT_Point> Generate(int count)
        {
            var points = new List<MKT_Point>();
            var bounds = GetBounds();
            var pointN = GlobalSettings.N;

            float[] stepPoint = new float[2];
            stepPoint[0] = bounds.Bounds[0].x;
            stepPoint[1] = bounds.Bounds[0].y;

            float width = bounds.Bounds[0].x + bounds.Bounds[1].x;
            float height = bounds.Bounds[0].y + bounds.Bounds[1].y;

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
                    if (widthLineCount * heightLineCount == pointN)
                    {
                        stop = true;
                        break;
                    }
                }
            }

            arrayStep[0] = width / widthLineCount;
            arrayStep[1] = height / heightLineCount;

            for (int i = 0; i < pointN; i++)
            {
                points.Add(new MKT_Point(stepPoint[0], stepPoint[1], bounds.fn));
                stepPoint[0] += arrayStep[0];
                if (stepPoint[0] >= bounds.Bounds[1].x - arrayStep[0])
                {
                    stepPoint[0] = bounds.Bounds[0].x;
                    stepPoint[1] += arrayStep[1];
                }
            }

            return points;
        }
    }
}
