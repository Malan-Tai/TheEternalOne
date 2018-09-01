using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheEternalOne.Code.Map;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace TheEternalOne.Code.GUI
{
    class MiniMap
    {
        public Tile[,] Map { get; set; }
        public int x { get; set; }
        public int y { get; set; }

        Texture2D texture;
        int tileWidth = (int)(10 * Game1.GLOBAL_SIZE_MOD / 100);

        public MiniMap(int x, int y, Tile[,] map)
        {
            this.x = x;
            this.y = y;
            Map = map;

            texture = Game1.textureDict["white"];
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            int width = Game1.WIDTH - 2 * x;
            int height = Game1.HEIGHT - 2 * y;
            spriteBatch.Draw(texture, new Rectangle(x, y, width, height), Color.Peru);

            int mapX = (width - GameManager.MAP_WIDTH * tileWidth) / 2;
            int mapY = (height - GameManager.MAP_HEIGHT * tileWidth) / 2;

            for (int x = 0; x < GameManager.MAP_WIDTH; x++)
            {
                for (int y = 0; y < GameManager.MAP_HEIGHT; y++)
                {
                    if (!Map[x, y].Blocked)
                    {
                        spriteBatch.Draw(texture, new Rectangle(mapX + x * tileWidth, mapY + y * tileWidth, tileWidth, tileWidth), Color.Wheat);
                    }
                }
            }
        }
    }
}
