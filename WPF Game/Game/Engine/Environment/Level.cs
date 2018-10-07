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
        private static readonly Dictionary<PhysicalType, Image> Sprites = new Dictionary<PhysicalType, Image>();
        private static bool LevelClassPrepared;
        private readonly string LevelName;
        public readonly List<Tile> Tiles = new List<Tile>();

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

        private Level()
        {
            PrepareLevelClass();
        }

        private Level(string Name)
        {
            PrepareLevelClass();
            LevelName = Name;
        }

        public static Level Load(string File)
        {
            PrepareLevelClass();
            XmlSerializer serializer = new XmlSerializer(typeof(Level));
            StreamReader reader = new StreamReader(File);
            Level l = (Level) serializer.Deserialize(reader);
            Level lvl = new Level(l.LevelName);
            reader.Close();
            foreach (var Tile in l.Tiles)
                lvl.Tiles.Add(new Tile(Sprites.First(o => o.Key == Tile.physicalType).Value, Tile.physicalType,
                    (int) Tile.X, (int) Tile.Y, Tile.Width / 32, Tile.Height, Tile.Collidable));
            return lvl;
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
    }
}