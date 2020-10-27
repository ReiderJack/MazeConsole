using Xunit;
using System.Linq;
using System.Collections.Generic;

namespace Maze.Tests
{
    public class UnitTest1
    {
        [Fact]
        public void Create_EmptyMazeWithAStartingCell_EnumerableWithOneCellWithPlusChar()
        {
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

            Assert.Empty(list);
        }

        [Fact]
        public void Check_StringMaze()
        {
            var generator = new MazeGenerator(4, 4);

            Assert.Empty(generator.GetMazeInString());
        }
    }
}
