using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheEternalOne.Code.Map
{
    class Room
    {
        public int x1 { get; private set; }
        public int x2 { get; private set; }
        public int y1 { get; private set; }
        public int y2 { get; private set; }
        public Vector2[] Tiles { get; private set; }

        public Vector2 Center { get; private set; }

        public Room(int x, int y, int w, int h)
        {
            x1 = x;
            y1 = y;
            x2 = x + w;
            y2 = y + h;

            Tiles = new Vector2[(w - 1) * (h - 1)];
            int i = 0;
            for (x = x1 + 1; x < x2; x++)
            {
                for (y = y1 + 1; y < y2; y++)
                {
                    Tiles[i] = new Vector2(x, y);
                    i++;
                }
            }

            Center = new Vector2((x1 + x2) / 2, (y1 + y2) / 2);
        }

        public bool Intersect(Room other)
        {
            return (x1 <= other.x2) && (x2 >= other.x1) && (y1 <= other.y2) && (y2 >= other.y1);
        }

        public bool CheckForCaveIntersection(Tuple<int, int>[] caveTiles)
        {
            foreach (Vector2 tile in Tiles)
            {
                if (Array.IndexOf(caveTiles, tile) != -1) return false;
            }
            return true;

        }
    }
}
