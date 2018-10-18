using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Serialization;

namespace GameEngine
{
    [Serializable]
    public class Level
    {
        public Level(string Name)
        {
            PrepareLevelClass();
            this.Name = Name;
        }

        #region Static

        #region Variables

        private static readonly Dictionary<PhysicalType, Image> Sprites = new Dictionary<PhysicalType, Image>();
        private static bool LevelClassPrepared;
        public static readonly List<Level> Levels = new List<Level>();
        public static int Level_index;

        #endregion

        #region Methods

        private Level()
        {
        }

        private static void PrepareLevelClass()
        {
            if (LevelClassPrepared) return;
            Sprites.Add(PhysicalType.Ground,
                Image.FromFile(AppDomain.CurrentDomain.BaseDirectory + "Scene/ground.gif"));
            Sprites.Add(PhysicalType.BeginFlag,
                Image.FromFile(AppDomain.CurrentDomain.BaseDirectory + "Scene/begin-point-sprite.gif"));
            Sprites.Add(PhysicalType.EndFlag,
                Image.FromFile(AppDomain.CurrentDomain.BaseDirectory + "Scene/end-point-sprite.gif"));
            Sprites.Add(PhysicalType.Lava,
                Image.FromFile(AppDomain.CurrentDomain.BaseDirectory + "Scene/lava.gif"));
            Sprites.Add(PhysicalType.GroundSide,
                Image.FromFile(AppDomain.CurrentDomain.BaseDirectory + "Scene/ground-side.gif"));
            Sprites.Add(PhysicalType.GroundSideRight,
                Image.FromFile(AppDomain.CurrentDomain.BaseDirectory + "Scene/ground-side-right.gif"));
            Sprites.Add(PhysicalType.Enemy,
                Image.FromFile(AppDomain.CurrentDomain.BaseDirectory + "Scene/enemy.gif"));
            Sprites.Add(PhysicalType.Coin,
                Image.FromFile(AppDomain.CurrentDomain.BaseDirectory + "Scene/coin.png"));
            Sprites.Add(PhysicalType.Brick, Image.FromFile(AppDomain.CurrentDomain.BaseDirectory + "Scene/brick.gif"));
            Sprites.Add(PhysicalType.Cloud, Image.FromFile(AppDomain.CurrentDomain.BaseDirectory + "Scene/cloud.gif"));
            Sprites.Add(PhysicalType.Fire_Goast,
                Image.FromFile(AppDomain.CurrentDomain.BaseDirectory + "Scene/Fire-Goast.gif"));
            Sprites.Add(PhysicalType.Goomba,
                Image.FromFile(AppDomain.CurrentDomain.BaseDirectory + "Scene/Goomba.gif"));
            Sprites.Add(PhysicalType.Grass,
                Image.FromFile(AppDomain.CurrentDomain.BaseDirectory + "Scene/grass_ground.gif"));
            Sprites.Add(PhysicalType.Stone, Image.FromFile(AppDomain.CurrentDomain.BaseDirectory + "Scene/stone.gif"));
            Sprites.Add(PhysicalType.Pipe, Image.FromFile(AppDomain.CurrentDomain.BaseDirectory + "Scene/Pipe.gif"));
            Sprites.Add(PhysicalType.Spike, Image.FromFile(AppDomain.CurrentDomain.BaseDirectory + "Scene/spikes.gif"));
            Sprites.Add(PhysicalType.Water, Image.FromFile(AppDomain.CurrentDomain.BaseDirectory + "Scene/water.gif"));
            Sprites.Add(PhysicalType.Sand, Image.FromFile(AppDomain.CurrentDomain.BaseDirectory + "Scene/Sand.png"));
            LevelClassPrepared = true;
        }

