using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheEternalOne.Code.Game.Death;
using TheEternalOne.Code.Utils;
using static TheEternalOne.Code.GameManager.GameState;

namespace TheEternalOne.Code.GUI
{
    public static class DeathScreen
    {
        public const int X_OFFSET = 30;
        public const int Y_OFFSET = 20;

        public const int FIRST_Y = 50;
        public const int ADDITIONAL_Y_MARGIN = -4;

        public const int CHOICES_Y_MARGIN = 100;
        public const int DESC_Y_MARGIN = 50;

        public static int WIDTH = Game1.WIDTH - 2*X_OFFSET;
        public static int HEIGHT = Game1.HEIGHT -2*Y_OFFSET;

        public static Texture2D texture = Game1.textureDict["white"];

        private static SpriteFont Font26pt;
        private static SpriteFont Font32pt;
        private static SpriteFont Font18pt;

        public static Rectangle? Choice1Select;
        public static Rectangle? Choice2Select;
        public static Rectangle? Choice3Select;
        public static Rectangle? RerollSelect;

        public static int SelectedIndex = -1;

        public static List<I_DeathEffect> AllEffects;
        public static List<I_DeathEffect> PossibleEffects;
        public static List<I_DeathEffect> CurrentEffects;

        public static bool Disable = false;

        public static void OnStart()
        {
            AllEffects = new List<I_DeathEffect>
            {
                new DrinkLoss(),
                new DropLoss(),
                new EquipLoss(),
                new FireballLoss(),
                new HealLoss(),
                new MapLoss(),
                new MovementLoss(),
                new PickUpLoss(),
                new ShieldLoss(),
                new SwordLoss(),
                new TPLoss(),
                new UnequipLoss()
            };

        }

        public static void Initialize()
        {
            WIDTH = Game1.WIDTH - 2*(int)(X_OFFSET * Game1.GLOBAL_SIZE_MOD / 100);
            HEIGHT = Game1.HEIGHT - 2*(int)(Y_OFFSET * Game1.GLOBAL_SIZE_MOD / 100);

            Font26pt = Game1.Font;
            Font32pt = Game1.Font32pt;
            Font18pt = Game1.Font18pt;

            Choice1Select = null;
            Choice2Select = null;
            Choice3Select = null;
            RerollSelect = null;

            Disable = false;


            CurrentEffects = RollEffects();
            
            SelectedIndex = -1;

        }

