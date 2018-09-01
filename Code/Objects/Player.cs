using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheEternalOne.Code.Objects
{
    public class Player
    {
        public int LeftXP { get; set; }

        public int FireballDmg { get; set; }
        public int HealPower { get; set; }
        public int ShieldPower { get; set; }

        public int MaxMP { get; set; }
        public int MP { get; set; }

        public GameObject Owner { get; set; }

        public List<string> Spells;

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

        public Player(int mp)
        {
            FireballDmg = 1;
            HealPower = 1;
            ShieldPower = 1;

            MaxMP = mp;
            MP = mp;

            Spells = new List<string> { "Sword", "Shield", "Fireball", "Heal", "Teleport" };
        }

        public void Cast(string spell)
        {
            Fighter fighter = Owner.Fighter;

            if (spell == "Sword")
            {

            }
            else if (spell == "Shield")
            {
                fighter.Armor += ShieldPower;

            }
            else if (spell == "Fireball")
            {

            }
            else if (spell == "Heal" || MP >= 3)
            {
                fighter.HP = Math.Min(fighter.MaxHP, fighter.HP + HealPower);
                MP -= 3;
            }
            else if (spell == "Teleport" || MP >= 1)
            {

                MP -= 1;
            }
        }
    }
}
