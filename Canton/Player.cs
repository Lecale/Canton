using System;
using System.Collections.Generic;

namespace Canton
{
    public class Player: IComparable<Player>
    {
        public string Name;
        public string Club;
        public string Country;
        public int Rating;
            public int Seed;
        public string Rank;
        public float Swiss;
        public float InitSwiss;
        protected float[] score;
        protected bool[] participation;
        protected int[] opponent;
        protected int[] handi;
        protected int[] BlackWhite; //Black 0 White 1
        public float SOS = -1; //Solkoff
        public float MOS = -1; //Median
        public float SODOS = -1; //Sonnenborg-Bergen
        public float SOSOS = -1; //
        public int EGDPin;
            public int Deed = -1; //Deed is the draw seeding for a particular round

        private static List<string> Tiebreaker = new List<string>();

        public Player(int _seed, string _nom, int _rat, string _ctry, string _club, bool[] par, string _grd)
        {
            Seed = -1; 
            EGDPin = _seed;
            Rating = _rat;
            Rank = _grd;
            participation = new bool[par.Length];
            score = new float[par.Length];
            BlackWhite = new int[par.Length];
            handi = new int[par.Length];
            opponent = new int[par.Length];
            for (int i = 0; i < par.Length; i++)
                participation[i] = par[i];
        }

        public void setResult(int rnd, int op, float _score, int _handicap = 0, int BW = 1)
        {
            rnd--; //0 based arrary as always
            participation[rnd] = true; //manually removed byes must be erased
            score[rnd] = _score;
            opponent[rnd] = op;
            handi[rnd] = _handicap;
            BlackWhite[rnd] = BW;
            setSwiss(getSwiss());
        }

        public float getSwiss()
        {
            float f = InitSwiss;
            if (opponent != null)
                for (int i = 0; i < opponent.Length; i++)
                    f += score[i];
            return f;
        }
        public float getSwiss(int rnd)
        {
            return InitSwiss + getScore(rnd);
        }
        public void setSwiss(float s)
        {
            Swiss = s;
        }
        public float getResult(int rnd)
        {
            return score[rnd];
        }
        public float getScore(int rnd)
        {
            float f = 0;
            for (int i = 0; i < rnd; i++)
                f += score[i];
            return f;
        }
        public void AssignBye(int rnd)
        {
            rnd--; //0 based arrary as always
            participation[rnd] = false;
            score[rnd] = 0.5f;
            setSwiss(getSwiss(rnd));
        }
        public void SetParticipation(int rnd, bool play = true)
        {
            participation[rnd] = play;
        }

        public void SetSeed(int s)
        {
            Seed = s;
        }

        public int CompareTo(Player p)
        {
            return -1;
        }
    }
}
