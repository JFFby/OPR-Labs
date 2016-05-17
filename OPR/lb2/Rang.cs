using System;
using OPR.lb2.Interfaces.Common;
using System.Collections.Generic;
using System.Linq;
using OPR.SSGA2.Interfaces;

namespace OPR.lb2
{
    public class Rang<TValue> : ISeparator<TValue> where TValue : IValue
    {

        public IList<TValue> Separate(IList<TValue> inpuList, int count, bool isAscending)
        {
            int[] arrayOfKey = new int[Math.Max(count, 3)];
            arrayOfKey[0] = 1;
            arrayOfKey[1] = 3;
            arrayOfKey[2] = 2;
            for (var i = 3; i < count; ++i)
            {
                arrayOfKey[i] = 1;
            }

            var binaryGeneration = inpuList.OrderBy(o => o.Value).ToList();

            List<TValue> rang = new List<TValue>();

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
