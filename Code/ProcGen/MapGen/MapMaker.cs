using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheEternalOne.Code.Map;
using TheEternalOne.Code.Objects;
using TheEternalOne.Code.Objects.Equipments;
using TheEternalOne.Code.Objects.Items;
using TheEternalOne.Code.Objects.Mobs;
using TheEternalOne.Code.Utils;

namespace TheEternalOne.Code.ProcGen.MapGen
{
    public static class MapMaker
    {
        const int MAX_ITER = 50000;
        const int MAP_WIDTH = GameManager.MAP_WIDTH;
        const int MAP_HEIGHT = GameManager.MAP_HEIGHT;

        const int TRASH_MOB_NUMBER = 20;
        const int ITEM_NUMBER = 25;

        const float ROOM_RATIO = 0.6f;
        const int TUN_STEP_HOR = 50;
        const int TUN_STEP_VERT = 20;

        static Tile[,] currentMap;
        static Room[] rooms;

        const int MIN_DUG_DIST = 5;
        const int TURN_CHANCE = 20;

        static void CreateRoom(Room room)
        {
            foreach (Vector2 Vector2 in room.Tiles)
            {
                Tile tile = currentMap[(int)Vector2.X, (int)Vector2.Y];
                tile.InRoom = true;
                tile.Blocked = false;
                tile.Wall = false;
            }
        }

        static void CreateHorizTunnel(int x1, int x2, int y, int maxWidth = 1)
        {
            int minX = Math.Min(x1, x2);
            int maxX = Math.Max(x1, x2);
            int width = (maxX - minX) / TUN_STEP_HOR;
            if (width > maxWidth) width = maxWidth;

            int x, wy;
            for (x = minX; x <= maxX; x++)
            {
                for (wy = -width; wy <= width; wy++)
                {
                    int ty = y + wy;
                    Tile tile = currentMap[x, ty];
                    tile.InTunnel = true;
                    tile.Blocked = false;
                    tile.Wall = false;
                }
            }
        }

        static void CreateVertTunnel(int y1, int y2, int x, int maxWidth = 1)
        {
            int minY = Math.Min(y1, y2);
            int maxY = Math.Max(y1, y2);
            int width = (maxY - minY) / TUN_STEP_VERT;
            if (width > maxWidth) width = maxWidth;

            int y, wx;
            for (y = minY; y <= maxY; y++)
            {
                for (wx = -width; wx <= width; wx++)
                {
                    int tx = x + wx;
                    Tile tile = currentMap[tx, y];
                    tile.InTunnel = true;
                    tile.Blocked = false;
                    tile.Wall = false;
                }
            }
        }

        static Vector2 ClosestDir(int x1, int y1, int x2, int y2, Vector2 curDir)
        {
            Vector2 bestDir = curDir;
            int cx = (int)curDir.X;
            int cy = (int)curDir.Y;
            double minDist = Distance.GetDistance(x1 + cx, y1 + cy, x2, y2);

            foreach (Vector2 dir in GlobalConstants.CardinalDirections)
            {
                int dx = (int)dir.X;
                int dy = (int)dir.Y;
                double dist = Distance.GetDistance(x1 + dx, y1 + dy, x2, y2);
                if (dist < minDist && !(dx == cx && dy == cy) && !(dx == -cx && dy == -cy))
                {
                    minDist = dist;
                    bestDir = dir;
                }
            }

            return bestDir;
        }

        static void CreateMazeTunnel(int x1, int y1, int x2, int y2, Vector2[] targetList)
        {
            int x = x1;
            int y = y1;
            int dugDist = 0;
            Vector2 dir = ClosestDir(x, y, x2, y2, new Vector2(0, 0));

            while (Array.IndexOf(targetList, new Vector2(x, y)) == -1)
            {
                x += (int)dir.X;
                y += (int)dir.Y;
                Tile tile = currentMap[x, y];
                tile.InTunnel = true;
                tile.Blocked = false;
                tile.Wall = false;
                dugDist++;

                if ((dugDist >= MIN_DUG_DIST && Dice.GetRandint(1, 101) <= TURN_CHANCE) || x + (int)dir.X < 1 || x + (int)dir.X >= MAP_WIDTH - 2 || y + (int)dir.Y < 1 || y + (int)dir.Y >= MAP_HEIGHT - 2)
                {
                    dir = ClosestDir(x, y, x2, y2, dir);
                    dugDist = 0;
                }
            }
        }


