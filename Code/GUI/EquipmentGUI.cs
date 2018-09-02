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
    class EquipmentGUI
    {
        public int x { get; set; }
        public int y { get; set; }

        Texture2D texture;

        public EquipmentGUI(int x)
        {
            this.x = x;
            this.y = GameManager.EquipmentY;

            texture = Game1.textureDict["white"];
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            int i = 0;
            foreach (GameObject obj in GameManager.PlayerObject.Player.GetAllEquipped())
            {
                int width = (int)(obj.textureWidth * Game1.GLOBAL_SIZE_MOD / 100);

                Color borderColor = new Color(50, 50, 50);
                Color plainColor = Color.Black;

                if (i == InputManager.EqIndex)
                {
                    borderColor = Color.Silver;
                    plainColor = new Color(50, 50, 50);
                }

                spriteBatch.Draw(texture, new Rectangle(x, y + i * GameManager.EquipmentHeight + i * 5, GameManager.EquipmentWidth, GameManager.EquipmentHeight), borderColor);
                spriteBatch.Draw(texture, new Rectangle(x + 5, y + i * GameManager.EquipmentHeight + i * 5 + 5, GameManager.EquipmentWidth - 10, GameManager.EquipmentHeight - 10), plainColor);

                spriteBatch.Draw(obj.texture, new Rectangle(x + 5, y + i * GameManager.EquipmentHeight + i * 5 + 5, width, width), Color.White);

                Vector2 pos = new Vector2(x + width + 10, y + i * GameManager.EquipmentHeight + i * 5 + 10);
                spriteBatch.DrawString(Game1.Font, obj.Name, pos, borderColor, 0f, new Vector2(0, 0), 1.2f, new SpriteEffects(), 0f);

                int startY = (int)pos.Y + 25;
                int yFactor = 0;
                int yOffset = 20;

                if (obj.Equipment.SwordMod != 0)
                {
                    Color color;
                    string prefix = "";
                    if (obj.Equipment.SwordMod > 0)
                    {
                        color = Color.Green;
                        prefix = "+";
                    }
                    else
                    {
                        color = Color.Red;
                    }
                    spriteBatch.DrawString(Game1.Font18pt, "Sword : " + prefix + obj.Equipment.SwordMod.ToString(), new Vector2(pos.X, startY + 10 + yFactor * yOffset), color);
                    yFactor++;
                }

                if (obj.Equipment.ShieldMod != 0)
                {
                    Color color;
                    string prefix = "";
                    if (obj.Equipment.ShieldMod > 0)
                    {
                        color = Color.Green;
                        prefix = "+";
                    }
                    else
                    {
                        color = Color.Red;
                    }
                    spriteBatch.DrawString(Game1.Font18pt, "Shield : " + prefix + obj.Equipment.ShieldMod.ToString(), new Vector2(pos.X, startY + 10 + yFactor * yOffset), color);
                    yFactor++;
                }

                if (obj.Equipment.FireballMod != 0)
                {
                    Color color;
                    string prefix = "";
                    if (obj.Equipment.FireballMod > 0)
                    {
                        color = Color.Green;
                        prefix = "+";
                    }
                    else
                    {
                        color = Color.Red;
                    }
                    spriteBatch.DrawString(Game1.Font18pt, "Fireball : " + prefix + obj.Equipment.FireballMod.ToString(), new Vector2(pos.X, startY + 10 + yFactor * yOffset), color);
                    yFactor++;
                }

                if (obj.Equipment.HealMod != 0)
                {
                    Color color;
                    string prefix = "";
                    if (obj.Equipment.HealMod > 0)
                    {
                        color = Color.Green;
                        prefix = "+";
                    }
                    else
                    {
                        color = Color.Red;
                    }
                    spriteBatch.DrawString(Game1.Font18pt, "Heal : " + prefix + obj.Equipment.HealMod.ToString(), new Vector2(pos.X, startY + 10 + yFactor * yOffset), color);
                    yFactor++;
                }

                if (obj.Equipment.Armor != 0)
                {
                    Color color;
                    string prefix = "";
                    if (obj.Equipment.Armor > 0)
                    {
                        color = Color.Green;
                        prefix = "+";
                    }
                    else
                    {
                        color = Color.Red;
                    }
                    spriteBatch.DrawString(Game1.Font18pt, "Armor : " + prefix + obj.Equipment.Armor.ToString(), new Vector2(pos.X, startY + 10 + yFactor * yOffset), color);
                    yFactor++;
                }



                i++;
            }
        }
    }
}
