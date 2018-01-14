using System;
using System.Collections.Generic;

namespace MazeLib
{
    public class MazeBuilder
    {
        private const int SPLITTABLE_NONE = 0;
        private const int SPLITTABLE_BOTH = 1;
        private const int SPLITTABLE_HORIZONTALLY = 2;
        private const int SPLITTABLE_VERTICALLY = 3;
        public Maze GenerateRandomMaze(int w, int h, Random rnd)
        {
            // Point2D exit1 = new Point2D(rnd.Next(1, w - 2), 0);
            // Point2D exit2 = new Point2D(rnd.Next(1, w - 2), h - 1);
            Point2D exit1 = new Point2D(0, rnd.Next(1, h - 2));
            Point2D exit2 = new Point2D(w - 1, rnd.Next(1, h - 2));

            MazeRoom mazeRoom = new MazeRoom(new Maze(w, h), exit1, exit2);

            GenerateMazeSections(mazeRoom, rnd);

            return mazeRoom.maze;
        }
        // public Maze GenerateTestMaze()
        // {
        //     Maze maze = CreateRoom(30, 30, new Point2D(1, 0), new Point2D(28, 29));

        //     // Maze subMaze1 = CreateRoom(9, 9, new Point2D(1, 0), new Point2D(7, 8));
        //     // MergeMazeWalls(maze, subMaze1, new Point2D(0, 0));

        //     // Maze subMaze2 = CreateRoom(9, 9, new Point2D(7, 0), new Point2D(3, 8));
        //     // MergeMazeWalls(maze, subMaze2, new Point2D(0, 8));
        //     Tuple<Maze, Maze> subMazes1 = SplitRoom(maze, new Point2D(1, 0), new Point2D(28, 29));
        //     MergeMazeWalls(maze, subMazes1.Item1, new Point2D(0, 0));
        //     MergeMazeWalls(maze, subMazes1.Item2, new Point2D(0, 2));

        //     Tuple<Maze, Maze> subMazes2 = SplitRoom(subMazes1.Item2, new Point2D(9, 0), new Point2D(28, 27));
        //     MergeMazeWalls(maze, subMazes2.Item1, new Point2D(0, 2));
        //     MergeMazeWalls(maze, subMazes2.Item2, new Point2D(0, 4));

        //     return maze;
        // }

        // private Maze CreateRoom(int w, int h, Point2D exit1, Point2D exit2)
        // {
        //     Maze maze = new Maze(w, h);

        //     AddOuterWalls(maze, exit1, exit2);

        //     return maze;
        // }

        // private Tuple<Maze, Maze> SplitRoom(Maze maze, Point2D exit1, Point2D exit2)
        // {
        //     Maze subMaze1 = CreateRoom(maze.width, 3, exit1, new Point2D(9, 2));
        //     Maze subMaze2 = CreateRoom(maze.width, maze.height - 2, new Point2D(9, 0), new Point2D(exit2.x, maze.height - 3));

        //     return new Tuple<Maze, Maze>(subMaze1, subMaze2);
        // }

