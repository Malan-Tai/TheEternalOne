using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheEternalOne.Code.Map;
using TheEternalOne.Code.Objects;

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
        public const int VisibleMapWidth = 15;
        public const int VisibleMapHeight = 15;

        public const int screenPlayerX = VisibleMapWidth / 2 + 1;
        public const int screenPlayerY = VisibleMapHeight / 2 + 1;

        public static GameObject PlayerObject;
        public static List<GameObject> Objects;

        public static void NewGame()
        {
            PlayerObject = new GameObject(20, 20);
            Objects = new List<GameObject> { PlayerObject };

            Map = new Tile[MapWidth, MapHeight];
            for (int x = 0; x < MapWidth; x++)
            {
                for (int y = 0; y < MapHeight; y++)
                {
                    Map[x, y] = new Tile(x, y, false);
                }
            }
        }


    }
}
