using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using System.Threading;
using Microsoft.Xna.Framework.Graphics;
using TheEternalOne.Code;

namespace TheEternalOne
{
    static class InputManager
    {
        //public static Game1 GameInstance;
        #region VariablesDeclaration
        public static KeyboardState PreviousKeyboardState;
        public static MouseState PreviousMouseState;
        public static bool MouseInMap;
        public static int MouseMapX;
        public static int MouseMapY;
        public static int SpellIndex = -1;

        public static int leftMapX = GameManager.DrawMapX;
        public static int rightMapX = GameManager.DrawMapX + GameManager.VisibleMapWidth * GameManager.TileWidth;
        public const int upMapY = GameManager.DrawMapY;
        public const int downMapY = GameManager.DrawMapY + GameManager.VisibleMapHeight * GameManager.TileWidth;

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
            GameManager.PlayerObject.Move(dx, dy);
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
                    MoveInput(-1, 0);
                }
                if (KeyboardState.IsKeyDown(Keys.Right) && !PreviousKeyboardState.IsKeyDown(Keys.Right))
                {
                    MoveInput(1, 0);
                    //OnKeyboardPress.Invoke(1, 0);
                }
                if (KeyboardState.IsKeyDown(Keys.Up) && !PreviousKeyboardState.IsKeyDown(Keys.Up))
                {
                    MoveInput(0, -1);
                }
                if (KeyboardState.IsKeyDown(Keys.Down) && !PreviousKeyboardState.IsKeyDown(Keys.Down))
                {
                    MoveInput(0, 1);
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
                    int xRatio = (MouseState.X - GameManager.DrawMapX) / GameManager.TileWidth;
                    int yRatio = (MouseState.Y - GameManager.DrawMapY) / GameManager.TileWidth;
                    //MouseMapX = xRatio + Game1.minMapX;
                    //MouseMapY = yRatio + Game1.minMapY;
                }

                if (MouseState.LeftButton == ButtonState.Pressed && PreviousMouseState.LeftButton == ButtonState.Released)
                {
                    
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
    }
}
