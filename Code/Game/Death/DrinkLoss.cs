using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheEternalOne.Code.Game.Death
{
    public class DrinkLoss : I_DeathEffect
    {
        public string Name { get; set; } = "No Potions";
        public string Description { get; set; } = "Lose the ability to drink potions";

        bool I_DeathEffect.CheckIfPossible()
        {
            return GameManager.PlayerObject.Player.CanDrink;
        }

        void I_DeathEffect.Apply()
        {
            GameManager.PlayerObject.Player.CanDrink = false;
        }
    }
}
