using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            GameManager.PlayerObject.Fighter.HP += POTION_HEAL;
            if (GameManager.PlayerObject.Fighter.HP > GameManager.PlayerObject.Fighter.MaxHP)
            {
                GameManager.PlayerObject.Fighter.HP = GameManager.PlayerObject.Fighter.MaxHP;
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
    }
}
