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

        #region Swiss.Result.Bye.Participation

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
        public void setInitSwiss(float s)
        {
            InitSwiss = s;
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
        public int getSeed()
        { return Seed; }
        public string getName()
        {
            return Name;
        }
        public int getRating()
        {
            return Rating;
        }

        public bool getParticipation(int i)
        {
            try
            {
                return participation[i];
            }
            catch (Exception e)
            {
                Console.WriteLine("EXCEPTION in getParticipation rnd " + i);
                Console.WriteLine(e.Message);
                Console.WriteLine("par " + participation.Length);
                return false;
            }
        }
        public void setOpponent(int i, int rnd)
        {
            opponent[rnd] = i;
        }
        public int nBye()
        {
            int n = 0;
            for (int i = 0; i < participation.Length; i++)
                if (!participation[i])
                    n++;
            return n;
        }
        #endregion

        #region Tiebreak
        public int getOpponent(int i)
        {
            return opponent[i];
        }

        public int getAdjHandi(int i)   //ATTENTION - Not appropriate for SwissHandicap
        {
            //if black substract handicap , if White add handicap to SOS
            try
            {
                if (BlackWhite[i] == 1)
                    return handi[i] * BlackWhite[i];
                else
                    return handi[i] * -1;
            }
            catch (Exception e)
            {
                Console.WriteLine("EXCEPTION in getAdjHandi rnd " + i);
                Console.WriteLine(e.Message);
                Console.WriteLine("handi " + handi.Length);
                Console.WriteLine("BlackWhite " + BlackWhite.Length);
                return 0;
            }
        }
        #endregion
        public int[] GetOpposition()
        {
            return opponent;
        }
        public override bool Equals(System.Object obj)
        {
            if (obj == null)
                return false;
            try
            {
                Player p = (Player)obj;

                if (Seed > 0)
                {
                    if (Seed == p.Seed)
                        return true;
                }
                if (EGDPin == p.EGDPin)
                    return true;
            }
            catch (Exception e)
            {
                return false;
            }
            return false;
        }
        public static void SetTiebreakers(List<string> _tie)
        {
            Tiebreaker = _tie;
        }
        public float qSc()  //What is this?
        {
            return Swiss - InitSwiss;
        }



        public int CompareTo(Player p)
        {
            if (p.Swiss > Swiss)
                return 1;
            if (p.Swiss == Swiss)
            {
                foreach (string tie in Tiebreaker)
                {
                    if (tie.Equals("SOS"))
                    {
                        if (p.SOS > SOS)
                            return 1;
                        if (p.SOS < SOS)
                            return -1;
                    }
                    if (tie.Equals("SOSOS"))
                    {
                        if (p.SOSOS > SOSOS)
                            return 1;
                        if (p.SOSOS < SOSOS)
                            return -1;
                    }
                    if (tie.Equals("MOS"))
                    {
                        if (p.MOS > MOS)
                            return 1;
                        if (p.MOS < MOS)
                            return -1;
                    }
                    if (tie.Equals("SODOS"))
                    {
                        //SODOS should be split by Wins first
                        if (p.qSc() > qSc())
                            return 1;
                        if (p.qSc() < qSc())
                            return -1;
                        //wins are even
                        if (p.SODOS > SODOS)
                            return 1;
                        if (p.SODOS < SODOS)
                            return -1;
                    }
                }
                return 0;
            }
            return -1;
        }

        #region output
        public override string ToString()
        {
            return Name + " " + Rating + "(" + Swiss + ")";
        }

        public string ToDebug()
        {
            return ToString() + ".IM." + InitSwiss + " S(" + Seed + ")";
        }

        public string ToStore()
        {
            return EGDPin + "\t" + Seed + "\t" + InitSwiss + "\t" + Name;
        }

        public string ToFile()
        {
            char[] c = { ' ' };
            string[] split = Name.Split(c);
            if (split.Length == 2)
                return split[0] + "." + split[1].Substring(0, 1).ToUpper() + "(" + Seed + ")";
            else
                return split[0] + "(" + Seed + ")";
        }

        public string ToStanding(int rnd)
        {
            string s = ToFile();
            s = s + "\t(" + Rank + ")\t(" + Rating + ")\t";
            s = s + getSwiss() + "\t" + getScore(rnd) + "\t";
            for (int i = 0; i < rnd; i++)
            {
                s = s + opponent[i];
                if (score[i] == 0) s = s + "-\t";
                if (score[i] == 1) s = s + "+\t";
                if (score[i] == 0.5) s = s + "=\t";
            }
            return s;
        }
        public string ToStandingVerbose(int rnd)
        {
            string s = ToFile();
            s = s + "\t(" + Rank + ")\t(" + Rating + ")\t";
            s = s + getSwiss() + "\t" + getScore(rnd) + "\t";
            for (int i = 0; i < rnd; i++)
            {
                s = s + opponent[i];
                if (score[i] == 0) s = s + "-";
                if (score[i] == 1) s = s + "+";
                if (score[i] == 0.5) s = s + "=";
                if (BlackWhite[i] == 0) s += "b";
                if (BlackWhite[i] == 1) s += "w";//bye mightbe2
                s += getAdjHandi(i);
                s += "\t";
            }

            return s;
        }

        public string ToEGF()
        {
            string s = Name + " " + Rank + " " + Country + " " + Club + " "; //EGD identifiers
            return s;
        }

        public string EGFColour(int rnd)
        {
            if (BlackWhite[rnd] == 0)
                return "/b";
            return "/w";
        }

        #endregion

    }
}
