using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Maze;

namespace Main
{
    public class Game
    {
        public int BoxCount { get; private set; }
        public void MainMenu()
        {
            PrintMainMenu();
            ConsoleKeyInfo keyInfo;
            while ((keyInfo = Console.ReadKey(true)).Key != ConsoleKey.Escape)
            {
                switch (keyInfo.Key)
                {
                    case ConsoleKey.D1:
                        CreateNewMaze();
                        break;
                    case ConsoleKey.D2:
                        OpenMaze();
                        break;
                }
            }
        }

        private void CreateNewMaze()
        {
            try
            {
                var generator = TryCreateMaze();
                Console.WriteLine("Write path where to save maze: ");

                var absolutePath = TryCreateAFile();
                SaveDungeonToFile(generator, absolutePath);
                Console.WriteLine($"Dungeon was created at \"{absolutePath}\"");
                SpawnBoxes(generator);
                Console.WriteLine("Write path where to save box locations: ");
                absolutePath = TryCreateAFile();
                SaveBoxLocationsToFile(absolutePath, generator);
                Console.WriteLine("Press enter to start.");
                if (Console.ReadKey().Key == ConsoleKey.Enter) StartGame(generator);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        private string TryCreateAFile()
        {
            string inputPath = Console.ReadLine();
            var uri = new Uri(inputPath);
            return uri.AbsolutePath;
        }

        private void SaveBoxLocationsToFile(string filePath, MazeGenerator generator)
        {
            var grid = generator.GetMazeAsCharGrid();
            var points = GetBoxesFromTileGrid(grid).ToArray();
            using (Stream s = File.Create(filePath))
            using (TextWriter writer = new StreamWriter(s))
            {
                for (int i = 0; i < points.Count(); i++)
                {
                    writer.WriteLine($"Box {i}: x:{points[i].X}, y:{points[i].Y}");
                }
            }
        }

        private IEnumerable<Point> GetBoxesFromTileGrid(char[,] tileGrid)
        {
            List<Point> points = new List<Point>();
            for (int y = 0; y < tileGrid.GetLength(0); y++)
            {
                for (int x = 0; x < tileGrid.GetLength(1); x++)
                {
                    if (tileGrid[y, x] == Cell.Box)
                    {
                        points.Add(new Point(y + 1, x + 1));
                    }
                }
            }
            return points;
        }

        private void SpawnBoxes(MazeGenerator generator)
        {
            Console.WriteLine("Write box count to spawn: ");
            int boxCount;
            while (BoxCount <= 0)
            {
                while (int.TryParse(Console.ReadLine(), out boxCount) == false)
                {
                    Console.WriteLine("Invalid number.\nTry again:");
                }

                if (generator.SpawnBoxesRandomly(boxCount) == false)
                {
                    Console.WriteLine("Number is too small or big!\nTry again:");
                    continue;
                }

                BoxCount = boxCount;
            }
        }

        private MazeGenerator TryCreateMaze()
        {
            Console.WriteLine("Write Maze tile width:");
            int X;
            while (int.TryParse(Console.ReadLine(), out X) == false)
            {
                Console.WriteLine("Invalid input.\nTry again:");
            }

            Console.WriteLine("Write Maze tile height:");
            int Y;
            while (int.TryParse(Console.ReadLine(), out Y) == false)
            {
                Console.WriteLine("Invalid input.\nTry again:");
            }
            return new MazeGenerator(Y / 2, X / 2);
        }

        private void OpenMaze()
        {
            Console.WriteLine("Write dungeon path:");
            try
            {
                var absolutePath = TryCreateAFile();
                var maze = GetMazeFromFile(absolutePath);
                Console.WriteLine("Got maze from file.");
                SpawnBoxes(maze);
                Console.WriteLine("Write path where to save box locations: ");
                absolutePath = TryCreateAFile();
                SaveBoxLocationsToFile(absolutePath, maze);
                Console.WriteLine("Press enter to start.");
                if (Console.ReadKey().Key == ConsoleKey.Enter) StartGame(maze);
            }
            catch (Exception e)
            {
                Console.WriteLine("\n" + e.Message);
            }
        }

        private void StartGame(MazeGenerator generator)
        {
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
                if (AnnounceLeftBoxes(character))
                {
                    Console.WriteLine("You collected all boxes!\nYou won!\nPress Escape to exit.");
                    break;
                }
            }
        }

        private void DrawMaze(char[,] maze)
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

        private bool AnnounceLeftBoxes(Character character)
        {
            int boxesLeft = BoxCount - character.CollectedBoxes;
            Console.WriteLine($"Boxes left to collect: {boxesLeft}");
            return boxesLeft == 0;
        }

        public void PrintMainMenu()
        {
            Console.Clear();
            Console.WriteLine("1) Create new maze.");
            Console.WriteLine("2) Open maze.");
            Console.WriteLine("Press escape to exit.");
        }

        private MazeGenerator GetMazeFromFile(string filePath)
        {
            var CellsGrid = MazeGenerator.GetMazeFromStrings(GetStringsFromFile(filePath));
            var maze = new MazeGenerator(CellsGrid);
            foreach (var cell in CellsGrid.Cast<Cell>())
            {
                BoxCount += cell.Grid.Cast<char>().Count(c => c == Cell.Box);
            }
            return maze;
        }

        private IEnumerable<string> GetStringsFromFile(string filePath)
        {
            List<string> result = new List<string>();
            using (StreamReader reader = new StreamReader(filePath))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    result.Add(line);
                }
            }
            return result;
        }

        private void SaveDungeonToFile(MazeGenerator maze, string path)
        {
            using Stream s = File.Create(path);
            using TextWriter writer = new StreamWriter(s);
            foreach (string row in maze.GetMazeInRowsAsStrings())
            {
                writer.WriteLine(row);
            }
        }
    }
}
