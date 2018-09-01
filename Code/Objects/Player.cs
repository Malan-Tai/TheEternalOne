﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheEternalOne.Code.Utils;
using Microsoft.Xna.Framework;
using TheEternalOne.Code.Game;

namespace TheEternalOne.Code.Objects
{
    public class Player
    {
        public int LeftXP { get; set; }

        public const int MAX_INVENTORY_SLOTS = 10;
        public int FireballDmg { get; set; }
        public int HealPower { get; set; }
        public int ShieldPower { get; set; }

        public int MaxMP { get; set; }
        public int MP { get; set; }

        public GameObject Owner { get; set; }

        public List<string> Spells;
        public List<GameObject> Inventory;

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
            Inventory = new List<GameObject>();
            FireballDmg = 1;
            HealPower = 1;
            ShieldPower = 1;

            MaxMP = mp;
            MP = mp;

            Spells = new List<string> { "Sword", "Shield", "Fireball", "Heal", "Teleport" };
        }

        public void Cast(int i, int x, int y)
        {
            Fighter fighter = Owner.Fighter;
            string spell = Spells[i];
            Console.Out.WriteLine("casting {0} at {1} MP", spell, MP);

            if (spell == "Sword" && Distance.GetDistance(Owner.x, Owner.y, x, y) < 2)
            {
                foreach (GameObject obj in GameManager.Objects)
                {
                    if (obj.Position.x == x && obj.Position.y == y && obj.Fighter != null)
                    {
                        Owner.Fighter.Attack(obj.Fighter);
                        break;
                    }
                }

            }
            else if (spell == "Shield")
            {
                fighter.Armor += ShieldPower;
                if (Distance.GetDistance(Owner.x, Owner.y, x, y) < 2)
                {
                    foreach (GameObject obj in GameManager.Objects)
                    {
                        if (obj.Position.x == x && obj.Position.y == y && obj.Fighter != null)
                        {
                            obj.Move(x - Owner.x, y - Owner.y);
                            break;
                        }
                    }
                }
            }
            else if (spell == "Fireball" && MP >= 5)
            {
                MP -= 5;
                foreach (GameObject obj in GameManager.Objects)
                {
                    if (obj.Position.x == x && obj.Position.y == y && obj.Fighter != null)
                    {
                        obj.Fighter.TakeDamage(FireballDmg);
                        break;
                    }
                }
            }
            else if (spell == "Heal" && MP >= 3)
            {
                int prevHP = fighter.HP;
                fighter.HP = Math.Min(fighter.MaxHP, fighter.HP + HealPower);
                int healed = fighter.HP - prevHP;

                if (healed > 0)
                {
                    MP -= 3;
                    Effect effect = new Effect("+" + healed.ToString(), Color.DarkGreen);
                    Owner.Effects.Add(effect);
                }
            }
            else if (spell == "Teleport" && MP >= 1 && !GameManager.Map[x, y].IsBlocked)
            {
                MP -= 1;

                Owner.Position = new Coord(x, y);
                Owner.BigPos = new Coord(x * (int)(GameManager.TileWidth * Game1.GLOBAL_SIZE_MOD / 100), y * (int)(GameManager.TileWidth * Game1.GLOBAL_SIZE_MOD / 100));
                Owner.OffsetPos = new Coord(x * (int)(GameManager.TileWidth * Game1.GLOBAL_SIZE_MOD / 100), y * (int)(GameManager.TileWidth * Game1.GLOBAL_SIZE_MOD / 100));
            }
        }

        public void UpgradeSpell(int i)
        {
            Fighter fighter = Owner.Fighter;
            string spell = Spells[i];

            if (spell == "Sword")
            {
                fighter.Power++;
            }
            else if (spell == "Shield")
            {
                ShieldPower++;
            }
            else if (spell == "Fireball")
            {
                FireballDmg++;
            }
            else if (spell == "Heal" || MP >= 3)
            {
                HealPower++;
            }
        }

        public GameObject FindObjectInInventory(string name)
        {
            foreach (GameObject item in Inventory)
            {
                if (item.Name == name)
                {
                    return item;
                }
            }
            return null;
        }
    }
}
