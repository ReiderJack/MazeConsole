using Xunit;
using System.Linq;
using System.Collections.Generic;

namespace Maze.Tests
{
    public class UnitTest1
    {
        [Fact]
        public void Check_CreatedMaze()
        {
            var generator = new MazeGenerator(4, 4);
            var castedMaze = generator.MazeGrid.Cast<Cell>();
            var list = new List<IEnumerable<char>>();

            foreach (var cell in castedMaze)
            {
                list.Add(cell.Grid.Cast<char>());
            }

            Assert.True(true);
        }

        [Fact]
        public void Check_AllCellsBeingVisited_ReturnTrue()
        {
            var generator = new MazeGenerator(16, 16);

            Assert.DoesNotContain(generator.MazeGrid.Cast<Cell>(), c => c.IsVisited == false);
        }

        [Fact]
        public void CheckCellGrids_GetMazeFromStrings_ReturnMaze()
        {
            var cell00 = new Cell(new Point(0, 0));
            cell00.Grid[0, 0] = ' ';
            cell00.Grid[0, 1] = ' ';
            cell00.Grid[1, 0] = '+';
            cell00.Grid[1, 1] = ' ';
            var cell01 = new Cell(new Point(0, 1));
            cell01.Grid[0, 0] = '+';
            cell01.Grid[0, 1] = ' ';
            cell01.Grid[1, 0] = ' ';
            cell01.Grid[1, 1] = '+';
            var cell10 = new Cell(new Point(1, 0));
            cell10.Grid[0, 0] = ' ';
            cell10.Grid[0, 1] = ' ';
            cell10.Grid[1, 0] = ' ';
            cell10.Grid[1, 1] = ' ';
            var cell11 = new Cell(new Point(1, 1));
            cell11.Grid[0, 0] = ' ';
            cell11.Grid[0, 1] = '+';
            cell11.Grid[1, 0] = ' ';
            cell11.Grid[1, 1] = ' ';

            Cell[,] expectedCells = { { cell00, cell01 },
                                    { cell10, cell11 } };
            var expected = expectedCells.Cast<Cell>().Select(c => c.Grid);

            var strings = new List<string> { "  + ",
                                             "+  +",
                                             "   +",
                                             "    " };

            Cell[,] cells= MazeGenerator.GetMazeFromStrings(strings);
            var actual = cells.Cast<Cell>().Select(c => c.Grid);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void CheckCellPointInMaze_GetMazeFromStrings_ReturnMaze()
        {
            var cell00 = new Cell(new Point(0, 0));
            cell00.Grid[0, 0] = ' ';
            cell00.Grid[0, 1] = ' ';
            cell00.Grid[1, 0] = '+';
            cell00.Grid[1, 1] = ' ';
            var cell01 = new Cell(new Point(0, 1));
            cell01.Grid[0, 0] = '+';
            cell01.Grid[0, 1] = ' ';
            cell01.Grid[1, 0] = ' ';
            cell01.Grid[1, 1] = '+';
            var cell10 = new Cell(new Point(1, 0));
            cell10.Grid[0, 0] = ' ';
            cell10.Grid[0, 1] = ' ';
            cell10.Grid[1, 0] = ' ';
            cell10.Grid[1, 1] = ' ';
            var cell11 = new Cell(new Point(1, 1));
            cell11.Grid[0, 0] = ' ';
            cell11.Grid[0, 1] = '+';
            cell11.Grid[1, 0] = ' ';
            cell11.Grid[1, 1] = ' ';

            Cell[,] expectedCells = { { cell00, cell01 },
                                    { cell10, cell11 } };
            var expected = expectedCells.Cast<Cell>().Select(c => c.PointInMaze);

            var strings = new List<string> { "  + ",
                                             "+  +",
                                             "   +",
                                             "    " };

            Cell[,] cells= MazeGenerator.GetMazeFromStrings(strings);
            var actual = cells.Cast<Cell>().Select(c => c.PointInMaze);

            Assert.Equal(expected, actual);
        }
    }
}
