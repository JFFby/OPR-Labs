using System;
using System.Collections.Generic;

namespace OPR.KP.MKT_Items
{
    public sealed class MktStepEntities : EventArgs
    {
        public IList<MKT_Point> StartPoints { get; set; }

        public IList<MKT_Point> BestFromStart { get; set; }

        public IList<MKT_Point> ShlpFromBest { get; set; }

        public IList<MKT_Point> LambdaPoints { get; set; }

        public IList<MKT_Point> ShlpFromLambda { get; set; }

        public MKT_Point WorstPoint { get; set; }

        public IList<MKT_Point> TopPoints { get; set; }
    }
}
