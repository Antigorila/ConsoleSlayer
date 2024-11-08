using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleSlayer_02
{
    internal class Map
    {
        public static Dictionary<Texture, Texture2D> Textures = new Dictionary<Texture, Texture2D>();
        public static Tile[,] Map_Normal;
        public static Tile[,] Map_Decor;
        public static int Rows;
        public static int Columns;
        public const int BlockSize = 64;
        public static bool IsThereAnyMapInitialized = false;

        public static void InitializeMap(string MapName)
        {
            StreamReader readerNormal = new StreamReader($"Maps/{MapName}/{MapName}_Normal.txt");

            // Get the column count
            Columns = GetMapColumnCount(readerNormal);

            // Reset the reader to the beginning of the file
            readerNormal.BaseStream.Seek(0, SeekOrigin.Begin);
            readerNormal.DiscardBufferedData();

            // Get the row count
            Rows = GetMapRowCount(readerNormal);

            // Reset the reader again to read the file content
            readerNormal.BaseStream.Seek(0, SeekOrigin.Begin);
            readerNormal.DiscardBufferedData();

            // Initialize your map arrays
            Map_Normal = new Tile[Columns, Rows];
            Map_Decor = new Tile[Columns, Rows];
            //Dont need to include the .txt part

            int rowCount = 0;
            string line;
            while ((line = readerNormal.ReadLine()) != null) // Read each line only once
            {
                AddTile(line, rowCount);
                rowCount++;
            }
            readerNormal.Close();

            rowCount = 0;
            StreamReader readerDecor = new StreamReader($"Maps/{MapName}/{MapName}_Decor.txt");

            while ((line = readerDecor.ReadLine()) != null)
            {
                AddTile(line, rowCount);
                rowCount++;
            }
            readerDecor.Close();

            IsThereAnyMapInitialized = true;
        }
        private static int GetMapRowCount(StreamReader mapReader)
        {
            int count = 0;
            while (mapReader.ReadLine() != null)
            {
                count++;
            }
            return count;
        }
        private static int GetMapColumnCount(StreamReader mapReader)
        {
            return mapReader.ReadLine().Split('|').Length;
        }

        public static void LoadTextures(ContentManager Content)
        {
            #region Map Animation Load
            //S:00 will be the spawn
            //F:00 will be the finish
            //Tiles
            Map.Textures.Add(Texture.RockRoad, Content.Load<Texture2D>("RockRoad"));//T:01
            Map.Textures.Add(Texture.DoomBlock, Content.Load<Texture2D>("DoomBlock"));//T:02
            Map.Textures.Add(Texture.Lava, Content.Load<Texture2D>("Lava"));//T:03
            Map.Textures.Add(Texture.RockTile, Content.Load<Texture2D>("RockTile"));//T:04
            //Decor
            Map.Textures.Add(Texture.Ground_Bones, Content.Load<Texture2D>("Ground_Bones"));//D:01
            Map.Textures.Add(Texture.Ground_Bone, Content.Load<Texture2D>("Ground_Bone"));//D:02
            Map.Textures.Add(Texture.Plant_Eye, Content.Load<Texture2D>("Plant_Eye"));//D:03
            Map.Textures.Add(Texture.Ground_Rock2, Content.Load<Texture2D>("Ground_Rock2"));//D:04
            Map.Textures.Add(Texture.Ground_Rock, Content.Load<Texture2D>("Ground_Rock"));//D:05
            Map.Textures.Add(Texture.Plant_Sword, Content.Load<Texture2D>("Plant_Sword"));//D:06
            Map.Textures.Add(Texture.Plant_Spike, Content.Load<Texture2D>("Plant_Spike"));//D:07
            Map.Textures.Add(Texture.Plant_Tentacle, Content.Load<Texture2D>("Plant_Tentacle"));//D:08
            Map.Textures.Add(Texture.Ground_Skull, Content.Load<Texture2D>("Ground_Skull"));//D:09
            #endregion
        }
        private static void AddTile(string line, int row)
        {
            if (!string.IsNullOrWhiteSpace(line))
            {
                string[] TileLine = line.Split('|');

                for (int i = 0; i < TileLine.Length; i++)
                {
                    string[] Datas = TileLine[i].Split(':');
                    if (Datas.Length < 2) continue;

                    string flag = Datas[0];
                    string type = Datas[1];
                    Vector2 position = new Vector2(i * BlockSize, row * BlockSize);

                    switch (flag)
                    {
                        case "T":
                            switch (type)
                            {
                                case "01":
                                    Map_Normal[i, row] = new Tile(position, Type.Road, Texture.RockRoad);
                                    break;
                                case "02":
                                    Map_Normal[i, row] = new Tile(position, Type.Wall, Texture.DoomBlock);
                                    break;
                                case "03":
                                    Map_Normal[i, row] = new Tile(position, Type.Lava, Texture.Lava);
                                    break;
                                case "04":
                                    Map_Normal[i, row] = new Tile(position, Type.Wall, Texture.RockTile);
                                    break;
                            }
                            break;

                        case "D":
                            switch (type)
                            {
                                case "01":
                                    Map_Decor[i, row] = new Tile(position, Type.Decor, Texture.Ground_Bones);
                                    break;
                                case "02":
                                    Map_Decor[i, row] = new Tile(position, Type.Decor, Texture.Ground_Bone);
                                    break;
                                case "03":
                                    Map_Decor[i, row] = new Tile(position, Type.Decor, Texture.Plant_Eye);
                                    break;
                                case "04":
                                    Map_Decor[i, row] = new Tile(position, Type.Decor, Texture.Ground_Rock2);
                                    break;
                                case "05":
                                    Map_Decor[i, row] = new Tile(position, Type.Decor, Texture.Ground_Rock);
                                    break;
                                case "06":
                                    Map_Decor[i, row] = new Tile(position, Type.Decor, Texture.Plant_Sword);
                                    break;
                                case "07":
                                    Map_Decor[i, row] = new Tile(position, Type.Decor, Texture.Plant_Spike);
                                    break;
                                case "08":
                                    Map_Decor[i, row] = new Tile(position, Type.Decor, Texture.Plant_Tentacle);
                                    break;
                                case "09":
                                    Map_Decor[i, row] = new Tile(position, Type.Decor, Texture.Ground_Skull);
                                    break;
                            }
                            break;

                        case "S":
                            break;
                        case "F":
                            break;
                    }
                }
            }
        }
    }
}
