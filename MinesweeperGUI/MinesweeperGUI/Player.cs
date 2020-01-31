using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinesweeperGUI
{
    class Player : IComparable<Player>
    {
        public string Name { get; set; }
        public int Score { get; set; }

        public Player()
        {
        }

        public Player(string name, int score)
        {
            this.Name = name;
            this.Score = score;
        }

        public override string ToString()
        {
            return Name + ", " +Score;
        }
        public int CompareTo(Player p)
        {
            return p.Score.CompareTo(this.Score);
        }
    }
}
