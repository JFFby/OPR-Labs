using System.Collections.Generic;
using System.Linq;
using OPR.lb2.Interfaces.Common;
using OPR.SSGA2;
using OPR.SSGA2.Interfaces;

namespace OPR.lb2
{
    public class Tournament<TValueService, TGenom> : ISeparator<Entity<TValueService, TGenom>> where TValueService : IValueService, new()
        where TGenom : IGenom, new()
    {
        public IList<Entity<TValueService, TGenom>> Separate(IList<Entity<TValueService, TGenom>> inpuList, int count, bool isAscending)
        {
            int evenPair = 0;
            int oddPair = 0;
            int currentCount = count;
            IList<Entity<TValueService, TGenom>> tournament = new List<Entity<TValueService, TGenom>>();
            IList<Entity<TValueService, TGenom>> roulette = new List<Entity<TValueService, TGenom>>();
            IList<Entity<TValueService, TGenom>>[] tournamentPart;


            while(inpuList.Count % currentCount != 0) {
                ++oddPair;
                currentCount -= 3;
            }
            evenPair = inpuList.Count / currentCount;

            tournamentPart = new List<Entity<TValueService, TGenom>>[evenPair + oddPair];
            
            for (int i = 0, j = 0; i < evenPair; ++i)
            {
                tournamentPart[i] = new List<Entity<TValueService, TGenom>>();
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
