using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheEternalOne.Code.Game.Death
{
    public class ShieldLoss : I_DeathEffect
    {
        public string Name { get; set; } = "No Shield";
        public string Description { get; set; } = "Lose the ability to use your shield";

        bool I_DeathEffect.CheckIfPossible()
        {
            return GameManager.PlayerObject.Player.CanShield;
        }

        void I_DeathEffect.Apply()
        {
            GameManager.PlayerObject.Player.CanShield = false;
        }
    }
}
