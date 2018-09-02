using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheEternalOne.Code.Game.Death
{
    public class SwordLoss : I_DeathEffect
    {
        public string Name { get; set; } = "No Sword";
        public string Description { get; set; } = "Lose the ability to attack in melee";

        bool I_DeathEffect.CheckIfPossible()
        {
            return GameManager.PlayerObject.Player.CanMelee;
        }

        void I_DeathEffect.Apply()
        {
            GameManager.PlayerObject.Player.CanMelee = false;
        }
    }
}
