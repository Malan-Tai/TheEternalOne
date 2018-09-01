using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheEternalOne.Code.Map;
using Priority_Queue;

namespace TheEternalOne.Code.Objects.Mobs
{
    static class Astar
    {
        private static int Heuristic(int x1, int y1, int x2, int y2)
        {
            return Math.Abs(x1 - x2) + Math.Abs(y1 - y2);
        }

        public static Stack<Coord> AstarPath(int startX, int startY, int goalX, int goalY, ref Tile[,] map, bool flying = false) //, int size = 1)
        {
            //Console.Out.WriteLine("Creating A* path from {0} {1} to {2} {3}", startX, startY, goalX, goalY);

            Tile start = map[startX, startY];
            Tile goal = map[goalX, goalY];

            FastPriorityQueue<Tile> frontier = new FastPriorityQueue<Tile>(500);
            frontier.Enqueue(start, 0.0f);

            Dictionary<Tile, Tile> cameFrom = new Dictionary<Tile, Tile>();
            Dictionary<Tile, float> costSoFar = new Dictionary<Tile, float>();

            cameFrom[start] = null;
            costSoFar[start] = 0.0f;

            Tile current = map[0, 0];
            while (frontier.Count > 0)
            {
                current = frontier.Dequeue();
                //Console.Out.WriteLine("In A* loop, {0} {1}", current.x, current.y);
                if (current == goal)
                {
                    //Console.Out.WriteLine("Goal found");
                    break;
                }


                foreach (Tile next in current.Neighbors(ref map, false))
                {
                    if ((!next.IsBlocked) || (next.x == goalX && next.y == goalY))
                    {
                        float newCost = costSoFar[current] + next.MoveCost;
                        if (!costSoFar.ContainsKey(next) || newCost < costSoFar[next])
                        {
                            costSoFar[next] = newCost;
                            int heurCost = Heuristic(next.x, next.y, goalX, goalY);
                            float prio = newCost + heurCost;

                            frontier.Enqueue(next, prio);
                            cameFrom[next] = current;
                        }
                    }
                }
            }

            Stack<Coord> path = new Stack<Coord>();
            path.Push(new Coord(current.x, current.y));
            //path.Add(new Coord(current.x, current.y));

            while (current != start)
            {
                //Console.Out.WriteLine("current is tile {0} {1}", current.x, current.y);

                Tile former = current;
                current = cameFrom[former];
                path.Push(new Coord(current.x, current.y));
                //path.Add(new Coord(current.x, current.y));
            }

            //int n = path.Count;
            //Coord[] finalPath = new Coord[n];
            //for (int i = 0; i < n; i++)
            //{
            //    finalPath[i] = path[n - i - 1];
            //}

            Coord first = path.Peek();
            if (first.x == startX && first.y == startY) first = path.Pop();

            return path;
        }
    }
}

