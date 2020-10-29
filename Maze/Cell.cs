using System;
using System.Linq;
using System.Collections.Generic;

namespace Maze
{
    public class Cell
    {
        public static readonly char Wall = ' ';
        public static readonly char Space = '+';
        public static readonly char Box = 'B';

        public char[,] Grid { get; } = new char[,] { { Wall, Wall }, { Wall, Wall } };
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

        public Cell GetRandomNotVisitedNeighbourOrNull(Random random)
        {
            var neighbours = NeighbourCells;
            var neighboursCount = neighbours.Count;
            var checkedNeighbours = new List<int>();
            while (neighboursCount > checkedNeighbours.Count)
            {
                int randomNum = random.Next(0, neighboursCount);

                if (checkedNeighbours.Contains(randomNum)) continue;
                checkedNeighbours.Add(randomNum);

                if (neighbours[randomNum].IsVisited) continue;
                return neighbours[randomNum];
            }

            return null;
        }

        public void AddAllCloseNeighboursFromMaze(Cell[,] maze)
        {
            int lengthY = maze.GetLength(0);
            int downY = PointInMaze.Y + 1;
            TryAddNeighbourFromMaze(downY, PointInMaze.X, maze, downY < lengthY);

            int upY = PointInMaze.Y - 1;
            TryAddNeighbourFromMaze(upY, PointInMaze.X, maze, upY >= 0);

            int lengthX = maze.GetLength(1);
            int rightX = PointInMaze.X + 1;
            TryAddNeighbourFromMaze(PointInMaze.Y, rightX, maze, rightX < lengthX);

            int leftX = PointInMaze.X - 1;
            TryAddNeighbourFromMaze(PointInMaze.Y, leftX, maze, leftX >= 0);
        }

        private void TryAddNeighbourFromMaze(int y, int x, Cell[,] maze, bool areYAndXInMazeBounds)
        {
            if (areYAndXInMazeBounds)
            {
                Cell cell = maze[y, x];
                if (NeighbourCells.Contains(cell) == false)
                {
                    NeighbourCells.Add(cell);
                }
            }
        }

        public bool TrySpawnBoxAtSpaceRandomly()
        {
            Point? spacePoint = TryGetTilePointRandom(Space);
            if (spacePoint == null) return false;
            Grid[spacePoint.Value.Y, spacePoint.Value.X] = Box;
            return true;
        }

        public List<Point> GetAllPointsOfTile(char tile)
        {
            var points = new List<Point>();
            for (int y = 0; y < Grid.GetLength(0); y++)
            {
                for (int x = 0; x < Grid.GetLength(1); x++)
                {
                    if (Grid[y, x] == tile) points.Add(new Point(y, x));
                }
            }
            return points;
        }

        public Point? TryGetTilePoint(char tile)
        {
            return GetAllPointsOfTile(tile).FirstOrDefault();
        }

        public Point? TryGetTilePointRandom(char tile)
        {
            var points = GetAllPointsOfTile(tile);
            int count = points.Count;
            if (count == 0) return null;
            if (count == 1) return points[0];
            return points[new Random().Next(0, count)];
        }
    }
}
