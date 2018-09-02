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
        public enum GameState
        {
            Playing,
            Dead,
            GameOver
        }

        public static GameState CurrentState = GameState.Playing;
        public static Tile[,] Map;

        public const int TileWidth = 85;
        public const int VisibleMapWidth = 15;
        public const int VisibleMapHeight = 11;

        public const int MAX_MESSAGES = 4;

        public static bool Started = false;

        public static int DrawMapX = (Game1.WIDTH - VisibleMapWidth * (int)(TileWidth * Game1.GLOBAL_SIZE_MOD / 100)) / 2;
        public static int DrawMapY = 10;

        public const int screenPlayerX = VisibleMapWidth / 2;
        public const int screenPlayerY = VisibleMapHeight / 2;

        public static int AbilityHeight = (int)((DrawMapY + VisibleMapHeight * (int)(TileWidth * Game1.GLOBAL_SIZE_MOD / 100) - 25) / 5f);
        public static int AbilityWidth = DrawMapX - 20;

        public static int EquipmentY = 3 * (int)(70 * Game1.GLOBAL_SIZE_MOD / 100) + 25;
        public static int EquipmentWidth = DrawMapX - 10;
        public static int EquipmentHeight = (int)((DrawMapY + VisibleMapHeight * (int)(TileWidth * Game1.GLOBAL_SIZE_MOD / 100) - 25 - EquipmentY) / 5f);

        public static int InventoryX = DrawMapX;
        public static int InventoryY = DrawMapY + VisibleMapHeight * (int)(TileWidth * Game1.GLOBAL_SIZE_MOD / 100) + 5;
        public static int InventoryHeight = Game1.HEIGHT - InventoryY - 5;
        public static int InventoryWidth = (int)(VisibleMapWidth * (int)(TileWidth * Game1.GLOBAL_SIZE_MOD / 100) / 10);

        public static Coord StartPosition;

        public static GameObject PlayerObject;
        public static List<GameObject> Objects;
        public static List<GameObject> ToRemove;
		
        public static AbilityGUI abilityGUI;
        public static StatusGUI statusGUI;
        public static MiniMap miniMap;
        public static EquipmentGUI equipmentGUI;
        public static InventoryGUI inventoryGUI;

        public const int MAP_WIDTH = 100;
        public const int MAP_HEIGHT = 80;

        public static List<Message> Log;
        public static Message ActiveMessage = null;

        public static void NewGame()
        {
            Objects = new List<GameObject> ();
            Map = new Tile[MAP_WIDTH, MAP_HEIGHT];
            Log = new List<Message>();
            abilityGUI = new AbilityGUI();
            statusGUI = new StatusGUI();
            equipmentGUI = new EquipmentGUI(5);
            inventoryGUI = new InventoryGUI();

            Map = MapMaker.MakeTunnelMap(false);

            miniMap = new MiniMap(30, 20, Map);

            PlayerObject = new GameObject(StartPosition.x, StartPosition.y, "white", 100, 100);
            PlayerObject.Player = new Player(10);
            PlayerObject.Player.Owner = PlayerObject;
            PlayerObject.Fighter = new Fighter(2, 5, 0, 0);
            PlayerObject.Fighter.Owner = PlayerObject;

            Objects.Add(PlayerObject);
            DeathScreen.OnStart();
            Started = true;
            ToRemove = new List<GameObject>();

            //Console.Out.WriteLine(PlayerObject.x.ToString() + ";" + PlayerObject.y.ToString());
            //Console.Out.WriteLine("Position : " + PlayerObject.Position.x.ToString() + ";" + PlayerObject.Position.y.ToString());
            //Console.Out.WriteLine("InventoryWidth:" + InventoryWidth.ToString());
        }

        public static void LogWarning(string message)
        {
            Log.Add(new Message(message, Color.Orange));
        }

        public static void LogSuccess(string message)
        {
            Log.Add(new Message(message, Color.Green));
        }

        public static void LogInfo(string message)
        {
            GameManager.ActiveMessage = new Message(message, Color.White);
        }
    }
}
