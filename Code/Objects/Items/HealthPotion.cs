using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheEternalOne.Code.Game;
using Microsoft.Xna.Framework;

namespace TheEternalOne.Code.Objects.Items
{
    class HealthPotion : Item
    {
        public const int POTION_HEAL = 5;
        public HealthPotion(int amount = 1)
        {
            this.Stackable = true;
            this.Amount = amount;
        }

        public override void Use()
        {
            if (GameManager.PlayerObject.Player.CanDrink)
            {
                int prevHP = GameManager.PlayerObject.Fighter.HP;
                GameManager.PlayerObject.Fighter.HP += POTION_HEAL;
                if (GameManager.PlayerObject.Fighter.HP > GameManager.PlayerObject.Fighter.MaxHP)
                {
                    GameManager.PlayerObject.Fighter.HP = GameManager.PlayerObject.Fighter.MaxHP;
                }

                int healed = GameManager.PlayerObject.Fighter.HP - prevHP;
                if (healed > 0)
                {
                    Effect effect = new Effect("+" + healed.ToString(), Color.DarkGreen);
                    GameManager.PlayerObject.Effects.Add(effect);
                }

                if (Amount > 1)
                {
                    Amount -= 1;
                }
                else
                {
                    GameManager.PlayerObject.Player.Inventory.Remove(Owner);
                }
            }
            else
            {
                GameManager.LogWarning("You cannot drink potions anymore !");
            }
        }
    }
}
