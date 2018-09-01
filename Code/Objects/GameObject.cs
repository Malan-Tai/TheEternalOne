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
        public int x { get; set; }
        public int y { get; set; }

        private int DrawOffsetX { get; set; }
        private int DrawOffsetY { get; set; }

        public GameObject(int x, int y, int dx, int dy)
        {
            this.x = x;
            this.y = y;

            DrawOffsetX = dx;
            DrawOffsetY = dy;
        }

        public void Move(int dx, int dy)
        {
            if (!GameManager.Map[x + dx, y + dy].Blocked)
            {
                x += dx;
                y += dy;
            }
        }

        public void Draw(SpriteBatch spriteBatch, int px, int py)
        {
            int drawX = GameManager.DrawMapX + DrawOffsetX;
            Vector2 position = new Vector2();
        }
    }
}
