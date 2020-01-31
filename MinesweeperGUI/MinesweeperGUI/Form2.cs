using MinesweeperLibrary;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MinesweeperGUI
{
    public partial class Form2 : Form
    {
        string difficulty = "";
        //board that will hold cells 
        static Board myBoard;
        //2d array of buttons
        public Button[,] btnGrid;
        Stopwatch watch = new Stopwatch();
        Playerstats scoring = new Playerstats();
        public Form2(string choice)
        {
            InitializeComponent();
            //autosize to buttons panel width and height
            this.AutoSize = true;
            //set difficulty as global variable
            difficulty = choice;
            scoring.CurrentDifficulty = difficulty;
            //set up the board and button grid
            setBoard();

        }
        //sets and resets the board
        private void setBoard()
        {
            //clear all buttons on the panel
            btnPanel.Controls.Clear();
            //replace all buttons on the panel and reset the button grid to a new layout
            populateGrid(difficulty);
            //reset and start the stopwatch
            watch.Reset();
            watch.Start();
        }
        //Set button grid and board sizes and amount of bombs.
        private void populateGrid(string difficulty)
        {
            int buttonSize = 50;
            //Create Sizes Based on Grid
            //easy is 10x10
            if (difficulty.Equals("easy")) {
                btnPanel.Height = 50 * 10;
                btnPanel.Width = btnPanel.Height;
                myBoard = new Board(10);
                btnGrid = new Button[myBoard.Size, myBoard.Size];
                myBoard.setDifficulty(7);
                myBoard.setupLiveNeighbors();
                myBoard.calculateLiveNeighbors();
            }
            //medium is 15x15
            else if (difficulty.Equals("medium")) {
                btnPanel.Height = 50 * 15;
                btnPanel.Width = btnPanel.Height;
                myBoard = new Board(15);
                btnGrid = new Button[myBoard.Size, myBoard.Size];
                myBoard.setDifficulty(10);
                myBoard.setupLiveNeighbors();
                myBoard.calculateLiveNeighbors();
            }
            //hard is 20x20
            else if (difficulty.Equals("hard")) {
                btnPanel.Height = 50 * 18;
                btnPanel.Width = btnPanel.Height;
                myBoard = new Board(18);
                btnGrid = new Button[myBoard.Size, myBoard.Size];
                myBoard.setDifficulty(17);
                myBoard.setupLiveNeighbors();
                myBoard.calculateLiveNeighbors();
            }
            else {
                btnPanel.Height = 50 * 2;
                btnPanel.Width = btnPanel.Height;
                myBoard = new Board(2);
                btnGrid = new Button[myBoard.Size, myBoard.Size];
            }

            //
            for (int i = 0; i < myBoard.Size; i++) {
                for (int j = 0; j < myBoard.Size; j++) {

                    btnGrid[i, j] = new Button();
                    btnGrid[i, j].Height = buttonSize;
                    btnGrid[i, j].Width = buttonSize;
                    btnGrid[i, j].Font = new Font(btnGrid[i, j].Font.FontFamily, 14);
                    //add click event to each Button
                    btnGrid[i, j].MouseUp += Grid_Button_MouseUp;
                    btnPanel.Controls.Add(btnGrid[i, j]);

                    //set the buttons physical location and the text of the button
                    btnGrid[i, j].Location = new Point(i * buttonSize, j * buttonSize);
                    btnGrid[i, j].Text = "";

                    //data stored by the button that is not displayed
                    //this can be almost anything, an array, text a number 
                    btnGrid[i, j].Tag = new Point(i, j);
                }

            }
        }

        

        //Using the mouse up event because it can detect a left and right click event
        private void Grid_Button_MouseUp(object sender, MouseEventArgs e)
        {
            //get the row and column of the button clicked
            //sender is the object clicked - check parameters
            //this is how you get what button was clicked
            Button clickedButton = (Button)sender;
            //get point location of button for button grid
            Point location = (Point)clickedButton.Tag;
            int x = location.X;
            int y = location.Y;
            //MessageBox.Show(x + " " + y);



            //if left click reveal space and draw board after flood fill
            if (e.Button == MouseButtons.Left) {
                
                myBoard.revealSpace(x, y);
                redrawBoard();
            }
            //if right click flag space
            if (e.Button == MouseButtons.Right) {
                myBoard.flagSpace(x, y);
                redrawBoard();
            }
            //check win conditions
            myBoard.checkStatus();
            if(myBoard.isWin) {
                watch.Stop();
                MessageBox.Show("You WON! \nTime Elapsed: "+watch.Elapsed);
                
                scoring.FinalTime = (int)(watch.ElapsedMilliseconds / 1000);
                StatsForm form = new StatsForm(scoring.GetScore());
                form.Show();
                this.Hide();
            }
            //Lose condition
            if(myBoard.isLose){
                watch.Stop();
                //show the bombs
                for (int c = 0; c < myBoard.Size; c++) {
                    for (int r = 0; r < myBoard.Size; r++) {
                        if(myBoard.Grid[r,c].isLive)
                        myBoard.Grid[r, c].isVisited = true;
                    }
                }
                redrawBoard();
                MessageBox.Show("You LOST \nTime Elapsed: " + watch.Elapsed);

                //I suck to much so i added this to test the high score function
                //Final model will pass 0 instead of a score
                scoring.FinalTime = (int)(watch.ElapsedMilliseconds / 1000);
                //StatsForm form = new StatsForm(scoring.GetScore());
                StatsForm form = new StatsForm(0);
                form.Show();
                this.Hide();
                

                //resets the buttons to be all hidden
                //No longer neded
                //setBoard();
            }

        }

        //re write all buttons on board to update from flood fill and make sure proper tags in place
        private void redrawBoard()
        {
            for(int x = 0; x < myBoard.Size; x++) {
                for (int y = 0; y < myBoard.Size; y++) {
                    //check if it is flaggd first
                    if (myBoard.Grid[x, y].isFlagged) {
                        btnGrid[x, y].Text = "";
                        btnGrid[x, y].BackgroundImageLayout = ImageLayout.Stretch;
                        btnGrid[x, y].BackgroundImage = MinesweeperGUI.Properties.Resources.Flag;
                    }
                    //check if slot is NOT live
                    else if (!myBoard.Grid[x, y].isVisited) {
                        btnGrid[x, y].BackgroundImage = null;
                        btnGrid[x, y].Text = "";
                    }
                    //if is visited and NOT live
                    else if (!myBoard.Grid[x, y].isLive) {
                        btnGrid[x, y].BackgroundImage = null;
                        btnGrid[x, y].Enabled = false;
                        btnGrid[x, y].Text = myBoard.Grid[x, y].liveNeighbors.ToString();
                    }
                    //if is visted and IS live
                    else if (myBoard.Grid[x, y].isLive) {
                        btnGrid[x, y].Text = "";
                        btnGrid[x, y].BackgroundImageLayout = ImageLayout.Stretch;
                        btnGrid[x, y].BackgroundImage = MinesweeperGUI.Properties.Resources.Mine;

                    }
                    //if error
                    else {
                        btnGrid[x, y].Text = "Nothing";
                    }
                }
            }
            
        }

        private void Form2_Load(object sender, EventArgs e)
        {

        }
    }
}
