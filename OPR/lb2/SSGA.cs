using System;
using System.Collections.Generic;
using System.Linq;
using OPR.lb1;
using OPR.lb2.Interfaces.Common;
using OPR.lb2.Enums;

namespace OPR.lb2
{
    public sealed class SSGA: ISelection
    {
        private readonly float[] interval_x = new float[] { -4, 4 };
        private readonly float[] interval_y = new float[] { -4, 4 };
        private readonly List<BinaryGeneration> generations = new List<BinaryGeneration>();
        private readonly byte N = 10;
        private readonly byte count = 10;
        private readonly bool randomOrGrid = false;
        private readonly int indexFirstSelection;

        public SSGA(float[] interval_x, float[] interval_y, byte N, byte n, bool randomOrGridStatus, int indexFirstSelection)
        {
            randomOrGrid = randomOrGridStatus;
            count = n;
            this.N = N;
            this.indexFirstSelection = indexFirstSelection;
            this.interval_x = interval_x;
            this.interval_y = interval_y;
        }

        public List<BinaryGeneration> Start()
        {
            var firstGeneration = CreateFirstPopulation();
            generations.Add(firstGeneration);
            return generations;
        }

        public List<BinaryGeneration> EvalutionStep()
        {
            var generation = generations.Last();
            var bestEntities = generation.GetBest(2);
            var children = ((BinaryEntity)bestEntities[0]).NextGeneration(bestEntities[1] as BinaryEntity,
                x => fn(x.X.Value, x.Y.Value));
            var usefunChildren = children.Where(x => IsPointValid(x.Genom.X.Value, x.Genom.Y.Value)).ToList();
            var nextGeneration = CreateNextGeneration(generation, usefunChildren);
            generation.AddChilds(children);
            generations.Add(nextGeneration);
            return generations;
        }

        private BinaryGeneration CreateNextGeneration(BinaryGeneration generation, IList<BinaryEntity> children)
        {
            if (children.Any())
            {
                var bestChild = children.First(x => Math.Abs(x.Value - children.Min(c => c.Value)) < 0.01);
                bestChild.Function = EntityFunction.BestChild;
                var nextGeneration = new List<Entity<BinaryGenom>>(generation.Winners());
                nextGeneration.Add(bestChild);
                return new BinaryGeneration(nextGeneration
                    .OfType<BinaryEntity>()
                    .Select(x => new BinaryEntity(x))
                    .OfType<Entity<BinaryGenom>>()
                    .ToList())
                    .MarkUpGenereation();
            }

            return generation;
        }

        private BinaryGeneration Selection_nOfN()
        {
            float[] step = new float[2];
            float[] stepPoint = new float[2];
            stepPoint[0] = interval_x[0];
            stepPoint[1] = interval_y[0];
            var entites = new List<Entity<BinaryGenom>>();
            step = (randomOrGrid) ?
                step :
                GridPointsHelper.getStepOfGrid((interval_x[1] - interval_x[0]), (interval_y[1] - interval_y[0]), N);

            for (int i = 0; i < N; i++)
            {
                Point<float> point;
                if (randomOrGrid)
                {
                    point = new Point<float>(
                    (float)Math.Round(RandomHelper.RandomFloat(interval_x[0], interval_x[1]), 1),
                    (float)Math.Round(RandomHelper.RandomFloat(interval_y[0], interval_y[1]), 1));
                }
                else
                {
                    point = new Point<float>(stepPoint[0], stepPoint[1]);
                    stepPoint[0] += step[0];
                    if(stepPoint[0] >= interval_x[1] - step[0])
                    {
                        stepPoint[0] = interval_x[0];
                        stepPoint[1] += step[1];
                    }
                }

                entites.Add(new BinaryEntity(point, fn(point.x, point.y)));
            }
            return new BinaryGeneration(entites);
        }

        private BinaryGeneration CreateFirstPopulation()
        {
            var entites = Selection_nOfN();
            
            switch (indexFirstSelection)
            {
                case 1: entites.Entities = getChildOfTournament(entites.Entities); break;
                case 2: entites.Entities = getChildOfRang(entites.Entities); break;

                default: entites.Entities = getChildOfRoulette(entites.Entities); break;
            }
            
            return entites.MarkUpGenereation();
        }

