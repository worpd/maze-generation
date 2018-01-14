using System;
using MazeLib;

namespace MazeConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            MazeBuilder simpleBuilder = new MazeBuilder();

            Random rnd1 = new Random();
            Maze simpleMaze1 = simpleBuilder.GenerateRandomMaze(31, 31, rnd1);
            
            // Random rnd2 = new Random(2);
            // Maze simpleMaze2 = simpleBuilder.GenerateRandomMaze(7, 3, rnd2);
            
            MazeConsoleRenderer.renderMaze(simpleMaze1);
            Console.WriteLine("======================");
            // MazeConsoleRenderer.renderMaze(simpleMaze2);

            // Maze testMaze = simpleBuilder.GenerateTestMaze();
            // MazeConsoleRenderer.renderMaze(testMaze);
        }
    }

    class MazeConsoleRenderer
    {
        public static void renderMaze(Maze maze)
        {
            int height = maze.height;
            int width = maze.width;

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    Point2D point = new Point2D(x, y);
                    if (maze.hasWallAt(point))
                    {
                        Console.BackgroundColor = ConsoleColor.Red;
                        Console.Write(" · ");
                    }
                    else
                    {
                        Console.Write(" · ");
                    }
                    Console.ResetColor();
                }
                Console.WriteLine();
            }

            // Console.WriteLine(lines.Trim());
        }
    }
}
