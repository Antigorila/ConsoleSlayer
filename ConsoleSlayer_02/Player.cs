using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleSlayer_02
{
    enum Action
    {
        Dead,
        Idle,
        Run_Left,
        Run_Right,
        Shot_Left,
        Shot_Right,
        Walk_Left,
        Walk_Right,
    }
    internal class Player
    {
        //TODO: Do a ui, add hp and stuff like this
        public static Vector2 Position;
        public static bool IsDead = false;
        public static int HP = 100;
        public static Dictionary<Action, Texture2D> Textures = new Dictionary<Action, Texture2D>();
        public static Action CurrentAction = Action.Idle;
        public static int Ammo = 45;
        private static KeyboardState previousKeyboardState = new KeyboardState();
        public static float Speed = 1f;
        public static Tile CurrentTile;

        public static void Die()
        {
            IsDead = true;
        }
        public static void LoadTextures(ContentManager Content)
        {
            Player.Textures.Add(Action.Dead, Content.Load<Texture2D>("Dead"));
            Player.Textures.Add(Action.Idle, Content.Load<Texture2D>("Idle"));
            Player.Textures.Add(Action.Run_Left, Content.Load<Texture2D>("Run_Left"));
            Player.Textures.Add(Action.Run_Right, Content.Load<Texture2D>("Run_Right"));
            Player.Textures.Add(Action.Shot_Left, Content.Load<Texture2D>("Shot_Left"));
            Player.Textures.Add(Action.Shot_Right, Content.Load<Texture2D>("Shot_Right"));
            Player.Textures.Add(Action.Walk_Left, Content.Load<Texture2D>("Walk_Left"));
            Player.Textures.Add(Action.Walk_Right, Content.Load<Texture2D>("Walk_Right"));
        }
        public static void Update(GameTime gameTime)
        {
            KeyboardState _keyboardState = Keyboard.GetState();
            if (_keyboardState.GetPressedKeys().Length != 0)
            {
                if (_keyboardState.GetPressedKeys().Contains(Keys.LeftShift))
                {
                    if (_keyboardState.GetPressedKeys().Length == 1)
                    {
                        Player.CurrentAction = Action.Idle;
                    }
                    else
                    {
                        if (_keyboardState.IsKeyDown(Keys.W))
                        {
                            Player.Position.Y -= Speed * 2;
                            Player.CurrentAction = Action.Run_Left;
                        }
                        if (_keyboardState.IsKeyDown(Keys.A))
                        {
                            Player.Position.X -= Speed * 2;
                            Player.CurrentAction = Action.Run_Left;
                        }
                        if (_keyboardState.IsKeyDown(Keys.S))
                        {
                            Player.Position.Y += Speed * 2;
                            Player.CurrentAction = Action.Run_Left;
                        }
                        if (_keyboardState.IsKeyDown(Keys.D))
                        {
                            Player.Position.X += Speed * 2;
                            Player.CurrentAction = Action.Run_Right;
                        }
                    }
                }
                else
                {
                    if (_keyboardState.IsKeyDown(Keys.W))
                    {
                        Player.Position.Y -= Speed;
                        Player.CurrentAction = Action.Walk_Left;
                    }
                    if (_keyboardState.IsKeyDown(Keys.A))
                    {
                        Player.Position.X -= Speed;
                        Player.CurrentAction = Action.Walk_Left;
                    }
                    if (_keyboardState.IsKeyDown(Keys.S))
                    {
                        Player.Position.Y += Speed;
                        Player.CurrentAction = Action.Walk_Left;
                    }
                    if (_keyboardState.IsKeyDown(Keys.D))
                    {
                        Player.Position.X += Speed;
                        Player.CurrentAction = Action.Walk_Right;
                    }
                }

                if (_keyboardState.IsKeyDown(Keys.Space) && !previousKeyboardState.IsKeyDown(Keys.Space))
                {
                    Player.Ammo--;
                    if (_keyboardState.IsKeyDown(Keys.D))
                    {
                        Player.CurrentAction = Action.Shot_Right;
                    }
                    else
                    {
                        Player.CurrentAction = Action.Shot_Left;
                    }
                }
            }
            else
            {
                Player.CurrentAction = Action.Idle;
            }
            previousKeyboardState = _keyboardState;

            if (Map.Map_Normal != null)
            {
                SetCurrentTileType(Map.Map_Normal, Player.Position, Map.BlockSize);
            }
        }
        public static void Draw(SpriteBatch _spriteBatch)
        {
            //spriteBatch.DrawString(gameFont, "Text", new Vector2(0, 0), Color.Black);

            _spriteBatch.Draw(Player.Textures[Player.CurrentAction], Player.Position, Color.White);
        }
        public static void SetCurrentTileType(Tile[,] map, Vector2 playerPosition, float blockSize)
        {
            int tileX = (int)(playerPosition.X / blockSize);
            int tileY = (int)(playerPosition.Y / blockSize);

            if (tileX >= 0 && tileX < map.GetLength(0) && tileY >= 0 && tileY < map.GetLength(1))
            {
                Player.CurrentTile = map[tileX, tileY];
            }
            else
            {
                Player.CurrentTile = null;
            }
        }
    }
}
