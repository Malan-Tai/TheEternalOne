using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheEternalOne.Code.Game.Death
{
    public class DropLoss : I_DeathEffect
    {
        public string Name { get; set; } = "Hoarding Obsession";
        public string Description { get; set; } = "Lose the ability to drop items";

        bool I_DeathEffect.CheckIfPossible()
        {
            return GameManager.PlayerObject.Player.CanDrop;
        }

        void I_DeathEffect.Apply()
        {
            GameManager.PlayerObject.Player.CanDrop = false;
        }
    }
}
