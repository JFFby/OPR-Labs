using System;
using System.Collections.Generic;
using System.Linq;
using OPR.lb1;
using OPR.lb2.Interfaces.Common;

namespace OPR.KP.MKT_Items
{
    public sealed class BestSeparator : ISeparator<MKT_Point>
    {
        public IList<MKT_Point> Separate(IList<MKT_Point> inpuList, int count, bool isAscending, object state)
        {
            //var fn = GetFunc(state);
            var sortedInputs = isAscending
                ? inpuList.OrderBy(x => x.Value)
                : inpuList.OrderByDescending(x => x.Value);
            return sortedInputs.Take(count).ToList();
        }

        private Func<float, float, float> GetFunc(object state)
        {
            var fn = state as Func<float, float, float>;
            if (fn == null)
            {
                throw new  ArgumentException("state");
            }

            return fn;
        } 
    }
}
