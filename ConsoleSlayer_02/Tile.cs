using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleSlayer_02
{
    enum Type
    {
        None,
        Decor,
        Wall,
        Road,
        Spawn,
        Finish,
        Lava
    }
    enum Texture
    {
        RockRoad,
        DoomBlock,
        Lava,
        RockTile,
        Ground_Bones,
        Ground_Bone,
        Plant_Eye,
        Ground_Rock2,
        Ground_Rock,
        Plant_Sword,
        Plant_Spike,
        Plant_Tentacle,
        Ground_Skull,
        Skull_Door,
        None
    }
    internal class Tile
    {
        public Vector2 Position { get; set; }
        public Type Type { get; set; }
        public Texture2D Texture { get; set; }
        public Texture TextureType { get; set; }

        public Tile(Vector2 position, Type type, Texture textureType)
        {
            Position = position;
            Type = type;
            TextureType = textureType;
            if (textureType != ConsoleSlayer_02.Texture.None)
            {
                Texture = Map.Textures[textureType];
            }
        }
        public int Get_X()
        {
            return (int)(this.Position.X / 64);
        }
        public int Get_Y()
        {
            return (int)(this.Position.Y / 64);
        }
    }
}
