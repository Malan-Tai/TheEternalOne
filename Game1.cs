using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;
using TheEternalOne.Code;
using TheEternalOne.Code.Map;

namespace TheEternalOne
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        public static System.Drawing.Rectangle res = Screen.PrimaryScreen.Bounds;

        public static int WIDTH = res.Width;
        public static int HEIGHT = res.Height;
        public static float GLOBAL_SIZE_MOD = WIDTH * 100 / 1920;

        public static Dictionary<string, Texture2D> textureDict = new Dictionary<string, Texture2D>();
        private static List<string> allTextures = new List<string> { "tile50x50", "wall" };

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            graphics.PreferredBackBufferWidth = WIDTH;
            graphics.PreferredBackBufferHeight = HEIGHT;
            //graphics.PreferMultiSampling = true;

            graphics.IsFullScreen = false;

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

            InputManager.GetInGameInput();
            // TODO: Add your update logic here


            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Microsoft.Xna.Framework.Color.Black);

            // TODO: Add your drawing code here
            spriteBatch.Begin();

            for (int x = 0; x < GameManager.MAP_WIDTH; x++)
            {
                for (int y = 0; y < GameManager.MAP_HEIGHT; y++)
                {
                    Tile tile = GameManager.Map[x, y];
                    tile.Draw(spriteBatch, 0, 0);
                }
            }

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
