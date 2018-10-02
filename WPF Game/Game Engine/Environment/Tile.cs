using System.Drawing;

namespace GameEngine
{
    public class Tile : PhysicalObject
    {
        //boolean represents if the Tile is a standable Tile
        public readonly bool Ground;
        //instances tile given reffered sprite, position, fatness:), and standability
        public Tile(ref Image Sprite, int x, int y, int width, int height, bool standable = false)
        {
            X = x;
            Y = y;
            Width = width;
            Height = height;
            collision = new Rectangle((int) X, (int) Y, Width, Height);
            this.Sprite = Sprite;
            Ground = standable;
        }
    }
}