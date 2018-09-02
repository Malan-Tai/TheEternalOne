using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;
using TheEternalOne.Code;
using TheEternalOne.Code.Map;
using TheEternalOne.Code.Objects;
using TheEternalOne.Code.GUI;

namespace TheEternalOne
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        public GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        public static System.Drawing.Rectangle res = Screen.PrimaryScreen.Bounds;

        public static int WIDTH = res.Width;
        public static int HEIGHT = res.Height;
        public static float GLOBAL_SIZE_MOD = WIDTH * 100 / 1920;

        public static Dictionary<string, Texture2D> textureDict = new Dictionary<string, Texture2D>();
        private static List<string> allTextures = new List<string> { "tile50x50", "white", "wall", "big_target", "HP_GUI", "MP_GUI", "XP_GUI", "Shield_GUI", "basicenemy_placeholder",
                                                                    "upgrade_GUI", "upgrade_GUI_lit", "healthpotion_placeholder", "sword_placeholder", "amulet_placeholder" };

        public static SpriteFont Font;
        public static SpriteFont Font32pt;
        public static SpriteFont Font18pt;

        public static int minMapX;
        public static int minMapY;
        public static int maxMapX;
        public static int maxMapY;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            graphics.PreferredBackBufferWidth = WIDTH;
            graphics.PreferredBackBufferHeight = HEIGHT;
            //graphics.PreferMultiSampling = true;

            graphics.IsFullScreen = true;

            IsMouseVisible = true;
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
            InputManager.Init(this);
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            Font = Content.Load<SpriteFont>("Dungeon");
            Font32pt = Content.Load<SpriteFont>("Dungeon32pt");
            Font18pt = Content.Load<SpriteFont>("Dungeon18pt");
            // TODO: use this.Content to load your game content here
            foreach (string str in allTextures)
            {
                Console.Out.WriteLine(str);
                Texture2D texture = Content.Load<Texture2D>(str);
                textureDict[str] = texture;
            }

            GameManager.NewGame();
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {

            // TODO: Add your update logic here
            string kbState = InputManager.GetKeyboardInput();
            string msState = InputManager.GetMouseInput();

            foreach (GameObject obj in GameManager.Objects)
            {
                obj.Update();
            }

            if (kbState == "move" || kbState == "pickup" || msState == "cast" || msState == "pickup")
            {
                foreach (GameObject obj in GameManager.Objects)
                {
                    if (obj.AI != null && obj.Fighter.HP > 0)
                    {
                        obj.AI.TakeTurn();
                    }
                }

                GameManager.PlayerObject.Player.UpdateTurn();
            }

            foreach (GameObject obj in GameManager.ToRemove)
            {
                GameManager.Objects.Remove(obj);
            }
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            minMapX = Math.Max(GameManager.PlayerObject.x - GameManager.VisibleMapWidth / 2, 0);
            maxMapX = Math.Min(GameManager.PlayerObject.x + GameManager.VisibleMapWidth / 2 + 1, GameManager.MAP_WIDTH);
            minMapY = Math.Max(GameManager.PlayerObject.y - GameManager.VisibleMapHeight / 2, 0);
            maxMapY = Math.Min(GameManager.PlayerObject.y + GameManager.VisibleMapHeight / 2 + 1, GameManager.MAP_HEIGHT);

            GraphicsDevice.Clear(Microsoft.Xna.Framework.Color.Black);

            //Console.Out.WriteLine("X Start : " + minMapX.ToString() + " | X Stop : " + maxMapX.ToString());
            //Console.Out.WriteLine("Y Start : " + minMapY.ToString() + " | Y Stop : " + maxMapY.ToString());
            // TODO: Add your drawing code here
            spriteBatch.Begin();

            for (int x = minMapX; x < maxMapX; x++)
            {
                for (int y = minMapY; y < maxMapY; y++)

                {
                    Tile tile = GameManager.Map[x, y];
                    tile.Draw(spriteBatch, GameManager.PlayerObject.Position.x, GameManager.PlayerObject.Position.y);
                }
            }

            if (InputManager.MouseInMap)
            {
                int picX = GameManager.DrawMapX + (GameManager.VisibleMapWidth / 2 + InputManager.MouseMapX - GameManager.PlayerObject.Position.x) * (int)(GameManager.TileWidth * Game1.GLOBAL_SIZE_MOD / 100);
                int picY = GameManager.DrawMapY + (GameManager.VisibleMapHeight / 2 + InputManager.MouseMapY - GameManager.PlayerObject.Position.y) * (int)(GameManager.TileWidth * Game1.GLOBAL_SIZE_MOD / 100);

                int offsetX = -GameManager.PlayerObject.OffsetPos.x + GameManager.PlayerObject.BigPos.x;
                int offsetY = -GameManager.PlayerObject.OffsetPos.y + GameManager.PlayerObject.BigPos.y;

                int width = (int)(GameManager.TileWidth * GLOBAL_SIZE_MOD / 100);

                spriteBatch.Draw(textureDict["big_target"], new Microsoft.Xna.Framework.Rectangle(picX + offsetX, picY + offsetY, width, width), Microsoft.Xna.Framework.Color.Wheat);
            }

            foreach (GameObject obj in SortGameObjectsForDraw(GameManager.Objects))
            {
                if (minMapX <= obj.x && obj.x <= maxMapX && minMapY <= obj.y && obj.y <= maxMapY)
                {
                    obj.Draw(spriteBatch, GameManager.PlayerObject.Position.x, GameManager.PlayerObject.Position.y);
                }
            }

            GameManager.abilityGUI.Draw(spriteBatch);
            GameManager.statusGUI.Draw(spriteBatch);
            GameManager.equipmentGUI.Draw(spriteBatch);
            GameManager.inventoryGUI.Draw(spriteBatch);

            //string TestString = "Font drawing test";
            //Vector2 position = new Vector2(InputManager.GameInstance.Window.ClientBounds.Width / 2, InputManager.GameInstance.Window.ClientBounds.Height - Font.MeasureString(TestString).Y);
            //spriteBatch.DrawString(Font, TestString, position, Microsoft.Xna.Framework.Color.OrangeRed);

            //DrawMiniLog(spriteBatch);

            if (InputManager.deathOpen) DeathScreen.Draw(spriteBatch);
            else if (InputManager.mapOpen) GameManager.miniMap.Draw(spriteBatch);

            spriteBatch.End();

            base.Draw(gameTime);
        }

        private List<GameObject> SortGameObjectsByDrawPriority(List<GameObject> baseList)
        {
            List<GameObject> temp = new List<GameObject>(baseList);
            temp.Sort((x, y) => x.Position.y.CompareTo(y.Position.y));
            return temp;
        }

        private List<GameObject> SortGameObjectsForDraw(List<GameObject> baseList)
        {
            List<GameObject> TempList = new List<GameObject>(baseList);
            List<GameObject> Items = new List<GameObject>();
            List<GameObject> Others = new List<GameObject>();

            foreach (GameObject gameObj in baseList)
            {
                if (gameObj.Item != null)
                {
                    Items.Add(gameObj);
                }
                else
                {
                    Others.Add(gameObj);
                }
                TempList.Remove(gameObj);
            }

            if (TempList.Count > 0)
            {
                throw new IndexOutOfRangeException("TempList is not empty (length = " + TempList.Count.ToString() + ")");
            }
            else
            {
                List<GameObject> SortedItems = SortGameObjectsByDrawPriority(Items);
                List<GameObject> SortedOthers = SortGameObjectsByDrawPriority(Others);

                foreach (GameObject gameObj in SortedItems)
                {
                    TempList.Add(gameObj);
                }
                foreach (GameObject gObj in SortedOthers)
                {
                    TempList.Add(gObj);
                }
                return TempList;
            }


        }

        public void DrawMiniLog(SpriteBatch spriteBatch)
        {
            int displayedMessages = Math.Min(GameManager.Log.Count, GameManager.MAX_MESSAGES - 1);
            if (displayedMessages > 0)
            {
                string RefString = "The quick fox jumps over the lazy brown dog";
                for (int i = 0; i < displayedMessages; i++)
                {
                    TheEternalOne.Code.Game.Message curMessage;
                    if (GameManager.Log.Count > 3)
                    {
                        curMessage = GameManager.Log[GameManager.Log.Count - (GameManager.MAX_MESSAGES - 1) + i];
                    }
                    else
                    {
                        curMessage = GameManager.Log[i];
                    }
                    Vector2 position = new Vector2((InputManager.GameInstance.Window.ClientBounds.Width - Font.MeasureString(curMessage.Content).X) / 2, InputManager.GameInstance.Window.ClientBounds.Height - (GameManager.MAX_MESSAGES - 1- i) * (Font.MeasureString(RefString).Y) - 10);
                    spriteBatch.DrawString(Font, curMessage.Content, position, curMessage.Color);
                }
            }
        }
    }
}
