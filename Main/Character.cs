using System;
using Maze;

namespace Main
{
    public class Character
    {
        public Point CurrentPosition { get; set; }
        public char[,] Maze { get; set; }
        public int CollectedBoxes { get; set; }

        public Character(Point startingPosition, char[,] maze)
        {
            Maze = maze;
            CurrentPosition = startingPosition;
            Maze[startingPosition.Y, startingPosition.X] = '@';
            Console.SetCursorPosition(CurrentPosition.Y, CurrentPosition.X);
        }

        public void MoveHero(int y, int x)
        {
            Point nextPosition = new Point(CurrentPosition.Y + y, CurrentPosition.X + x);
            if (CanMoveTo(nextPosition.Y, nextPosition.X))
            {
                RemoveHero();
                CurrentPosition = nextPosition;
                Maze[CurrentPosition.Y, CurrentPosition.X] = '@';
                Console.CursorVisible = false;
            }
        }

        private void RemoveHero()
        {
            Maze[CurrentPosition.Y, CurrentPosition.X] = '+';
        }

        private bool CanMoveTo(int y, int x)
        {
            if (y < 0 || y >= Maze.GetLength(0) ||
                x < 0 || x >= Maze.GetLength(1)) return false;
            if (Maze[y, x] == Cell.Wall) return false;
            if (Maze[y, x] == Cell.Box) CollectedBoxes++;
            return true;
        }
    }
}