        static void OpenRoom(Room room)
        {
            Vector2 center = room.Center;
            int cx = (int)center.X;
            int cy = (int)center.Y;

            Tile tile = currentMap[room.x1, cy]; //left wall
            if (room.x1 - 1 > 0 && currentMap[room.x1 - 1, cy].InTunnel && tile.Blocked)
            {
                tile.Door = true;
                tile.Blocked = false;
                tile.InTunnel = true;
                tile.Wall = false;
            }

            tile = currentMap[room.x2, cy]; //right wall
            if (room.x2 + 1 < MAP_WIDTH && currentMap[room.x2 + 1, cy].InTunnel && tile.Blocked)
            {
                tile.Door = true;
                tile.Blocked = false;
                tile.InTunnel = true;
                tile.Wall = false;
            }

            tile = currentMap[cx, room.y1]; //upper wall
            if (room.y1 - 1 > 0 && currentMap[cx, room.y1 - 1].InTunnel && tile.Blocked)
            {
                tile.Door = true;
                tile.Blocked = false;
                tile.InTunnel = true;
                tile.Wall = false;
            }

            tile = currentMap[cx, room.y2]; //lower wall
            if (room.y2 + 1 < MAP_HEIGHT && currentMap[cx, room.y2 + 1].InTunnel && tile.Blocked)
            {
                tile.Door = true;
                tile.Blocked = false;
                tile.InTunnel = true;
                tile.Wall = false;
            }

        }

        static int GetMidWall(int wallLength)
        {
            if (wallLength % 2 != 0) return (wallLength - 1) / 2;
            else return wallLength / 2 - 1;
        }

