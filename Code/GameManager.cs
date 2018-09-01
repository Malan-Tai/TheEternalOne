using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheEternalOne.Code.Map;

namespace TheEternalOne.Code
{
    static class GameManager
    {
        public static Tile[,] Map;

        public const int DrawMapX = 0;
        public const int DrawMapY = 0;

        public const int TileWidth = 85;

        public static void NewGame()
        {
            Map = new Tile[10, 10];
            for (int x = 0; x < 10; x++)
            {
                for (int y = 0; y < 10; y++)
                {
                    Map[x, y] = new Tile(x, y, false);
                }
            }
        }


    }
}
