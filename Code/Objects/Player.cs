using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheEternalOne.Code.Objects
{
    class Player
    {
        public int LeftXP { get; set; }

        public int FireballDmg { get; set; }
        public int HealPower { get; set; }
        public int ShieldPower { get; set; }

        public int MaxMP { get; set; }
        public int MP { get; set; }

        public GameObject Owner { get; set; }

        public List<string> Spells;

        public Player(int mp)
        {
            FireballDmg = 1;
            HealPower = 1;
            ShieldPower = 1;

            MaxMP = mp;
            MP = mp;

            Spells = new List<string> { "sword", "shield", "fireball", "heal", "teleport" };
        }

        public void Cast(string spell)
        {
            Fighter fighter = Owner.Fighter;

            if (spell == "sword")
            {

            }
            else if (spell == "shield")
            {
                fighter.Armor += ShieldPower;

            }
            else if (spell == "fireball")
            {

            }
            else if (spell == "heal" || MP >= 3)
            {
                fighter.HP = Math.Min(fighter.MaxHP, fighter.HP + HealPower);
                MP -= 3;
            }
            else if (spell == "teleport" || MP >= 1)
            {

                MP -= 1;
            }
        }
    }
}
