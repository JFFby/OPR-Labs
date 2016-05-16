using OPR.lb2.Interfaces.Common;
using OPR.SSGA2;
using System.Collections.Generic;
using System.Linq;
using System;
using OPR.SSGA2.Interfaces;

namespace OPR.lb2
{
    public class Rang<TValueService, TGenom> : ISeparator<Entity<TValueService, TGenom>> where TValueService : IValueService, new()
        where TGenom : IGenom, new()
    {

        public IList<Entity<TValueService, TGenom>> Separate(IList<Entity<TValueService, TGenom>> inpuList, int count, bool isAscending)
        {
            int[] arrayOfKey = new int[count];
            arrayOfKey[0] = 1;
            arrayOfKey[1] = 3;
            arrayOfKey[2] = 2;
            for (var i = 3; i < count; ++i)
            {
                arrayOfKey[i] = 1;
            }

            var binaryGeneration = inpuList.OrderBy(o => o.Value).ToList();

            List<Entity<TValueService, TGenom>> rang = new List<Entity<TValueService, TGenom>>();

            for (var i = 0; rang.Count < count; ++i)
            {
                for (var j = 0; j < arrayOfKey[i]; ++j)
                {
                    rang.Add(binaryGeneration.ToList()[i]);
                }
            }
            return rang;
        }
    }
}
