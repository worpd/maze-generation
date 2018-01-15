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
            // height = 11
            // possible exits = 1, 3, 5, 7, 9
            int randomY1 = (rnd.Next((h - 1) / 2) * 2) + 1;
            int randomY2 = (rnd.Next((h - 1) / 2) * 2) + 1;

            // Point2D exit1 = new Point2D(rnd.Next(1, w - 2), 0);
            // Point2D exit2 = new Point2D(rnd.Next(1, w - 2), h - 1);
            Point2D exit1 = new Point2D(0, randomY1);
            Point2D exit2 = new Point2D(w - 1, randomY2);

            Console.WriteLine("Maze Exits: " + exit1 + exit2);

            MazeRoom mazeRoom = new MazeRoom(new Maze(w, h), new List<Point2D>() {exit1, exit2});

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
            Console.WriteLine("CanBeSubdivided: " + splittable);

            switch (splittable)
            {
                case SPLITTABLE_BOTH:
                    int coinFlip = rnd.Next(2);
                    Console.WriteLine("Coin Flip: " + coinFlip);
                    if (coinFlip == 0)
                    {
                        SplitHorizontally(mazeRoom, rnd);
                        // SplitVertically(mazeRoom, rnd);
                    }
                    else
                    {
                        SplitVertically(mazeRoom, rnd);
                        // SplitHorizontally(mazeRoom, rnd);
                    }
                    break;
                case SPLITTABLE_HORIZONTALLY:
                    // SplitHorizontally(mazeRoom, rnd);
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
            // int randomMax = (mazeRoom.height - 3) / 2;
            // int randomInt = rnd.Next(randomMax) * 2;
            // int randomHeight = 3 + randomInt;
            List<int> possibleHeights = mazeRoom.getAvailableVerticalSplits();
            int randomIndex = rnd.Next(possibleHeights.Count);
            int randomHeight = possibleHeights[randomIndex] + 1;
            Console.WriteLine("splitting vertically at h="+randomHeight);

            int randomX = (rnd.Next((mazeRoom.width - 1) / 2) * 2) + 1;
            
            Point2D newWallExit1 = new Point2D(randomX, randomHeight-1);
            Console.WriteLine("newWallExit1: " + newWallExit1);
            List<Point2D> newExits1 = mazeRoom.FindMatchingExits(mazeRoom.width, randomHeight, new Point2D(0, 0));
            newExits1.Add(newWallExit1);

            MazeRoom subMaze1 = new MazeRoom(new Maze(mazeRoom.width, randomHeight), newExits1);
            // Console.WriteLine("Sub Maze Exit At: "+splitExit1.ToString());
            GenerateMazeSections(subMaze1, rnd);
            // AddOuterWalls(subMaze1);
            MergeMazeWalls(mazeRoom, subMaze1, new Point2D(0, 0));

            Point2D newWallExit2 = new Point2D(randomX, 0);
            Console.WriteLine("newWallExit2: " + newWallExit2);
            List<Point2D> newExits2 = mazeRoom.FindMatchingExits(
                mazeRoom.width,
                mazeRoom.height - randomHeight,
                new Point2D(0, randomHeight - 1));
            newExits2.Add(newWallExit2);

            MazeRoom subMaze2 = new MazeRoom(
                new Maze(mazeRoom.width, mazeRoom.height - randomHeight + 1),
                newExits2);
            GenerateMazeSections(subMaze2, rnd);
            // AddOuterWalls(subMaze2);
            MergeMazeWalls(mazeRoom, subMaze2, new Point2D(0, randomHeight - 1));
        }
        private void SplitHorizontally(MazeRoom mazeRoom, Random rnd)
        {
            List<int> possibleWidths = mazeRoom.getAvailableHorizontalSplits();
            int randomIndex = rnd.Next(possibleWidths.Count);
            int randomWidth = possibleWidths[randomIndex] + 1;
            Console.WriteLine("splitting vertically at w="+randomWidth);

            int randomY = (rnd.Next((mazeRoom.height - 1) / 2) * 2) + 1;

            Point2D newWallExit1 = new Point2D(randomWidth-1, randomY);
            Console.WriteLine("newWallExit1: " + newWallExit1);
            List<Point2D> newExits1 = mazeRoom.FindMatchingExits(randomWidth, mazeRoom.height, new Point2D(0, 0));
            newExits1.Add(newWallExit1);
            
            MazeRoom subMaze1 = new MazeRoom(new Maze(randomWidth, mazeRoom.height), newExits1);
            GenerateMazeSections(subMaze1, rnd);
            // AddOuterWalls(subMaze1);
            MergeMazeWalls(mazeRoom, subMaze1, new Point2D(0, 0));

            Point2D newWallExit2 = new Point2D(0, randomY);
            Console.WriteLine("newWallExit2: " + newWallExit2);
            List<Point2D> newExits2 = mazeRoom.FindMatchingExits(
                mazeRoom.width - randomWidth,
                mazeRoom.height,
                new Point2D(randomWidth - 1, 0));
            newExits2.Add(newWallExit2);

            MazeRoom subMaze2 = new MazeRoom(
                new Maze(mazeRoom.width - randomWidth + 1, mazeRoom.height), newExits2);
            GenerateMazeSections(subMaze2, rnd);
            // AddOuterWalls(subMaze2);
            MergeMazeWalls(mazeRoom, subMaze2, new Point2D(randomWidth - 1, 0));
        }
        private void AddOuterWalls(MazeRoom mazeRoom)
        {
            for (int y = 0; y < mazeRoom.height; y++)
            {
                for (int x = 0; x < mazeRoom.width; x++)
                {
                    Point2D point = new Point2D(x, y);
                    if (IsOuterWall(mazeRoom.maze, x, y) & !mazeRoom.isAnExit(point))
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
                    if (IsOuterWall(mainMaze.maze, x, y)) continue;

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
            List<int> horizontalSplits = mazeRoom.getAvailableHorizontalSplits();
            List<int> verticalSplits = mazeRoom.getAvailableVerticalSplits();
            
            bool horizontally = horizontalSplits.Count > 0;
            bool vertically = verticalSplits.Count > 0;
            
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
        public List<Point2D> exits { get; private set; }
        public Maze maze { get; private set; }
        public MazeRoom(Maze m, List<Point2D> es)
        {
            maze = m;
            width = maze.width;
            height = maze.height;
            exits = es;
        }
        public List<int> getAvailableHorizontalSplits()
        {
            List<int> results = new List<int>();

            for (int x = 2; x < width - 1; x += 2)
            {
                if (hasAnyMatchingXs(x)) continue;

                results.Add(x);
            }

            return results;
        }
        public List<int> getAvailableVerticalSplits()
        {
            List<int> results = new List<int>();

            for (int y = 2; y < height - 1; y += 2)
            {
                if (hasAnyMatchingYs(y)) continue;

                results.Add(y);
            }

            return results;
        }
        public List<Point2D> FindMatchingExits(int w, int h, Point2D offset)
        {
            List<Point2D> results = new List<Point2D>();

            for (int x = 0; x < w; x++)
            {
                for (int y = 0; y < h; y++)
                {
                    Point2D point = new Point2D(x + offset.x, y + offset.y);
                    if (isAnExit(point)) results.Add(point);
                }
            }

            return results;
        }
        public bool isAnExit(Point2D point)
        {
            foreach(Point2D exit in exits)
            {
                if (exit.Equals(point)) return true;
            }
            return false;
        }
        private bool hasAnyMatchingXs(int x)
        {
            foreach(Point2D exit in exits)
            {
                if (x == exit.x) return true;
            }

            return false;
        }
        private bool hasAnyMatchingYs(int y)
        {
            foreach(Point2D exit in exits)
            {
                if (y == exit.y) return true;
            }

            return false;
        }
    }
}