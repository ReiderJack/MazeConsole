using System;
using System.Collections.Generic;
using Maze;

namespace Main
{
    static class Program
    {
        static void Main(string[] args)
        {
            MazeGenerator generator = new MazeGenerator();
            Console.WriteLine("Open existing dungeon or create new one? [1,2]");
            if (Console.ReadKey().KeyChar == '1')
            {
                Console.WriteLine("\nWrite dungeon path:");
                string input = Console.ReadLine();
                try
                {
                    var uri = new Uri(input);
                    string absolutePath = uri.AbsolutePath;
                    generator = new MazeGenerator(MazeGenerator.GetMazeFromStrings(MazeGenerator.GetStringsFromFile(absolutePath)));
                }
                catch (Exception e)
                {
                    Console.WriteLine("\n" + e.Message);
                }
            }
            else
            {
                try
                {
                    Console.WriteLine("\n Write path where to save");
                    string inputPath = Console.ReadLine();
                    var uri = new Uri(inputPath);
                    string absolutePath = uri.AbsolutePath;

                    Console.WriteLine("\nWrite maximum X of the dungeon:");
                    int X;
                    if (int.TryParse(Console.ReadLine(), out X) == false)
                    {
                        Console.WriteLine("Invalid input");
                    }

                    Console.WriteLine("\nWrite maximum Y of the dungeon:");
                    int Y;
                    if (int.TryParse(Console.ReadLine(), out Y) == false)
                    {
                        Console.WriteLine("Invalid input");
                    }
                    generator = new MazeGenerator(Y, X);

                    Console.WriteLine("Write box count to spawn: ");
                    int boxCount;
                    if (int.TryParse(Console.ReadLine(), out boxCount) == false)
                    {
                        Console.WriteLine("Invalid input");
                    }
                    generator.SpawnBoxesRandomly(boxCount);
                    generator.SaveDungeonToFile(absolutePath);
                    Console.WriteLine($"Dungeon was created at \"{absolutePath}\"");
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
            var mazeAsChars = generator.GetMazeAsCharGrid();
            Character character = new Character(new Point(1, 1), mazeAsChars);
            Console.CursorVisible = false;
            DrawMaze(mazeAsChars);

            ConsoleKeyInfo keyInfo;
            while ((keyInfo = Console.ReadKey(true)).Key != ConsoleKey.Escape)
            {
                switch (keyInfo.Key)
                {
                    case ConsoleKey.UpArrow:
                        character.MoveHero(-1, 0);
                        break;

                    case ConsoleKey.RightArrow:
                        character.MoveHero(0, 1);
                        break;

                    case ConsoleKey.DownArrow:
                        character.MoveHero(1, 0);
                        break;

                    case ConsoleKey.LeftArrow:
                        character.MoveHero(0, -1);
                        break;
                }
                DrawMaze(character.Maze);
            }
        }
        public static void DrawMaze(char[,] maze)
        {
            Console.Clear();
            for (int y = 0; y < maze.GetLength(0); y++)
            {
                for (int x = 0; x < maze.GetLength(1); x++)
                {
                    Console.Write(maze[y, x]);
                }
                Console.WriteLine();
            }
        }
    }
}
