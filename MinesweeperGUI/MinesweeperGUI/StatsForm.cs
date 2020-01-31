using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MinesweeperGUI
{
    public partial class StatsForm : Form
    {
        BindingSource bs = new BindingSource();
        List<Player> stats = new List<Player>();
        int currScore = 0;
        public StatsForm(int score)
        {
            InitializeComponent();
            //gets score from the game form and shows it to the player
            currScore = score;
            lb_Score.Text = currScore.ToString();
        }

        private void StatsForm_Load(object sender, EventArgs e)
        {
            //Create the data source for the file
            bs.DataSource = stats;
            listBox1.DataSource = bs;
            listBox1.DisplayMember = ToString();

            //Get and load file into the stats class
            string filePath = @"Z:\Documents\Programming\C#\MinesweeperGUI\MinesweeperGUI\Properties\stats.txt";
            List<String> lines = File.ReadAllLines(filePath).ToList();

            //add all scores in the file as objects to the player list
            foreach (string line in lines) {
                string[] entries = line.Split(',');
                if (entries.Length != 2) {
                    Console.WriteLine("Could not parse into player: " + line);
                }
                else {
                    Player p = new Player();
                    p.Name = entries[0];
                    p.Score = Int32.Parse(entries[1]);
                    stats.Add(p);

                }
            }
            var Top10 =
                (from Player p in stats
                 orderby p.Score descending
                 select p).Take(10);
            //change datasource so that the list is limited to 10
            bs.DataSource = Top10;
            //re load the list box
            bs.ResetBindings(false);

        }

        private void button1_Click(object sender, EventArgs e)
        {
            //create person out of the text field
            Player player = new Player();
            player.Name = textBox1.Text;
            try {
                player.Score = Int32.Parse(lb_Score.Text);
            }
            catch {
                player.Score = -1;
            }

            //add to the list
            stats.Add(player);


            //sort the list in decending order
            var Top10 =
                (from Player p in stats
                 orderby p.Score descending
                 select p).Take(10);

            var sortedList =
                from Player p in stats
                 orderby p.Score descending
                 select p;
            //change datasource so that the new addition is reflected
            bs.DataSource = Top10;
            //re load the list box
            bs.ResetBindings(false);

            //save to  file
            List<string> outputLines = new List<string>();
            //save everyting in the sorted list to the file
            foreach (Player p in sortedList) {
                outputLines.Add(p.Name + ", " + p.Score);
            }
            //write to another file
            string outPath = @"Z:\Documents\Programming\C#\MinesweeperGUI\MinesweeperGUI\Properties\stats.txt";
            File.WriteAllLines(outPath, outputLines);

            //make it so you cant add yourself 100 times
            textBox1.Text = "";
            textBox1.Enabled = false;
            button1.Enabled = false;

        }

        private void button4_Click(object sender, EventArgs e)
        {
            Form1 form = new Form1();
            form.Show();
            this.Hide();
        }
        //close button
        private void button3_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
