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
using TheEternalOne.Code.Objects;
using TheEternalOne.Code.Utils;
using TheEternalOne.Code.GUI;
using static TheEternalOne.Code.GameManager.GameState;
using TheEternalOne.Code.Objects.Mobs;

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

        public static int EqIndex = -1;
        public static int InvIndex = -1;

        public static int leftMapX = GameManager.DrawMapX;
        public static int rightMapX = GameManager.DrawMapX + GameManager.VisibleMapWidth * (int)(GameManager.TileWidth * Game1.GLOBAL_SIZE_MOD / 100);
        public static int upMapY = GameManager.DrawMapY;
        public static int downMapY = GameManager.DrawMapY + GameManager.VisibleMapHeight * (int)(GameManager.TileWidth * Game1.GLOBAL_SIZE_MOD / 100);

        public static bool forceMouseUpdate = false;

        public static bool mapOpen = false;
        public static bool deathOpen = false;

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
            forceMouseUpdate = true;
            return "move";
        }

        public static string GetKeyboardInput()
        {
            string state = "none";
            
            try
            {
                KeyboardState KeyboardState = Keyboard.GetState();
                if (GameManager.CurrentState == Playing)
                {
                    #region KBInputProcess
                    if (KeyboardState.IsKeyDown(Keys.Left) && !PreviousKeyboardState.IsKeyDown(Keys.Left) && !mapOpen)
                    {
                        state = MoveInput(-1, 0);
                    }
                    if (KeyboardState.IsKeyDown(Keys.Right) && !PreviousKeyboardState.IsKeyDown(Keys.Right) && !mapOpen)
                    {
                        state = MoveInput(1, 0);
                        //OnKeyboardPress.Invoke(1, 0);
                    }
                    if (KeyboardState.IsKeyDown(Keys.Up) && !PreviousKeyboardState.IsKeyDown(Keys.Up) && !mapOpen)
                    {
                        state = MoveInput(0, -1);
                    }
                    if (KeyboardState.IsKeyDown(Keys.Down) && !PreviousKeyboardState.IsKeyDown(Keys.Down) && !mapOpen)
                    {
                        state = MoveInput(0, 1);
                        //OnKeyboardPress.Invoke(0, 1);
                    }

                    if (KeyboardState.IsKeyDown(Keys.D1) && !PreviousKeyboardState.IsKeyDown(Keys.D1))
                    {
                        GameObject toUse;
                        try
                        {
                            toUse = GameManager.PlayerObject.Player.Inventory[0];
                        }
                        catch (ArgumentOutOfRangeException)
                        {
                            return "error";
                        }

                        if (toUse != null)
                        {
                            toUse.Item.Use();
                            state = "move"; //Skips turn
                        }
                    }

                    if (KeyboardState.IsKeyDown(Keys.D2) && !PreviousKeyboardState.IsKeyDown(Keys.D2))
                    {
                        GameObject toUse;
                        try
                        {
                            toUse = GameManager.PlayerObject.Player.Inventory[1];
                        }
                        catch (ArgumentOutOfRangeException)
                        {
                            return "error";
                        }

                        if (toUse != null)
                        {
                            toUse.Item.Use();
                            state = "move"; //Skips turn
                        }
                    }

                    if (KeyboardState.IsKeyDown(Keys.D3) && !PreviousKeyboardState.IsKeyDown(Keys.D3))
                    {
                        GameObject toUse;
                        try
                        {
                            toUse = GameManager.PlayerObject.Player.Inventory[2];
                        }
                        catch (ArgumentOutOfRangeException)
                        {
                            return "error";
                        }

                        if (toUse != null)
                        {
                            toUse.Item.Use();
                            state = "move"; //Skips turn
                        }
                    }

                    if (KeyboardState.IsKeyDown(Keys.D4) && !PreviousKeyboardState.IsKeyDown(Keys.D4))
                    {
                        GameObject toUse;
                        try
                        {
                            toUse = GameManager.PlayerObject.Player.Inventory[3];
                        }
                        catch (ArgumentOutOfRangeException)
                        {
                            return "error";
                        }

                        if (toUse != null)
                        {
                            toUse.Item.Use();
                            state = "move"; //Skips turn
                        }
                    }

                    if (KeyboardState.IsKeyDown(Keys.F11) && !PreviousKeyboardState.IsKeyDown(Keys.F11))
                    {
                        GameInstance.graphics.IsFullScreen = !GameInstance.graphics.IsFullScreen;
                        GameInstance.graphics.ApplyChanges();
                    }

                    if (KeyboardState.IsKeyDown(Keys.G) && !PreviousKeyboardState.IsKeyDown(Keys.G) && !mapOpen)
                    {
                        if (GameManager.PlayerObject.Player.CanPickUp)
                        {
                            foreach (GameObject gameObj in GameManager.Objects)
                            {
                                if (gameObj.Position.x == GameManager.PlayerObject.Position.x && gameObj.Position.y == GameManager.PlayerObject.Position.y && gameObj.Item != null)
                                {
                                    gameObj.Item.PickUp();
                                    PreviousKeyboardState = KeyboardState;
                                    return "pickup";
                                }
                            }

                            GameManager.LogWarning("No item to pick up here");
                        }
                        else
                        {
                            GameManager.LogWarning("You have already lost the ability to pick up objects !");
                        }
                    }

                    if (KeyboardState.IsKeyDown(Keys.I) && !PreviousKeyboardState.IsKeyDown(Keys.I))
                    {
                        GameManager.PlayerObject.Player.DisplayInventory();
                    }

                    if (KeyboardState.IsKeyDown(Keys.Space) && !PreviousKeyboardState.IsKeyDown(Keys.Space))
                    {
                        foreach (GameObject obj in GameManager.Objects)
                        {
                            if (obj.isStairs && obj.Position.x == GameManager.PlayerObject.Position.x && obj.Position.y == GameManager.PlayerObject.Position.y)
                            {
                                GameManager.NextFloor();
                            }
                        }
                    }


                    if (KeyboardState.IsKeyDown(Keys.E) && !PreviousKeyboardState.IsKeyDown(Keys.E))
                    {
                        GameManager.PlayerObject.Player.DisplayEquipment();
                    }
                    if (KeyboardState.IsKeyDown(Keys.M) && !PreviousKeyboardState.IsKeyDown(Keys.M))
                    {
                        if (GameManager.PlayerObject.Player.CanMap)
                        {
                            mapOpen = !mapOpen;
                        }
                    }

                    if (KeyboardState.IsKeyDown(Keys.D) && !PreviousKeyboardState.IsKeyDown(Keys.D))
                    {
                        DeathScreen.Initialize();
                        GameManager.CurrentState = Dead;
                    }

                    if (KeyboardState.IsKeyDown(Keys.F2) && !PreviousKeyboardState.IsKeyDown(Keys.F2))
                    {
                        //GameManager.Objects.Add(MobFactory.CreateTower(GameManager.PlayerObject.Position.x, GameManager.PlayerObject.Position.y - 1));
                    }

                    PreviousKeyboardState = KeyboardState;
                    #endregion
                }
                else if (GameManager.CurrentState == GameOver)
                {
                    if (KeyboardState.IsKeyDown(Keys.Space) && !PreviousKeyboardState.IsKeyDown(Keys.Space))
                    {
                        GameManager.GoToMainMenu();
                    }
                }
                else
                {
                    if (KeyboardState.IsKeyDown(Keys.F11) && !PreviousKeyboardState.IsKeyDown(Keys.F11))
                    {
                        GameInstance.graphics.IsFullScreen = !GameInstance.graphics.IsFullScreen;
                        GameInstance.graphics.ApplyChanges();
                    }
                }
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
            if ((forceMouseUpdate || !((PreviousMouseState.LeftButton == MouseState.LeftButton) && (PreviousMouseState.RightButton == MouseState.RightButton) && (PreviousMouseState.X == MouseState.X) && (PreviousMouseState.Y == MouseState.Y))) && !mapOpen)
            {
                if (GameManager.CurrentState == Playing)
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
                        EqIndex = -1;
                        InvIndex = -1;
                    }
                    else if (MouseState.X >= GameManager.abilityGUI.x)
                    {
                        MouseInMap = false;
                        EqIndex = -1;
                        InvIndex = -1;
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
                    else if (MouseState.X <= GameManager.equipmentGUI.x + GameManager.EquipmentWidth)
                    {
                        SpellIndex = -1;
                        MouseInMap = false;
                        UpgradeSpell = false;
                        InvIndex = -1;

                        for (int i = 0; i < GameManager.PlayerObject.Player.GetAllEquipped().Count; i++)
                        {
                            if (GameManager.EquipmentY + i * GameManager.EquipmentHeight + i * 5 <= MouseState.Y && MouseState.Y < GameManager.EquipmentY + (i + 1) * GameManager.EquipmentHeight + i * 5)
                            {
                                EqIndex = i;
                                break;
                            }
                        }
                    }
                    else if (MouseState.Y > GameManager.InventoryY)
                    {
                        SpellIndex = -1;
                        MouseInMap = false;
                        UpgradeSpell = false;
                        EqIndex = -1;

                        for (int i = 0; i < GameManager.PlayerObject.Player.Inventory.Count; i++)
                        {
                            if (GameManager.InventoryX + i * GameManager.InventoryWidth + i * 5 <= MouseState.X && MouseState.X < GameManager.InventoryX + (i + 1) * GameManager.InventoryWidth + i * 5)
                            {
                                InvIndex = i;
                                break;
                            }
                        }
                    }
                    else
                    {
                        SpellIndex = -1;
                        MouseInMap = false;
                        UpgradeSpell = false;
                        EqIndex = -1;
                        InvIndex = -1;
                    }
                }
                else if (GameManager.CurrentState == Dead)
                {
                    Rectangle?[] selectButtons = new Rectangle?[4]
                    {
                        DeathScreen.Choice1Select,
                        DeathScreen.Choice2Select,
                        DeathScreen.Choice3Select,
                        DeathScreen.RerollSelect
                    };

                    bool found = false;
                    foreach (Rectangle? rect in selectButtons)
                    {
                        if (rect != null)
                        {
                            if ((MouseState.X >= rect.Value.X) && (MouseState.X <= (rect.Value.X + rect.Value.Width)) && (MouseState.Y >= rect.Value.Y) && (MouseState.Y <= (rect.Value.Y + rect.Value.Height)))
                            {
                                found = true;
                                DeathScreen.SelectedIndex = Array.IndexOf(selectButtons, rect);
                                break;
                            }
                        }
                    }
                    if (!found)
                    {
                        DeathScreen.SelectedIndex = -1;
                    }
                }
                else if (GameManager.CurrentState == MainMenu)
                {
                    Rectangle?[] selectButtons = new Rectangle?[2]
                    {
                        Game1.Button1,
                        Game1.Button2
                    };

                    bool found = false;
                    foreach (Rectangle? rect in selectButtons)
                    {
                        if (rect != null)
                        {
                            if ((MouseState.X >= rect.Value.X) && (MouseState.X <= (rect.Value.X + rect.Value.Width)) && (MouseState.Y >= rect.Value.Y) && (MouseState.Y <= (rect.Value.Y + rect.Value.Height)))
                            {
                                found = true;
                                Game1.menuSelectIndex = Array.IndexOf(selectButtons, rect);
                                break;
                            }
                        }
                    }
                    if (!found)
                    {
                        Game1.menuSelectIndex = -1;
                    }

                }

                if (MouseState.LeftButton == ButtonState.Pressed && PreviousMouseState.LeftButton == ButtonState.Released)
                {
                    if (GameManager.CurrentState == Playing)
                    {
                        if (MouseState.X >= GameManager.abilityGUI.x)
                        {
                            MouseInMap = false;
                            for (int i = 0; i < 5; i++)
                            {
                                if (GameManager.abilityGUI.y + i * GameManager.AbilityHeight + i * 5 <= MouseState.Y && MouseState.Y < GameManager.abilityGUI.y + (i + 1) * GameManager.AbilityHeight + i * 5)
                                {
                                    if (i != 4 && UpgradeSpell)
                                    {
                                        if (GameManager.PlayerObject.Fighter.XP > 0)
                                        {
                                            GameManager.PlayerObject.Player.UpgradeSpell(i);
                                            GameManager.PlayerObject.Fighter.XP--;
                                        }
                                        SelectedSpellIndex = i;
                                    }
                                    else if (SelectedSpellIndex == i)
                                    {
                                        SelectedSpellIndex = -1;
                                        GameManager.ActiveMessage = null;
                                    }
                                    else
                                    {
                                        SelectedSpellIndex = i;
                                    }
                                    break;
                                }
                            }
                        }
                        else if (MouseInMap && SelectedSpellIndex != -1)
                        {
                            Console.Out.WriteLine("casting spell {0} at {1} {2}", SelectedSpellIndex, MouseMapX, MouseMapY);
                            GameManager.PlayerObject.Player.Cast(SelectedSpellIndex, MouseMapX, MouseMapY);
                            state = "cast";
                        }
                        else if (MouseInMap && Distance.GetDistance(GameManager.PlayerObject.x, GameManager.PlayerObject.y, MouseMapX, MouseMapY) < 2)
                        {
                            if (GameManager.PlayerObject.Player.CanPickUp)
                            {
                                bool found = false;
                                foreach (GameObject gameObj in GameManager.Objects)
                                {
                                    if (gameObj.Position.x == MouseMapX && gameObj.Position.y == MouseMapY && gameObj.Item != null)
                                    {
                                        gameObj.Item.PickUp();
                                        state = "pickup";
                                        found = true;
                                        break;
                                    }
                                }

                                if (!found) GameManager.LogWarning("No item to pick up here");
                            }
                            else
                            {
                                GameManager.LogWarning("You have already lost the ability to pick up objects !");
                            }
                        }
                        else if (InvIndex != -1)
                        {
                            GameManager.PlayerObject.Player.Inventory[InvIndex].Item.Use();
                        }
                        else if (EqIndex != -1)
                        {
                            GameManager.PlayerObject.Player.GetAllEquipped()[EqIndex].Item.Use();
                        }
                    }
                    else if (GameManager.CurrentState == Dead)
                    {
                        DeathScreen.OnLeftClick();
                    }
                    else if (GameManager.CurrentState == MainMenu)
                    {
                        if (Game1.menuSelectIndex == 0)
                        {
                            Game1.PlaySFX("HUD_Click_01");
                            GameManager.NewGame();
                        }
                        else if (Game1.menuSelectIndex == 1)
                        {
                            Game1.PlaySFX("HUD_Click_01");
                            Environment.Exit(0);
                        }
                    }
                }

                else if (MouseState.RightButton == ButtonState.Pressed && PreviousMouseState.RightButton == ButtonState.Released && GameManager.CurrentState == GameManager.GameState.Playing)
                {

                    if (InvIndex != -1)
                    {
                        GameManager.PlayerObject.Player.Inventory[InvIndex].Item.Drop();
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
