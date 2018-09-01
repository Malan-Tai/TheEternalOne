using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheEternalOne.Code.Map;
using TheEternalOne.Code.Objects;
using TheEternalOne.Code.GUI;
using TheEternalOne.Code.ProcGen.MapGen;
using TheEternalOne.Code.Game;
using Microsoft.Xna.Framework;

namespace TheEternalOne.Code
{
    static class GameManager
    {
        public static Tile[,] Map;

        public const int TileWidth = 85;
        public const int VisibleMapWidth = 15;
        public const int VisibleMapHeight = 11;

        public static int DrawMapX = (Game1.WIDTH - VisibleMapWidth * (int)(TileWidth * Game1.GLOBAL_SIZE_MOD / 100)) / 2;
        public static int DrawMapY = 10;

        public const int screenPlayerX = VisibleMapWidth / 2;
        public const int screenPlayerY = VisibleMapHeight / 2;

        public static int AbilityHeight = (Game1.HEIGHT - 25) / 5;
        public static int AbilityWidth = DrawMapX - 20;

        public static Coord StartPosition;

        public static GameObject PlayerObject;
        public static List<GameObject> Objects;
		
        public static AbilityGUI abilityGUI;
        public static StatusGUI statusGUI;

        public const int MAP_WIDTH = 100;
        public const int MAP_HEIGHT = 80;

        public static List<Message> Log;

        public static void NewGame()
        {
            Objects = new List<GameObject> ();
            Map = new Tile[MAP_WIDTH, MAP_HEIGHT];
            Log = new List<Message>();
            abilityGUI = new AbilityGUI();
            statusGUI = new StatusGUI();

            Map = MapMaker.MakeTunnelMap(false);

            PlayerObject = new GameObject(StartPosition.x, StartPosition.y, "white", 100, 100);
            PlayerObject.Player = new Player(10);
            PlayerObject.Player.Owner = PlayerObject;
            PlayerObject.Fighter = new Fighter(10, 3, 0);
            PlayerObject.Fighter.Owner = PlayerObject;

            Objects.Add(PlayerObject);

            Console.Out.WriteLine(PlayerObject.x.ToString() + ";" + PlayerObject.y.ToString());
            Console.Out.WriteLine("Position : " + PlayerObject.Position.x.ToString() + ";" + PlayerObject.Position.y.ToString());

        }

        public static void LogWarning(string message)
        {
            Log.Add(new Message(message, Color.Orange));
        }

        public static void LogSuccess(string message)
        {
            Log.Add(new Message(message, Color.Green));
        }
    }
}
