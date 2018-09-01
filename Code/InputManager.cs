using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using Microsoft.Xna.Framework.Graphics;
using TheEternalOne.Code;

namespace TheEternalOne
{
    static class InputManager
    {
        //public static Game1 GameInstance;
        #region VariablesDeclaration
        public static Game1 GameInstance;
        public static KeyboardState PreviousKeyboardState;
        public static MouseState PreviousMouseState;
        public static bool MouseInMap;
        public static int MouseMapX;
        public static int MouseMapY;
        public static int SelectedSpellIndex = -1;
        public static int SpellIndex = -1;
        public static bool UpgradeSpell = false;

        public static int leftMapX = GameManager.DrawMapX;
        public static int rightMapX = GameManager.DrawMapX + GameManager.VisibleMapWidth * (int)(GameManager.TileWidth * Game1.GLOBAL_SIZE_MOD / 100);
        public static int upMapY = GameManager.DrawMapY;
        public static int downMapY = GameManager.DrawMapY + GameManager.VisibleMapHeight * (int)(GameManager.TileWidth * Game1.GLOBAL_SIZE_MOD / 100);

        public static bool forceMouseUpdate = false;

        public delegate void KeyboardPressEvent(int dx, int dy);
        public static event KeyboardPressEvent OnKeyboardPress = new KeyboardPressEvent((int x, int y) => { });
        #endregion

        public static void InitializeInput()
        {
            PreviousKeyboardState = Keyboard.GetState();
            PreviousMouseState = Mouse.GetState();
            OnKeyboardPress += ForceMouseUpdate;
        }

        public static string MoveInput(int dx, int dy)
        {
            //bool canMove = GameManager.PlayerObject.Fighter.CanMove(dx, dy);
            //if (canMove) OnKeyboardPress.Invoke(dx, dy);
            //string state = GameManager.PlayerObject.Fighter.MoveOrAttack(dx, dy);
            //return state;
            if (GameManager.PlayerObject.Player.CanMove)
            {
                GameManager.PlayerObject.Move(dx, dy);
            }
            return "move";
        }

        public static string GetKeyboardInput()
        {
            string state = "none";
            try
            {
                KeyboardState KeyboardState = Keyboard.GetState();
                #region KBInputProcess
                if (KeyboardState.IsKeyDown(Keys.Left) && !PreviousKeyboardState.IsKeyDown(Keys.Left))
                {
                    state = MoveInput(-1, 0);
                }
                if (KeyboardState.IsKeyDown(Keys.Right) && !PreviousKeyboardState.IsKeyDown(Keys.Right))
                {
                    state = MoveInput(1, 0);
                    //OnKeyboardPress.Invoke(1, 0);
                }
                if (KeyboardState.IsKeyDown(Keys.Up) && !PreviousKeyboardState.IsKeyDown(Keys.Up))
                {
                    state = MoveInput(0, -1);
                }
                if (KeyboardState.IsKeyDown(Keys.Down) && !PreviousKeyboardState.IsKeyDown(Keys.Down))
                {
                    state = MoveInput(0, 1);
                    //OnKeyboardPress.Invoke(0, 1);
                }

                if (KeyboardState.IsKeyDown(Keys.F11) && !PreviousKeyboardState.IsKeyDown(Keys.F11))
                {
                    GameInstance.graphics.IsFullScreen = !GameInstance.graphics.IsFullScreen;
                    GameInstance.graphics.ApplyChanges();
                    //OnKeyboardPress.Invoke(0, 1);
                }

                PreviousKeyboardState = KeyboardState;
                #endregion
            }
            catch (System.InvalidOperationException)
            {
                Console.Out.WriteLine("Keyboard overload");
                state = "ERROR";
            }
            return state;
        }

        public static string GetMouseInput()
        {
            MouseState ms;
            return GetMouseInput(out ms);
        }

