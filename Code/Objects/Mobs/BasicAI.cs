using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheEternalOne.Code.Utils;

namespace TheEternalOne.Code.Objects.Mobs
{
    class BasicAI : I_AI
    {
        public GameObject Owner { get; set; }
        private Stack<Coord> Path;

        void I_AI.TakeTurn()
        {
            bool newPath = false;
            double dist = Distance.GetDistance(Owner.Position, GameManager.PlayerObject.Position);
            if (dist < 2)
            {
                Owner.Fighter.Attack(GameManager.PlayerObject.Fighter);
            }
            else if (Path != null && Path.Count > 0)
            {
                Coord next = Path.Pop();
                if (dist < Distance.GetDistance(GameManager.PlayerObject.Position, next) || GameManager.Map[next.x, next.y].IsBlocked) //if the path is going away from the player or is blocked by a monster
                {
                    newPath = true;
                }
                else
                {
                    Owner.MoveTo(next);
                }
            }
            else newPath = true;

            if (newPath && Distance.GetDistance(Owner.Position, GameManager.PlayerObject.Position) < 20)
            {
                Path = Astar.AstarPath(Owner.x, Owner.y, GameManager.PlayerObject.x, GameManager.PlayerObject.y, ref GameManager.Map);
                if (Path != null && Path.Count > 0) Owner.MoveTo(Path.Pop()); //Will sometimes not trigger, ie if the mob is blocked
            }
        }
    }
}
