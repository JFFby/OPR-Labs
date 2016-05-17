using OPR.lb2.Interfaces.Common;
using System.Collections.Generic;
using System.Linq;
using OPR.SSGA2.Interfaces;

namespace OPR.lb2
{
    public class Rang : ISeparator<IValue>
    {

        public IList<IValue> Separate(IList<IValue> inpuList, int count, bool isAscending)
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

            List<IValue> rang = new List<IValue>();

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
