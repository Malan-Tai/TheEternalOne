using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheEternalOne.Code.Objects.Items;

namespace TheEternalOne.Code.Objects.Equipments
{
    public static class EquipmentFactory
    {
        public static GameObject CreateSword(int x, int y)
        {
            GameObject gameObject = new GameObject(x, y, "sword_placeholder", 70, 70);

            Item itemComp = new EquipmentItem();
            Equipment equipment = new Equipment(Equipment.EquipmentSlot.Weapon, sword: 1, fireball: -1);

            gameObject.Name = "Short Sword";
            gameObject.Item = itemComp;
            gameObject.Equipment = equipment;

            return gameObject;
        }

        public static GameObject CreateMagicAmulet(int x, int y)
        {
            GameObject gameObject = new GameObject(x, y, "amulet_placeholder", 70, 70);

            Item itemComp = new EquipmentItem();
            Equipment equipment = new Equipment(Equipment.EquipmentSlot.Trinket, sword: -1, shield: -1, fireball: 2);

            gameObject.Name = "Magic Amulet";
            gameObject.Item = itemComp;
            gameObject.Equipment = equipment;

            return gameObject;
        }


    }
}
