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
            if (Distance.GetDistance(Owner.Position, GameManager.PlayerObject.Position) < 2)
            {
                Owner.Fighter.Attack(GameManager.PlayerObject.Fighter);
            }
            else if (Path != null && Path.Count > 0)
            {
                Owner.MoveTo(Path.Pop());
            }
            else if (Distance.GetDistance(Owner.Position, GameManager.PlayerObject.Position) < 20)
            {
                Path = Astar.AstarPath(Owner.x, Owner.y, GameManager.PlayerObject.x, GameManager.PlayerObject.y, ref GameManager.Map);
                if (Path != null && Path.Count > 0) Owner.MoveTo(Path.Pop()); //Will sometimes not trigger, ie if the mob is blocked
            }
        }
    }
}
