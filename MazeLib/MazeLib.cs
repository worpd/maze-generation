using System;
using System.Collections.Generic;

namespace MazeLib
{
    public class Maze
    {
        public int width { get; private set; }
        public int height { get; private set; }
        public MazeCellCollection walls;
        public Maze(int w, int h) {
            width = w;
            height = h;
            walls = new MazeCellCollection();
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
        public void removeWall(MazeWall wall)
        {
            walls.removeWall(wall);
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
        public Point2D Up()
        {
            return new Point2D(x, y - 1);
        }
        public Point2D Down()
        {
            return new Point2D(x, y + 1);
        }
        public Point2D Left()
        {
            return new Point2D(x - 1, y);
        }
        public Point2D Right()
        {
            return new Point2D(x + 1, y);
        }
        public Point2D TopLeft()
        {
            return new Point2D(x - 1, y - 1);
        }
        public Point2D TopRight()
        {
            return new Point2D(x + 1, y - 1);
        }
        public Point2D BottomLeft()
        {
            return new Point2D(x - 1, y + 1);
        }
        public Point2D BottomRight()
        {
            return new Point2D(x + 1, y + 1);
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

    public class MazeCellCollection
    {
        private Dictionary<Point2D, MazeWall> walls;
        public MazeCellCollection()
        {
            walls = new Dictionary<Point2D, MazeWall>(new Point2D.EqualityComparer());
        }
        public void addWall(MazeWall wall)
        {
            walls.Add(wall.point, wall);

        }
        public void removeWall(MazeWall wall)
        {
            walls.Remove(wall.point);
        }
        public bool hasWallAt(Point2D point)
        {
            return walls.ContainsKey(point);
        }
    }
}
