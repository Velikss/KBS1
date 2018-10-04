using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine
{
    public class Level
    {
        public List<Tile> Tiles = new List<Tile>();
        public readonly string LevelName;
        public Level(string Name)
        {
            LevelName = Name;
        }

        public void Load(string File)
        {

        }

        public void Load(List<Tile> tiles)
        {
            this.Tiles = tiles;
        }
    }
}
