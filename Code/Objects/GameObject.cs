﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using TheEternalOne.Code.Objects.Mobs;
using TheEternalOne.Code.Game;
using TheEternalOne.Code.Objects.Items;
using TheEternalOne.Code.Objects.Equipments;

namespace TheEternalOne.Code.Objects
{
    public class GameObject
    {
        public Coord Position { get; set; }
        public Coord OffsetPos { get; set; }
        public Coord BigPos { get; set; }

        public int Speed = 1;

        public List<Game.Effect> Effects { get; set; }

        public int x
        {
            get
            {
                return Position.x;
            }
            set
            {
                Position = new Coord(value, Position.y);
            }
        }

        public int y
        {
            get
            {
                return Position.y;
            }
            set
            {
                Position = new Coord(Position.x, value);
            }
        }

        public Player Player { get; set; }
        private Fighter _fighter;
        public Fighter Fighter
        {
            get
            {
                return _fighter;
            }
            set
            {
                _fighter = value;
                
                if (value != null) _fighter.Owner = this;
            }
        }

        private I_AI _ai;
        public I_AI AI
        {
            get
            {
                return _ai;
            }
            set
            {
                _ai = value;
                if (value != null) _ai.Owner = this;
            }
        }

        private Item _item;
        public Item Item
        {
            get
            {
                return _item;
            }
            set
            {
                _item = value;
                _item.Owner = this;
            }
        }

        private Equipment _equipment;
        public Equipment Equipment
        {
            get
            {
                return _equipment;
            }
            set
            {
                _equipment = value;
                _equipment.Owner = this;
            }
        }
        public string Name { get; set; }

        public Texture2D texture;
        public int textureWidth;
        public int textureHeight;

        public bool isStairs = false;

        int textureOffsetX;
        int textureOffsetY;

        public bool dieOnEffectsEnd = false;

        public GameObject(int x, int y, string textureString, int textureW, int textureH, int textureX = 0, int textureY = 0)
        {
            this.Position = new Coord(x, y);
            this.OffsetPos = new Coord(x * (int)(GameManager.TileWidth * Game1.GLOBAL_SIZE_MOD / 100), y * (int)(GameManager.TileWidth * Game1.GLOBAL_SIZE_MOD / 100));
            this.BigPos = new Coord(x * (int)(GameManager.TileWidth * Game1.GLOBAL_SIZE_MOD / 100), y * (int)(GameManager.TileWidth * Game1.GLOBAL_SIZE_MOD / 100));

            texture = Game1.textureDict[textureString];
            textureWidth = textureW;
            textureHeight = textureH;

            textureOffsetX = (int)(textureX * Game1.GLOBAL_SIZE_MOD / 100f);
            textureOffsetY = (int)(textureY * Game1.GLOBAL_SIZE_MOD / 100f);

            Effects = new List<Game.Effect>();
        }

        public void Move(int dx, int dy)
        {
            if (!GameManager.Map[x + dx, y + dy].Blocked)
            {
                GameObject foundObject = null;
                if (Fighter != null)
                {
                    foreach (GameObject gameObject in GameManager.Objects)
                    {
                        if (gameObject.Fighter != null && gameObject.Position.x == x + dx && gameObject.Position.y == y + dy)
                        {
                            foundObject = gameObject;
                            break;
                        }
                    }
                }
                if (foundObject != null)
                {
                    if (Player == null)
                    {
                        Fighter.Attack(foundObject.Fighter);
                    }
                    else
                    {
                        Player.Cast(0, foundObject.Position.x, foundObject.Position.y);
                    }
                }
                else
                {
                    Position = new Coord(Position.x + dx, Position.y + dy);
                    //Console.Out.WriteLine(x.ToString() + ";" + y.ToString());
                    BigPos = new Coord(Position.x * (int)(GameManager.TileWidth * Game1.GLOBAL_SIZE_MOD / 100), Position.y * (int)(GameManager.TileWidth * Game1.GLOBAL_SIZE_MOD / 100));
                }
            }
        }

        public void MoveTo(int nx, int ny)
        {
            if (!GameManager.Map[nx, ny].Blocked)
            {
                GameObject foundObject = null;
                if (Fighter != null)
                {
                    foreach (GameObject gameObject in GameManager.Objects)
                    {
                        if (gameObject.Fighter != null && gameObject.Position.x == nx && gameObject.Position.y == ny)
                        {
                            foundObject = gameObject;
                            break;
                        }
                    }
                }
                if (foundObject != null)
                {
                    Fighter.Attack(foundObject.Fighter);
                }
                else
                {
                    Position = new Coord(nx, ny);
                    //Console.Out.WriteLine(x.ToString() + ";" + y.ToString());
                    BigPos = new Coord(Position.x * (int)(GameManager.TileWidth * Game1.GLOBAL_SIZE_MOD / 100), Position.y * (int)(GameManager.TileWidth * Game1.GLOBAL_SIZE_MOD / 100));
                }
            }
        }

