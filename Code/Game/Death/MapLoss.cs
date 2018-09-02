using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheEternalOne.Code.Game.Death
{
    public class MapLoss : I_DeathEffect
    {
        public string Name { get; set; } = "No Map";
        public string Description { get; set; } = "Lose the ability to use the map";

        bool I_DeathEffect.CheckIfPossible()
        {
            return GameManager.PlayerObject.Player.CanMap;
        }

        void I_DeathEffect.Apply()
        {
            InputManager.mapOpen = false;
            GameManager.PlayerObject.Player.CanMap = false;
        }
    }
}
