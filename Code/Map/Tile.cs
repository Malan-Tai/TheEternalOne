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
    public class Tile
    {
        public int x { get; set; }
        public int y { get; set; }

        public bool Blocked { get; set; }
        public bool Wall { get; set; }
        public bool InTunnel { get; set; }
        public bool Door { get; set; }
        public bool InRoom { get; set; }


        public Tile(int x, int y, bool blocked)
        {
            this.x = x;
            this.y = y;
            Blocked = blocked;
            Wall = blocked;
 
            InTunnel = false;
            Door = false;
            InRoom = false;
        }

        public void Draw(SpriteBatch spriteBatch, int px, int py)
        {
            int picX = GameManager.DrawMapX + (GameManager.VisibleMapWidth / 2 + x - px) * (int)(GameManager.TileWidth * Game1.GLOBAL_SIZE_MOD / 100);
            int picY = GameManager.DrawMapY + (GameManager.VisibleMapHeight / 2 + y - py) * (int)(GameManager.TileWidth * Game1.GLOBAL_SIZE_MOD / 100);

            GameObject player = GameManager.PlayerObject;
            int offsetX = -player.OffsetPos.x + player.BigPos.x;
            int offsetY = -player.OffsetPos.y + player.BigPos.y;


            int width = (int)(GameManager.TileWidth * Game1.GLOBAL_SIZE_MOD / 100);
            Texture2D texture;
            if (!Blocked)
            {
                texture = Game1.textureDict["tile50x50"];
            }
            else
            {
                texture = Game1.textureDict["wall"];
            }
            spriteBatch.Draw(texture, new Rectangle(picX + offsetX, picY + offsetY, width, width), Color.White);
        }

        public Tile[] Neighbors(ref Tile[,] map, bool cardinal)
        {
            Tile[] neighbors = new Tile[8];
            int i = 0;

            bool canUp = (y > 0);
            bool canDown = (y < GameManager.MAP_HEIGHT - 1);
            bool canLeft = (x > 0);
            bool canRight = (x < GameManager.MAP_WIDTH - 1);

            if (canUp)
            {
                neighbors[i] = map[y - 1, x];
                i++;
            }
            if (canDown)
            {
                neighbors[i] = map[y + 1, x];
                i++;
            }
            if (canLeft)
            {
                neighbors[i] = map[y, x - 1];
                i++;
            }
            if (canRight)
            {
                neighbors[i] = map[y, x + 1];
                i++;
            }
            if (canUp && canLeft && !cardinal)
            {
                neighbors[i] = map[y - 1, x - 1];
                i++;
            }
            if (canUp && canRight && !cardinal)
            {
                neighbors[i] = map[y - 1, x + 1];
                i++;
            }
            if (canDown && canLeft && !cardinal)
            {
                neighbors[i] = map[y + 1, x - 1];
                i++;
            }
            if (canDown && canRight && !cardinal)
            {
                neighbors[i] = map[y + 1, x + 1];
                i++;
            }

            Tile[] finalNeigh = new Tile[i];
            int j;
            for (j = 0; j < i; j++)
            {
                finalNeigh[j] = neighbors[j];
            }

            return finalNeigh;
        }

        public int CountNeighbors(ref Tile[,] map, bool cardinal = false)
        {
            int n = 0;
            foreach (Tile tile in Neighbors(ref map, cardinal))
            {
                if (tile.Blocked) n++;
            }

            return n;
        }
    }
}
