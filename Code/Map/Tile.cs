using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace TheEternalOne.Code.Map
{
    class Tile
    {
        public int x { get; set; }
        public int y { get; set; }

        public bool Blocked { get; set; }

        private Texture2D texture;

        public Tile(int x, int y, bool blocked)
        {
            this.x = x;
            this.y = y;
            Blocked = blocked;
            texture = Game1.textureDict["tile50x50"];
        }

        public void Draw(SpriteBatch spriteBatch, int px, int py)
        {
            int drawX = GameManager.DrawMapX + (int)(x * GameManager.TileWidth * Game1.GLOBAL_SIZE_MOD / 100);
            int drawY = GameManager.DrawMapY + (int)(y * GameManager.TileWidth * Game1.GLOBAL_SIZE_MOD / 100);

            int width = (int)(GameManager.TileWidth * Game1.GLOBAL_SIZE_MOD / 100);

            Console.Out.WriteLine("drawing tile at {0} {1} on pixels {2} {3}, width {4}", x, y, drawX, drawY, width);

            spriteBatch.Draw(texture, new Rectangle(drawX, drawY, width, width), Color.White);
        }
    }
}
