using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace TheEternalOne.Code.Game
{
    public class Message
    {
        public string Content;
        public Color Color;

        public Message(string text, Color color)
        {
            this.Content = text;
            this.Color = color;

            //Console.Out.WriteLine(text);
        }
    }
}
