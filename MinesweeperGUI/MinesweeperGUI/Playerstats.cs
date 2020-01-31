using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinesweeperGUI
{
    class Playerstats
    {   public string CurrentDifficulty { get; set; }
        public int Modifier { get; set; }
        public int TimeModifier { get; set; }
        public int FinalTime { get; set; }//in seconds

        public Playerstats()
        {
        }
        public int GetScore()
        {
            int finalScore = 0;
            if (CurrentDifficulty.Equals("easy")) {
                Modifier = 3;
                TimeModifier = 300;
            }
            else if (CurrentDifficulty.Equals("medium")) {
                Modifier = 2;
                TimeModifier = 720;
            }
            else if (CurrentDifficulty.Equals("hard")) {
                Modifier = 4;
                TimeModifier = 900;

            }

            //Score will be time -
            finalScore = TimeModifier - FinalTime;
            if (finalScore < 0)
                return 0;
            else
                return finalScore * Modifier;
        }

    }
}
