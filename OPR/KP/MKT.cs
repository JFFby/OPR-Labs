using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using OPR.KP.MKT_Items;
using OPR.KP.Shlp;
using OPR.lb1;
using OPR.lb2.Interfaces.Common;

namespace OPR.KP
{
    public sealed class MKT
    {
        private readonly IShlpWrapper shlpWrapper;
        private readonly ISeparator<MKT_Point> separator;
        private readonly IGenerator<MKT_Point> generator;
        private SquarePoint[] bounds;
        private int iterations;
        private int currentIteration;
        private int N;
        private int n;
        private byte lambda;
        private Func<float, float, float> fn;
        private IList<MKT_Point> topPointsForNextIteration;

        public EventHandler OnEnd { get; set; }

        public MKT(MKT_Config config)
        {
            this.shlpWrapper = config.Shlp;
            this.separator = config.Separator;
            this.generator = config.Generator;
            this.iterations = config.Iterations;
            this.bounds = config.Bounds;
            this.lambda = config.Lambda;
            this.N = config.N;
            this.n = config.n;
            this.fn = config.Fn;
        }

        public MKT_Point Solve()
        {
            var initialPoints = GeneratePoints(N);
            return Iteration(initialPoints);
        }

        public MktStepEntities Step()
        {
            var points = topPointsForNextIteration ?? GeneratePoints(N);
            var entities = GenreateEntitesForThisStep(points);
            topPointsForNextIteration = entities.TopPoints;

            if (++currentIteration == iterations && OnEnd != null)
            {
                var bestPoint = separator.Separate(entities.TopPoints, 1, true, fn).First();
                OnEnd(bestPoint, new EventArgs());
            }

            return entities;
        }

        private MKT_Point Iteration(IList<MKT_Point> points)
        {
            var entities = GenreateEntitesForThisStep(points);
            if (++currentIteration < iterations)
            {
                Iteration(entities.TopPoints);
            }

            return separator.Separate(entities.TopPoints, 1, true, fn).First();
        }

        private MktStepEntities GenreateEntitesForThisStep(IList<MKT_Point> points)
        {
            var best = separator.Separate(points, n, true, fn);
            var worst = separator.Separate(points, 1, false, fn).First();
            var shpFromBestPoints = best.Select(Shlp);
            var lambdaPoints = GeneratePoints(lambda);
            var additionalPoints = ShlpFromLambdaPoints(lambdaPoints, worst);
            var topPoints = separator.Separate(shpFromBestPoints
                .Union(additionalPoints)
                .ToList(),
                n,
                true,
                fn);

            return new MktStepEntities
            {
                BestFromStart = best,
                LambdaPoints = lambdaPoints,
                ShlpFromBest = shpFromBestPoints.ToList(),
                ShlpFromLambda = additionalPoints,
                StartPoints = points,
                WorstPoint = worst,
                TopPoints = topPoints
            };
        }

        private IList<MKT_Point> ShlpFromLambdaPoints(IList<MKT_Point> points, MKT_Point worstPoint)
        {
            var suitablePoints = new List<MKT_Point>();
            var watch2 = Stopwatch.StartNew();
            foreach (var mktPoint in points)
            {
                var shlpValue = Shlp(mktPoint);
                if (shlpValue.Value < worstPoint.Value)
                {
                    suitablePoints.Add(shlpValue);
                }
            }

            watch2.Stop();

            return suitablePoints;
        }

        private MKT_Point Shlp(MKT_Point point)
        {
            var result = shlpWrapper.GetShlpObject(point).Calculate();
            return new MKT_Point(result.x, result.y, point);
        }

        private IList<MKT_Point> GeneratePoints(int count)
        {
            return generator.Generate(count, new Genereate_MKT_Point_Arg { Bounds = bounds, fn = fn });
        }
    }
}
