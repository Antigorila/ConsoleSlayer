using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ConsoleSlayer_02
{
    enum GameSession
    {
        InGame,
        Menu,
    }
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private Camera _camera;
        private SpriteFont Font;
        private GameSession _session;
        private KeyboardState currentKeyboardState;
        private KeyboardState previousKeyboardState;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            _camera = new Camera(730 - 64 / 2, 380 - 64 / 2);
            Player.Position = new Vector2(0, 0);
            DebugDump.NewDump();
            _session = GameSession.Menu;
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            Font = Content.Load<SpriteFont>("Silkscreen-Regular");
            Map.LoadTextures(Content);
            Player.LoadTextures(Content);
            DemonController.LoadDemonTextures(Content);

        }
        
        private bool IsKeyPressed(Keys key)
        {
            return currentKeyboardState.IsKeyDown(key) && previousKeyboardState.IsKeyUp(key);
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            currentKeyboardState = Keyboard.GetState();

            if (Keyboard.GetState().IsKeyDown(Keys.F11))
            {
                _graphics.IsFullScreen = !_graphics.IsFullScreen;
                _graphics.ApplyChanges();
            }

            if (IsKeyPressed(Keys.Tab))
            {
                switch (_session)
                {
                    case GameSession.InGame:
                        _session = GameSession.Menu;
                        break;
                    case GameSession.Menu:
                        _session = GameSession.InGame;
                        break;
                }
            }

            //test
            if (!Map.IsThereAnyMapInitialized)
            {
                Map.InitializeMap("Map1");
                _session = GameSession.InGame;
            }


            switch (_session)
            {
                case GameSession.InGame:
                    Player.Update(gameTime);
                    _camera.Follow(Player.Position);

                    if (DemonController.Demons.Count == 0)
                    {
                        DemonController.IniDemons();
                    }
                    else
                    {
                        DemonController.UpdateDemons(gameTime);
                    }
                    break;
                case GameSession.Menu:
                    break;
            }


            base.Update(gameTime);
            previousKeyboardState = currentKeyboardState;
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            switch (_session)
            {
                case GameSession.InGame:
                    #region Transform
                    _spriteBatch.Begin(transformMatrix: _camera.Transform);

                    Vector2 position = Vector2.Zero;
                    for (int i = 0; i < Map.Rows; i++)
                    {
                        for (int j = 0; j < Map.Columns; j++)
                        {
                            _spriteBatch.Draw(Map.Map_Normal[i, j].Texture, position, Color.White);
                            if (Map.Map_Decor[i, j].TextureType != Texture.None)
                            {
                                _spriteBatch.Draw(Map.Map_Decor[i, j].Texture, position, Color.White);
                            }

                            position.X += Map.BlockSize;
                        }
                        position.X = 0;
                        position.Y += Map.BlockSize;
                    }

                    Player.DrawTransform(_spriteBatch);
                    DemonController.DrawDemons(Font, _spriteBatch);

                    #endregion
                    _spriteBatch.End();

                    //------------------

                    _spriteBatch.Begin();
                    #region Still
                    Player.Draw(_spriteBatch, Font, GraphicsDevice);
                    #endregion
                    _spriteBatch.End();
                    break;
                case GameSession.Menu:
                    _spriteBatch.Begin();
                    _spriteBatch.DrawString(Font, "MENU", new Vector2(50, 50), Color.White);
                    _spriteBatch.DrawString(Font, "Demons: " + DemonController.Demons.Count, new Vector2(50, 100), Color.White);
                    _spriteBatch.DrawString(Font, "Current Map: " + Map.CurrentMapName, new Vector2(50, 150), Color.White);
                    _spriteBatch.End();
                    break;            
            } 
            base.Draw(gameTime);
        }
    }
}
