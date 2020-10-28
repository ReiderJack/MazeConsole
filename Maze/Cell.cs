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