        public IList<BinaryEntity> getChildOfRoulette(IList<BinaryEntity>  binaryGeneration)
        {
            float summ = 0, procent = 0;
            int length = binaryGeneration.Count;
            float[] array = new float[length + 1]; array[0] = 0;
            float[] id = new float[count]; 
            for(var i = 0; i < count; ++i)
            {
                id[i] = -1;
            }

            float value = 0;
            foreach (var el in binaryGeneration)
            {
                summ += el.Value;
            }

            procent = 360 / summ;
            foreach (var el in binaryGeneration)
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

            List<BinaryEntity> roulette = new List<BinaryEntity>();

            for (var i = 0; i < count; ++i)
            {
                roulette.Add(binaryGeneration.Where(x => x.Id == id[i]).ToList()[0]);
            }
            return roulette;
        }

        public IList<BinaryEntity> getChildOfRang(IList<BinaryEntity> binaryGeneration)
        {
            int[] arrayOfKey = new int[count];
            arrayOfKey[0] = 2;
            arrayOfKey[1] = 2;
            arrayOfKey[2] = 2;
            for (var i = 3; i < count; ++i)
            {
                arrayOfKey[i] = 1;
            }

            binaryGeneration = binaryGeneration.OrderBy(o => o.Value).ToList();

            List<BinaryEntity> rang = new List<BinaryEntity>();


            for (var i = 0; rang.Count < count; ++i)
            {   for(var j = 0; j < arrayOfKey[i]; ++j)
                {
                    rang.Add(binaryGeneration.ToList()[i]);
                }
                
            }
            return rang;
        }

        public IList<BinaryEntity> getChildOfTournament(IList<BinaryEntity> binaryGeneration)
        {
            var evenNumber = (binaryGeneration.Count % 2 == 0);
            int countOfPair = 0;
            List<BinaryEntity> tournament = new List<BinaryEntity>();
            List<BinaryEntity> roulette = new List<BinaryEntity>();
            List<BinaryEntity>[] tournamentPart;
            if (evenNumber)
            {
                countOfPair = binaryGeneration.Count / 2;
                tournamentPart = new List<BinaryEntity>[countOfPair];

                for (int i = 0, j = 0; i < countOfPair; ++i)
                {
                    tournamentPart[i] = new List<BinaryEntity>();
                    for (var k = 0; k < 2; ++k, ++j)
                    {
                        tournamentPart[i].Add(binaryGeneration[j]);
                    }
                }
                
                for (int i = 0; i < tournamentPart.Length; ++i)
                {
                    if (tournamentPart[i][0].Value < tournamentPart[i][1].Value)
                    {
                        tournament.Add(tournamentPart[i][0]);
                    }
                    else
                    {
                        tournament.Add(tournamentPart[i][1]);
                    }
                }
            }
                else
            {
                countOfPair = (binaryGeneration.Count - 3) / 2;
                tournamentPart = new List<BinaryEntity>[countOfPair];

                for (int j = 0; j < 3; ++j)
                {
                    tournamentPart[0].Add(binaryGeneration[j]);
                }

                for (int i = 1, j = 3; i < countOfPair + 1; ++i)
                {
                    for (var k = 0; k < 2; ++k, ++j)
                    {
                        tournamentPart[i].Add(binaryGeneration[j]);
                    }
                }

                if (tournamentPart[0][0].Value > tournamentPart[0][1].Value)
                {
                    if (tournamentPart[0][1].Value > tournamentPart[0][2].Value)
                    {
                        tournament.Add(tournamentPart[0][2]);
                    }
                    else
                    {
                        tournament.Add(tournamentPart[0][1]);
                    }
                }
                else
                {
                    if (tournamentPart[0][0].Value > tournamentPart[0][2].Value)
                    {
                        tournament.Add(tournamentPart[0][2]);
                    }
                    else
                    {
                        tournament.Add(tournamentPart[0][0]);
                    }
                }






                for (int i = 1; i < tournamentPart.Length; ++i)
                {
                    if (tournamentPart[i][0].Value < tournamentPart[i][1].Value)
                    {
                        tournament.Add(tournamentPart[i][0]);
                    }
                    else
                    {
                        tournament.Add(tournamentPart[i][1]);
                    }
                }
            }

           

            return tournament;
        }

        private bool IsPointValid(float x, float y)
        {
            return x > interval_x[0] && x < interval_x[1] && y > interval_y[0] && y < interval_y[1];
        }

        private float fn(float x, float y)
        {
            return (float)(Math.Pow(x, 2) + Math.Pow(y, 2));
            ///return (float)(100 * Math.Pow((y - Math.Pow(x, 2)), 2) + Math.Pow((1 - x), 2));
            ///return (float)(Math.Pow((y - Math.Pow(x, 2)), 2) + Math.Pow((1 - x), 2));
        }

    }
}