        private static List<I_DeathEffect> RollEffects()
        {
            PossibleEffects = new List<I_DeathEffect>();

            foreach (I_DeathEffect effect in AllEffects)
            {
                if (effect.CheckIfPossible())
                {
                    PossibleEffects.Add(effect);
                }
            }

            List<I_DeathEffect> TempList = new List<I_DeathEffect>();
            if (PossibleEffects.Count > 3)
            {
                for (int i = 0; i < 3; i++)
                {
                    Console.Out.WriteLine(PossibleEffects.Count.ToString());
                    Console.Out.WriteLine("i = " + i.ToString() + " | PossibleEffects = " + PossibleEffects.Count.ToString());
                    TempList.Add(PossibleEffects[Dice.GetRandint(0, PossibleEffects.Count)]);
                    PossibleEffects.Remove(TempList[i]);

                }
            }
            else if (PossibleEffects.Count > 2)
            {
                for (int i = 0; i <= 2; i++)
                {
                    TempList.Add(PossibleEffects[i]);
                }
            }
            else if (PossibleEffects.Count > 1)
            {
                for (int i = 0; i <= 1; i++)
                {
                    TempList.Add(PossibleEffects[i]);
                }
            }
            else
            {
                Console.Out.WriteLine("GAME OVER");
                Disable = true;
                GameManager.GoToGameOver();
                if (GameManager.CurrentState != GameOver)
                {
                    GameManager.CurrentState = GameOver;
                }
                if (GameManager.CurrentState != GameOver)
                {
                    GameManager.CurrentState = GameOver;


                }
                if (GameManager.CurrentState != GameOver)
                {
                    GameManager.CurrentState = GameOver;
                }
            }
            return TempList;

        }
        #region Drawing
        public static void Draw(SpriteBatch spriteBatch)
        {
            if (!Disable)
            {
                spriteBatch.Draw(texture, new Rectangle((int)(X_OFFSET * Game1.GLOBAL_SIZE_MOD / 100), (int)(Y_OFFSET * Game1.GLOBAL_SIZE_MOD / 100), WIDTH, HEIGHT), Color.Silver);
                spriteBatch.Draw(texture, new Rectangle((int)(X_OFFSET * Game1.GLOBAL_SIZE_MOD / 100) + 5, (int)(Y_OFFSET * Game1.GLOBAL_SIZE_MOD / 100) + 5, WIDTH - 10, HEIGHT - 10), new Color(20, 20, 20));
                DrawStringCentered(0, "The end ?", Font32pt, Color.Red, spriteBatch);
                Vector2 firstStringSize = DrawStringCentered((int)(FIRST_Y * Game1.GLOBAL_SIZE_MOD / 100), "The wounds you have been inflicted would have been more than enough to kill any mere mortal.", Font18pt, Color.White, spriteBatch);
                int secondStringY = (int)(FIRST_Y * Game1.GLOBAL_SIZE_MOD / 100) + (int)firstStringSize.Y + ADDITIONAL_Y_MARGIN;
                Vector2 secondStringSize = DrawStringCentered(secondStringY, "But fortunately, thanks to the contract you passed with the devil before entering the dungeon, you are now immortal.", Font18pt, Color.White, spriteBatch);
                int thirdStringY = secondStringY + (int)secondStringSize.Y + ADDITIONAL_Y_MARGIN;
                Vector2 thirdStringSize = DrawStringCentered(thirdStringY, "However, each time you regenerate from otherwise fatal wounds, you must pay a dire price...", Font18pt, Color.White, spriteBatch);

                int startChoicesY = thirdStringY + (int)thirdStringSize.Y + CHOICES_Y_MARGIN;
                int firstBorder = (int)WIDTH / 3 * (int)Game1.GLOBAL_SIZE_MOD / 100;
                int secondBorder = 2 * firstBorder;

                Vector2 firstChoiceTitleSize = DrawStringCenteredCustom(0, startChoicesY, firstBorder, CurrentEffects[0].Name, Font32pt, Color.Red, spriteBatch);
                if (CurrentEffects.Count > 1)
                {
                    Vector2 secondChoiceTitleSize = DrawStringCenteredCustom(firstBorder, startChoicesY, secondBorder - firstBorder, CurrentEffects[1].Name, Font32pt, Color.Red, spriteBatch);
                }

                if (CurrentEffects.Count > 2)
                {
                    Vector2 thirdChoiceTitleSize = DrawStringCenteredCustom(secondBorder, startChoicesY, WIDTH - secondBorder, CurrentEffects[2].Name, Font32pt, Color.Red, spriteBatch);
                }

                int startDescY = startChoicesY + (int)firstChoiceTitleSize.Y + DESC_Y_MARGIN;
                Vector2 firstChoiceDescSize = DrawStringCenteredCustom(0, startDescY, firstBorder, CurrentEffects[0].Description, Font18pt, Color.White, spriteBatch);
                if (CurrentEffects.Count > 1)
                {
                    Vector2 secondChoiceDescSize = DrawStringCenteredCustom(firstBorder, startDescY, secondBorder - firstBorder, CurrentEffects[1].Description, Font18pt, Color.White, spriteBatch);
                }
                if (CurrentEffects.Count > 2)
                {
                    Vector2 thirdChoiceDescSize = DrawStringCenteredCustom(secondBorder, startDescY, WIDTH - secondBorder, CurrentEffects[2].Description, Font18pt, Color.White, spriteBatch);
                }

                int startSelectY = startDescY + (int)firstChoiceDescSize.Y + DESC_Y_MARGIN;

                Color[] selectColors = new Color[4]
                {
                Color.White,
                Color.White,
                Color.White,
                Color.White
                };

                if (SelectedIndex > -1 && SelectedIndex < 5)
                {
                    selectColors[SelectedIndex] = Color.Yellow;
                }

                Vector2 firstSelectPos;
                Vector2 secondSelectPos;
                Vector2 thirdSelectPos;

                Vector2 firstSelectSize = DrawStringCenteredCustom(0, startSelectY, firstBorder, "Choose", Font32pt, selectColors[0], spriteBatch, out firstSelectPos);

                Vector2 measuredString = Font32pt.MeasureString("Choose");
                if (CurrentEffects.Count > 2)
                {
                    Vector2 thirdSelectSize = DrawStringCenteredCustom(secondBorder, startSelectY, WIDTH - secondBorder, "Choose", Font32pt, selectColors[2], spriteBatch, out thirdSelectPos);
                    Choice3Select = new Rectangle((int)thirdSelectPos.X, (int)thirdSelectPos.Y, (int)measuredString.X, (int)measuredString.Y);
                }
                else
                {
                    Choice3Select = null;
                }


                Choice1Select = new Rectangle((int)firstSelectPos.X, (int)firstSelectPos.Y, (int)measuredString.X, (int)measuredString.Y);
                if (CurrentEffects.Count > 1)
                {
                    Vector2 secondSelectSize = DrawStringCenteredCustom(firstBorder, startSelectY, secondBorder - firstBorder, "Choose", Font32pt, selectColors[1], spriteBatch, out secondSelectPos);
                    Choice2Select = new Rectangle((int)secondSelectPos.X, (int)secondSelectPos.Y, (int)measuredString.X, (int)measuredString.Y);
                }
                else
                {
                    Choice2Select = null;
                }


                int startRerollY = startSelectY + (int)firstSelectSize.Y + CHOICES_Y_MARGIN + DESC_Y_MARGIN;
                Vector2 firstRerollSize = DrawStringCentered(startRerollY, "If you like neither of these three choices, you may choose to reroll them so as to get three new random choices", Font18pt, Color.White, spriteBatch);
                int secondRerollY = startRerollY + (int)firstRerollSize.Y + ADDITIONAL_Y_MARGIN;
                Vector2 secondRerollSize = DrawStringCentered(secondRerollY, "However, the number of rerolls you are allowed is limited and does not reset between ressurections. You should therefore use them with extreme caution", Font18pt, Color.White, spriteBatch);
                int thirdRerollY = secondRerollY + (int)secondRerollSize.Y + ADDITIONAL_Y_MARGIN;
                Vector2 thirdRerollSize = DrawStringCentered(thirdRerollY, "Rerolls left : " + GameManager.PlayerObject.Player.Rerolls.ToString(), Font18pt, Color.White, spriteBatch);

                Vector2 rerollSelectPos;

                int startRerollSelectY = thirdRerollY + (int)thirdRerollSize.Y + DESC_Y_MARGIN;
                Vector2 rerollSelectSize = DrawStringCentered(startRerollSelectY, "Reroll", Font32pt, selectColors[3], spriteBatch, out rerollSelectPos);

                RerollSelect = new Rectangle((int)rerollSelectPos.X, (int)rerollSelectPos.Y, (int)rerollSelectSize.X, (int)rerollSelectSize.Y);
            }
        }

