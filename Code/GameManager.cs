using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheEternalOne.Code.Map;
using TheEternalOne.Code.Objects;
using TheEternalOne.Code.ProcGen.MapGen;

namespace TheEternalOne.Code
{
    static class GameManager
    {
        public static Tile[,] Map;

        public const int DrawMapX = 0;
        public const int DrawMapY = 0;


        public const int TileWidth = 85;
        public const int MapWidth = 50;
        public const int MapHeight = 50;
        public const int VisibleMapWidth = 17;
        public const int VisibleMapHeight = 11;

        public static int DrawMapX = (Game1.WIDTH - (int)(VisibleMapWidth * TileWidth * Game1.GLOBAL_SIZE_MOD / 100)) / 2;
        public static int DrawMapY = 10;

        public const int screenPlayerX = VisibleMapWidth / 2;
        public const int screenPlayerY = VisibleMapHeight / 2;

        public static GameObject PlayerObject;
        public static List<GameObject> Objects;

        public const int MAP_WIDTH = 100;

        public const int MAP_HEIGHT = 80;

        public static void NewGame()
        {

            PlayerObject = new GameObject(20, 20, "white", 100, 100);
            PlayerObject.Player = new Player(10);
            PlayerObject.Player.Owner = PlayerObject;
            PlayerObject.Fighter = new Fighter(10, 20, 0);
            PlayerObject.Fighter.Owner = PlayerObject;

            Objects = new List<GameObject> { PlayerObject };

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
