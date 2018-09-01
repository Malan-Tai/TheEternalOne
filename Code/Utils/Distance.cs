using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheEternalOne.Code.Map;

namespace TheEternalOne.Code.Utils
{
    public static class Distance
    {
        public static double GetDistance(Vector2 start, Vector2 end)
        {
            return Math.Sqrt(Math.Pow((int)end.X - (int)start.X, 2) + Math.Pow((int)end.Y - (int)start.Y, 2));
        }

        public static double GetDistance(Coord start, Coord end)
        {
            return Math.Sqrt(Math.Pow(end.x - start.x, 2) + Math.Pow(end.y - start.y, 2));
        }

        public static double GetDistance(int x1, int y1, int x2, int y2)
        {
            return Math.Sqrt(Math.Pow(x2 - x1, 2) + Math.Pow(y2 - y1, 2));
        }

        public static double GetDistance(Tile tile1, Tile tile2)
        {
            return Math.Sqrt(Math.Pow(tile2.x - tile1.x, 2) + Math.Pow(tile2.y - tile1.y, 2));
        }
    }
}
