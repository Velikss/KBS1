using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

namespace GameEngine
{

    public class Tile : PhysicalObject
    {

        // Use `using` blocks for GDI objects you create, so they'll be released
        // quickly when you're done with them.

        //boolean represents if the Tile is a standable Tile
        public readonly bool Ground;

        //instances tile given referred sprite, position, fatness:), and standability
        public Tile(ref Image Sprite, int x, int y, int height, int widthRepeater, bool standable = false)
        {
            int destWidth = widthRepeater * 32;
            X = x;
            Y = y;
            Width = destWidth;
            Height = height;
            collision = new Rectangle((int) X, (int) Y, destWidth, Height);
            if (widthRepeater > 1)
            {
                Image destImage = (Image) new Bitmap(destWidth, 32);

                using (TextureBrush brush = new TextureBrush(Sprite, WrapMode.Tile))
                using (Graphics g = Graphics.FromImage(destImage))
                {
                    g.FillRectangle(brush, 0, 0, destImage.Width, destImage.Height);
                }

                this.Sprite = destImage;
            }
            else
            {
                this.Sprite = Sprite;
            }

            Ground = standable;
        }
    }
}
