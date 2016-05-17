using System.Collections.Generic;
using System.Linq;
using OPR.lb2.Interfaces.Common;
using OPR.SSGA2.Interfaces;
using System;

namespace OPR.lb2
{
    public class Roulette: ISeparator<IValue>
    {
        public IList<IValue> Separate(IList<IValue> inpuList, int count, bool isAscending)
        {
            float summ = 0, procent = 0;
            int length = inpuList.Count;
            float[] array = new float[length + 1]; array[0] = 0;
            float[] id = new float[count];
            for (var i = 0; i < count; ++i)
            {
                id[i] = -1;
            }

            float value = 0;
            foreach (var el in inpuList)
            {
                summ += el.Value;
            }

            procent = 360 / summ;
            foreach (var el in inpuList)
            {
                value += el.Value * procent;
                array[el.Id] = value;
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

            IList<IValue> roulette = new List<IValue>();

            for (var i = 0; i < count; ++i)
            {
                roulette.Add(inpuList.Where(x => x.Id == id[i]).ToList()[0]);
            }
            return roulette;
        }
    }
}
