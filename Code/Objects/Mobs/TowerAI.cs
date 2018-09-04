using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheEternalOne.Code.Utils;

namespace TheEternalOne.Code.Objects.Mobs
{
    class TowerAI : I_AI
    {
        public GameObject Owner { get; set; }

        public const int CHARGE_COOLDOWN = 5;
        private Stack<Coord> Path;

        public int ChargeCooldown = 0;

        void I_AI.TakeTurn()
        {
            bool charged = false;
            if (ChargeCooldown > 0)
            {
                ChargeCooldown--;
            }
            else
            {
                if (Distance.GetDistance(Owner.Position, GameManager.PlayerObject.Position) <= 5 && Distance.GetDistance(Owner.Position, GameManager.PlayerObject.Position) >= 3)
                {
                    Console.Out.WriteLine("Trying charge");
                    Coord deltaPos = null;
                    if (Owner.Position.x == GameManager.PlayerObject.Position.x)
                    {
                        if (Owner.Position.y > GameManager.PlayerObject.Position.y)
                        {
                            Console.Out.WriteLine("Charging below");
                            deltaPos = new Coord(0, 1);
                        }
                        else
                        {
                            Console.Out.WriteLine("Charging above");
                            deltaPos = new Coord(0, -1);
                        }
                    }
                    else if (Owner.Position.y == GameManager.PlayerObject.Position.y)
                    {
                        if (Owner.Position.x > GameManager.PlayerObject.Position.x)
                        {
                            Console.Out.WriteLine("Charging right");
                            deltaPos = new Coord(1, 0);
                        }
                        else
                        {
                            Console.Out.WriteLine("Charging left");
                            deltaPos = new Coord(-1, 0);
                        }
                    }

                    if (deltaPos != null)
                    {
                        Coord chargeDest = new Coord(GameManager.PlayerObject.Position.x + deltaPos.x, GameManager.PlayerObject.Position.y + deltaPos.y);
                        if (!GameManager.Map[chargeDest.x, chargeDest.y].Blocked)
                        {
                            bool suitable = true;
                            foreach (GameObject obj in GameManager.Objects)
                            {
                                if (obj.Fighter != null && obj.Position.x == chargeDest.x && obj.Position.y == chargeDest.y)
                                {
                                    suitable = false;
                                    break;
                                }
                            }
                            if (suitable)
                            {
                                Owner.Speed = 4;
                                Owner.MoveTo(chargeDest);
                                Owner.Fighter.Attack(GameManager.PlayerObject.Fighter);
                                ChargeCooldown = CHARGE_COOLDOWN;
                                charged = true;
                            }
                        }
                    }
                }
            }

            if (!charged)
            {
                bool newPath = false;
                double dist = Distance.GetDistance(Owner.Position, GameManager.PlayerObject.Position);
                if (dist < 2)
                {
                    Owner.Fighter.Attack(GameManager.PlayerObject.Fighter);
                }
                else if (Path != null && Path.Count > 0)
                {
                    Owner.Speed = 1;
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
            else
            {
                Owner.Speed = 1;
            }
        }
    }
}
