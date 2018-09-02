using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheEternalOne.Code.Game.Death
{
    public class TPLoss : I_DeathEffect
    {
        public string Name { get; set; } = "No Teleport";
        public string Description { get; set; } = "Lose the ability to teleport at will";

        bool I_DeathEffect.CheckIfPossible()
        {
            return GameManager.PlayerObject.Player.CanTPSpell;
        }

        void I_DeathEffect.Apply()
        {
            GameManager.PlayerObject.Player.CanTPSpell = false;
        }
    }
}
