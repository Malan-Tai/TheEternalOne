using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheEternalOne.Code.Game;
using Microsoft.Xna.Framework;
using TheEternalOne.Code.GUI;

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
            int ActualPower;
            if (Owner.Player != null)
            {
                int sum = 0;
                foreach (GameObject equip in Owner.Player.GetAllEquipped())
                {
                    sum += equip.Equipment.SwordMod;
                }
                ActualPower = Math.Max(Power + sum, 0) ;
            }
            else
            {
                ActualPower = Power;
            }
            bool canAttack = (Owner.Player == null || Owner.Player.CanMelee);
            if (canAttack)
            {
                
                other.TakeDamage(ActualPower);
              
                if (other.Owner.Player != null)
                {
                    //GameManager.LogWarning(Owner.Name + " attacked you for " + ActualPower.ToString() + " damage !");
                }
                else
                {
                    //GameManager.LogSuccess(other.Owner.Name + " took " + ActualPower.ToString() + " damage !");
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
            int HPdmg = dmg - armorDmg;
            HP -= HPdmg;
            
            if (HPdmg > 0)
            {
                Color effectColor;
                if (Owner.Player != null)
                {
                    effectColor = Color.DarkRed;
                }
                else
                {
                    effectColor = Color.White;
                }
                Effect effect = new Effect("-" + HPdmg.ToString(), effectColor);
                Owner.AddEffect(effect);
            }

            if (HP <= 0)
            {
                if (Owner.Player != null)
                {
                    DeathScreen.Initialize();
                    GameManager.CurrentState = GameManager.GameState.Dead;
                }
                else
                {
                    GameManager.PlayerObject.Fighter.XP += this.XP;
                    //if (GameManager.Objects.Contains(this.Owner))
                    //{
                    //    GameManager.Objects.Remove(Owner);
                    //}
                    Owner.dieOnEffectsEnd = true;
                }
            }
        }
    }
}