        static void EncaseRoom(Room room)
        {
            int i, wallLength, j, k, mid;
            Tile[] fullWall;
            bool allNull;

            //left
            allNull = true;
            wallLength = room.y2 - room.y1 + 1;
            fullWall = new Tile[wallLength];
            for (i = 0; i < wallLength; i++)
            {
                Tile tile = currentMap[room.x1, room.y1 + i];
                if (tile.Blocked)
                {
                    fullWall[i] = null;
                    allNull = false;
                }
                else
                {
                    if (allNull) fullWall[i] = null;
                    else fullWall[i] = tile;
                }
            }

            for (i = 0; i < wallLength; i++)
            {
                Tile tile = fullWall[i];
                if (tile != null && !tile.Blocked)
                {
                    j = Array.IndexOf(fullWall, null, i);
                    mid = GetMidWall(j - i);
                    if (currentMap[room.x1 - 1, room.y1 + i + mid].Blocked && (currentMap[room.x1 - 1, room.y1 + j -1 - mid].Blocked)) mid = 0; //if it wall leave an opening on nothing
                    for (k = 0; k < mid; k++)
                    {
                        fullWall[i + k].Blocked = true;
                        fullWall[i + k].Wall = true;
                        fullWall[i + k] = null;
                        fullWall[j - 1 - k].Blocked = true;
                        fullWall[j - 1 - k].Wall = true;
                        fullWall[j - 1 - k] = null;
                    }
                }
            }

            //right
            allNull = true;
            wallLength = room.y2 - room.y1 + 1;
            fullWall = new Tile[wallLength];
            for (i = 0; i < wallLength; i++)
            {
                Tile tile = currentMap[room.x2, room.y1 + i];
                if (tile.Blocked)
                {
                    fullWall[i] = null;
                    allNull = false;
                }
                else
                {
                    if (allNull) fullWall[i] = null;
                    else fullWall[i] = tile;
                }
            }

            for (i = 0; i < wallLength; i++)
            {
                Tile tile = fullWall[i];
                if (tile != null && !tile.Blocked)
                {
                    j = Array.IndexOf(fullWall, null, i);
                    mid = GetMidWall(j - i);
                    if (currentMap[room.x2 + 1, room.y1 + i + mid].Blocked && (currentMap[room.x2 + 1, room.y1 + j - 1 - mid].Blocked)) mid = 0; //if it wall leave an opening on nothing
                    for (k = 0; k < mid; k++)
                    {
                        fullWall[i + k].Blocked = true;
                        fullWall[i + k].Wall = true;
                        fullWall[i + k] = null;
                        fullWall[j - 1 - k].Blocked = true;
                        fullWall[j - 1 - k].Wall = true;
                        fullWall[j - 1 - k] = null;
                    }
                }
            }

            //up
            allNull = true;
            wallLength = room.x2 - room.x1 + 1;
            fullWall = new Tile[wallLength];
            for (i = 0; i < wallLength; i++)
            {
                Tile tile = currentMap[room.x1 + i, room.y1];
                if (tile.Blocked)
                {
                    fullWall[i] = null;
                    allNull = false;
                }
                else
                {
                    if (allNull) fullWall[i] = null;
                    else fullWall[i] = tile;
                }
            }

            for (i = 0; i < wallLength; i++)
            {
                Tile tile = fullWall[i];
                if (tile != null && !tile.Blocked)
                {
                    j = Array.IndexOf(fullWall, null, i);
                    mid = GetMidWall(j - i);
                    if (currentMap[room.x1 + i + mid, room.y1 - 1].Blocked && (currentMap[room.x1 + j - 1 - mid, room.y1 - 1].Blocked)) mid = 0; //if it wall leave an opening on nothing
                    for (k = 0; k < mid; k++)
                    {
                        fullWall[i + k].Blocked = true;
                        fullWall[i + k].Wall = true;
                        fullWall[i + k] = null;
                        fullWall[j - 1 - k].Blocked = true;
                        fullWall[j - 1 - k].Wall = true;
                        fullWall[j - 1 - k] = null;
                    }
                }
            }

            //down
            allNull = true;
            wallLength = room.x2 - room.x1 + 1;
            fullWall = new Tile[wallLength];
            for (i = 0; i < wallLength; i++)
            {
                Tile tile = currentMap[room.x1 + i, room.y2];
                if (tile.Blocked)
                {
                    fullWall[i] = null;
                    allNull = false;
                }
                else
                {
                    if (allNull) fullWall[i] = null;
                    else fullWall[i] = tile;
                }
            }

            for (i = 0; i < wallLength; i++)
            {
                Tile tile = fullWall[i];
                if (tile != null && !tile.Blocked)
                {
                    j = Array.IndexOf(fullWall, null, i);
                    mid = GetMidWall(j - i);
                    if (currentMap[room.x1 + i + mid, room.y2 + 1].Blocked && (currentMap[room.x1 + j - 1 - mid, room.y2 + 1].Blocked)) mid = 0; //if it wall leave an opening on nothing
                    for (k = 0; k < mid; k++)
                    {
                        fullWall[i + k].Blocked = true;
                        fullWall[i + k].Wall = true;
                        fullWall[i + k] = null;
                        fullWall[j - 1 - k].Blocked = true;
                        fullWall[j - 1 - k].Wall = true;
                        fullWall[j - 1 - k] = null;
                    }
                }
            }
        }

        static void PlaceDoors(int prob = 100, bool pillars = false)
        {
            int x, y;
            for (y = 0; y < MAP_HEIGHT; y++)
            {
                for (x = 0; x < MAP_WIDTH; x++)
                {
                    Tile tile = currentMap[x, y];
                    Tile[] neighbors = tile.Neighbors(ref currentMap, true);

                    bool neighInRoom = false;
                    int blockedNeigh = 0;
                    int dx = 0;
                    int dy = 0;
                    int ndx, ndy;
                    bool neighDoor = false;

                    foreach (Tile neigh in neighbors)
                    {
                        if (neigh.InRoom) neighInRoom = true;

                        ndx = neigh.x - x;
                        ndy = neigh.y - y;
                        if (neigh.Blocked && ((dx == 0 && dy == 0) || (ndx == -dx && ndy == -dy)))
                        {
                            blockedNeigh++;
                            dx = ndx;
                            dy = ndy;
                        }

                        if (neigh.Door) neighDoor = true;
                    }

                    if (tile.InTunnel && neighInRoom && !tile.InRoom && blockedNeigh == 2 && Dice.GetRandint(1, 101) <= prob && !neighDoor)
                    {
                        tile.Door = true;
                    }
                }
            }
        }

