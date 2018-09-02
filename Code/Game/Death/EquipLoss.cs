using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheEternalOne.Code.Game.Death
{
        public class EquipLoss : I_DeathEffect
        {
            public string Name { get; set; } = "No Equipping";
            public string Description { get; set; } = "Lose the ability to equip objects";

            bool I_DeathEffect.CheckIfPossible()
            {
                return GameManager.PlayerObject.Player.CanEquip;
            }

            void I_DeathEffect.Apply()
            {
                GameManager.PlayerObject.Player.CanEquip = false;
            }
        }
    }

