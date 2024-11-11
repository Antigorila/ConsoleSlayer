using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleSlayer_02
{
    enum DemonType
    {
        Karasu,
        SkeletonSpearman,
        SkeletonWarrior,
        BlackWerewolf
    }
    enum DemonActions
    {
        Attack_Left,
        Attack_Right,
        Dead,
        Run_Left,
        Run_Right
    }
    class Demon
    {
        public Vector2 Position { get; set; }
        public Tile CurrentTile { get; set; }
        public int HP { get; set; }
        public int AttackStrenght { get; set; }
        public DemonType Type { get; set; }
        public DemonActions CurrentAction { get; set; }
        private Dictionary<DemonActions, Texture2D> ActionTextures { get; set; }
        private Random rng;
        private Direction currentDirection;
        private double timeSinceLastAction = 0;
        private double delayDuration = 0.5;

        public Demon(DemonType type)
        {
            Type = type;
            CurrentTile = GetSpawTile();
            CurrentAction = DemonActions.Run_Left;
            Position = CurrentTile.Position;
            rng = new Random();

            switch (type)
            {
                case DemonType.Karasu:
                    HP = 100;
                    AttackStrenght = 20;
                    ActionTextures = DemonController.KarasuTextures;
                    break;
                case DemonType.SkeletonSpearman:
                    HP = 80;
                    AttackStrenght = 15;
                    ActionTextures = DemonController.SkeletonSpearman;
                    break;
                case DemonType.SkeletonWarrior:
                    HP = 80;
                    AttackStrenght = 15;
                    ActionTextures = DemonController.SkeletonWarrior;
                    break;
                case DemonType.BlackWerewolf:
                    HP = 150;
                    AttackStrenght = 30;
                    ActionTextures = DemonController.BlackWerewolfTextures;
                    break;
            }
        }
        private Tile GetSpawTile()
        {
            if (Map.IsThereAnyMapInitialized)
            {
                int randomX = new Random().Next(0, Map.Columns);
                int randomY = new Random().Next(0, Map.Rows);
                if (Map.Map_Normal[randomY, randomX].Type == ConsoleSlayer_02.Type.Road)
                {
                    return Map.Map_Normal[randomY, randomX];
                }
                else
                {
                    return GetSpawTile();
                }
            }
            else
            {
                return null;
            }
        }
        public void Update(GameTime gameTime)
        {
            timeSinceLastAction += gameTime.ElapsedGameTime.TotalSeconds;
            if (timeSinceLastAction >= delayDuration)
            {
                currentDirection = (Direction)rng.Next(0, 4);
                MoveDemon(currentDirection);
                timeSinceLastAction = 0;
            }
            else
            {
                MoveDemon(currentDirection);
            }
        }

        private bool CanMove(Direction direction)
        {
            switch (direction)
            {
                case Direction.Left:
                    if (this.Position.X - 1 < -64)
                    {
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                case Direction.Right:
                    if (this.Position.X + 1 > (Map.Columns * Map.BlockSize) - 64)
                    {
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                case Direction.Up:
                    if (this.Position.Y - 1 < -64)
                    {
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                case Direction.Down:
                    if (this.Position.Y + 1 > (Map.Rows * Map.BlockSize) - 120)
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

        private void MoveDemon(Direction direction)
        {
            if (CanMove(direction))
            {
                Tile prevTile = this.CurrentTile;
                Vector2 newPosition = this.Position;

                switch (direction)
                {
                    case Direction.Up:
                        newPosition.Y -= 1;
                        if (this.Position.Y < this.CurrentTile.Position.Y - 64)
                        {
                            this.CurrentTile = Map.Map_Normal[this.CurrentTile.Get_Y() - 1, this.CurrentTile.Get_X()];
                        }
                        break;

                    case Direction.Down:
                        newPosition.Y += 1;
                        if (this.Position.Y > this.CurrentTile.Position.Y - 64)
                        {
                            try
                            {
                                this.CurrentTile = Map.Map_Normal[this.CurrentTile.Get_Y() + 1, this.CurrentTile.Get_X()];
                            }
                            catch (System.IndexOutOfRangeException)
                            {
                                this.CurrentTile = prevTile;
                            }
                        }
                        break;

                    case Direction.Left:
                        newPosition.X -= 1;
                        if (this.Position.X < this.CurrentTile.Position.X - 64)
                        {
                            this.CurrentTile = Map.Map_Normal[this.CurrentTile.Get_Y(), this.CurrentTile.Get_X() - 1];
                        }
                        break;

                    case Direction.Right:
                        newPosition.X += 1;
                        if (this.Position.X > this.CurrentTile.Position.X)
                        {
                            this.CurrentTile = Map.Map_Normal[this.CurrentTile.Get_Y(), this.CurrentTile.Get_X() + 1];
                        }
                        break;
                }

                if (this.CurrentTile.Type == ConsoleSlayer_02.Type.Wall)
                {
                    this.CurrentTile = prevTile;
                }
                else
                {
                    this.Position = newPosition;
                    UpdateAction(direction);
                }
            }
        }

        private void UpdateAction(Direction direction)
        {
            switch (direction)
            {
                case Direction.Up:
                case Direction.Left:
                    this.CurrentAction = DemonActions.Run_Left;
                    break;
                case Direction.Down:
                case Direction.Right:
                    this.CurrentAction = DemonActions.Run_Right;
                    break;
            }
        }

        public void Draw(SpriteBatch _spriteBatch)
        {
            _spriteBatch.Draw(ActionTextures[CurrentAction], Position, Color.White);           
        }

        public void NoTransformDraw(SpriteBatch _spriteBatch, SpriteFont Font, GraphicsDevice GraphicsDevice)
        {
            _spriteBatch.DrawString(Font, this.Position.ToString(), new Vector2(0, 0), Color.White);
            _spriteBatch.DrawString(Font, this.CurrentTile.Type.ToString(), new Vector2(150, 0), Color.White);
        }
    }
}
