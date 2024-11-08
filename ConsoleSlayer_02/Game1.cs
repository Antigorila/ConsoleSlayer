using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace ConsoleSlayer_02
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private Camera _camera;
        private SpriteFont Font;


        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
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

            // TODO: use this.Content to load your game content here
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

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            _spriteBatch.Begin(transformMatrix: _camera.Transform);
            for (int i = 0; i < Map.Columns; i++)
            {
                for (int j = 0; j < Map.Rows; j++)
                {


                    _spriteBatch.Draw(Map.Map_Normal[i, j].Texture, Map.Map_Normal[i, j].Position, Color.White);
                    _spriteBatch.DrawString(Font, $"{i}:{j}||", Map.Map_Normal[i, j].Position, Color.White);


                    //DebugDump.Dump("Dekor");
                    //DebugDump.Dump(Map.Map_Decor[i, j].Texture);
                    //DebugDump.Dump(Map.Map_Decor[i, j].Position);

                    //_spriteBatch.Draw(Map.Map_Decor[i, j].Texture, Map.Map_Decor[i, j].Position, Color.White);
                }
            }

            Player.Draw(_spriteBatch);
            _spriteBatch.End();

            _spriteBatch.Begin();
            _spriteBatch.DrawString(Font, "Ammo: " + Player.Ammo, new Vector2(5, GraphicsDevice.Viewport.Height - Font.LineSpacing), Color.White);
            _spriteBatch.DrawString(Font, "Col: " + Map.Columns, new Vector2(150, GraphicsDevice.Viewport.Height - Font.LineSpacing), Color.White);
            _spriteBatch.DrawString(Font, "Row: " + Map.Rows, new Vector2(250, GraphicsDevice.Viewport.Height - Font.LineSpacing), Color.White);
            //if (Player.CurrentTile != null)
            //{
            //    _spriteBatch.DrawString(Font, "TileType: " + Player.CurrentTile.Type, new Vector2(150, GraphicsDevice.Viewport.Height - Font.LineSpacing), Color.White);
            //}
            _spriteBatch.End();


            base.Draw(gameTime);
        }
    }
}
