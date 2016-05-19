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

            bool onMinimum = true;
            float summ = 0, procent = 0;
            var items = GetRouletteItem(inpuList.OrderBy(x => x.Value));
            var lastItem = inpuList.Max(x => x.Value);
            var tempList = new float[items.Count];
            int length = items.Count;
            float[] array = new float[length+1]; array[0] = 0;
            float[] id = new float[count];
            for (var i = 0; i < count; ++i)
            {
                id[i] = -1;
            }

            var num = 0;
            foreach (var el in items)
            {
                tempList[num++] = lastItem - el.Entity.Value;
            }

            float value = 0;
            foreach (var number in tempList)
            {
                summ += number;
            }

            procent = 360 / summ;
            num = 0;
            foreach (var el in items)
            {
                value += tempList[num++] * procent;
                array[el.RouletteItemId] = value;
            }

            num = 0;
            for (; num != count;)
            {
                float sector = (float)Math.Round(RandomHelper.RandomFloat(0, 360), 1);
                for (var i = 1; i < array.Length - 1; ++i)
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
            RouletteItemId = i + 1;
        }

       public TValue Entity { get; set; }

        public int RouletteItemId { get; set; }
    }
}