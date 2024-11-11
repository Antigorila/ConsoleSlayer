using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace ConsoleSlayer_02
{
    internal class DemonController
    {
        public static List<Demon> Demons = new List<Demon>();
        public static Dictionary<DemonActions, Texture2D> BlackWerewolfTextures;
        public static Dictionary<DemonActions, Texture2D> KarasuTextures;
        public static Dictionary<DemonActions, Texture2D> SkeletonSpearman;
        public static Dictionary<DemonActions, Texture2D> SkeletonWarrior;

        public static void LoadDemonTextures(ContentManager Content)
        {
            DemonController.BlackWerewolfTextures = new Dictionary<DemonActions, Texture2D>();

            DemonController.BlackWerewolfTextures.Add(DemonActions.Attack_Left, Content.Load<Texture2D>("Wolf_Attack_Left"));
            DemonController.BlackWerewolfTextures.Add(DemonActions.Attack_Right, Content.Load<Texture2D>("Wolf_Attack_Right"));
            DemonController.BlackWerewolfTextures.Add(DemonActions.Dead, Content.Load<Texture2D>("Wolf_Dead"));
            DemonController.BlackWerewolfTextures.Add(DemonActions.Run_Left, Content.Load<Texture2D>("Wolf_Run_Left"));
            DemonController.BlackWerewolfTextures.Add(DemonActions.Run_Right, Content.Load<Texture2D>("Wolf_Run_Right"));

            DemonController.KarasuTextures = new Dictionary<DemonActions, Texture2D>();

            DemonController.KarasuTextures.Add(DemonActions.Attack_Left, Content.Load<Texture2D>("Karasu_Attack_Left"));
            DemonController.KarasuTextures.Add(DemonActions.Attack_Right, Content.Load<Texture2D>("Karasu_Attack_Right"));
            DemonController.KarasuTextures.Add(DemonActions.Dead, Content.Load<Texture2D>("Karasu_Dead"));
            DemonController.KarasuTextures.Add(DemonActions.Run_Left, Content.Load<Texture2D>("Karasu_Run_Left"));
            DemonController.KarasuTextures.Add(DemonActions.Run_Right, Content.Load<Texture2D>("Karasu_Run_Right"));

            DemonController.SkeletonSpearman = new Dictionary<DemonActions, Texture2D>();

            DemonController.SkeletonSpearman.Add(DemonActions.Attack_Left, Content.Load<Texture2D>("SkeletonSpearman_Attack_Left"));
            DemonController.SkeletonSpearman.Add(DemonActions.Attack_Right, Content.Load<Texture2D>("SkeletonSpearman_Attack_Right"));
            DemonController.SkeletonSpearman.Add(DemonActions.Dead, Content.Load<Texture2D>("SkeletonSpearman_Dead"));
            DemonController.SkeletonSpearman.Add(DemonActions.Run_Left, Content.Load<Texture2D>("SkeletonSpearman_Run_Left"));
            DemonController.SkeletonSpearman.Add(DemonActions.Run_Right, Content.Load<Texture2D>("SkeletonSpearman_Run_Right"));


            DemonController.SkeletonWarrior = new Dictionary<DemonActions, Texture2D>();

            DemonController.SkeletonWarrior.Add(DemonActions.Attack_Left, Content.Load<Texture2D>("SkeletonWarrior_Attack_Left"));
            DemonController.SkeletonWarrior.Add(DemonActions.Attack_Right, Content.Load<Texture2D>("SkeletonWarrior_Attack_Right"));
            DemonController.SkeletonWarrior.Add(DemonActions.Dead, Content.Load<Texture2D>("SkeletonWarrior_Dead"));
            DemonController.SkeletonWarrior.Add(DemonActions.Run_Left, Content.Load<Texture2D>("SkeletonWarrior_Run_Left"));
            DemonController.SkeletonWarrior.Add(DemonActions.Run_Right, Content.Load<Texture2D>("SkeletonWarrior_Run_Right"));
        }
        public static void DrawDemons(SpriteFont Font, SpriteBatch _spriteBatch)
        {
            if (DemonController.Demons.Count > 0)
            {
                for (int i = 0; i < DemonController.Demons.Count; i++)
                {
                    DemonController.Demons[i].Draw(_spriteBatch);
                    Vector2 vector2 = new Vector2(DemonController.Demons[i].Position.X, DemonController.Demons[i].Position.Y + 64);
                    _spriteBatch.DrawString(Font, "HP: " + DemonController.Demons[i].HP, vector2, Color.White);
                }
            }
        }

        public static void UpdateDemons(GameTime gameTime)
        {
            for (int i = 0; i < DemonController.Demons.Count; i++)
            {
                DemonController.Demons[i].Update(gameTime);
            }
        }

        public static void IniDemons()
        {
            DemonController.Demons = new List<Demon>();
            Random rng = new Random();
            //for (int i = 0; i < rng.Next(10, 51); i++)
            for (int i = 0; i < 50; i++)
            {
                DemonType demonType = (DemonType)rng.Next(0, 5);
                switch (demonType)
                {
                    case DemonType.Karasu:
                        DemonController.Demons.Add(new Demon(DemonType.Karasu));
                        break;
                    case DemonType.SkeletonSpearman:
                        DemonController.Demons.Add(new Demon(DemonType.SkeletonSpearman));
                        break;
                    case DemonType.SkeletonWarrior:
                        DemonController.Demons.Add(new Demon(DemonType.SkeletonWarrior));
                        break;
                    case DemonType.BlackWerewolf:
                        DemonController.Demons.Add(new Demon(DemonType.BlackWerewolf));
                        break;
                }
            }
        }

    }
}
