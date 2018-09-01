using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheEternalOne.Code.Objects
{
    class Fighter
    {
        public int Power { get; set; }
        public int MaxHP { get; set; }
        public int HP { get; set; }
        public int Armor { get; set; }

        public GameObject Owner { get; set; }
        
        public Fighter(int pow, int hp, int arm)
        {
            Power = pow;
            MaxHP = hp;
            HP = hp;
            Armor = arm;
        }

        public void TakeDamage(int dmg)
        {
            int armorDmg = Math.Min(Armor, dmg);
            Armor -= armorDmg;
            HP -= dmg - armorDmg;
        }
    }
}
