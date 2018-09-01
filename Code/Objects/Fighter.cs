using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheEternalOne.Code.Objects
{
    public class Fighter
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

        public void Attack(Fighter other)
        {
            bool canAttack = (Owner.Player == null || Owner.Player.CanMelee);
            if (canAttack)
            {
                other.TakeDamage(Power);
                if (other.Owner.Player != null)
                {
                    GameManager.LogWarning(Owner.Name + " attacked you for " + Power.ToString() + " damage !");
                }
                else
                {
                    GameManager.LogSuccess(other.Owner.Name + " took " + Power.ToString() + " damage !");
                }
            }
            else
            {
                GameManager.LogWarning("You cannot attack in melee anymore !");
            }
        }

        public void TakeDamage(int dmg)
        {
            int armorDmg = Math.Min(Armor, dmg);
            Armor -= armorDmg;
            HP -= dmg - armorDmg;

            if (HP <= 0)
            {
                if (Owner.Player != null)
                {
                    //TO-DO : Player "death" handling
                }
                else
                {
                    if (GameManager.Objects.Contains(this.Owner))
                    {
                        GameManager.Objects.Remove(Owner);
                    }
                }
            }
        }
    }
}
