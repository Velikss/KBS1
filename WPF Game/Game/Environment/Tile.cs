using System;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace GameEngine
{
    [Serializable]
    public class Tile : PhysicalObject
    {
        private Tile()
        {
        }

        public Tile(Image Sprite, PhysicalType pt, int x, int y, int widthRepeater, int height = 32,
            bool collidable = false)
        {
            physicalType = pt;
            X = x;
            Y = y;
            Width = widthRepeater * 32;
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

        public Tile(Rectangle collision, PhysicalType pt, ref Image Sprite, int x, int y, int widthRepeater,
            int height = 32, bool Collidable = false)
        {
            physicalType = pt;
            X = x;
            Y = y;
            Width = widthRepeater * 32;
            Height = height;
            this.collision = collision;
            if (Width > 32)
            {
                this.Sprite = new Bitmap(Width, Height);
                using (var brush = new TextureBrush(Sprite, WrapMode.Tile))
                using (var g = Graphics.FromImage(this.Sprite))
                    g.FillRectangle(brush, 0, 0, Width, Height);
            }
            else
                this.Sprite = Sprite;

            this.Collidable = Collidable;
        }
    }
}