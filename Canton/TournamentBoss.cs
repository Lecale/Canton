using Ionic.Zip;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;

namespace Canton
{
    class TournamentBoss
    {
        #region variables
        public int nRounds = 0;
        public int currentRound = 0;
        string exeDirectory;
        string TournamentName;
        private string workDirectory;
        bool TopBar = false;
        bool RatingFloor = false;
        bool HandiAboveBar = false;
        bool Verbose = false;
        bool TeaBreak = false; //EndTiebreakers
        int HandiAdjust = 1;
        int nMaxHandicap = 9;
        int nTopBar = 5000;
        int nRatingFloor = 100;
        int nGradeWidth = 100; //to take from Settings
        string PairingStrategy = "Simple";
        List<Player> AllPlayers = new List<Player>();
        List<Player> RoundPlayers;
        List<Pairing> AllPairings = new List<Pairing>();
        List<Pairing> RoundPairings = new List<Pairing>();
        List<string> Tiebreakers = new List<string>(); //to take from Settings
        List<string> EndTiebreakers = new List<string>(); //Final round only
        #endregion
    }
}
