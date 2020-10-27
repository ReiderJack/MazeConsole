using System;

namespace Maze
{
    public struct Point
    {
        public readonly int Y, X;
        public readonly double Length;

        public Point(int y, int x)
        {
            Y = y;
            X = x;
            Length = Math.Sqrt((x * x) + (y * y));
        }

        public static Point operator -(Point a, Point b)
        {
            return new Point(a.Y - b.Y, a.X - b.X);
        }

        public static Point operator +(Point a, Point b)
        {
            return new Point(a.Y + b.Y, a.X + b.X);
        }
    }
}