        public static string GetMouseInput(out MouseState ms)
        {
            string state = "none";
            MouseState MouseState = Mouse.GetState();
            if (forceMouseUpdate || !((PreviousMouseState.LeftButton == MouseState.LeftButton) && (PreviousMouseState.RightButton == MouseState.RightButton) && (PreviousMouseState.X == MouseState.X) && (PreviousMouseState.Y == MouseState.Y)))
            {
                if (leftMapX <= MouseState.X && MouseState.X < rightMapX && upMapY <= MouseState.Y && MouseState.Y < downMapY)
                {
                    MouseInMap = true;
                    int xRatio = (MouseState.X - GameManager.DrawMapX) / (int)(GameManager.TileWidth * Game1.GLOBAL_SIZE_MOD / 100);
                    int yRatio = (MouseState.Y - GameManager.DrawMapY) / (int)(GameManager.TileWidth * Game1.GLOBAL_SIZE_MOD / 100);
                    SpellIndex = -1;
                    MouseMapX = xRatio + Game1.minMapX;
                    MouseMapY = yRatio + Game1.minMapY;
                    UpgradeSpell = false;
                }
                else if (MouseState.X >= GameManager.abilityGUI.x)
                {
                    MouseInMap = false;
                    for (int i = 0; i < 5; i++)
                    {
                        if (GameManager.abilityGUI.y + i * GameManager.AbilityHeight + i * 5 <= MouseState.Y && MouseState.Y < GameManager.abilityGUI.y + (i + 1) * GameManager.AbilityHeight + i * 5)
                        {
                            int width = (int)(GameManager.abilityGUI.upgradeTexture.Width * 0.7f * Game1.GLOBAL_SIZE_MOD / 100);
                            if (i != 4 && Game1.WIDTH - width - 20 <= MouseState.X && GameManager.abilityGUI.y + i * GameManager.AbilityHeight + i * 5 + 10 <= MouseState.Y &&
                                MouseState.Y <= GameManager.abilityGUI.y + i * GameManager.AbilityHeight + i * 5 + 10 + width)
                            {
                                UpgradeSpell = true;
                            }
                            else UpgradeSpell = false;
                            
                            SpellIndex = i;
                            break;
                        }
                    }
                }
                else
                {
                    SpellIndex = -1;
                    MouseInMap = false;
                    UpgradeSpell = false;
                }

                if (MouseState.LeftButton == ButtonState.Pressed && PreviousMouseState.LeftButton == ButtonState.Released)
                {
                    if (MouseState.X >= GameManager.abilityGUI.x)
                    {
                        MouseInMap = false;
                        for (int i = 0; i < 5; i++)
                        {
                            if (GameManager.abilityGUI.y + i * GameManager.AbilityHeight + i * 5 <= MouseState.Y && MouseState.Y < GameManager.abilityGUI.y + (i + 1) * GameManager.AbilityHeight + i * 5)
                            {
                                SelectedSpellIndex = i;
                                if (i != 4 && UpgradeSpell && GameManager.PlayerObject.Fighter.XP > 0)
                                {
                                    GameManager.PlayerObject.Player.UpgardeSpell(i);
                                    GameManager.PlayerObject.Fighter.XP--;
                                }
                                break;
                            }
                        }
                    }
                }
                PreviousMouseState = MouseState;
                forceMouseUpdate = false;
                ms = MouseState;
            }
            else
            {
                //Console.Out.WriteLine("Static mouse");
                ms = PreviousMouseState;
            }

            return state;
        }

        public static void ForceMouseUpdate(int dx, int dy)
        {
            MouseState ms;
            //forceMouseUpdate = true;
            GetMouseInput(out ms);
            //forceMouseUpdate = true;

            int tmpMapX = (GameManager.PlayerObject.x + dx) - GameManager.VisibleMapWidth / 2;
            int tmpMapY = (GameManager.PlayerObject.y + dy) - GameManager.VisibleMapHeight / 2;

            int xRatio = (ms.X - GameManager.DrawMapX) / GameManager.TileWidth;
            int yRatio = (ms.Y - GameManager.DrawMapY) / GameManager.TileWidth;
            MouseMapX = xRatio + tmpMapX;
            MouseMapY = yRatio + tmpMapY;
            //GameManager.GameInstance.CurrentScreen.DrawScreen();
        }

        public static void Init(Game1 owner)
        {
            GameInstance = owner;
        }

    }
}