        public static Vector2 DrawStringInDeathScreen(int x, int y, string text, SpriteFont font, Color color, SpriteBatch spriteBatch, out Vector2 pos)
        {
            int actualX = (int)((x + X_OFFSET)*Game1.GLOBAL_SIZE_MOD / 100);
            int actualY = (int)((y + Y_OFFSET)*Game1.GLOBAL_SIZE_MOD / 100);

            pos = new Vector2(actualX, actualY);

            spriteBatch.DrawString(font, text, new Vector2(actualX, actualY), color);
            return font.MeasureString(text);
        }

        public static Vector2 DrawStringInDeathScreen(int x, int y, string text, SpriteFont font, Color color, SpriteBatch spriteBatch)
        {
            Vector2 temp = new Vector2();
            return DrawStringInDeathScreen(x, y, text, font, color, spriteBatch, out temp);
        }

        public static Vector2 DrawStringCentered(int y, string text, SpriteFont font, Color color, SpriteBatch spriteBatch, out Vector2 pos)
        {
            Vector2 measuredString = font.MeasureString(text);
            int x = (int)((WIDTH - measuredString.X) / 2);
            return DrawStringInDeathScreen(x, y, text, font, color, spriteBatch, out pos);
        }

        public static Vector2 DrawStringCentered(int y, string text, SpriteFont font, Color color, SpriteBatch spriteBatch)
        {
            Vector2 temp = new Vector2();
            return DrawStringCentered(y, text, font, color, spriteBatch, out temp);
        }

        public static Vector2 DrawStringCenteredCustom(int xOffset, int y, int width, string text, SpriteFont font, Color color, SpriteBatch spriteBatch, out Vector2 pos)
        {
            string finalText = WrapText(text, font, width);
            Vector2 measuredString = font.MeasureString(finalText);
            int x = (int)((width - measuredString.X) / 2) + xOffset;
            return DrawStringInDeathScreen(x, y, finalText, font, color, spriteBatch, out pos);
        }

        public static Vector2 DrawStringCenteredCustom(int xOffset, int y, int width, string text, SpriteFont font, Color color, SpriteBatch spriteBatch)
        {
            Vector2 temp = new Vector2();
            return DrawStringCenteredCustom(xOffset, y, width, text, font, color, spriteBatch, out temp);
        }

        private static string WrapText(string text, SpriteFont font, int MaxLineWidth)
        {
            if (font.MeasureString(text).X < MaxLineWidth)
            {
                return text;
            }

            string[] words = text.Split(' ');
            StringBuilder wrappedText = new StringBuilder();
            float linewidth = 0f;
            float spaceWidth = font.MeasureString(" ").X;
            for (int i = 0; i < words.Length; ++i)
            {
                Vector2 size = font.MeasureString(words[i]);
                if (linewidth + size.X < MaxLineWidth)
                {
                    linewidth += size.X + spaceWidth;
                }
                else
                {
                    wrappedText.Append("\n");
                    linewidth = size.X + spaceWidth;
                }
                wrappedText.Append(words[i]);
                wrappedText.Append(" ");
            }

            return wrappedText.ToString();
        }
        #endregion
        public static void Reroll()
        {
            if (GameManager.PlayerObject.Player.Rerolls > 0)
            {
                GameManager.PlayerObject.Player.Rerolls -= 1;
                CurrentEffects = RollEffects();
            }
        }
        public static void OnLeftClick()
        {
            if (SelectedIndex == 3)
            {
                Reroll();
                Game1.PlaySFX("HUD_Click_01");
            }
            else if (SelectedIndex > -1)
            {
                CurrentEffects[SelectedIndex].Apply();
                GameManager.PlayerObject.Fighter.HP = GameManager.PlayerObject.Fighter.MaxHP;
                GameManager.PlayerObject.Player.MP = GameManager.PlayerObject.Player.MaxMP;
                GameManager.PlayerObject.Player.FreeTP = true;
                InputManager.SelectedSpellIndex = 4;
                GameManager.CurrentState = Playing;
                Game1.PlaySFX("HUD_Click_01");
            }
        }
    }
}
