using System;
using Maze;

namespace Main
{
    class Program
    {
        static void Main(string[] args)
        {
            var maze = new MazeGenerator(5,5).GetMazeInString();
            foreach (var cell in maze)
            {
               Console.WriteLine(cell); 
            }
        }
    }
}
