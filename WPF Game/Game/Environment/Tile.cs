using System;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace GameEngine
{
    [Serializable]
    public class Tile : PhysicalObject
    {
        public bool Visible = true;

        private Tile()
        {
        }

        public Tile(Image Sprite, PhysicalType pt, int x, int y, int width32) =>
            Initialize(Sprite, pt, x, y, width32, 32, true);

        public Tile(Image Sprite, PhysicalType pt, int x, int y, int width32, int height, bool collidable) =>
            Initialize(Sprite, pt, x, y, width32, height, collidable);

        private void Initialize(Image Sprite, PhysicalType pt, int x, int y, int width32, int height,
            bool collidable)
        {
            physicalType = pt;
            X = x;
            Y = y;
            Width = width32 * 32;
            Height = height;
            collision = new Rectangle((int) X, (int) Y, Width, Height);
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

            Collidable = collidable;
        }
    }
}