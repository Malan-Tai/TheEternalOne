using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheEternalOne.Code.Utils;

namespace TheEternalOne.Code.Objects.Mobs
{
    public class StaticAI : I_AI
    {
        public GameObject Owner { get; set; }

        void I_AI.TakeTurn()
        {
            if (Distance.GetDistance(Owner.Position, GameManager.PlayerObject.Position) <= 1)
            {
                Owner.Fighter.Attack(GameManager.PlayerObject.Fighter);
            }
            else
            {
                Console.Out.WriteLine("Dist : " + Distance.GetDistance(Owner.Position, GameManager.PlayerObject.Position).ToString());
            }
        }
    }
}
