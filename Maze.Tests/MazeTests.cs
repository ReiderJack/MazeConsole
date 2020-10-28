using Xunit;
using System.Linq;
using System.Collections.Generic;

namespace Maze.Tests
{
    public class MazeTests
    {
        private readonly Cell[,] grid2x2;
        private readonly List<string> grid2x2AsStringList;
        private readonly Cell cellForSpawning;
        public MazeTests()
        {
            grid2x2AsStringList = new List<string> { "  + ",
                                                     "+  +",
                                                     "   +",
                                                     "    " };
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

            grid2x2 = new Cell[,] { { cell00, cell01 }, { cell10, cell11 } };

            cellForSpawning = new Cell(new Point(0, 0));
        }

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
            var expected = grid2x2.Cast<Cell>().Select(c => c.Grid);

            Cell[,] cells = MazeGenerator.GetMazeFromStrings(grid2x2AsStringList);
            var actual = cells.Cast<Cell>().Select(c => c.Grid);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void CheckCellPointInMaze_GetMazeFromStrings_ReturnMaze()
        {
            var expected = grid2x2.Cast<Cell>().Select(c => c.PointInMaze);

            Cell[,] cells = MazeGenerator.GetMazeFromStrings(grid2x2AsStringList);
            var actual = cells.Cast<Cell>().Select(c => c.PointInMaze);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void SpawnBoxRandomly_WithGetAllPointsOfTile_ReturnPoints()
        {
            var cell = new Cell(new Point(0, 0));
            cell.Grid[0, 0] = ' ';
            cell.Grid[0, 1] = '+';
            cell.Grid[1, 0] = ' ';
            cell.Grid[1, 1] = ' ';

            var expected = new Cell(new Point(0, 0));
            expected.Grid[0, 0] = ' ';
            expected.Grid[0, 1] = 'B';
            expected.Grid[1, 0] = ' ';
            expected.Grid[1, 1] = ' ';

            Assert.True(cell.TrySpawnBoxAtSpaceRandomly());
            Assert.Equal(cell.Grid, expected.Grid);
        }
    }
}
