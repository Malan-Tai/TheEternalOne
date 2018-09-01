﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheEternalOne.Code.Objects.Equipments
{
    public class Equipment
    {
        public GameObject Owner { get; set; }

        public enum EquipmentSlot
        {
            Weapon,
            Shield,
            Armor,
            Trinket
        }

        public string Name
        {
            get
            {
                return Owner.Name;
            }
            set
            {
                Owner.Name = value;
            }
        }

        public EquipmentSlot Slot;

        public int SwordMod { get; set; } = 0;
        public int ShieldMod { get; set; } = 0;
        public int FireballMod { get; set; } = 0;
        public int TPMod { get; set; } = 0;
        public int HealMod { get; set; } = 0;

        public int Armor { get; set; } = 0;
        public int ArmorHP { get; set; } = 0;

        public Equipment(EquipmentSlot slot, int sword = 0, int shield = 0, int fireball = 0, int heal = 0, int tp = 0, int armor = 0, int armorHP = 0)
        {
            this.Slot = slot;
            this.SwordMod = sword;
            this.ShieldMod = shield;
            this.FireballMod = fireball;
            this.HealMod = heal;
            this.TPMod = tp;
            this.Armor = armor;
            this.ArmorHP = armorHP;
        }

        public bool Equip()
        {
            if (GameManager.PlayerObject.Player.CanEquip)
            {
                if (Slot != EquipmentSlot.Trinket)
                {
                    if (GameManager.PlayerObject.Player.Equipment[Slot] == null)
                    {
                        GameManager.PlayerObject.Player.Equipment[Slot] = Owner;
                        return true;
                    }
                    else
                    {
                        GameManager.LogWarning("There is already something equipped on this slot");
                        return false;
                    }
                }
                else
                {
                    for (int i=0; i<Player.MAX_TRINKETS; i++)
                    {
                        if (GameManager.PlayerObject.Player.Trinkets[i] == null)
                        {
                            GameManager.PlayerObject.Player.Trinkets[i] = Owner;
                            return true;
                        }
                    }
                    GameManager.LogWarning("All your trinket slots are already full.");
                    return false;
                }

            }
            else
            {
                GameManager.LogWarning("You cannot equip items anymore !");
                return false;
            }
        }

    }
}