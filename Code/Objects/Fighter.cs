using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheEternalOne.Code.Game;
using Microsoft.Xna.Framework;

namespace TheEternalOne.Code.Objects
{
    public class Fighter
    {
        public int Power { get; set; }
        public int MaxHP { get; set; }
        public int HP { get; set; }
        public int Armor { get; set; }
        public int XP { get; set; }

        public GameObject Owner { get; set; }
        
        public Fighter(int pow, int hp, int arm, int xp)
        {
            Power = pow;
            MaxHP = hp;
            HP = hp;
            Armor = arm;
            XP = xp;
        }

        public void Attack(Fighter other)
        {
            bool canAttack = (Owner.Player == null || Owner.Player.CanMelee);
            if (canAttack)
            {
                other.TakeDamage(Power);
                if (other.Owner.Player != null)
                {
                    //GameManager.LogWarning(Owner.Name + " attacked you for " + Power.ToString() + " damage !");
                }
                else
                {
                    //GameManager.LogSuccess(other.Owner.Name + " took " + Power.ToString() + " damage !");
                }
            }
            else
            {
                //GameManager.LogWarning("You cannot attack in melee anymore !");
            }
        }

        public void TakeDamage(int dmg)
        {
            int armorDmg = Math.Min(Armor, dmg);
            Armor -= armorDmg;
            int HPdmg = dmg - armorDmg;
            HP -= HPdmg;
            
            if (HPdmg > 0)
            {
                Effect effect = new Effect("-" + HPdmg.ToString(), Color.DarkRed);
                Owner.Effects.Add(effect);
            }

            if (HP <= 0)
            {
                if (Owner.Player != null)
                {
                    //TO-DO : Player "death" handling
                }
                else
                {
                    GameManager.PlayerObject.Fighter.XP += this.XP;
                    if (GameManager.Objects.Contains(this.Owner))
                    {
                        GameManager.Objects.Remove(Owner);
                    }
                }
            }
        }
    }
}
