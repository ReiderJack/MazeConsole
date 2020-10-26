using Xunit;
using Maze;
using System.Linq;

namespace Maze.Tests
{
    public class UnitTest1
    {
        [Fact]
        public void Create_EmptyMazeWithAStartingCell_EnumerableWithOneCellWithPlusChar()
        {
            var generator = new MazeGenerator().Generate(5, 5);

            Assert.Equal(new Cell[4, 5], generator);
        }
    }
}
