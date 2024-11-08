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
    enum Direction
    {
        Left,
        Right,
        Up,
        Down
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
        private static bool IsRunning(KeyboardState keyboardState)
        {
            if (keyboardState.GetPressedKeys().Contains(Keys.LeftShift))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        private static bool CanMove(Direction direction)
        {
            switch (direction)
            {
                case Direction.Left:
                    if (Player.Position.X - 1 < - 64)
                    {
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                case Direction.Right:
                    if (Player.Position.X + 1 > (Map.Columns * Map.BlockSize) - 64)
                    {
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                case Direction.Up:
                    if (Player.Position.Y - 1 < - 64)
                    {
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                case Direction.Down:
                    if (Player.Position.Y + 1 > (Map.Rows * Map.BlockSize) - 120)
                    {
                        return false;
                    }
                    else
                    {
                        return true;
                    }
            }
            return false;
        }
        public static void Update(GameTime gameTime)
        {
            KeyboardState _keyboardState = Keyboard.GetState();
            if (_keyboardState.GetPressedKeys().Length != 0)
            {
                if (_keyboardState.IsKeyDown(Keys.W) && Player.CanMove(Direction.Up))
                {
                    Tile prevTile = Player.CurrentTile;
                    float FelsoFal = Player.CurrentTile.Position.Y - 64;
                    if (Player.Position.Y < FelsoFal)
                    {
                        Player.CurrentTile = Map.Map_Normal[Player.CurrentTile.Get_Y() - 1, Player.CurrentTile.Get_X()];
                    }

                    if (Player.CurrentTile.Type == Type.Wall)
                    {
                        Player.CurrentTile = prevTile;
                    }
                    else
                    {
                        if (IsRunning(_keyboardState))
                        {
                            Player.Position.Y -= Speed * 2;
                            Player.CurrentAction = Action.Run_Left;
                        }
                        else
                        {
                            Player.Position.Y -= Speed;
                            Player.CurrentAction = Action.Walk_Left;
                        }
                    }
                }
                if (_keyboardState.IsKeyDown(Keys.A) && Player.CanMove(Direction.Left))
                {
                    Tile prevTile = Player.CurrentTile;
                    float BalFal = Player.CurrentTile.Position.X - 64;
                    if (Player.Position.X < BalFal)
                    {
                        Player.CurrentTile = Map.Map_Normal[Player.CurrentTile.Get_Y(), Player.CurrentTile.Get_X() - 1];
                    }

                    if (Player.CurrentTile.Type == Type.Wall)
                    {
                        Player.CurrentTile = prevTile;
                    }
                    else
                    {
                        if (IsRunning(_keyboardState))
                        {
                            Player.Position.X -= Speed * 2;
                            Player.CurrentAction = Action.Run_Left;
                        }
                        else
                        {
                            Player.Position.X -= Speed;
                            Player.CurrentAction = Action.Walk_Left;
                        }
                    }
                }
                if (_keyboardState.IsKeyDown(Keys.S) && Player.CanMove(Direction.Down))
                {
                    Tile prevTile = Player.CurrentTile;
                    float AlsoFal = Player.CurrentTile.Position.Y - 64;
                    if (Player.Position.Y > AlsoFal)
                    {
                        Player.CurrentTile = Map.Map_Normal[Player.CurrentTile.Get_Y() + 1, Player.CurrentTile.Get_X()];
                    }

                    if (Player.CurrentTile.Type == Type.Wall)
                    {
                        Player.CurrentTile = prevTile;
                    }
                    else
                    {
                        if (IsRunning(_keyboardState))
                        {
                            Player.Position.Y += Speed * 2;
                            Player.CurrentAction = Action.Run_Left;
                        }
                        else
                        {
                            Player.Position.Y += Speed;
                            Player.CurrentAction = Action.Walk_Left;
                        }
                    }
                }
                if (_keyboardState.IsKeyDown(Keys.D) && Player.CanMove(Direction.Right))
                {
                    Tile prevTile = Player.CurrentTile;
                    float JobbFal = Player.CurrentTile.Position.X;
                    if (Player.Position.X > JobbFal)
                    {
                        Player.CurrentTile = Map.Map_Normal[Player.CurrentTile.Get_Y(), Player.CurrentTile.Get_X() + 1];
                    }

                    if (Player.CurrentTile.Type == Type.Wall)
                    {
                        Player.CurrentTile = prevTile;
                    }
                    else
                    {
                        if (IsRunning(_keyboardState))
                        {
                            Player.Position.X += Speed * 2;
                            Player.CurrentAction = Action.Run_Right;
                        }
                        else
                        {
                            Player.Position.X += Speed;
                            Player.CurrentAction = Action.Walk_Right;
                        }
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
        }
        public static void Draw(SpriteBatch _spriteBatch)
        {
            //spriteBatch.DrawString(gameFont, "Text", new Vector2(0, 0), Color.Black);

            _spriteBatch.Draw(Player.Textures[Player.CurrentAction], Player.Position, Color.White);
        }
    }
}
