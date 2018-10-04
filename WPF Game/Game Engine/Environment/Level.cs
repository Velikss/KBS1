using System;
using System.Collections.Generic;
// ReSharper disable NotAccessedField.Local

namespace GameEngine
{
    [Serializable]
    public class Level
    {
        public List<Tile> Tiles = new List<Tile>();
        private readonly string LevelName;

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
