using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TheEternalOne.Code.Objects;

namespace TheEternalOne.Code.GUI
{
    class StatusGUI
    {
        public int x;
        public int y;
        Texture2D texture;
        //all following textures have the same size and are squares
        Texture2D textureHP;
        Texture2D textureMP;
        //Texture2D textureXP;
        Texture2D textureShield;

        int width;

        public StatusGUI()
        {
            x = 10;
            y = 5;
            texture = Game1.textureDict["white"];
            textureHP = Game1.textureDict["HP_GUI"];
            textureMP = Game1.textureDict["MP_GUI"];
            //textureXP = Game1.textureDict["XP_GUI"];
            textureShield = Game1.textureDict["Shield_GUI"];

            width = (int)(textureHP.Width * Game1.GLOBAL_SIZE_MOD / 100);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            GameObject player = GameManager.PlayerObject;

            spriteBatch.Draw(textureHP, new Rectangle(x, y, width, width), Color.White);

            string HPtext = "HP: " + player.Fighter.HP.ToString() + " / " + player.Fighter.MaxHP.ToString();
            Vector2 HPpos = new Vector2(2 * x + width, y);
            spriteBatch.DrawString(Game1.Font, HPtext, HPpos, Color.Red);

            spriteBatch.Draw(textureShield, new Rectangle(x, 2 * y + width, width, width), Color.White);

            string ShieldText = "Armor: " + player.Fighter.Armor.ToString();
            Vector2 ShieldPos = new Vector2(2 * x + width, 2 * y + width);
            spriteBatch.DrawString(Game1.Font, ShieldText, ShieldPos, Color.SaddleBrown);

            spriteBatch.Draw(textureMP, new Rectangle(x, 3 * y + 2 * width, width, width), Color.White);

            string MPtext = "MP: " + player.Player.MP.ToString() + " / " + player.Player.MaxMP.ToString();
            Vector2 MPpos = new Vector2(2 * x + width, 3 * y + 2 * width);
            spriteBatch.DrawString(Game1.Font, MPtext, MPpos, Color.Blue);

            //spriteBatch.Draw(textureXP, new Rectangle(x, 4 * y + 3 * width, width, width), Color.White);

            //string XPtext = "XP: " + player.Fighter.XP.ToString();
            //Vector2 XPpos = new Vector2(2 * x + width, 4 * y + 3 * width);
            //spriteBatch.DrawString(Game1.Font, XPtext, XPpos, Color.LightBlue);
        }
    }
}
