using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace ConsoleSlayer_02
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private Camera _camera;
        private SpriteFont Font;
        private Dictionary<DemonActions, Texture2D> BlackWerewolfTextures;
        private Dictionary<DemonActions, Texture2D> KarasuTextures;
        private Dictionary<DemonActions, Texture2D> SkeletonSpearman;
        private Dictionary<DemonActions, Texture2D> SkeletonWarrior;
        private List<Demon> Demons;
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
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            Font = Content.Load<SpriteFont>("Silkscreen-Regular");
            Map.LoadTextures(Content);
            Player.LoadTextures(Content);

            #region Demon actions load
            BlackWerewolfTextures = new Dictionary<DemonActions, Texture2D>();

            BlackWerewolfTextures.Add(DemonActions.Attack_Left, Content.Load<Texture2D>("Wolf_Attack_Left"));
            BlackWerewolfTextures.Add(DemonActions.Attack_Right, Content.Load<Texture2D>("Wolf_Attack_Right"));
            BlackWerewolfTextures.Add(DemonActions.Dead, Content.Load<Texture2D>("Wolf_Dead"));
            BlackWerewolfTextures.Add(DemonActions.Run_Left, Content.Load<Texture2D>("Wolf_Run_Left"));
            BlackWerewolfTextures.Add(DemonActions.Run_Right, Content.Load<Texture2D>("Wolf_Run_Right"));

            KarasuTextures = new Dictionary<DemonActions, Texture2D>();

            KarasuTextures.Add(DemonActions.Attack_Left, Content.Load<Texture2D>("Karasu_Attack_Left"));
            KarasuTextures.Add(DemonActions.Attack_Right, Content.Load<Texture2D>("Karasu_Attack_Right"));
            KarasuTextures.Add(DemonActions.Dead, Content.Load<Texture2D>("Karasu_Dead"));
            KarasuTextures.Add(DemonActions.Run_Left, Content.Load<Texture2D>("Karasu_Run_Left"));
            KarasuTextures.Add(DemonActions.Run_Right, Content.Load<Texture2D>("Karasu_Run_Right"));

            SkeletonSpearman = new Dictionary<DemonActions, Texture2D>();

            SkeletonSpearman.Add(DemonActions.Attack_Left, Content.Load<Texture2D>("SkeletonSpearman_Attack_Left"));
            SkeletonSpearman.Add(DemonActions.Attack_Right, Content.Load<Texture2D>("SkeletonSpearman_Attack_Right"));
            SkeletonSpearman.Add(DemonActions.Dead, Content.Load<Texture2D>("SkeletonSpearman_Dead"));
            SkeletonSpearman.Add(DemonActions.Run_Left, Content.Load<Texture2D>("SkeletonSpearman_Run_Left"));
            SkeletonSpearman.Add(DemonActions.Run_Right, Content.Load<Texture2D>("SkeletonSpearman_Run_Right"));


            SkeletonWarrior = new Dictionary<DemonActions, Texture2D>();

            SkeletonWarrior.Add(DemonActions.Attack_Left, Content.Load<Texture2D>("SkeletonWarrior_Attack_Left"));
            SkeletonWarrior.Add(DemonActions.Attack_Right, Content.Load<Texture2D>("SkeletonWarrior_Attack_Right"));
            SkeletonWarrior.Add(DemonActions.Dead, Content.Load<Texture2D>("SkeletonWarrior_Dead"));
            SkeletonWarrior.Add(DemonActions.Run_Left, Content.Load<Texture2D>("SkeletonWarrior_Run_Left"));
            SkeletonWarrior.Add(DemonActions.Run_Right, Content.Load<Texture2D>("SkeletonWarrior_Run_Right"));

            #endregion
        }
        private void IniDemons()
        {
            Demons = new List<Demon>();
            Random rng = new Random();
            for (int i = 0; i < rng.Next(10, 51); i++)
            {
                DemonType demonType = (DemonType)rng.Next(0, 5);
                switch (demonType)
                {
                    case DemonType.Karasu:
                        Demons.Add(new Demon(DemonType.Karasu, KarasuTextures));
                        break;
                    case DemonType.SkeletonSpearman:
                        Demons.Add(new Demon(DemonType.SkeletonSpearman, SkeletonSpearman));
                        break;
                    case DemonType.SkeletonWarrior:
                        Demons.Add(new Demon(DemonType.SkeletonWarrior, SkeletonWarrior));
                        break;
                    case DemonType.BlackWerewolf:
                        Demons.Add(new Demon(DemonType.BlackWerewolf, BlackWerewolfTextures));
                        break;
                }
            }
        }
        private void UpdateDemons(GameTime gameTime)
        {
            for (int i = 0; i < Demons.Count; i++)
            {
                Demons[i].Update(gameTime);
            }
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();


            if (Keyboard.GetState().IsKeyDown(Keys.F11))
            {
                _graphics.IsFullScreen = !_graphics.IsFullScreen;
                _graphics.ApplyChanges();
            }


            Player.Update(gameTime);
            _camera.Follow(Player.Position);

            if (! Map.IsThereAnyMapInitialized)
            {
                Map.InitializeMap("Map1");
            }

            if (Demons == null)
            {
                IniDemons();
            }
            else
            {
                UpdateDemons(gameTime);
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            _spriteBatch.Begin(transformMatrix: _camera.Transform);
            #region Transform
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

            if (Demons.Count > 0)
            {
                for (int i = 0; i < Demons.Count; i++)
                {
                    Demons[i].Draw(_spriteBatch);
                }
            }
            #endregion
            _spriteBatch.End();

            //------------------

            _spriteBatch.Begin();
            #region Still
            Player.Draw(_spriteBatch, Font, GraphicsDevice);
            #endregion
            _spriteBatch.End();


            base.Draw(gameTime);
        }
    }
}
