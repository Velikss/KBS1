﻿using System;
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
        private Level(string Name)
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
            Level l = new Level("World 1-3");
            l.Tiles.Add(new Tile(Sprites.First(o => o.Key == PhysicalType.Sand).Value, PhysicalType.Sand, 0, 470, 150));
            for (int x = 150; x < 4700; x += 32)
                l.Tiles.Add(new Tile(Sprites.First(o => o.Key == PhysicalType.Coin).Value, PhysicalType.Coin, x, 438,
                    1, 32, false));
            l.Tiles.Add(new Tile(Sprites.First(o => o.Key == PhysicalType.Sand).Value, PhysicalType.Sand, -64, 400, 2));
            for (int y = 200; y < 380; y += 32)
                l.Tiles.Add(new Tile(Sprites.First(o => o.Key == PhysicalType.Coin).Value, PhysicalType.Coin, -48, y,
                    1, 32, false));
            l.Tiles.Add(new Tile(Sprites.First(o => o.Key == PhysicalType.BeginFlag).Value, PhysicalType.BeginFlag, 10, 375, 1, 64, false));
            l.Tiles.Add(new Tile(Sprites.First(o => o.Key == PhysicalType.EndFlag).Value, PhysicalType.EndFlag, 4768, 375, 1, 64, false));
            l.BackgroundPath = "Levels/code.jpg";
            l.Background = Image.FromFile(AppDomain.CurrentDomain.BaseDirectory + l.BackgroundPath);
            Levels.Add(l);
        }

        private static Level Load(string File)
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