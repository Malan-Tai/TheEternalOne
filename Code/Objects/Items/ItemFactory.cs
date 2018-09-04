using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheEternalOne.Code.Objects.Items
{
    public static class ItemFactory
    {
        public static GameObject CreateHealthPotion(int x, int y)
        {
            GameObject gameObject = new GameObject(x, y, "healthPotion", 70, 70);
            HealthPotion itemComp = new HealthPotion();

            gameObject.Name = "Health Potion";
            gameObject.Item = itemComp;

            return gameObject;
        }

        public static GameObject CreateManaPotion(int x, int y)
        {
            GameObject gameObject = new GameObject(x, y, "manaPotion", 70, 70);
            ManaPotion itemComp = new ManaPotion();

            gameObject.Name = "Mana Potion";
            gameObject.Item = itemComp;

            return gameObject;
        }
    }
}
