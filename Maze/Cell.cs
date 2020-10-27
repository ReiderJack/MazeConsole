using System.Collections.Generic;

namespace Maze
{
    public class Cell
    {
        public char[,] Grid { get; } = new char[,] { { ' ', ' ' }, { ' ', ' ' } };
        public Point PointInGrid { get; set; }
        public Point PointInMaze { get; }

        public List<Cell> NeighbourCells { get; set; } = new List<Cell>();

        public Cell PreviousCell { get; set; }

        public bool IsVisited { get; set; }

        public Cell(Point pointInMaze)
        {
            PointInMaze = pointInMaze;
            PointInGrid = new Point(1, 1);
        }
    }
}
