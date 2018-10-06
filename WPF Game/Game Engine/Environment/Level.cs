using System;
using System.Collections.Generic;

// ReSharper disable NotAccessedField.Local

namespace GameEngine
{
    [Serializable]
    public class Level
    {
        private readonly string LevelName;
        public List<Tile> Tiles = new List<Tile>();

        private Level()
        {
        }

        public Level(string Name)
        {
            LevelName = Name;
        }

        public void Load(string File)
        {
        }

        public void Load(List<Tile> tiles)
        {
            Tiles = tiles;
        }
    }
}