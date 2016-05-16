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
            var copyOfInput = new List<TValue> (inpuList);
            var sortedInputs = isAscending
                ? copyOfInput.OrderBy(x => x.Value)
                : copyOfInput.OrderByDescending(x => x.Value);
            var winners = sortedInputs.Take(count).ToList();
            return inpuList.Where(x => winners.Contains(x)).ToList();
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
