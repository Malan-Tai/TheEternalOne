using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using TheEternalOne.Code.Objects;

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
            if (texture != null)
            {
                int picX = GameManager.DrawMapX + (GameManager.VisibleMapWidth / 2 + x - px) * (int)(GameManager.TileWidth * Game1.GLOBAL_SIZE_MOD / 100);
                int picY = GameManager.DrawMapY + (GameManager.VisibleMapHeight / 2 + y - py) * (int)(GameManager.TileWidth * Game1.GLOBAL_SIZE_MOD / 100);

                GameObject player = GameManager.PlayerObject;
                int offsetX = -player.OffsetPos.x + player.BigPos.x;
                int offsetY = -player.OffsetPos.y + player.BigPos.y;

                int width = (int)(GameManager.TileWidth * Game1.GLOBAL_SIZE_MOD / 100);

                spriteBatch.Draw(texture, new Rectangle(picX + offsetX, picY + offsetY, width, width), Color.White);
            }
        }
    }
}
