using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using OPR.KP.MKT_Items;
using OPR.KP.Shlp;
using OPR.lb1;
using OPR.lb2.Interfaces.Common;
using OPR.SSGA2;

namespace OPR.KP
{
    public sealed class MKT
    {
        private readonly IShlpWrapper shlpWrapper;
        private readonly ISeparator<MKT_Point> firstSeparator;
        private readonly ISeparator<MKT_Point> secondSeparator;
        private readonly IGenerator<MKT_Point> generator;
        private readonly SquarePoint[] bounds;
        private readonly int iterations;
        private int currentIteration;
        private readonly int N;
        private readonly int n;
        private readonly byte lambda;
        private Func<float, float, float> fn;
        private IList<MKT_Point> topPointsForNextIteration;

        public EventHandler OnEnd { get; set; }

        public MKT(MKT_Config config)
        {
            this.shlpWrapper = config.Shlp;
            this.firstSeparator = config.FirstSeparator;
            secondSeparator = config.SecondSeparator;
            this.generator = config.Generator;
            this.iterations = 3;
            this.bounds = GlobalSettings.GetBounds();
            this.lambda = config.Lambda;
            this.N = config.N;
            this.n = config.n;
            this.fn = GlobalSettings.Fn;
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
                var bestPoint = firstSeparator.Separate(entities.TopPoints, 1, true).First();
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

            return firstSeparator.Separate(entities.TopPoints, 1, true).First();
        }

        private MktStepEntities GenreateEntitesForThisStep(IList<MKT_Point> points)
        {
            var best = firstSeparator.Separate(points, n, true);
            var worst = firstSeparator.Separate(points, 1, false).First();
            var shpFromBestPoints = best.Select(Shlp);
            var lambdaPoints = GeneratePoints(lambda);
            var additionalPoints = ShlpFromLambdaPoints(lambdaPoints, worst);
            var topPoints = secondSeparator.Separate(shpFromBestPoints
                .Union(additionalPoints)
                .ToList(),
                n,
                true);

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

            var state = new
            {
                state = new Genereate_MKT_Point_Arg { Bounds = bounds, fn = fn }
            };

            generator.SetupState(state);
            return generator.Generate(count);
        }
    }
}
