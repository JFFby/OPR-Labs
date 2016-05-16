using System;
using System.Collections.Generic;
using System.Linq; 
using OPR.lb2.Interfaces.Common;
using OPR.SSGA2;
using OPR.SSGA2.Interfaces;

namespace OPR.KP.MKT_Items
{
    public sealed class BestSeparator<TValue> : ISeparator<TValue> where TValue: IValue
    {
        private readonly object state;
        
        public IList<TValue> Separate(IList<TValue> inpuList, int count, bool isAscending)
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
