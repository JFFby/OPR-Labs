using System;
using System.Collections.Generic;
using System.Linq; 
using OPR.lb2.Interfaces.Common;
using OPR.SSGA2;

namespace OPR.KP.MKT_Items
{
    public sealed class BestSeparator : ISeparator<MKT_Point>
    {
        private readonly object state;
        
        public IList<MKT_Point> Separate(IList<MKT_Point> inpuList, int count, bool isAscending)
        {
            //var fn = GetFunc(state);
            var sortedInputs = isAscending
                ? inpuList.OrderBy(x => x.Value)
                : inpuList.OrderByDescending(x => x.Value);
            return sortedInputs.Take(count).ToList();
        }

        private Func<float, float, float> GetFunc(object state)
        {
            var fn = GlobalSettings.Fn;
            if (fn == null)
            {
                throw new  ArgumentException("state");
            }

            return fn;
        } 
    }
}
