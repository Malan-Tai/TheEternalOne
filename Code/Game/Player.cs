using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheEternalOne.Code.Game
{
    class Player
    {
        public bool CanMelee = true;
        public bool Canranged = true;
        public bool CanOffenseSpell = true;
        public bool CanDebuffSpell = true;
        public bool CanBuffSpell = true;
        public bool CanHealSpell = true;
        public bool CanPickUp = true;
        public bool CanDrop = true;
        public bool CanAccessInventory = true;
        public bool CanEquip = true;
        public bool CanUnequip = true;
        public bool CanMove = true;
    }
}
