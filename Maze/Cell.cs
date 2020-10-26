namespace Maze
{
    public class Cell
    {
        public char[,] Grid { get; set; } = new char[,] { { ' ', ' ' }, { ' ', ' ' } };
        public bool IsVisited { get; set; }
    }
}
