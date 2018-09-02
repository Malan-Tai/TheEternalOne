using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheEternalOne.Code.Game.Death
{
    public class UnequipLoss : I_DeathEffect
    {
        public string Name { get; set; } = "Cursed Equipment";
        public string Description { get; set; } = "Lose the ability to manually unequip objects";

        bool I_DeathEffect.CheckIfPossible()
        {
            return GameManager.PlayerObject.Player.CanUnequip;
        }

        void I_DeathEffect.Apply()
        {
            GameManager.PlayerObject.Player.CanUnequip = false;
        }
    }
}
