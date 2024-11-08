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
        public static int Pos_I = 0;
        public static int Pos_J = 0;
        public static string Coordinates = string.Empty;

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

                            float FelsoFal = Player.CurrentTile.Position.Y - 64;
                            if (Player.Position.Y < FelsoFal)
                            {
                                DebugDump.Dump($"w");

                                int i = (int)(Player.CurrentTile.Position.Y / 64);
                                int j = (int)(Player.CurrentTile.Position.X / 64);


                                //Player.CurrentTile = Map.Map_Normal[Player.Pos_I, Player.Pos_J];
                                Player.CurrentTile = Map.Map_Normal[i - 1, j];
                                Coordinates = $"I: {i}, J: {j}";
                            }
                        }
                        if (_keyboardState.IsKeyDown(Keys.A))
                        {
                            Player.Position.X -= Speed * 2;
                            Player.CurrentAction = Action.Run_Left;

                            float BalFal = Player.CurrentTile.Position.X - 64;
                            if (Player.Position.X < BalFal)
                            {
                                DebugDump.Dump($"a");


                                int i = (int)(Player.CurrentTile.Position.Y / 64);
                                int j = (int)(Player.CurrentTile.Position.X / 64);

                                //Player.CurrentTile = Map.Map_Normal[Player.Pos_I, Player.Pos_J];
                                Player.CurrentTile = Map.Map_Normal[i, j - 1];
                                Coordinates = $"I: {i}, J: {j}";

                            }
                        }
                        if (_keyboardState.IsKeyDown(Keys.S))
                        {
                            Player.Position.Y += Speed * 2;
                            Player.CurrentAction = Action.Run_Left;

                            float AlsoFal = Player.CurrentTile.Position.Y;
                            if (Player.Position.Y > AlsoFal)
                            {

                                int i = (int)(Player.CurrentTile.Position.Y / 64);
                                int j = (int)(Player.CurrentTile.Position.X / 64);

                                DebugDump.Dump($"s");


                                Player.Pos_I++;
                                //Player.CurrentTile = Map.Map_Normal[Player.Pos_I, Player.Pos_J];
                                Player.CurrentTile = Map.Map_Normal[i + 1, j];
                                Coordinates = $"I: {i}, J: {j}";

                            }
                        }
                        if (_keyboardState.IsKeyDown(Keys.D))
                        {
                            Player.Position.X += Speed * 2;
                            Player.CurrentAction = Action.Run_Right;

                            float JobbFal = Player.CurrentTile.Position.X;
                            if (Player.Position.X > JobbFal)
                            {
                                int i = (int)(Player.CurrentTile.Position.Y / 64);
                                int j = (int)(Player.CurrentTile.Position.X / 64);

                                DebugDump.Dump($"d");


                                Player.Pos_J++;
                                //Player.CurrentTile = Map.Map_Normal[Player.Pos_I, Player.Pos_J];
                                Player.CurrentTile = Map.Map_Normal[i, j + 1];
                                i = (int)(Player.CurrentTile.Position.Y / 64);
                                j = (int)(Player.CurrentTile.Position.X / 64);


                                Coordinates = $"I: {i}, J: {j}";

                            }
                        }
                    }
                }
                else
                {
                    if (_keyboardState.IsKeyDown(Keys.W))
                    {
                        Player.Position.Y -= Speed;
                        Player.CurrentAction = Action.Walk_Left;

                        float FelsoFal = Player.CurrentTile.Position.Y - 64;
                        if (Player.Position.Y < FelsoFal)
                        {
                            DebugDump.Dump($"w");

                            int i = (int)(Player.CurrentTile.Position.Y / 64);
                            int j = (int)(Player.CurrentTile.Position.X / 64);
                            Player.Pos_I--;
                            //Player.CurrentTile = Map.Map_Normal[Player.Pos_I, Player.Pos_J];
                            Player.CurrentTile = Map.Map_Normal[i - 1, j];

                            Coordinates = $"I: {i}, J: {j}";
                        }
                    }
                    if (_keyboardState.IsKeyDown(Keys.A))
                    {
                        Player.Position.X -= Speed;
                        Player.CurrentAction = Action.Walk_Left;

                        float BalFal = Player.CurrentTile.Position.X - 64;
                        if (Player.Position.X < BalFal)
                        {
                            DebugDump.Dump("a");

                            int i = (int)(Player.CurrentTile.Position.Y / 64);
                            int j = (int)(Player.CurrentTile.Position.X / 64);
                            Player.Pos_J--;
                            //Player.CurrentTile = Map.Map_Normal[Player.Pos_I, Player.Pos_J];
                            Player.CurrentTile = Map.Map_Normal[i, j - 1];
                            Coordinates = $"I: {i}, J: {j}";


                        }
                    }
                    if (_keyboardState.IsKeyDown(Keys.S))
                    {
                        Player.Position.Y += Speed;
                        Player.CurrentAction = Action.Walk_Left;

                        float AlsoFal = Player.CurrentTile.Position.Y;
                        if (Player.Position.Y > AlsoFal)
                        {
                            DebugDump.Dump("s");

                            int i = (int)(Player.CurrentTile.Position.Y / 64);
                            int j = (int)(Player.CurrentTile.Position.X / 64);
                            Player.Pos_I++;
                            //Player.CurrentTile = Map.Map_Normal[Player.Pos_I, Player.Pos_J];
                            Player.CurrentTile = Map.Map_Normal[i + 1, j];
                            Coordinates = $"I: {i}, J: {j}";

                        }
                    }
                    if (_keyboardState.IsKeyDown(Keys.D))
                    {
                        Player.Position.X += Speed;
                        Player.CurrentAction = Action.Walk_Right;

                        float JobbFal = Player.CurrentTile.Position.X;
                        if (Player.Position.X > JobbFal)
                        {
                            DebugDump.Dump("d");


                            int i = (int)(Player.CurrentTile.Position.Y / 64);
                            int j = (int)(Player.CurrentTile.Position.X / 64);

                            Player.Pos_J++;
                            //Player.CurrentTile = Map.Map_Normal[Player.Pos_I, Player.Pos_J];
                            Player.CurrentTile = Map.Map_Normal[i, j + 1];
                            i = (int)(Player.CurrentTile.Position.Y / 64);
                            j = (int)(Player.CurrentTile.Position.X / 64);
                            Coordinates = $"I: {i}, J: {j}";

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
            //MapWidth = Map.BlockSize * MapColumn
            //MapHeight = Map.BlockSize * MapHeight

            //MapWidth / Player.Position.X => i
            //MapHeight / Player.Position.Y => j
            //Normal[i,j]

        }
    }
}