        public static void LoadLevels(string Dir)
        {
            foreach (var c in Directory.GetFiles(Dir))
                if (Path.GetExtension(c) == ".lvl")
                    Levels.Add(Load(c));

//            Level l = new Level("World 1-3");
//            l.Tiles.Add(new Tile(Sprites.First(o => o.Key == PhysicalType.Brick).Value, PhysicalType.Brick, 0, 470, 5));
//            int xStairs = 165;
//            int yStairs = 470;
//            for (int i = 0; i < 10; i++)
//            { 
//                xStairs += 64;
//                yStairs -= 32;
//                l.Tiles.Add(new Tile(Sprites.First(o => o.Key == PhysicalType.Brick).Value, PhysicalType.Brick, xStairs, yStairs, 1));
//            }
//            int xCoin = 165;
//            int yCoin = 430;
//            for (int i = 0; i < 10; i++)
//            {
//                xCoin += 64;
//                yCoin -= 32;
//                l.Tiles.Add(new Tile(Sprites.First(o => o.Key == PhysicalType.Coin).Value, PhysicalType.Coin, xCoin, yCoin, 1, 32, false));
//            }
//            l.Tiles.Add(new Tile(Sprites.First(o => o.Key == PhysicalType.Brick).Value, PhysicalType.Brick, 805, 150, 15));
//            int xStairs2 = 1285;
//            int yStairs2 = 150;
//            for (int i = 0; i < 5; i++)
//            {
//                xStairs2 += 64;
//                yStairs2 += 64;
//                l.Tiles.Add(new Tile(Sprites.First(o => o.Key == PhysicalType.Brick).Value, PhysicalType.Brick, xStairs2, yStairs2, 1));
//            }
//            l.Tiles.Add(new Tile(Sprites.First(o => o.Key == PhysicalType.Brick).Value, PhysicalType.Brick, 1585, 150, 1));
//            l.Tiles.Add(new Tile(Sprites.First(o => o.Key == PhysicalType.Coin).Value, PhysicalType.Coin, 1585, 118, 1));
//            l.Tiles.Add(new Tile(Sprites.First(o => o.Key == PhysicalType.Brick).Value, PhysicalType.Brick, 1777, 150, 1));
//            l.Tiles.Add(new Tile(Sprites.First(o => o.Key == PhysicalType.Coin).Value, PhysicalType.Coin, 1777, 118, 1));
//            l.Tiles.Add(new Tile(Sprites.First(o => o.Key == PhysicalType.Brick).Value, PhysicalType.Brick, 1969, 150, 1));
//            l.Tiles.Add(new Tile(Sprites.First(o => o.Key == PhysicalType.Coin).Value, PhysicalType.Coin, 1969, 118, 1));
//
//            l.Tiles.Add(new Tile(Sprites.First(o => o.Key == PhysicalType.Brick).Value, PhysicalType.Brick, 2290, 110, 15));
//            
//            l.Tiles.Add(new Tile(Sprites.First(o => o.Key == PhysicalType.BeginFlag).Value, PhysicalType.BeginFlag, 10, 375, 1, 64, false));
//            l.Tiles.Add(new Tile(Sprites.First(o => o.Key == PhysicalType.EndFlag).Value, PhysicalType.EndFlag, 2650, 14, 1, 64, false));
//            l.BackgroundPath = "Levels/back3.jpeg";
//            l.Background = Image.FromFile(AppDomain.CurrentDomain.BaseDirectory + l.BackgroundPath);
//            l.GenerateFile(AppDomain.CurrentDomain.BaseDirectory + "Levels/level3.lvl");
//            Levels.Add(l);
        }

        public static Level Load(string File)
        {
            PrepareLevelClass();
            Console.WriteLine(File + ", added");
            var serializer = new XmlSerializer(typeof(Level));
            var reader = new StreamReader(File);
            var l = (Level) serializer.Deserialize(reader);
            var lvl = new Level(l.Name);
            reader.Close();
            foreach (var Tile in l.Tiles)
                lvl.Tiles.Add(new Tile(Sprites.First(o => o.Key == Tile.physicalType).Value, Tile.physicalType,
                    (int) Tile.X, (int) Tile.Y, Tile.Width / 32, Tile.Height, Tile.Collidable));
            foreach (var enemy in l.Enemies)
                lvl.Enemies.Add(new Enemy(enemy.baseX, enemy.baseY, enemy.stopFollowingAt));
            lvl.BackgroundPath = l.BackgroundPath;
            lvl.Background = Image.FromFile(AppDomain.CurrentDomain.BaseDirectory + lvl.BackgroundPath);
            return lvl;
        }

        public static Level Load(int index)
        {
            PrepareLevelClass();
            foreach (var i in Levels[index].Tiles)
            {
                i.running = false;
                i.Visible = true;
            }

            return Levels[index];
        }

        #endregion

        #endregion

        #region Variables

        public string BackgroundPath;
        [XmlIgnore] public Image Background;
        public string Name;
        public List<Tile> Tiles = new List<Tile>();
        public List<Enemy> Enemies = new List<Enemy>();

        #endregion

        #region Methods

        public Level LoadLevel(string File)
        {
            return Load(File);
        }

        public void GenerateFile(string FileLocation)
        {
            using (var sww = new StringWriter())
            using (var writer = XmlWriter.Create(sww))
            {
                new XmlSerializer(typeof(Level)).Serialize(writer, this);
                File.WriteAllText(FileLocation, sww.ToString());
            }
        }

        public void Reset()
        {
        }

        #endregion
    }
}