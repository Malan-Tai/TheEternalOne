using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace TheEternalOne.Code.Objects
{
    class GameObject
    {
        public Coord Position { get; set; }
        public Coord OffsetPos { get; set; }
        public Coord BigPos { get; set; }

        public int x
        {
            get
            {
                return Position.x;
            }
            set
            {
                Position = new Coord(value, Position.y);
            }
        }

        public int y
        {
            get
            {
                return Position.y;
            }
            set
            {
                Position = new Coord(Position.x, value);
            }
        }

        public Player Player { get; set; }
        public Fighter Fighter { get; set; }

        public Texture2D texture;
        public int textureWidth;
        public int textureHeight;

        public GameObject(int x, int y)
        {
            this.Position = new Coord(x, y);
            this.OffsetPos = new Coord((int)(x * GameManager.TileWidth * Game1.GLOBAL_SIZE_MOD / 100), (int)(y * GameManager.TileWidth * Game1.GLOBAL_SIZE_MOD / 100));
            this.BigPos = new Coord((int)(x * GameManager.TileWidth * Game1.GLOBAL_SIZE_MOD / 100), (int)(y * GameManager.TileWidth * Game1.GLOBAL_SIZE_MOD / 100));
        }

        public void Move(int dx, int dy)
        {
            if (!GameManager.Map[x + dx, y + dy].Blocked)
            {
                Position = new Coord(Position.x + dx, Position.y + dy);
                BigPos = new Coord((int)(Position.x * GameManager.TileWidth * Game1.GLOBAL_SIZE_MOD / 100), (int)(Position.y * GameManager.TileWidth * Game1.GLOBAL_SIZE_MOD / 100));
            }
        }

        public void Draw(SpriteBatch spriteBatch, int px, int py)
        {
            int offsetX;
            int offsetY;
            if (Player != null)
            {
                offsetX = 0;
                offsetY = 0;
            }
            else
            {
                offsetX = OffsetPos.x - BigPos.x - GameManager.PlayerObject.OffsetPos.x + GameManager.PlayerObject.BigPos.x;
                offsetY = OffsetPos.y - BigPos.y - GameManager.PlayerObject.OffsetPos.y + GameManager.PlayerObject.BigPos.y;
            }

            int? x = (int)((GameManager.screenPlayerX + GameManager.VisibleMapWidth / 2 + Position.x - px) * GameManager.TileWidth * Game1.GLOBAL_SIZE_MOD / 100) + GameManager.DrawMapX + (int)((GameManager.TileWidth - textureWidth) * Game1.GLOBAL_SIZE_MOD / 200) + offsetX;
            int? y = (int)((GameManager.screenPlayerY + GameManager.VisibleMapHeight / 2 + Position.y - py) * GameManager.TileWidth * Game1.GLOBAL_SIZE_MOD / 100) + GameManager.DrawMapY + (int)((GameManager.TileWidth - textureHeight) * Game1.GLOBAL_SIZE_MOD / 100) + offsetY; //- GameManager.feetOffset  ;

            Vector2 position = new Vector2(x ?? default(int), y ?? default(int)); // The statement var1 = var2 ?? var3 assigns the value var2 to var1 if var2 is not null, otherwise it assigns var3.

            spriteBatch.Draw(texture, new Rectangle(x ?? default(int), y ?? default(int), textureWidth, textureHeight), Color.White);
        }

        public void Update()
        {
            if (OffsetPos.x != BigPos.x || OffsetPos.y != BigPos.y)
            {
                int dx = 0, dy = 0;
                if (OffsetPos.x < BigPos.x) dx = 5;
                else if (OffsetPos.x > BigPos.x) dx = -5;

                if (OffsetPos.y < BigPos.y) dy = 5;
                else if (OffsetPos.y > BigPos.y) dy = -5;

                OffsetPos = new Coord(OffsetPos.x + dx, OffsetPos.y + dy);
            }
        }
    }
}
