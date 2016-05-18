using System.Collections.Generic;
using System.Linq;
using OPR.lb2.Interfaces.Common;
using OPR.SSGA2.Interfaces;
using System;

namespace OPR.lb2
{
    public class Roulette<TValue> : ISeparator<TValue> where TValue : IValue
    {
        public IList<TValue> Separate(IList<TValue> inpuList, int count, bool isAscending)
        {
            if (inpuList.Count == count)
            {
                return inpuList;
            }

            float summ = 0, procent = 0;
            var items = GetRouletteItem(inpuList);
            int length = items.Count;
            float[] array = new float[length + 1]; array[0] = 0;
            float[] id = new float[count];
            for (var i = 0; i < count; ++i)
            {
                id[i] = -1;
            }

            float value = 0;
            foreach (var el in items)
            {
                summ += el.Entity.Value;
            }

            procent = 360 / summ;
            foreach (var el in items)
            {
                value += el.Entity.Value * procent;
                array[el.RouletteItemId] = value;
            }

            var num = 0;
            for (; num != count;)
            {
                float sector = (float)Math.Round(RandomHelper.RandomFloat(0, 360), 1);
                for (var i = 1; i < array.Length; ++i)
                {
                    if (array[i - 1] < sector && sector <= array[i])
                    {
                        if (Array.IndexOf(id, i) == -1)
                        {
                            id[num] = i;
                            num++;
                            break;
                        }
                    }
                }
            }

            IList<RouletteItem<TValue>> roulette = new List<RouletteItem<TValue>>();

            for (var i = 0; i < count; ++i)
            {
                roulette.Add(items.Where(x => x.RouletteItemId == id[i]).ToList()[0]);
            }

            return roulette.Select(x => x.Entity).ToList();
        }

        private List<RouletteItem<TValue>> GetRouletteItem<TValue>(IEnumerable<TValue> entities) where TValue : IValue
        {
            return entities.Select((x, i) => new RouletteItem<TValue>(x, i)).ToList();
        }
    }

    public class RouletteItem<TValue> where TValue : IValue
    {
        public RouletteItem(TValue entity, int i)
        {
            Entity = entity;
            RouletteItemId = i;
        }

       public TValue Entity { get; set; }

        public int RouletteItemId { get; set; }
    }
}
