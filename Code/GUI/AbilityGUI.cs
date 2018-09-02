using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using TheEternalOne.Code;
using Microsoft.Xna.Framework;
using TheEternalOne.Code.Objects;
using TheEternalOne.Code.Objects.Equipments;

namespace TheEternalOne.Code.GUI
{
    class AbilityGUI
    {
        public int x;
        public int y;
        Texture2D texture;
        public Texture2D upgradeTexture;

        public AbilityGUI()
        {
            x = Game1.WIDTH - GameManager.AbilityWidth - 10;
            y = 5;
            texture = Game1.textureDict["white"];
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            GameObject player = GameManager.PlayerObject;

            List<GameObject> allEquip = player.Player.GetAllEquipped();
            int FireballBonus = 0;
            int ShieldBonus = 0;
            int HealBonus = 0;
            int TPBonus = 0;
            int PowerBonus = 0;

            foreach (GameObject equip in allEquip)
            {
                Equipment eqComp = equip.Equipment;
                FireballBonus += eqComp.FireballMod;
                ShieldBonus += eqComp.ShieldMod;
                HealBonus += eqComp.HealMod;
                TPBonus += eqComp.TPMod;
                PowerBonus += eqComp.SwordMod;
            }

            for (int i = 0; i < 5; i++)
            {
                Color borderColor = new Color(50, 50, 50);
                Color plainColor = Color.Black;

                if (i == InputManager.SpellIndex || i == InputManager.SelectedSpellIndex)
                {
                    borderColor = Color.Silver;
                    plainColor = new Color(50, 50, 50);
                }

                spriteBatch.Draw(texture, new Rectangle(x, y + i * GameManager.AbilityHeight + i * 5, GameManager.AbilityWidth, GameManager.AbilityHeight), borderColor);
                spriteBatch.Draw(texture, new Rectangle(x + 5, y + i * GameManager.AbilityHeight + i * 5 + 5, GameManager.AbilityWidth - 10, GameManager.AbilityHeight - 10), plainColor);

                string spell = GameManager.PlayerObject.Player.Spells[i];
                Vector2 pos = new Vector2(x + 15, y + i * GameManager.AbilityHeight + i * 5 + 10);
                spriteBatch.DrawString(Game1.Font, spell, pos, borderColor, 0f, new Vector2(0, 0), 1.2f, new SpriteEffects(), 0f);

                if (i != 4)
                {
                    upgradeTexture = Game1.textureDict["upgrade_GUI"];
                    if (InputManager.UpgradeSpell && i == InputManager.SpellIndex)
                    {
                        upgradeTexture = Game1.textureDict["upgrade_GUI_lit"];
                    }
                    int width = (int)(upgradeTexture.Width * 0.7f * Game1.GLOBAL_SIZE_MOD / 100);

                    spriteBatch.Draw(upgradeTexture, new Rectangle(Game1.WIDTH - width - 20, y + i * GameManager.AbilityHeight + i * 5 + 10, width, width), Color.White);
                }

                List<string> desc = new List<string> { "" };
                if (i == 0)
                {
                    desc = new List<string> { "Hit target adjacent", "enemy for " + Math.Max(0, player.Fighter.Power + PowerBonus).ToString() + " damage." };
                }
                else if (i == 1)
                {
                    desc = new List<string> { "Gain " + Math.Max(0, player.Player.ShieldPower + ShieldBonus).ToString() + " armor and push", "target adjacent enemy", "away." };
                }
                else if (i == 2)
                {
                    desc = new List<string> { "Shoot target enemy for", Math.Max(0, player.Player.FireballDmg + FireballBonus).ToString() + " damage.", "(5 MP)" };
                }
                else if (i == 3)
                {
                    desc = new List<string> { "Heal yourself for " + Math.Max(0, player.Player.HealPower + HealBonus).ToString() + " HP.", "(3 MP)" };
                }
                else if (i == 4)
                {
                    desc = new List<string> { "Teleport to target free", "tile.", "(1 MP)" };
                }

                for (int j = 0; j < desc.Count; j++)
                {
                    pos = new Vector2(x + 15, y + i * GameManager.AbilityHeight + i * 5 + 50 + j * 20);
                    spriteBatch.DrawString(Game1.Font, desc[j], pos, borderColor);
                }
            }
        }
    }
}
