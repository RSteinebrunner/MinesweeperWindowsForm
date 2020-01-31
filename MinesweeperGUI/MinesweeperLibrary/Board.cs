using System;
using System.Collections.Generic;
using System.Text;

namespace MinesweeperLibrary
{
    public class Board
    {
        public int Size { get; set; }
        public Cell[,] Grid { get; set; }
        //percentage for cells that will become bombs later on
        public int Difficulty { get; set; }
        //properties that determine if the game is over
        public bool isLose { get; set; }
        public bool isWin { get; set; }
        //used to calculate which cell recieves a bomb
        Random random = new Random();
        //Default contructor(not used)
        public Board()
        {
            //set the size to 0 for default
            this.Size = 0;
            //set an array of 1
            Grid = new Cell[Size, Size];
            //no bombs
            Difficulty = 0;
        }

        //Normally used constructor
        public Board(int size)
        {
            //Board size and difficulty setting
            this.Size = size;
            Difficulty = 0;

            //set win/fail conditions to false
            isLose = false;
            isWin = false;

            //create a new grid
            Grid = new Cell[Size, Size];

            //fill this 2d array
            for (int i = 0; i < Size; i++) {
                for (int j = 0; j < Size; j++) {
                    //create a new cell at the proper location
                    //make sure the cell is a 'default'
                    Grid[i, j] = new Cell(i, j,0,false,false,false);
                }
            }

        }
        public void revealSpace(int x, int y)
        {
            //When showing space, remove flages, show, and check flood fill
             Grid[x, y].isFlagged = false;
             Grid[x, y].isVisited = true;
             floodFill(x,y);
        }
        //place and remove flags
        public void flagSpace(int x, int y)
        {
            //if already flagged then unflag the space
            if (Grid[x, y].isFlagged)
                Grid[x, y].isFlagged = false;
            //if not flagged then flag the space
            else
                Grid[x, y].isFlagged = true;
        }
        public void floodFill(int x, int y)
        {
            if (Grid[x,y].liveNeighbors != 0)
                return;
            //check safe
            if (isSafe(x, y + 1)) {
                //if not live show cell
                if (!Grid[x, y + 1].isLive && !Grid[x, y + 1].isVisited) 
                {
                    Grid[x, y + 1].isVisited = true ;
                    Grid[x, y + 1].isFlagged = false;
                    //if cell has no live neighbors call on it
                    if (Grid[x, y + 1].liveNeighbors == 0) {
                        floodFill(x,y+1);
                    }
                }
            }
            if (isSafe(x, y - 1)) {
                //if not live show cell
                if (!Grid[x, y - 1].isLive&& !Grid[x, y - 1].isVisited) 
                {
                    Grid[x, y - 1].isVisited = true;
                    Grid[x, y - 1].isFlagged = false;
                    //if cell has no live neighbors call on it
                    if (Grid[x, y - 1].liveNeighbors == 0) {
                        floodFill(x, y - 1);
                    }
                }
            }
            if (isSafe(x+1, y)) {
                //if not live show cell
                if (!Grid[x+1, y].isLive&& !Grid[x + 1, y].isVisited) 
                {
                    Grid[x+1, y].isVisited = true;
                    Grid[x+1, y].isFlagged = false;
                    //if cell has no live neighbors call on it
                    if (Grid[x+1, y].liveNeighbors == 0) {
                        floodFill(x+1, y);
                    }
                }
            }
            if (isSafe(x - 1, y)) {
                //if not live show cell
                if (!Grid[x - 1, y].isLive&& !Grid[x - 1, y].isVisited)
                {
                    Grid[x - 1, y].isVisited = true;
                    Grid[x - 1, y].isFlagged = false;
                    //if cell has no live neighbors call on it
                    if (Grid[x - 1, y].liveNeighbors == 0) {
                        floodFill(x - 1, y);
                    }
                }
            }
        }
        //set difficulty settings (percent bombs)
        public void setDifficulty(int dif)
        {
            if (dif >= 1 && dif <= 100)
                this.Difficulty = dif;
            else
                Console.WriteLine("Difficulty entered is invalid. Value must be between 1 and 100");
        }
        //add all bombs into board
        public void setupLiveNeighbors()
        {
            int numBombs = calculateBombs();
            //keep adding until out until you are out of bombs
            while (numBombs > 0) {
                //loop through all spots and make it a bomb or not
                for (int i = 0; i < Size; i++) {
                    for (int j = 0; j < Size; j++) {
                        //if probablility is good, and there isnt a bomb yet, and you still have 
                        //bombs left to place
                        if (randomNumber() < 100 && !Grid[i, j].isLive && numBombs>0) {
                            Grid[i, j].isLive = true;
                            numBombs--;
                        }
                    }
                }
                //Console.WriteLine("loop again");

            }
        }
        //calculate how many live bombs are next to each inactive spot
        public void calculateLiveNeighbors()
        {  
            //loop through entire board
            for (int i = 0; i < Size; i++) {
                for (int j = 0; j < Size; j++) {
                    
                    if (isSafe(i - 1, j + 1)){
                        if (Grid[i - 1, j + 1].isLive) {
                            Grid[i, j].liveNeighbors++;
                        }
                    }
                    if (isSafe(i, j + 1)) {
                        if (Grid[i, j + 1].isLive) {
                            Grid[i, j].liveNeighbors++;
                        }
                    }
                    if (isSafe(i + 1, j + 1)) {
                        if (Grid[i + 1, j + 1].isLive) {
                            Grid[i, j].liveNeighbors++;
                        }
                    }
                    if (isSafe(i + 1, j)) {
                        if (Grid[i + 1, j].isLive) {
                            Grid[i, j].liveNeighbors++;
                        }
                    }
                    if (isSafe(i + 1, j - 1)) {
                        if (Grid[i + 1, j - 1].isLive) {
                            Grid[i, j].liveNeighbors++;
                        }
                    }
                    if (isSafe(i, j - 1)) {
                        if (Grid[i, j - 1].isLive) {
                            Grid[i, j].liveNeighbors++;
                        }
                    }
                    if (isSafe(i - 1, j - 1)) {
                        if (Grid[i - 1, j - 1].isLive) {
                            Grid[i, j].liveNeighbors++;
                        }
                    }
                    if (isSafe(i - 1, j)) {
                        if (Grid[i - 1, j].isLive) {
                            Grid[i, j].liveNeighbors++;
                        }
                    }

                }
            }
        
        }
        //check bounds on cells that may not be on the board
        public bool isSafe(int row, int column)
        {
            if (row > Size-1 || column > Size-1 || row < 0 || column < 0) {
                //Console.WriteLine("row: " + row + "\ncolumn: " + column);
                return false;

            }
            //Console.WriteLine("row: " + row + "\ncolumn: " + column);
            return true;

        }
        //check win condition and set variables, returns nothing
        public void checkStatus()
        {
            
            int visitedCount = 0;
            //calculate how many spaces are not bombs
            int totalSpaces = (Size * Size) - calculateBombs();
            
            //check all tiles
            for (int i = 0; i < Size; i++) {
                for (int j = 0; j < Size; j++) {
                    //if it is both visited and bomb game lose
                    if (Grid[i, j].isVisited == true && Grid[i, j].isLive == true) {
                        isLose = true;
                        return;
                    }
                    //check to see if all tiles that are not bombs are visited
                    else if (Grid[i, j].isVisited == true && Grid[i, j].isLive == false)
                        visitedCount++;

                }
            }
            //if you make it through all tiles and they are either visited or a bomb, but not both, then it is a win
            if (visitedCount == totalSpaces) {
                isWin = true;
            }

        }
        //determine how many bombs should be on the board based off the percent difficulty
        public int calculateBombs()
        {
            // Difficulty is percentage of spots with bombs so we need to get how many bombs will be on the board.
            int numSpaces = this.Size * this.Size;
            //Spaces times difficulty/100 should equal amount of bombs
            double dif = (double)this.Difficulty / 100.0;
            int numBombs = Convert.ToInt32(numSpaces * dif);
            //test to see if we have the right val
            //Console.WriteLine("Spaces: " + numSpaces + "\nDifficulty: "+ dif+"\nNumBombs: "+numBombs);
            return numBombs;
        }
        //get a random number from 1 to 1000 
        public int randomNumber()
        {
            
            return random.Next(0, 1000);
        }


    }
}
