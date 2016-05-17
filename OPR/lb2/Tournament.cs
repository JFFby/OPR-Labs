using System.Collections.Generic;
using System.Linq;
using OPR.lb2.Interfaces.Common;
using OPR.SSGA2.Interfaces;

namespace OPR.lb2
{
    public class Tournament: ISeparator<IValue>
    {
        public IList<IValue> Separate(IList<IValue> inpuList, int count, bool isAscending)
        {
            int evenPair = 0;
            int oddPair = 0;
            int currentCount = count;
            IList<IValue> tournament = new List<IValue>();
            IList<IValue>[] tournamentPart;


            while(inpuList.Count % currentCount != 0) {
                ++oddPair;
                currentCount -= 3;
            }
            evenPair = inpuList.Count / 2;

            tournamentPart = new List<IValue>[evenPair + oddPair];
            
            for (int i = 0, j = 0; i < evenPair; ++i)
            {
                tournamentPart[i] = new List<IValue>();
                for (var k = 0; k < 2; ++k, ++j)
                {
                    tournamentPart[i].Add(inpuList[j]);
                }
            }

            for (int i = evenPair * 2; i < evenPair * 2 + oddPair; ++i)
            {
                tournamentPart[evenPair].Add(inpuList[i]);
            }

            for (int i = 0; i < tournamentPart.Length; ++i)
            {
                tournament.Add(tournamentPart[i].First(x => x.Value == tournamentPart[i].Min(e => e.Value)));
            }

            return tournament;
        }
    }
}
