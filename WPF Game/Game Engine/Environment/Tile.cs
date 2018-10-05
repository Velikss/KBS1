using System;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace GameEngine
{
    [Serializable]
    public class Tile : PhysicalObject
    {
        // Use `using` blocks for GDI objects you create, so they'll be released
        // quickly when you're done with them.

        //boolean represents if the Tile is a standable Tile
        public readonly bool Collidable;
        // public required for xmlparser
        // ReSharper disable once MemberCanBePrivate.Global
        public string TileType;
        private Tile()
        {
        }

        //instances tile given referred sprite, position, fatness:), and standability
        public Tile(ref Image Sprite, int x, int y, int widthRepeater, int height = 32, bool standable = false)
        {
            X = x;
            Y = y;
            Width = widthRepeater * 32;
            Height = height;
            collision = new Rectangle((int) X, (int) Y, Width, Height);
            TileType = Sprite.Tag.ToString();
            if (Width > 32)
            {
                this.Sprite = new Bitmap(Width, Height);
                using (var brush = new TextureBrush(Sprite, WrapMode.Tile))
                using (var g = Graphics.FromImage(this.Sprite))
                {
                    g.FillRectangle(brush, 0, 0, Width, Height);
                }
            }
            else
            {
                this.Sprite = Sprite;
            }

            Collidable = standable;
        }

        public Tile(Rectangle collision, ref Image Sprite, int x, int y, int widthRepeater, int height = 32, bool standable = false)
        {
            X = x;
            Y = y;
            Width = widthRepeater * 32;
            Height = height;
            this.collision = collision;
            TileType = Sprite.Tag.ToString();
            if (Width > 32)
            {
                this.Sprite = new Bitmap(Width, Height);
                using (var brush = new TextureBrush(Sprite, WrapMode.Tile))
                using (var g = Graphics.FromImage(this.Sprite))
                {
                    g.FillRectangle(brush, 0, 0, Width, Height);
                }
            }
            else
            {
                this.Sprite = Sprite;
            }

            Collidable = standable;
        }
    }
}