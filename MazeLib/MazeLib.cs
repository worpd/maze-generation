using System;
using System.Collections.Generic;

namespace MazeLib
{
    public class Maze
    {
        public int width { get; private set; }
        public int height { get; private set; }
        public MazeWallCollection walls;
        public Maze(int w, int h) {
            width = w;
            height = h;
            walls = new MazeWallCollection();
        }

        // Returns whether the cell at coordinates (x, y) is a wall.
        public bool hasWallAt(Point2D point)
        {
            return walls.hasWallAt(point);
        }

        public void addWall(MazeWall wall)
        {
            walls.addWall(wall);
        }
    }

    public class Point2D
    {
        public int x { get; private set; }
        public int y { get; private set; }
        public Point2D(int x, int y)
        {
            this.x = x;
            this.y = y;
        }
        override public string ToString()
        {
            return base.ToString()+"("+x+","+y+")";
        }

        public bool Equals(Point2D point)
        {
            return ((point.x == x) & (point.y == y));
        }
        public class EqualityComparer : IEqualityComparer<Point2D>
        {
            public bool Equals(Point2D point1, Point2D point2)
            {
                return point1.Equals(point2);
            }

            public int GetHashCode(Point2D point)
            {
                return point.x ^ point.y;
            }
        }
    }

    public class MazeCell
    {
        public Point2D point { get; private set; }
        public MazeCell(Point2D point)
        {
            this.point = point;
        }
    }

    public class MazeWall: MazeCell
    {
        public MazeWall(Point2D point): base(point) {}
    }
    public class MazeEntrance: MazeCell
    {
        public MazeEntrance(Point2D point): base(point) {}
    }
    public class MazeExit: MazeCell
    {
        public MazeExit(Point2D point): base(point) {}
    }

    public class MazeWallCollection
    {
        private Dictionary<Point2D, MazeWall> walls;
        public MazeWallCollection()
        {
            walls = new Dictionary<Point2D, MazeWall>(new Point2D.EqualityComparer());
        }
        public void addWall(MazeWall wall)
        {
            walls.Add(wall.point, wall);

        }
        public bool hasWallAt(Point2D point)
        {
            return walls.ContainsKey(point);
        }
    }
}
