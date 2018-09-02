﻿using System;
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
            Fighter fighter = new Fighter(hp : 3, pow : 1, arm : 0, xp : 1);
            GameObject gameObject = new GameObject(x, y, "pawn", 115, 100, -20, -20);

            gameObject.Name = "Pawn";
            gameObject.Fighter = fighter;
            gameObject.AI = new BasicAI();

            return gameObject;
        }

        public static GameObject CreateTower(int x, int y)
        {
            Fighter fighter = new Fighter(hp: 10, pow: 3, arm: 0, xp: 3);
            GameObject gameObject = new GameObject(x, y, "tower", 120, 200 + 100, (int)(0.11 * Game1.WIDTH - 208.42), (int)(0.61 * Game1.HEIGHT - 670));

            gameObject.Name = "Tower";
            gameObject.Fighter = fighter;
            gameObject.AI = new TowerAI();

            return gameObject;
        }
    }
}
