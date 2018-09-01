using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheEternalOne.Code
{
    static class InputManager
    {
        public static Game1 GameInstance;
        
        public static void GetInGameInput()
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                GameInstance.Exit();
            }
        }

        public static void Init(Game1 owner)
        {
            GameInstance = owner;
        }

    }
}
