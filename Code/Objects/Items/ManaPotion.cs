using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheEternalOne.Code.Game;
using Microsoft.Xna.Framework;

namespace TheEternalOne.Code.Objects.Items
{
    class ManaPotion : Item
    {
        public const int POTION_REGEN = 3;

        public ManaPotion(int amount = 1)
        {
            this.Stackable = true;
            this.Amount = amount;
        }

        public override void Use()
        {
            if (GameManager.PlayerObject.Player.CanDrink)
            {
                int prevMana = GameManager.PlayerObject.Player.MP;
                GameManager.PlayerObject.Player.MP = Math.Min(GameManager.PlayerObject.Player.MaxMP, GameManager.PlayerObject.Player.MP + POTION_REGEN);

                int regened = GameManager.PlayerObject.Player.MP - prevMana;
                if (regened > 0)
                {
                    Effect effect = new Effect("+" + regened.ToString(), Color.Blue, "MP_GUI");
                    Owner.AddEffect(effect);
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
