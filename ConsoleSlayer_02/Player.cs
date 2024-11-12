using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

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
        public static Vector2 Position;
        public static bool IsDead = false;
        public static int HP = 100;
        public static Dictionary<Action, Texture2D> Textures = new Dictionary<Action, Texture2D>();
        public static Action CurrentAction = Action.Idle;
        public static int Ammo = 45;
        private static KeyboardState previousKeyboardState = new KeyboardState();
        public static float Speed = 1f;
        public static Tile CurrentTile;
        public static int Damage = 10;
        private static double messageDisplayTime = 1.0;
        private static double messageTimer = 0.0;
        private static bool showMessage = false;
        private static string currentMessage = string.Empty;

        #region GameMethods
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
            if (Player.IsDead)
            {
                Player.CurrentAction = Action.Dead;
            }
            else
            {
                KeyboardState _keyboardState = Keyboard.GetState();

                if (_keyboardState.GetPressedKeys().Length != 0)
                {
                    if (_keyboardState.IsKeyDown(Keys.W) && CanMove(Direction.Up))
                    {
                        MovePlayer(Direction.Up, _keyboardState);
                    }
                    if (_keyboardState.IsKeyDown(Keys.S) && CanMove(Direction.Down))
                    {
                        MovePlayer(Direction.Down, _keyboardState);
                    }
                    if (_keyboardState.IsKeyDown(Keys.A) && CanMove(Direction.Left))
                    {
                        MovePlayer(Direction.Left, _keyboardState);
                    }
                    if (_keyboardState.IsKeyDown(Keys.D) && CanMove(Direction.Right))
                    {
                        MovePlayer(Direction.Right, _keyboardState);
                    }

                    if (_keyboardState.IsKeyDown(Keys.Space) && !previousKeyboardState.IsKeyDown(Keys.Space))
                    {
                        Shoot(_keyboardState);
                    }
                }
                else
                {
                    Player.CurrentAction = Action.Idle;
                }

                if (Player.CurrentTile != null)
                {
                    //You can add here how the player character should react when it touches other tiles
                    switch (Player.CurrentTile.Type)
                    {
                        case Type.None:
                            break;
                        case Type.Decor:
                            break;
                        case Type.Wall:
                            break;
                        case Type.Road:
                            break;
                        case Type.Spawn:
                            break;
                        case Type.Finish:
                            break;
                        case Type.Lava:
                            //Player.TakeDamage(1);
                            break;
                        case Type.Gate:
                            break;
                        case Type.Pickup:
                            break;
                    }
                }

                if (showMessage)
                {
                    messageTimer += gameTime.ElapsedGameTime.TotalSeconds;
                    if (messageTimer >= messageDisplayTime)
                    {
                        showMessage = false;
                        messageTimer = 0.0;
                    }
                }

                previousKeyboardState = _keyboardState;
            }
        }
        public static void DrawTransform(SpriteBatch _spriteBatch)
        {
            //spriteBatch.DrawString(gameFont, "Text", new Vector2(0, 0), Color.Black);

            _spriteBatch.Draw(Player.Textures[Player.CurrentAction], Player.Position, Color.White);
        }
        public static void Draw(SpriteBatch _spriteBatch, SpriteFont Font, GraphicsDevice GraphicsDevice)
        {
            //spriteBatch.DrawString(gameFont, "Text", new Vector2(0, 0), Color.Black);

            if (Player.IsDead)
            {
                GraphicsDevice.Clear(Color.Black);
                _spriteBatch.DrawString(Font, "Duty ends only in death...\nAnd so your duty has come to an end now...", new Vector2(10, 10), Color.White);
            }
            else
            {
                #region Draw "HUD"
                int hudHeight = Font.LineSpacing + 10;
                Rectangle hudBackground = new Rectangle(0, GraphicsDevice.Viewport.Height - hudHeight, GraphicsDevice.Viewport.Width, hudHeight);
                Texture2D blackTexture = new Texture2D(GraphicsDevice, 1, 1);
                blackTexture.SetData(new[] { Color.Black });
                _spriteBatch.Draw(blackTexture, hudBackground, Color.Black * 0.6f);
                #endregion

                if (showMessage)
                {
                    string text = currentMessage;
                    Vector2 textSize = Font.MeasureString(text);
                    Vector2 position = new Vector2((GraphicsDevice.Viewport.Width - textSize.X) / 2, (GraphicsDevice.Viewport.Height - textSize.Y) / 2);

                    _spriteBatch.DrawString(Font, text, position, Color.White);
                }

                _spriteBatch.DrawString(Font, "Ammo: " + Player.Ammo, new Vector2(5, GraphicsDevice.Viewport.Height - Font.LineSpacing), Color.White);
                _spriteBatch.DrawString(Font, "Health: " + Player.HP, new Vector2(150, GraphicsDevice.Viewport.Height - Font.LineSpacing), Color.White);
                _spriteBatch.DrawString(Font, Player.Position.ToString(), new Vector2(350, GraphicsDevice.Viewport.Height - Font.LineSpacing), Color.White);
            }
        }
        #endregion
        #region Move Player
        private static bool CanMove(Direction direction)
        {
            switch (direction)
            {
                case Direction.Left:
                    if (Player.Position.X - 1 < -64)
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
                    if (Player.Position.Y - 1 < -64)
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
        private static void MovePlayer(Direction direction, KeyboardState keyboardState)
        {
            Tile prevTile = Player.CurrentTile;
            float speed = IsRunning(keyboardState) ? Speed * 2 : Speed;
            Vector2 newPosition = Player.Position;

            switch (direction)
            {
                case Direction.Up:
                    newPosition.Y -= speed;
                    if (Player.Position.Y < Player.CurrentTile.Position.Y - 64)
                    {
                        Player.CurrentTile = Map.Map_Normal[Player.CurrentTile.Get_Y() - 1, Player.CurrentTile.Get_X()];
                    }
                    break;

                case Direction.Down:
                    newPosition.Y += speed;
                    if (Player.Position.Y > Player.CurrentTile.Position.Y - 64)
                    {
                        try
                        {
                            Player.CurrentTile = Map.Map_Normal[Player.CurrentTile.Get_Y() + 1, Player.CurrentTile.Get_X()];
                        }
                        catch (System.IndexOutOfRangeException)
                        {
                            Player.CurrentTile = prevTile;
                        }
                    }
                    break;

                case Direction.Left:
                    newPosition.X -= speed;
                    if (Player.Position.X < Player.CurrentTile.Position.X - 64)
                    {
                        Player.CurrentTile = Map.Map_Normal[Player.CurrentTile.Get_Y(), Player.CurrentTile.Get_X() - 1];
                    }
                    break;

                case Direction.Right:
                    newPosition.X += speed;
                    if (Player.Position.X > Player.CurrentTile.Position.X)
                    {
                        Player.CurrentTile = Map.Map_Normal[Player.CurrentTile.Get_Y(), Player.CurrentTile.Get_X() + 1];
                    }
                    break;
            }

            if (Player.CurrentTile.Type == Type.Wall)
            {
                Player.CurrentTile = prevTile;
            }
            else
            {
                Player.Position = newPosition;
                UpdateAction(direction, IsRunning(keyboardState));
            }
        }
        #endregion
        #region CharacterMethods
        public static void Die()
        {
            IsDead = true;
        }
        private static void TakeDamage(int ammount)
        {
            if (Player.HP - ammount > 0)
            {
                Player.HP -= ammount;
            }
            else
            {
                Player.Die();
            }
        }

        private static void Shoot(KeyboardState keyboardState)
        {
            if (Ammo > 0)
            {
                Player.Ammo--;
                float range = 64 * 3;

                if (keyboardState.IsKeyDown(Keys.D))
                {
                    for (int i = 0; i < DemonController.Demons.Count; i++)
                    {
                        var demon = DemonController.Demons[i];

                        if (!demon.IsDead)
                        {
                            float distanceX = Math.Abs(demon.Position.X - Player.Position.X);
                            float distanceY = Math.Abs(demon.Position.Y - Player.Position.Y);

                            if (distanceX <= range && distanceY <= range)
                            {
                                demon.TakeDamage(Player.Damage);
                            }
                        }
                    }
                    Player.CurrentAction = Action.Shot_Right;
                }
                else
                {
                    for (int i = 0; i < DemonController.Demons.Count; i++)
                    {
                        var demon = DemonController.Demons[i];

                        if (!demon.IsDead)
                        {
                            float distanceX = Player.Position.X - demon.Position.X;
                            float distanceY = Math.Abs(demon.Position.Y - Player.Position.Y);

                            if (distanceX >= 0 && distanceX <= range && distanceY <= range)
                            {
                                demon.TakeDamage(Player.Damage);
                            }
                        }
                    }
                    Player.CurrentAction = Action.Shot_Left;
                }
            }
            else
            {
                ShowMessageForDuration("Out of ammo!");
            }
        }
        #endregion
        #region SupportMethods
        private static void UpdateAction(Direction direction, bool isRunning)
        {
            switch (direction)
            {
                case Direction.Up:
                case Direction.Left:
                    Player.CurrentAction = isRunning ? Action.Run_Left : Action.Walk_Left;
                    break;
                case Direction.Down:
                case Direction.Right:
                    Player.CurrentAction = isRunning ? Action.Run_Right : Action.Walk_Right;
                    break;
            }
        }
        public static void ShowMessageForDuration(string message, double displayTime = 1.0)
        {
            currentMessage = message;
            messageDisplayTime = displayTime;
            messageTimer = 0.0;
            showMessage = true;
        }
        #endregion
    }
}
