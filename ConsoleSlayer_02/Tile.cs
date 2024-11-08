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
        Ground_Skull
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
            Texture = Map.Textures[textureType];
        }

        public bool IsAtPosition(Vector2 playerPosition, float blockSize)
        {
            return Position.X <= playerPosition.X && playerPosition.X < Position.X + blockSize &&
                   Position.Y <= playerPosition.Y && playerPosition.Y < Position.Y + blockSize;
        }
    }
}
