using System.Collections.Generic;
using System.Linq;
using OPR.lb2.Interfaces.Common;
using OPR.SSGA2.Interfaces;

namespace OPR.lb2
{
    public class Tournament<TValue> : ISeparator<TValue> where TValue : IValue
    {
        public IList<TValue> Separate(IList<TValue> inpuList, int count, bool isAscending)
        {
            int currentCount = count;
            int listCount = inpuList.Count;
            IList<TValue> tournament = new List<TValue>();
            IList<TValue>[] tournamentPart;
            bool isEven = !!(inpuList.Count % 2 == 0);
            bool goodCount = !!(inpuList.Count % count == 0);
            bool getCountIsEven = !!(count % 2 == 0);

            int num = inpuList.Count / count;
            int ostatok = inpuList.Count % count;
            tournamentPart = new List<TValue>[count];
            for (int i = 0, j = 0; i < count; ++i)
            {
                if(i == count - 1)
                {
                    num += ostatok;
                }
                tournamentPart[i] = new List<TValue>();
                for (var k = 0; k < num; ++k, ++j)
                {
                    tournamentPart[i].Add(inpuList[j]);
                }
            }
            for (int i = 0; i < tournamentPart.Length; ++i)
            {
                tournament.Add(tournamentPart[i].First(x => x.Value == tournamentPart[i].Min(e => e.Value)));
            }
            return tournament;
        }
    }
}
