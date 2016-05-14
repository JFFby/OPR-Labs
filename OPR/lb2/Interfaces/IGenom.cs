using OPR.lb1;
using System.Collections.Generic;

namespace OPR.lb2.Interfaces
{
     public interface IGenom
     {
         void Initialize(Point<float> point);

         IList<IGenom> NexGeneration(IGenom partner);

         IList<IGenom> Mutation(IGenom[] entities);
     }
}