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
        #region Static
        #region Variables
        private static readonly Dictionary<PhysicalType, Image> Sprites = new Dictionary<PhysicalType, Image>();
        private static bool LevelClassPrepared;
        public static readonly List<Level> Levels = new List<Level>();
        public static int Level_index;
        [XmlIgnore] public Dictionary<string, int> HighScore = new Dictionary<string, int>();

        #endregion
        #region Methods
        private Level(){}
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
            LevelClassPrepared = true;
        }
        public static void LoadLevels(string Dir)
        {
            foreach (var c in Directory.GetFiles(Dir))
            {
                if (Path.GetExtension(c) == ".lvl")
                    Levels.Add(Load(c));
            }

            Level l = new Level("Stage Test");
            l.Background = Levels[0].Background;
            l.Tiles = Levels[0].Tiles.ToList();
            l.Tiles.Add(new Tile(Image.FromFile(@"Scene\coin.png"), PhysicalType.Coin, 200, 300, 1, 32, false));
            l.Enemies.Add(new Enemy(800, 300, 1200));
            Levels.Add(l);
        }
        private static Level Load(string File)
        {
            PrepareLevelClass();
            Console.WriteLine(File + ", added");
            XmlSerializer serializer = new XmlSerializer(typeof(Level));
            StreamReader reader = new StreamReader(File);
            Level l = (Level) serializer.Deserialize(reader);
            Level lvl = new Level(l.Name);
            reader.Close();
            foreach (var Tile in l.Tiles)
                lvl.Tiles.Add(new Tile(Sprites.First(o => o.Key == Tile.physicalType).Value, Tile.physicalType,
                    (int) Tile.X, (int) Tile.Y, Tile.Width / 32, Tile.Height, Tile.Collidable));
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
        public Image Background;
        public string Name;
        public List<Tile> Tiles = new List<Tile>();
        [XmlIgnore]
        public List<Enemy> Enemies = new List<Enemy>();
        #endregion
        private Level(string Name)
        {
            PrepareLevelClass();
            this.Name = Name;
        }
        #region Methods
        
        public Level LoadLevel(string File) => Load(File);
        
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