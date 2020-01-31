using System;
using System.Collections.Generic;
using System.Text;

namespace MinesweeperLibrary
{
    public class Cell
    {
        public int RowNumber { get; set; }
        public int ColumnNumber { get; set; }
        public int liveNeighbors { get; set; }
        public bool isVisited { get; set; }
        public bool isLive { get; set; }
        public bool isFlagged { get; set; }

        /*Cell needs to know:
            Row
            Column
            How many bombs are next to it
            if it has been 'clicked' on 
            if it is a bomb
            if the cell is flagged 
             */
        public Cell(int column, int row, int liveN, bool visited, bool live, bool flagged)
        {
            this.RowNumber = row;
            this.ColumnNumber = column;
            this.liveNeighbors = liveN;
            this.isVisited = visited;
            this.isLive = live;
            this.isFlagged = flagged;

        }
        

        


    }
}
