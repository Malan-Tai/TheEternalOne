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
    class InventoryGUI
    {
        public int x { get; set; }
        public int y { get; set; }

        Texture2D texture;

        public InventoryGUI()
        {
            x = GameManager.InventoryX;
            y = GameManager.InventoryY;

            texture = Game1.textureDict["white"];
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            int i = 0;
            foreach (GameObject obj in GameManager.PlayerObject.Player.Inventory)
            {
                int width = (int)(obj.textureWidth * Game1.GLOBAL_SIZE_MOD / 100);

                Color borderColor = new Color(50, 50, 50);
                Color plainColor = Color.Black;

                if (i == InputManager.InvIndex)
                {
                    borderColor = Color.Silver;
                    plainColor = new Color(50, 50, 50);
                }

                spriteBatch.Draw(texture, new Rectangle(x + i * GameManager.InventoryWidth + i * 5, y, GameManager.InventoryWidth, GameManager.InventoryHeight), borderColor);
                spriteBatch.Draw(texture, new Rectangle(x + i * GameManager.InventoryWidth + i * 5 + 5, y + 5, GameManager.InventoryWidth - 10, GameManager.InventoryHeight - 10), plainColor);

                spriteBatch.Draw(obj.texture, new Rectangle(x + i * GameManager.InventoryWidth + i * 5 + 5, y + 5, width, width), Color.White);

                string bonusText = "";
                if (obj.Item.Stackable)
                {
                    bonusText = " (" + obj.Item.Amount.ToString() + ")";
                }

                Vector2 pos = new Vector2(x + i * GameManager.InventoryWidth + i * 5 + width + 10, y + 10);
                spriteBatch.DrawString(Game1.Font, obj.Name + bonusText, pos, borderColor);




                i++;
            }
        }
    }
}
