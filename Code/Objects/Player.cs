using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheEternalOne.Code.Utils;
using static TheEternalOne.Code.Objects.Equipments.Equipment;
using static TheEternalOne.Code.Objects.Equipments.Equipment.EquipmentSlot;
using TheEternalOne.Code.Objects.Equipments;
using Microsoft.Xna.Framework;
using TheEternalOne.Code.Game;

namespace TheEternalOne.Code.Objects
{
    public class Player
    {
        public int LeftXP { get; set; }

        public const int MAX_INVENTORY_SLOTS = 10;
        public const int MAX_TRINKETS = 2;
        public int FireballDmg { get; set; }
        public int HealPower { get; set; }
        public int ShieldPower { get; set; }

        public int MaxMP { get; set; }
        public int MP { get; set; }

        public int Rerolls { get; set; }

        public GameObject Owner { get; set; }

        public List<string> Spells;
        public List<GameObject> Inventory;
        public Dictionary<EquipmentSlot, GameObject> Equipment;

        public GameObject[] Trinkets;

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

        const int MANA_REGEN = 10;
        int manaRegen = MANA_REGEN;

        public Player(int mp)
        {
            Inventory = new List<GameObject>();
            FireballDmg = 1;
            HealPower = 1;
            ShieldPower = 1;

            Rerolls = 2;

            MaxMP = mp;
            MP = mp;

            Spells = new List<string> { "Sword", "Shield", "Fireball", "Heal", "Teleport" };
            Equipment = new Dictionary<EquipmentSlot, GameObject>();
            Equipment.Add(Weapon, null);
            Equipment.Add(Shield, null);
            Equipment.Add(Armor, null);

            Trinkets = new GameObject[MAX_TRINKETS];
        }

        public void Cast(int i, int x, int y)
        {
            Fighter fighter = Owner.Fighter;
            string spell = Spells[i];
            Console.Out.WriteLine("casting {0} at {1} MP", spell, MP);

            List<GameObject> allEquip = GetAllEquipped();
            int FireballBonus = 0;
            int ShieldBonus = 0;
            int HealBonus = 0;
            int TPBonus = 0;
            
            foreach (GameObject equip in allEquip)
            {
                Equipment eqComp = equip.Equipment;
                FireballBonus += eqComp.FireballMod;
                ShieldBonus += eqComp.ShieldMod;
                HealBonus += eqComp.HealMod;
                TPBonus += eqComp.TPMod;
            }



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
                int ActualShield = Math.Max(0, ShieldPower + ShieldBonus);
                fighter.Armor += ActualShield;
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
                int ActualFireball = Math.Max(0, FireballDmg + FireballBonus);
                MP -= 5;
                foreach (GameObject obj in GameManager.Objects)
                {
                    if (obj.Position.x == x && obj.Position.y == y && obj.Fighter != null)
                    {
                        obj.Fighter.TakeDamage(ActualFireball);
                        break;
                    }
                }
            }
            else if (spell == "Heal" && MP >= 3)
            {
                int prevHP = fighter.HP;
                int ActualHealPower = Math.Max(0, HealPower + HealBonus);
                fighter.HP = Math.Min(fighter.MaxHP, fighter.HP + ActualHealPower);
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
            else if (spell == "Heal")
            {
                HealPower++;
            }
        }

        public void DisplayInventory()
        {
            foreach (GameObject gameObj in Inventory)
            {
                string itemStr = "- " + gameObj.Name;
                if (gameObj.Item.Stackable)
                {
                    itemStr += " (" + gameObj.Item.Amount + ")";
                }
                Console.Out.WriteLine(itemStr);
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

        public void UpdateTurn()
        {
            manaRegen--;
            if (manaRegen <= 0)
            {
                MP = Math.Min(MaxMP, MP + 1);
                manaRegen = MANA_REGEN;
			}
		}

        public List<GameObject> GetAllEquipped()
        {
            List<GameObject> allEquip = new List<GameObject>();
            foreach (KeyValuePair<EquipmentSlot, GameObject> pair in Equipment)
            {
                if (pair.Value != null)
                {
                    allEquip.Add(pair.Value);
                }
            }
            foreach (GameObject trinket in Trinkets)
            {
                if (trinket != null)
                {
                    allEquip.Add(trinket);
                }
            }
            return allEquip;
        }

        public void DisplayEquipment()
        {
            foreach (KeyValuePair<EquipmentSlot, GameObject> pair in Equipment)
            {
                if (pair.Value != null)
                {
                    Console.Out.WriteLine("- " + pair.Key.ToString() + " : " + pair.Value.Name);
                }
            }
            foreach (GameObject trinket in Trinkets)
            {
                if (trinket != null)
                {
                    Console.Out.WriteLine("- Trinket " + Array.IndexOf(Trinkets, trinket).ToString() + " : " + trinket.Name);
                }
            }
        }
    }
}
