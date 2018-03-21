using System;
using System.Collections.Generic;

namespace Canton
{
    public class McLayer
    {
        public float SwissKey;
        public int Length = 0;

        private List<int> population;
        private Random r = new Random();
        public McLayer(float Swiss, int Seed)
        {
            SwissKey = Swiss;
            population = new List<int>();
            population.Add(Seed);
            Length = 1;
        }

        public void Shuffle()
        {
            if (population.Count > 0)
            {
                int hold = -1;
                int tmp = -1;
                for (int i = 0; i < population.Count; i++)
                {
                    tmp = r.Next(0, population.Count);
                    hold = population[tmp];
                    population[tmp] = population[i];
                    population[i] = hold;
                }
            }
        }

        public int GetAt(int i)
        {
            return population[i];
        }

        public void Add(int _seed)
        {
            population.Add(_seed);
            Length++;
        }

        public void Remove(int _Seed)
        {
            if (population.Count > 0)
            {
                population.Remove(_Seed);
                Length--;
            }
        }

    }
}