        public static Tile[,] CheckDoors(Tile[,] map)
        {
            int x, y;
            for (y = 0; y < MAP_HEIGHT; y++)
            {
                for (x = 0; x < MAP_WIDTH; x++)
                {
                    Tile tile = map[y, x];
                    if (tile.Door && tile.CountNeighbors(ref map, true) != 2) tile.Door = false;
                }
            }

            return map;
        }

        public static void PlaceMobs(Tile[,] map)
        {
            Dictionary<string, int> MobChances = new Dictionary<string, int>
            {
                {"Pawn", 55 },
                {"Tower", 30 },
                {"Bishop", 15 }
            };
            for (int i= 0; i < TRASH_MOB_NUMBER; i++)
            {
                bool foundSuitablePosition = false;
                int iter = 0;
                int x = -1;
                int y = -1;
                while (!foundSuitablePosition)
                {
                    iter++;
                    x = Dice.GetRandint(0, GameManager.MAP_WIDTH);
                    y = Dice.GetRandint(0, GameManager.MAP_HEIGHT);

                    if (!(map[x,y].Blocked) && !(x == GameManager.StartPosition.x && y == GameManager.StartPosition.y))
                    {
                        foundSuitablePosition = true;
                        foreach (GameObject obj in GameManager.Objects)
                        {
                            if (obj.Position.x == x && obj.Position.y == y)
                            {
                                foundSuitablePosition = false;
                                break;
                            }
                        }
                    }

                    if (iter > MAX_ITER)
                    {
                        break;
                    }
                }
                if (foundSuitablePosition)
                {
                    string chosen = RandomChoice(MobChances);
                    if (chosen == "Pawn")
                    {
                        GameManager.Objects.Add(MobFactory.CreateBasicTrashMob(x, y));
                    }
                    else if (chosen == "Tower")
                    {
                        GameManager.Objects.Add(MobFactory.CreateTower(x, y));
                    }
                    else if (chosen == "Bishop")
                    {
                        GameManager.Objects.Add(MobFactory.CreateBishop(x, y));
                    }
                }
            }
        }

        public static int RandomChoiceIndex(int[] chances)
        {
            int dice = Dice.GetRandint(1, chances.Sum());
            int runningSum = 0;
            int choice = 0;

            foreach (int chance in chances)
            {
                runningSum += chance;
                if (dice <= runningSum) break;
                choice++;
            }

            return choice;
        }

        public static string RandomChoice(Dictionary<string, int> dict)
        {
            string[] strings = dict.Keys.ToArray();
            int n = strings.Count();
            int[] chances = new int[n];

            for (int i = 0; i < n; i++)
            {
                chances[i] = dict[strings[i]];
            }

            return strings[RandomChoiceIndex(chances)];
        }

        public static void PlaceItems(Tile[,] map)
        {
            Dictionary<string, int> ItemChances = new Dictionary<string, int>
            {
                { "Potion", 60 },
                { "Sword", 10 },
                { "MagicAmulet", 10 },
                { "Shield", 10 },
                { "Armor", 10 }
            };

            Dictionary<String, int> PotionChances = new Dictionary<string, int>
            {
                {"Health", 50 },
                {"Mana", 50 }
            };
            for (int i = 0; i < ITEM_NUMBER; i++)
            {
                bool foundSuitablePosition = false;
                int iter = 0;
                int x = -1;
                int y = -1;
                while (!foundSuitablePosition)
                {
                iter++;
                x = Dice.GetRandint(0, GameManager.MAP_WIDTH);
                y = Dice.GetRandint(0, GameManager.MAP_HEIGHT);


                    if (!map[x, y].Blocked)
                    foundSuitablePosition = true;
                    {
                        foreach (GameObject obj in GameManager.Objects)
                        {
                            if (obj.Position.x == x && obj.Position.y == y && obj.Item != null)
                            {
                                foundSuitablePosition = false;
                                break;
                            }
                        }
                    }
                    

                    if (iter > MAX_ITER)
                    {
                        break;
                    }
                }
                if (foundSuitablePosition)
                {
                    string chosen = RandomChoice(ItemChances) ;
                    if (chosen == "Potion")
                    {
                        string subtype = RandomChoice(PotionChances);
                        if (subtype == "Health")
                        {
                            GameManager.Objects.Add(ItemFactory.CreateHealthPotion(x, y));
                        }
                        else if (subtype == "Mana")
                        {
                            GameManager.Objects.Add(ItemFactory.CreateManaPotion(x, y));
                        }
                    }
                    else if (chosen == "Sword")
                    {
                        GameManager.Objects.Add(EquipmentFactory.CreateSword(x, y));
                    }
                    else if (chosen == "MagicAmulet")
                    {
                        GameManager.Objects.Add(EquipmentFactory.CreateMagicAmulet(x, y));
                    }
                    else if (chosen == "Shield")
                    {
                        GameManager.Objects.Add(EquipmentFactory.CreateShield(x, y));
                    }
                    else if (chosen == "Armor")
                    {
                        GameManager.Objects.Add(EquipmentFactory.CreateArmor(x, y));
                    }
                }
            }
        }

