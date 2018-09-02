using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheEternalOne.Code.Game.Death
{
    public class HealLoss : I_DeathEffect
    {
        public string Name { get; set; } = "No Heal";
        public string Description { get; set; } = "Lose the ability to heal yourself using spells";

        bool I_DeathEffect.CheckIfPossible()
        {
            return GameManager.PlayerObject.Player.CanHealSpell;
        }

        void I_DeathEffect.Apply()
        {
            GameManager.PlayerObject.Player.CanHealSpell = false;
        }
    }
}
