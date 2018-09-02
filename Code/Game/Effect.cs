using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TheEternalOne.Code.Game
{
    public class Effect
    {
        public string Text { get; set; }
        public Color Color { get; set; }
        public int TimeLeft { get; set; }

        public Effect(string text, Color color)
        {
            Text = text;
            Color = color;
            TimeLeft = 30;
        }

        public void Draw(SpriteBatch spriteBatch, int x, int y)
        {
            spriteBatch.DrawString(Game1.Font32pt, Text, new Vector2(x, y - TimeLeft), Color);
        }

        public void Update()
        {
            TimeLeft--;
        }
    }
}