        static public GameObject MakeStairs(int x, int y)
        {
            GameObject gObj = new GameObject(x, y, "stairs", 100, 100);
            gObj.Name = "Stairs";
            gObj.isStairs = true;
            return gObj;
        }

        static public Tile[,] MakeTunnelMap(bool doors, int roomNumber = 15, int minSize = 6, int maxSize = 17)
        {
            currentMap = Map.Map.InitMap(true);
            rooms = new Room[roomNumber];
            int curRooms = 0;
            int iter = 0;

            while (curRooms < roomNumber && iter < MAX_ITER)
            {
                iter++;
                int w = Dice.GetRandint(minSize, maxSize + 1);
                int h = Dice.GetRandint(minSize, maxSize + 1);
                while (w / h < ROOM_RATIO || h / w < ROOM_RATIO)
                {
                    w = Dice.GetRandint(minSize, maxSize + 1);
                    h = Dice.GetRandint(minSize, maxSize + 1);
                }

                int x = Dice.GetRandint(0, MAP_WIDTH - w);
                int y = Dice.GetRandint(0, MAP_HEIGHT - h);

                Room newRoom = new Room(x, y, w, h);
                bool intersect = false;
                foreach (Room room in rooms)
                {
                    if (room != null && newRoom.Intersect(room))
                    {
                        intersect = true;
                        break;
                    }
                }

                if (!intersect)
                {
                    CreateRoom(newRoom);
                    Vector2 center = newRoom.Center;

                    if (curRooms != 0)
                    {
                        Vector2 prevCenter = rooms[curRooms - 1].Center;
                        int tun = Dice.GetRandint(1, 101);
                        if (tun <= 10)
                        {
                            CreateHorizTunnel((int)prevCenter.X, (int)center.X, (int)prevCenter.Y);
                            CreateVertTunnel((int)prevCenter.Y, (int)center.Y, (int)center.X);
                        }
                        else if (tun <= 60)
                        {
                            CreateMazeTunnel((int)prevCenter.X, (int)prevCenter.Y, (int)center.X, (int)center.Y, newRoom.Tiles);
                        }
                        else
                        {
                            CreateVertTunnel((int)prevCenter.Y, (int)center.Y, (int)center.X);
                            CreateHorizTunnel((int)prevCenter.X, (int)center.X, (int)prevCenter.Y);
                        }
                    }

                    rooms[curRooms] = newRoom;
                    curRooms++;
                }
            }


            foreach (Room room in rooms)
            {
                OpenRoom(room);
                EncaseRoom(room);
            }

            if (doors)
            {
                PlaceDoors();
                currentMap = CheckDoors(currentMap);
            }

            Room lastRoom = rooms[rooms.Length - 1];
            GameManager.StartPosition = new Coord((int)lastRoom.Center.X, (int)lastRoom.Center.Y);
            Room firstRoom = rooms[0];
            GameManager.Objects.Add(MakeStairs((int)firstRoom.Center.X, (int)firstRoom.Center.Y));
            PlaceMobs(currentMap);
            PlaceItems(currentMap);

            Map.Map.UpdateTexture(currentMap);

            return currentMap;
        }


    }
}
