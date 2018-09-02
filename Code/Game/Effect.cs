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
        public int Offset { get; set; }

        public Texture2D texture;

        public Effect(string text, Color color, string textureName = null)
        {
            Text = text;
            Color = color;
            TimeLeft = 30;
            Offset = 0;

            if (textureName != null) texture = Game1.textureDict[textureName];
            else texture = null;
        }

        public void Draw(SpriteBatch spriteBatch, int x, int y)
        {
            spriteBatch.DrawString(Game1.Font32pt, Text, new Vector2(x, y - TimeLeft - Offset), Color);
            if (texture != null)
            {
                int length = (int)Game1.Font32pt.MeasureString(Text).X;
                spriteBatch.Draw(texture, new Rectangle(x + length, y - TimeLeft - Offset + 10, 30, 30), Color.White);
            }
        }

        public void Update()
        {
            TimeLeft--;
        }
    }
}
