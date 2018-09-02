using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheEternalOne.Code.Game.Death
{
    public class FireballLoss : I_DeathEffect
    {
        public string Name { get; set; } = "No Fireball";
        public string Description { get; set; } = "Lose the ability to cast offensive spells";

        bool I_DeathEffect.CheckIfPossible()
        {
            return GameManager.PlayerObject.Player.CanRanged;
        }

        void I_DeathEffect.Apply()
        {
            GameManager.PlayerObject.Player.CanRanged = false;
        }
    }
}