        public void MoveTo(Coord coord)
        {
            int nx = coord.x;
            int ny = coord.y;
            if (!GameManager.Map[nx, ny].Blocked)
            {
                GameObject foundObject = null;
                if (Fighter != null)
                {
                    foreach (GameObject gameObject in GameManager.Objects)
                    {
                        if (gameObject.Fighter != null && gameObject.Position.x == nx && gameObject.Position.y == ny)
                        {
                            foundObject = gameObject;
                            break;
                        }
                    }
                }
                if (foundObject != null)
                {
                    Fighter.Attack(foundObject.Fighter);
                }
                else
                {
                    Position = new Coord(nx, ny);
                    //Console.Out.WriteLine(x.ToString() + ";" + y.ToString());
                    BigPos = new Coord(Position.x * (int)(GameManager.TileWidth * Game1.GLOBAL_SIZE_MOD / 100), Position.y * (int)(GameManager.TileWidth * Game1.GLOBAL_SIZE_MOD / 100));
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch, int px, int py)
        {
            int offsetX;
            int offsetY;
            if (Player != null)
            {
                offsetX = 0;
                offsetY = 0;
            }
            else
            {
                offsetX = OffsetPos.x - BigPos.x - GameManager.PlayerObject.OffsetPos.x + GameManager.PlayerObject.BigPos.x;
                offsetY = OffsetPos.y - BigPos.y - GameManager.PlayerObject.OffsetPos.y + GameManager.PlayerObject.BigPos.y;
            }

            int x = (GameManager.screenPlayerX + Position.x - px) * (int)(GameManager.TileWidth * Game1.GLOBAL_SIZE_MOD / 100) + GameManager.DrawMapX + (GameManager.TileWidth - textureWidth) * (int)(Game1.GLOBAL_SIZE_MOD / 100) / 2 + offsetX + textureOffsetX;
            int y = (GameManager.screenPlayerY + Position.y - py) * (int)(GameManager.TileWidth * Game1.GLOBAL_SIZE_MOD / 100) + GameManager.DrawMapY + (GameManager.TileWidth - textureHeight) * (int)(Game1.GLOBAL_SIZE_MOD / 100) + offsetY + textureOffsetY; //- GameManager.feetOffset  ;

            //Vector2 position = new Vector2(x ?? default(int), y ?? default(int)); // The statement var1 = var2 ?? var3 assigns the value var2 to var1 if var2 is not null, otherwise it assigns var3.

            if (!dieOnEffectsEnd)
            {
                spriteBatch.Draw(texture, new Rectangle(x, y, (int)(textureWidth * Game1.GLOBAL_SIZE_MOD / 100), (int)(textureHeight * Game1.GLOBAL_SIZE_MOD / 100)), Color.White);
            }

            foreach (Game.Effect eff in Effects)
            {
                eff.Draw(spriteBatch, x + 20, y - 50);
            }
        }

        public void Update()
        {
            if (dieOnEffectsEnd && Fighter != null) Fighter = null;
            if (dieOnEffectsEnd && AI != null) AI = null;

            if (OffsetPos.x != BigPos.x || OffsetPos.y != BigPos.y)
            {
                int dx = 0, dy = 0;
                if (OffsetPos.x < BigPos.x) dx = 5;
                else if (OffsetPos.x > BigPos.x) dx = -5;

                if (OffsetPos.y < BigPos.y) dy = 5;
                else if (OffsetPos.y > BigPos.y) dy = -5;

                OffsetPos = new Coord(OffsetPos.x + (dx * Speed) , OffsetPos.y + (dy * Speed));
            }

            List<Game.Effect> toRemove = new List<Game.Effect>();
            foreach (Game.Effect eff in Effects)
            {
                eff.Update();
                if (eff.TimeLeft <= 0) toRemove.Add(eff);
            }
            foreach (Game.Effect eff in toRemove)
            {
                Effects.Remove(eff);
            }
            if (dieOnEffectsEnd && Effects.Count <= 0) GameManager.ToRemove.Add(this);
        }

        public void AddEffect(Game.Effect effect)
        {
            effect.Offset = 30 * Effects.Count;
            Effects.Add(effect);
        }
    }
}
