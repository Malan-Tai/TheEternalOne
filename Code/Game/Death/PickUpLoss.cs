using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheEternalOne.Code.Game.Death
{
    public class PickUpLoss : I_DeathEffect
    {
        public string Name { get; set; } = "Slippy Hands";
        public string Description { get; set; } = "Lose the ability to pick up objects from the floor";

        bool I_DeathEffect.CheckIfPossible()
        {
            return GameManager.PlayerObject.Player.CanPickUp;
        }

        void I_DeathEffect.Apply()
        {
            GameManager.PlayerObject.Player.CanPickUp = false;
        }
    }
}
