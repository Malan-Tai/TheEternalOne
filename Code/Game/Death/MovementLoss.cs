using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheEternalOne.Code.Game.Death
{
    public class MovementLoss : I_DeathEffect
    {
        public string Name { get; set; } = "Immobility";
        public string Description { get; set; } = "Lose the ability to move by traditional means";

        bool I_DeathEffect.CheckIfPossible()
        {
            return GameManager.PlayerObject.Player.CanMove;
        }

        void I_DeathEffect.Apply()
        {
            GameManager.PlayerObject.Player.CanMove = false;
        }
    }
}
