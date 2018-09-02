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
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Audio;

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

        public const int LOGO_WIDTH = 990;
        public const int LOGO_HEIGHT = 180;

        public const int LOGO_Y = 100;
        public const int FIRST_GAME_OVER_LINE_PADDING = 200;
        public const int BUTTONS_PADDING = 50;

        public static Dictionary<string, Texture2D> textureDict = new Dictionary<string, Texture2D>();
        private static List<string> allTextures = new List<string> { "tile50x50", "white", "wall", "big_target", "HP_GUI", "MP_GUI", "XP_GUI", "Shield_GUI", "basicenemy_placeholder",
            "upgrade_GUI", "upgrade_GUI_lit", "healthpotion_placeholder", "sword_placeholder", "amulet_placeholder", "hero", "pawn", "wall_down", "wall_down_up", "wall_left",
            "wall_left_down", "wall_left_down_corner", "wall_left_down_up", "wall_left_up", "wall_left_up_corner", "wall_right", "wall_right_down", "wall_right_down_corner",
            "wall_right_down_up", "wall_right_left", "wall_right_left_down", "wall_right_left_up", "wall_right_up", "wall_right_up_corner", "wall_up", "floor_tile", "stairs_placeholder", "background_placeholder", "logo_placeholder",
            "healthPotion", "manaPotion", "magicAmulet", "sword", "shield", "armor", "tower", "stairs", "background"};

        public static Dictionary<string, Song> songDict = new Dictionary<string, Song>();
        private static List<string> allSongs = new List<string> { "M_Menu_Goth_Loop_01", "M_Goth_Loop_01" };

        public static Dictionary<string, SoundEffect> sfxDict = new Dictionary<string, SoundEffect>();
        private static List<string> allSFX = new List<string> {  "HUD_Click_01", "SFX_Open_Map_01", "SFX_Take_Ojbect_01", "SFX_Drop_Ojbect_01", "SFX_UnEquip_01", "SFX_Equip_01", "SFX_Potion_01", "SFX_Teleport_01", "SFX_Health_01", "SFX_Fireball_01", "SFX_Bash_Shield_01", "SFX_Whoosh_Sword_01" };

        public static SpriteFont Font;
        public static SpriteFont Font32pt;
        public static SpriteFont Font18pt;

        public static int minMapX;
        public static int minMapY;
        public static int maxMapX;
        public static int maxMapY;

        public static Microsoft.Xna.Framework.Rectangle? Button1 = null;
        public static Microsoft.Xna.Framework.Rectangle? Button2 = null;

        public static int menuSelectIndex = -1;

        public static Song CurrentSong;

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

            foreach (string str in allSongs)
            {
                Console.Out.WriteLine(str);
                Song song = Content.Load<Song>(str);
                songDict[str] = song;
            }

            foreach (string str in allSFX)
            {
                Console.Out.WriteLine(str);
                SoundEffect sfx = Content.Load<SoundEffect>(str);
                sfxDict[str] = sfx;
            }
            GoToMainMenu();
            //GameManager.NewGame();
        }

        public static void PlayCurrentSong()
        {
            MediaPlayer.IsRepeating = true;
            MediaPlayer.Play(CurrentSong);
        }

        public static void PlaySFX(string str)
        {
            sfxDict[str].Play();
        }

        public void GoToMainMenu()
        {
            CurrentSong = songDict["M_Menu_Goth_Loop_01"];
            PlayCurrentSong();
            GameManager.CurrentState = GameManager.GameState.MainMenu;
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
            if (GameManager.CurrentState != GameManager.GameState.MainMenu && GameManager.CurrentState != GameManager.GameState.GameOver)
            {
                
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
                    //InputManager.SelectedSpellIndex = -1;
                    if (GameManager.Started && GameManager.ActiveMessage != null)
                    {
                        if (GameManager.ActiveMessage.Content == "Click on a tile to teleport to")
                        {
                            //GameManager.ActiveMessage = null;
                        }
                    }
                }

                foreach (GameObject obj in GameManager.ToRemove)
                {
                    GameManager.Objects.Remove(obj);
                }
            }
            base.Update(gameTime);
        }

        public void DrawGame(GameTime gameTime)
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
                if (minMapX <= obj.x && obj.x < maxMapX && minMapY <= obj.y && obj.y < maxMapY)
                {
                    obj.Draw(spriteBatch, GameManager.PlayerObject.Position.x, GameManager.PlayerObject.Position.y);
                }
            }

            GameManager.abilityGUI.Draw(spriteBatch);
            GameManager.statusGUI.Draw(spriteBatch);
            GameManager.equipmentGUI.Draw(spriteBatch);
            GameManager.inventoryGUI.Draw(spriteBatch);

            if (InputManager.SelectedSpellIndex == 4)
            {
                GameManager.LogInfo("Click on a tile to teleport to");
            }

            if (GameManager.ActiveMessage != null)
            {
                Vector2 position = new Vector2((int)(InputManager.GameInstance.Window.ClientBounds.Width - Font.MeasureString(GameManager.ActiveMessage.Content).X) / 2, 50 + (int)Font.MeasureString(GameManager.ActiveMessage.Content).Y);
                spriteBatch.DrawString(Font, GameManager.ActiveMessage.Content, position, GameManager.ActiveMessage.Color);
            }

            //string TestString = "Font drawing test";
            //Vector2 position = new Vector2(InputManager.GameInstance.Window.ClientBounds.Width / 2, InputManager.GameInstance.Window.ClientBounds.Height - Font.MeasureString(TestString).Y);
            //spriteBatch.DrawString(Font, TestString, position, Microsoft.Xna.Framework.Color.OrangeRed);

            //DrawMiniLog(spriteBatch);

            if (GameManager.CurrentState == GameManager.GameState.Dead) DeathScreen.Draw(spriteBatch);
            else if (InputManager.mapOpen) GameManager.miniMap.Draw(spriteBatch);

            spriteBatch.End();
        }
        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        
        protected override void Draw(GameTime gameTime)
        {
            if (GameManager.isGameOver)
            {
                GameManager.CurrentState = GameManager.GameState.GameOver;
            }
            if ((GameManager.CurrentState == GameManager.GameState.Playing) || (GameManager.CurrentState == GameManager.GameState.Dead))
            {

                DrawGame(gameTime);
            }
            else if (GameManager.CurrentState == GameManager.GameState.GameOver)
            {
                Console.Out.WriteLine("YOU LOST");
                DrawGameOver(gameTime);
            }
            else
            {
                DrawMainMenu(gameTime);
            }
            base.Draw(gameTime);
        }

        public static void StopMusic()
        {
            MediaPlayer.Stop();
        }
        public void DrawGameOver(GameTime gameTime)
        {
            Console.Out.WriteLine("GAme over draw start");
            spriteBatch.Begin();
            Console.Out.WriteLine("Game over draw started");
            spriteBatch.Draw(textureDict["white"], new Microsoft.Xna.Framework.Rectangle(0, 0, WIDTH, HEIGHT), Microsoft.Xna.Framework.Color.Black);

            int GameOverY = (int)(LOGO_Y * GLOBAL_SIZE_MOD / 100);
            Vector2 GameOverDimensions = Font32pt.MeasureString("Game Over !");
            int GameOverX = (int)(WIDTH - (int)GameOverDimensions.X) / 2;
            spriteBatch.DrawString(Font32pt, "Game Over", new Vector2(GameOverX, GameOverY), Microsoft.Xna.Framework.Color.Red);

            string firstLine = "Having lost the entirety of both your soul and body to the devil, you are now stuck for eternity in this dungeon";
            Vector2 FirstLineDimensions = Font18pt.MeasureString(firstLine);
            int FirstLineY = GameOverY + FIRST_GAME_OVER_LINE_PADDING;
            int FirstLineX = (int)(WIDTH - (int)FirstLineDimensions.X) / 2;
            spriteBatch.DrawString(Font18pt, firstLine, new Vector2(FirstLineX, FirstLineY), Microsoft.Xna.Framework.Color.White);

            string lastLine = "Press Space to return to main menu...";
            Vector2 LastLineDimensions = Font18pt.MeasureString(lastLine);
            int LastLineY = (int)(WIDTH / 2);
            int LastLineX = (int)(WIDTH - (int)LastLineDimensions.X) / 2;
            spriteBatch.DrawString(Font18pt, lastLine, new Vector2(LastLineX, LastLineY), Microsoft.Xna.Framework.Color.White);
            Console.Out.WriteLine("Game over draw end");
            spriteBatch.End();
            Console.Out.WriteLine("Game over draw ended");
        }

        public void DrawMainMenu(GameTime gameTime)
        {
            spriteBatch.Begin();
            spriteBatch.Draw(textureDict["background"], new Microsoft.Xna.Framework.Rectangle(0, 0, WIDTH, HEIGHT), Microsoft.Xna.Framework.Color.White);

            int actualLogoY = (int)(LOGO_Y * GLOBAL_SIZE_MOD / 100);
            int actualLogoWidth = (int)(LOGO_WIDTH * GLOBAL_SIZE_MOD / 100);
            int actualLogoHeight = (int)(LOGO_HEIGHT * GLOBAL_SIZE_MOD / 100);

            //spriteBatch.Draw(textureDict["logo_placeholder"], new Microsoft.Xna.Framework.Rectangle((int)((WIDTH - actualLogoWidth) / 2), actualLogoY, actualLogoWidth, actualLogoHeight), Microsoft.Xna.Framework.Color.White);

            Microsoft.Xna.Framework.Color[] SelectedColors = new Microsoft.Xna.Framework.Color[2]
            {
                Microsoft.Xna.Framework.Color.White,
                Microsoft.Xna.Framework.Color.White
            };

            if (menuSelectIndex > -1)
            {
                SelectedColors[menuSelectIndex] = Microsoft.Xna.Framework.Color.Yellow;
            }

            int centerY = (int)HEIGHT / 2;
            int firstX = (WIDTH - (int)Font32pt.MeasureString("New Game").X) / 2;
            int firstY = (int)(centerY - BUTTONS_PADDING * GLOBAL_SIZE_MOD / 100);
            int secondX = (WIDTH - (int)Font32pt.MeasureString("Quit").X) / 2;
            int secondY = (int)(centerY + BUTTONS_PADDING * GLOBAL_SIZE_MOD / 100);

            spriteBatch.DrawString(Font32pt, "New Game", new Vector2(firstX, firstY), SelectedColors[0]);
            spriteBatch.DrawString(Font32pt, "Quit", new Vector2(secondX, secondY), SelectedColors[1]);

            Button1 = new Microsoft.Xna.Framework.Rectangle(firstX, firstY, (int)Font32pt.MeasureString("New Game").X, (int)Font32pt.MeasureString("New Game").Y);
            Button2 = new Microsoft.Xna.Framework.Rectangle(secondX, secondY, (int)Font32pt.MeasureString("Quit").X, (int)Font32pt.MeasureString("Quit").Y);

            spriteBatch.End();



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
