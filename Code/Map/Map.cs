using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;

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

        public static void UpdateTexture(Tile[,] map)
        {
            for (int x = 0; x < GameManager.MAP_WIDTH; x++)
            {
                for (int y = 0; y < GameManager.MAP_HEIGHT; y++)
                {
                    bool right = (x != GameManager.MAP_WIDTH - 1 && !map[x + 1, y].Blocked);    //bool = right blocked
                    bool left = (x != 0 && !map[x - 1, y].Blocked);
                    bool above = (y != 0 && !map[x, y - 1].Blocked);
                    bool under = (y != GameManager.MAP_HEIGHT - 1 && !map[x, y + 1].Blocked);

                    bool right_up = (x != GameManager.MAP_WIDTH - 1 && y != 0 && !map[x + 1, y - 1].Blocked);
                    bool right_down = (x != GameManager.MAP_WIDTH - 1 && y != GameManager.MAP_HEIGHT - 1 && !map[x + 1, y + 1].Blocked);
                    bool left_up = (x != 0 && y != 0 && !map[x - 1, y - 1].Blocked);
                    bool left_down = (x != 0 && y != GameManager.MAP_HEIGHT - 1 && !map[x - 1, y + 1].Blocked);

                    Texture2D texture = null;
                    if (map[x, y].Blocked)
                    {
                        if (left && right && above && under && right_down && right_up && left_down && left_up) texture = null;
                        else if (right && left && above) texture = Game1.textureDict["wall_right_left_up"];
                        else if (right && left && under) texture = Game1.textureDict["wall_right_left_down"];
                        else if (above && under && left) texture = Game1.textureDict["wall_left_down_up"];
                        else if (above && under && right) texture = Game1.textureDict["wall_right_down_up"];
                        else if (right && left) texture = Game1.textureDict["wall_right_left"];
                        else if (above && under) texture = Game1.textureDict["wall_down_up"];
                        else if (right && above) texture = Game1.textureDict["wall_right_up"];
                        else if (right && under) texture = Game1.textureDict["wall_right_down"];
                        else if (left && above) texture = Game1.textureDict["wall_left_up"];
                        else if (left && under) texture = Game1.textureDict["wall_left_down"];
                        else if (left) texture = Game1.textureDict["wall_left"];
                        else if (right) texture = Game1.textureDict["wall_right"];
                        else if (above) texture = Game1.textureDict["wall_up"];
                        else if (under) texture = Game1.textureDict["wall_down"];
                        else if (right_up) texture = Game1.textureDict["wall_right_up_corner"];
                        else if (right_down) texture = Game1.textureDict["wall_right_down_corner"];
                        else if (left_up) texture = Game1.textureDict["wall_left_up_corner"];
                        else if (left_down) texture = Game1.textureDict["wall_left_down_corner"];
                        //else if (!(left || right || above || under || right_down || right_up || left_down || left_up)) texture = Game1.textureDict["floor_tile"];
                    }
                    else texture = Game1.textureDict["floor_tile"];

                    map[x, y].texture = texture;
                }
            }
        }
    }
}
