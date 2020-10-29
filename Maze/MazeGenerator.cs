using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Maze
{
    public class MazeGenerator
    {
        public Cell[,] CellsGrid;
        public Cell StartingCell { get; }
        public Cell CurrentCell { get; private set; }

        public MazeGenerator()
        {
        }

        public MazeGenerator(Cell[,] maze)
        {
            CellsGrid = maze;
        }

        public MazeGenerator(int yLength, int xLength)
        {
            CellsGrid = GenerateAnEmptyMaze(yLength, xLength);
            SetStartingCell(CellsGrid);
            StartingCell = CellsGrid[0, 0];
            CurrentCell = StartingCell;
            CurrentCell.IsVisited = true;
            PopulateAllCells();
            CarveMaze();
        }

        public bool TrySpawnBoxesRandomly(int count)
        {
            if (count <= 0 || count > CellsGrid.Length) return false;
            Random random = new Random();
            List<Cell> checkedCells = new List<Cell>();
            int lengthY = CellsGrid.GetLength(0);
            int lengthX = CellsGrid.GetLength(1);
            while (count > 0)
            {
                int randomY = random.Next(0, lengthY);
                int randomX = random.Next(0, lengthX);
                if (checkedCells.Contains(CellsGrid[randomY, randomX])) continue;
                if (CellsGrid[randomY, randomX].TrySpawnBoxAtSpaceRandomly())
                {
                    checkedCells.Add(CellsGrid[randomY, randomX]);
                    count--;
                }
            }
            return true;
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

        private void SetStartingCell(Cell[,] maze)
        {
            maze[0, 0].Grid[1, 1] = Cell.Space;
        }

        private void SetStartingCell(Cell[,] maze, Point pointInMaze)
        {
            maze[pointInMaze.Y, pointInMaze.X].Grid[0, 0] = Cell.Space;
        }

        private void MakeStartingCell(Cell[,] maze, Point pointInMaze, Point pointInCellGrid)
        {
            maze[pointInMaze.Y, pointInMaze.X].Grid[pointInCellGrid.Y, pointInCellGrid.X] = Cell.Space;
        }

        private void PopulateAllCells()
        {
            for (int y = 0; y < CellsGrid.GetLength(0); y++)
            {
                for (int x = 0; x < CellsGrid.GetLength(1); x++)
                {
                    CellsGrid[y, x].AddAllCloseNeighboursFromMaze(CellsGrid);
                }
            }
        }

        private void CarveMaze()
        {
            Random random = new Random();
            while (CellsGrid.Cast<Cell>().Any(c => !c.IsVisited))
            {
                Cell nextCell = CurrentCell.GetRandomNotVisitedNeighbourOrNull(random);
                if (nextCell == null)
                {
                    CurrentCell = CurrentCell.PreviousCell;
                    continue;
                }
                MakePathBetweenCells(CurrentCell, nextCell);
                MoveCurrentCellTo(nextCell);
            }
        }

        private void MoveCurrentCellTo(Cell nextCell)
        {
            nextCell.IsVisited = true;
            nextCell.PreviousCell = CurrentCell;
            CurrentCell = nextCell;
        }

        public static void MakePathBetweenCells(Cell startCell, Cell nextCell)
        {
            Point directionToNextCell = nextCell.PointInMaze - startCell.PointInMaze;
            Point tileToOpenInNextCell = startCell.PointInGrid - directionToNextCell;
            if (tileToOpenInNextCell.Y > 1 || tileToOpenInNextCell.Y < 0 ||
                    tileToOpenInNextCell.X > 1 || tileToOpenInNextCell.X < 0)
            {
                tileToOpenInNextCell = startCell.PointInGrid;
            }
            nextCell.Grid[Math.Abs(tileToOpenInNextCell.Y),
                Math.Abs(tileToOpenInNextCell.X)] = Cell.Space;
            nextCell.PointInGrid = tileToOpenInNextCell;

            Point tileToOpenInStartCell = startCell.PointInGrid + directionToNextCell;
            if (tileToOpenInStartCell.Y <= 1 && tileToOpenInStartCell.Y >= 0 &&
                    tileToOpenInStartCell.X <= 1 && tileToOpenInStartCell.X >= 0)
            {
                startCell.Grid[Math.Abs(tileToOpenInStartCell.Y),
                    Math.Abs(tileToOpenInStartCell.X)] = Cell.Space;
                startCell.PointInGrid = tileToOpenInStartCell;
            }
        }

        public IEnumerable<string> GetMazeInRowsAsStrings()
        {
            var list = new List<string>();
            StringBuilder firstRow = new StringBuilder();
            StringBuilder secondRow = new StringBuilder();
            for (int y = 0; y < CellsGrid.GetLength(0); y++)
            {
                for (int x = 0; x < CellsGrid.GetLength(1); x++)
                {
                    firstRow.Append(CellsGrid[y, x].Grid[0, 0]);
                    firstRow.Append(CellsGrid[y, x].Grid[0, 1]);
                    secondRow.Append(CellsGrid[y, x].Grid[1, 0]);
                    secondRow.Append(CellsGrid[y, x].Grid[1, 1]);
                }
                list.Add(firstRow.ToString());
                list.Add(secondRow.ToString());
                firstRow.Clear();
                secondRow.Clear();
            }

            return list;
        }

        public char[,] GetMazeAsCharGrid()
        {
            var strings = GetMazeInRowsAsStrings().ToList();
            int lenghtY = strings.Count;
            int lenghtX = strings[0].Length;
            char[,] result = new char[lenghtY, lenghtY];
            for (int y = 0; y < lenghtY; y++)
            {
                for (int x = 0; x < lenghtX; x++)
                {
                    result[y, x] = strings[y][x];
                }
            }

            return result;
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
                    int yDiv = y / 2;
                    int xDiv = x / 2;
                    var cell = new Cell(new Point(yDiv, xDiv));
                    cell.Grid[0, 0] = strList[y][x];

                    int yPlus = y + 1;
                    int xPlus = x + 1;
                    cell.Grid[1, 1] = strList[yPlus][xPlus];
                    cell.Grid[1, 0] = strList[yPlus][x];
                    cell.Grid[0, 1] = strList[y][xPlus];
                    maze[yDiv, xDiv] = cell;
                }
            }

            return maze;
        }

    }
}
