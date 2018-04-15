using System;
using MazeLib;

namespace MazeConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            //MazeBuilder simpleBuilder2 = new MazeBuilder();
            RandomDirectionMazeBuilder simpleBuilder2 = new RandomDirectionMazeBuilder();

            //Maze simpleMaze1 = simpleBuilder.GenerateRandomMaze(51, 51, rnd1);

            for (int x = 200; x < 10000; x++)
            {
                Random rnd1 = new Random(1);
                Maze simpleMaze2 = simpleBuilder2.GenerateRandomMaze(51, 51, rnd1, x);
                MazeConsoleRenderer.renderMaze(simpleMaze2);
                Console.Read();
            }
            
            // Random rnd2 = new Random(2);
            // Maze simpleMaze2 = simpleBuilder.GenerateRandomMaze(7, 3, rnd2);
            
            
            // Console.WriteLine("======================");
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
