using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheEternalOne.Code.Map;
using TheEternalOne.Code.ProcGen.MapGen;

namespace TheEternalOne.Code
{
    static class GameManager
    {
        public static Tile[,] Map;

        public const int DrawMapX = 0;
        public const int DrawMapY = 0;


        public const int TileWidth = 85;

        public const int MAP_WIDTH = 100;

        public const int MAP_HEIGHT = 80;

        public static void NewGame()
        {
            Map = MapMaker.MakeTunnelMap(false);
            //for (int x = 0; x < MAP_WIDTH; x++)
            //{
            //    for (int y = 0; y < MAP_HEIGHT; y++)
            //    {
            //        Map[x, y] = new Tile(x, y, false);
            //    }
            //}


        }


    }
}
