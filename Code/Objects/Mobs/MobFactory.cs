using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheEternalOne.Code.Objects.Mobs
{
    public static class MobFactory
    {
        public static GameObject CreateBasicTrashMob(int x, int y)
        {
            Fighter fighter = new Fighter(hp : 5, pow : 1, arm : 0, xp : 1);
            GameObject gameObject = new GameObject(x, y, "basicenemy_placeholder", 100, 100);

            gameObject.Name = "Trash Mob";
            gameObject.Fighter = fighter;
            gameObject.AI = new StaticAI();

            return gameObject;
        }
    }
}
