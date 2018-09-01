using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheEternalOne.Code.Map
{
    public static class Map
    {

        public static Tile[,] InitMap(bool blocked)
        {
            Tile[,] map = new Tile[GameManager.MAP_WIDTH, GameManager.MAP_HEIGHT];

            for (int y = 0; y < GameManager.MAP_HEIGHT; y++)
            {
                for (int x = 0; x < GameManager.MAP_WIDTH; x++)
                {
                    bool wall = (x == 0) || (y == 0) || (y == GameManager.MAP_HEIGHT - 1) || (x == GameManager.MAP_WIDTH - 1);
                    bool actualWall = (wall || blocked);
                    map[x, y] = new Tile(x, y, actualWall);
                }
            }
            return map;
        }
    }
}
