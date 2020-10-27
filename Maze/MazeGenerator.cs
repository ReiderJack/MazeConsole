using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Maze
{
    public class MazeGenerator
    {
        public readonly Cell[,] MazeGrid;
        public Cell StartingCell { get; }
        public Cell CurrentCell { get; private set; }

        public MazeGenerator(int yLength, int xLength)
        {
            MazeGrid = GenerateAnEmptyMaze(yLength, xLength);
            MakeStartingCell(MazeGrid);
            StartingCell = MazeGrid[0, 0];
            CurrentCell = StartingCell;
            CurrentCell.IsVisited = true;
            PopulateAllCells();
            CarveMaze();
        }

        private Cell[,] GenerateAnEmptyMaze(int yLength, int xLength)
        {
            var maze = new Cell[yLength, xLength];
            for (int y = 0; y < yLength; y++)
            {
                for (int x = 0; x < xLength; x++)
                {
                    maze[y, x] = new Cell(new Point(y, x));
                }
            }

            return maze;
        }

        private void MakeStartingCell(Cell[,] maze)
        {
            maze[0, 0].Grid[1, 1] = '+';
        }

        private void MakeStartingCell(Cell[,] maze, Point pointInMaze)
        {
            maze[pointInMaze.Y, pointInMaze.X].Grid[0, 0] = '+';
        }

        private void MakeStartingCell(Cell[,] maze, Point pointInMaze, Point pointInCellGrid)
        {
            maze[pointInMaze.Y, pointInMaze.X].Grid[pointInCellGrid.Y, pointInCellGrid.X] = '+';
        }

        private void PopulateAllCells()
        {
            for (int y = 0; y < MazeGrid.GetLength(0); y++)
            {
                for (int x = 0; x < MazeGrid.GetLength(1); x++)
                {
                    PopulateCellNeighbours(MazeGrid, MazeGrid[y, x]);
                }
            }
        }

        private void PopulateCellNeighbours(Cell[,] maze, Cell cell)
        {
            int mazeYLength = maze.GetLength(0);
            int downCoordinate = cell.PointInMaze.Y + 1;
            if (downCoordinate < mazeYLength)
            {
                Cell down = maze[downCoordinate, cell.PointInMaze.X];
                cell.NeighbourCells.Add(down);
            }
            int upCoordinate = cell.PointInMaze.Y - 1;
            if (upCoordinate >= 0)
            {
                Cell up = maze[upCoordinate, cell.PointInMaze.X];
                cell.NeighbourCells.Add(up);
            }

            int rightCoordinate = cell.PointInMaze.X + 1;
            int mazeXLength = maze.GetLength(1);
            if (rightCoordinate < mazeXLength)
            {
                Cell right = maze[cell.PointInMaze.Y, rightCoordinate];
                cell.NeighbourCells.Add(right);
            }
            int leftCoordinate = cell.PointInMaze.X - 1;
            if (leftCoordinate >= 0)
            {
                Cell left = maze[cell.PointInMaze.Y, leftCoordinate];
                cell.NeighbourCells.Add(left);
            }
        }

        private void CarveMaze()
        {
            Random random = new Random();
            while (MazeGrid.Cast<Cell>().Any(c => !c.IsVisited))
            {
                Cell nextCell = GetRandomNotVisitedNeighbourOrNull(random);
                if (nextCell == null)
                {
                    CurrentCell = CurrentCell.PreviousCell;
                    continue;
                }
                MakePathToUnVisitedCell(nextCell);
            }
        }

        private Cell GetRandomNotVisitedNeighbourOrNull(Random random)
        {
            var neighbours = CurrentCell.NeighbourCells;
            var neighboursCount = neighbours.Count();
            var checkedNeighbours = new List<int>();
            while (neighboursCount > checkedNeighbours.Count())
            {
                int randomNum = random.Next(0, neighboursCount);

                if (checkedNeighbours.Contains(randomNum)) continue;
                checkedNeighbours.Add(randomNum);

                if (neighbours[randomNum].IsVisited) continue;
                return neighbours[randomNum];
            }

            return null;
        }

        private void MakePathToUnVisitedCell(Cell nextCell)
        {
            Point neighbourCellGridCellToOpen = CurrentCell.PointInGrid;
            Point nextCellDirection = nextCell.PointInMaze - CurrentCell.PointInMaze;
            Point wallToDestroyInNextGrid = CurrentCell.PointInGrid - nextCellDirection;
            if (wallToDestroyInNextGrid.Y <= 1 && wallToDestroyInNextGrid.Y >= 0 &&
                    wallToDestroyInNextGrid.X <= 1 && wallToDestroyInNextGrid.X >= 0)
            {
                neighbourCellGridCellToOpen = wallToDestroyInNextGrid;
            }

            Point wallToDestroyInCurrentCellGrid = CurrentCell.PointInGrid + nextCellDirection;
            if (wallToDestroyInCurrentCellGrid.Y <= 1 && wallToDestroyInCurrentCellGrid.Y >= 0 &&
                    wallToDestroyInCurrentCellGrid.X <= 1 && wallToDestroyInCurrentCellGrid.X >= 0)
            {
                CurrentCell.Grid[Math.Abs(wallToDestroyInCurrentCellGrid.Y),
                    Math.Abs(wallToDestroyInCurrentCellGrid.X)] = '+';
                CurrentCell.PointInGrid = wallToDestroyInCurrentCellGrid;
            }

            nextCell.Grid[Math.Abs(neighbourCellGridCellToOpen.Y),
                Math.Abs(neighbourCellGridCellToOpen.X)] = '+';
            nextCell.IsVisited = true;
            nextCell.PointInGrid = neighbourCellGridCellToOpen;
            nextCell.PreviousCell = CurrentCell;

            CurrentCell = nextCell;
        }

        public IEnumerable<string> GetMazeInRowsAsStrings()
        {
            var list = new List<string>();
            StringBuilder firstRow = new StringBuilder();
            StringBuilder secondRow = new StringBuilder();
            for (int y = 0; y < MazeGrid.GetLength(0); y++)
            {
                for (int x = 0; x < MazeGrid.GetLength(1); x++)
                {
                    firstRow.Append(MazeGrid[y, x].Grid[0, 0]);
                    firstRow.Append(MazeGrid[y, x].Grid[0, 1]);
                    secondRow.Append(MazeGrid[y, x].Grid[1, 0]);
                    secondRow.Append(MazeGrid[y, x].Grid[1, 1]);
                }
                list.Add(firstRow.ToString());
                list.Add(secondRow.ToString());
                firstRow.Clear();
                secondRow.Clear();
            }

            return list;
        }

        public static Cell[,] GetMazeFromStrings(IEnumerable<string> strings)
        {
            List<string> strList = strings.ToList();
            int rowCount = strings.Count();
            int columnCount = strList[0].Length;
            var maze = new Cell[rowCount / 2, columnCount / 2];
            for (int y = 0; y < rowCount; y += 2)
            {
                for (int x = 0; x < columnCount; x += 2)
                {
                    var cell = new Cell(new Point(y / 2, x / 2));
                    cell.Grid[0, 0] = strList[y][x];
                    cell.Grid[1, 1] = strList[y + 1][x + 1];
                    cell.Grid[1, 0] = strList[y + 1][x];
                    cell.Grid[0, 1] = strList[y][x + 1];
                    maze[y / 2, x / 2] = cell;
                }
            }

            return maze;
        }

        public void SaveDungeonToFile(string path)
        {
            using (Stream s = File.Create(path))
            using (TextWriter writer = new StreamWriter(s))
            {
                foreach (string row in GetMazeInRowsAsStrings())
                {
                    writer.WriteLine(row);
                }
            }
        }
    }
}
