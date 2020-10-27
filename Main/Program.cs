using System;
using Maze;

namespace Main
{
    static class Program
    {
        static void Main(string[] args)
        {
            bool keepOpen = true;
            while (keepOpen)
            {
                Console.WriteLine("Open existing dungeon or create new one? [1,2]");
                if (Console.ReadKey().KeyChar == '1')
                {
                    Console.WriteLine("\nWrite dungeon path:");
                    string input = Console.ReadLine();
                    try
                    {
                        var uri = new Uri(input);
                        string absolutePath = uri.AbsolutePath;
                        // var generator = new MazeGenerator();
                        // var grid = generator.GetDungeonFromFile(absolutePath);
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
                        var generator = new MazeGenerator(Y, X);
                        generator.SaveDungeonToFile(absolutePath);
                        Console.WriteLine($"Dungeon was created at \"{absolutePath}\"");
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                }
            }
        }
    }
}