        private MazeRoom GenerateMazeSections(MazeRoom mazeRoom, Random rnd)
        {
            AddOuterWalls(mazeRoom);
            Console.WriteLine("height: " + mazeRoom.height);

            int splittable = CanBeSubdivided(mazeRoom);

            switch (splittable)
            {
                case SPLITTABLE_BOTH:
                    // SplitVertically(mazeRoom, rnd);
                    SplitHorizontally(mazeRoom, rnd);
                    break;
                case SPLITTABLE_HORIZONTALLY:
                    SplitHorizontally(mazeRoom, rnd);
                    break;
                case SPLITTABLE_VERTICALLY:
                    // SplitVertically(mazeRoom, rnd);
                    break;
            }

            return mazeRoom;
        }
        private void SplitVertically(MazeRoom mazeRoom, Random rnd)
        {
            // height = 9
            // possibles = 3, 5, 7 = 3+0, 3+2, 3+4
            // 3 + (rand(3) * 2)
            // rand((9-3)/2 = 6/2 = 3)

            // height = 11
            // possibles = 3, 5, 7, 9 = 3+0, 3+2, 3+4, 3+6
            // 3 + (rand(4) * 2)
            // rand((11-3)/2 = 8/2 = 4)
            int randomMax = (mazeRoom.height - 3) / 2;
            int randomInt = rnd.Next(randomMax) * 2;
            int randomHeight = 3 + randomInt;
            Console.WriteLine("splitting vertically at h="+randomHeight);

            Point2D splitExit1 = new Point2D(rnd.Next(1, mazeRoom.width - 3), randomHeight-1);
            MazeRoom subMaze1 = new MazeRoom(new Maze(mazeRoom.width, randomHeight), mazeRoom.exit1, splitExit1);
            // Console.WriteLine("Sub Maze Exit At: "+splitExit1.ToString());
            GenerateMazeSections(subMaze1, rnd);
            //AddOuterWalls(subMaze1, exit1, splitExit1);
            MergeMazeWalls(mazeRoom, subMaze1, new Point2D(0, 0));

            Point2D splitExit2 = new Point2D(mazeRoom.exit2.x, mazeRoom.height - randomHeight);
            MazeRoom subMaze2 = new MazeRoom(
                new Maze(mazeRoom.width, mazeRoom.height - randomHeight + 1),
                new Point2D(splitExit1.x, 0),
                splitExit2);
            GenerateMazeSections(subMaze2, rnd);
            // AddOuterWalls(subMaze2, new Point2D(splitExit1.x, 0), splitExit2);
            MergeMazeWalls(mazeRoom, subMaze2, new Point2D(0, randomHeight - 1));
        }
        private void SplitHorizontally(MazeRoom mazeRoom, Random rnd)
        {
            // height = 9
            // possibles = 3, 5, 7 = 3+0, 3+2, 3+4
            // 3 + (rand(3) * 2)
            // rand((9-3)/2 = 6/2 = 3)

            // height = 11
            // possibles = 3, 5, 7, 9 = 3+0, 3+2, 3+4, 3+6
            // 3 + (rand(4) * 2)
            // rand((11-3)/2 = 8/2 = 4)
            int randomMax = (mazeRoom.width - 3) / 2;
            int randomInt = rnd.Next(randomMax) * 2;
            int randomWidth = 3 + randomInt;
            Console.WriteLine("splitting vertically at w="+randomWidth);

            Point2D splitExit1 = new Point2D(randomWidth-1, rnd.Next(1, mazeRoom.height - 3));
            MazeRoom subMaze1 = new MazeRoom(new Maze(randomWidth, mazeRoom.height), splitExit1, splitExit1);
            GenerateMazeSections(subMaze1, rnd);
            MergeMazeWalls(mazeRoom, subMaze1, new Point2D(0, 0));

            Point2D splitExit2 = new Point2D(mazeRoom.width - randomWidth, mazeRoom.exit2.y);
            MazeRoom subMaze2 = new MazeRoom(
                new Maze(mazeRoom.width - randomWidth + 1, mazeRoom.height),
                new Point2D(0, splitExit1.y),
                splitExit2);
            GenerateMazeSections(subMaze2, rnd);
            // AddOuterWalls(subMaze2, new Point2D(splitExit1.x, 0), splitExit2);
            MergeMazeWalls(mazeRoom, subMaze2, new Point2D(randomWidth - 1, 0));
        }
        private void AddOuterWalls(MazeRoom mazeRoom)
        {
            for (int y = 0; y < mazeRoom.height; y++)
            {
                for (int x = 0; x < mazeRoom.width; x++)
                {
                    Point2D point = new Point2D(x, y);
                    if (IsOuterWall(mazeRoom.maze, x, y) & !point.Equals(mazeRoom.exit1) & !point.Equals(mazeRoom.exit2))
                    {
                        MazeWall wall = new MazeWall(point);
                        mazeRoom.maze.addWall(wall);
                    }
                }
            }
        }
        private MazeRoom MergeMazeWalls(MazeRoom mainMaze, MazeRoom subMaze, Point2D offset)
        {
            for (int x = 0; x < mainMaze.width; x++)
            {
                for (int y = 0; y < mainMaze.height; y++)
                {
                    Point2D point = new Point2D(x, y);
                    if (mainMaze.maze.hasWallAt(point)) continue;
                    if (x < offset.x) continue;
                    if (y < offset.y) continue;

                    Point2D subMazePoint = new Point2D(x - offset.x, y - offset.y);
                    if (!subMaze.maze.hasWallAt(subMazePoint)) continue;
                    // Console.WriteLine("adding wall from sub maze at: "+point.ToString());
                    MazeWall wall = new MazeWall(point);
                    mainMaze.maze.addWall(wall);
                }
            }

            return mainMaze;
        }
        private bool IsOuterWall(Maze maze, int x, int y)
        {
            if ((x == 0) || (y == 0))
            {
                return true;
            }

            if ((x == maze.width - 1) || (y == maze.height - 1))
            {
                return true;
            }

            return false;
        }

        private int CanBeSubdivided(MazeRoom mazeRoom)
        {
            if (mazeRoom.height == 1 | mazeRoom.width == 1) return SPLITTABLE_NONE;

            bool vertically = mazeRoom.height > 4;
            bool horizontally = mazeRoom.width > 4;

            if (vertically & horizontally) return SPLITTABLE_BOTH;
            if (vertically) return SPLITTABLE_VERTICALLY;
            if (horizontally) return SPLITTABLE_HORIZONTALLY;
            
            return SPLITTABLE_NONE;
        }
    }
    public class MazeRoom
    {
        public int width { get; private set; }
        public int height { get; private set; }
        public Point2D exit1 { get; private set; }
        public Point2D exit2 { get; private set; }
        public Maze maze { get; private set; }
        public MazeRoom(Maze m, Point2D e1, Point2D e2) {
            maze = m;
            width = maze.width;
            height = maze.height;
            exit1 = e1;
            exit2 = e2;
        }
    }
}