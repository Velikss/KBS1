using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Windows.Input;
using Brush = System.Drawing.Brush;
using Point = System.Drawing.Point;

namespace GameEngine
{
    public class Menu : Render
    {
        private Image background;
        private readonly List<MenuItem> items;
        private readonly bool overlay;

        public Menu(GameMaker gm, List<MenuItem> items, Image background) : base(gm)
        {
            Activated = true;
            gm.w.MouseDown += W_MouseDown;
            this.items = items;
            if (background != null)
                this.background = background;
            else
                overlay = true;
        }

        private void StartRender()
        {
            running = true;
            SizeF size;
            float x, y;
            new Thread((ThreadStart) delegate
            {
                for (;;)
                    if (Activated)
                        try
                        {
                            using (backend = Graphics.FromImage(_backend))
                            {
                                //draw background
                                backend.DrawImage(background, new Point(0, 0));
                                //draw buttons
                                foreach (var mi in items)
                                    if (mi is MenuButton mb)
                                    {
                                        backend.DrawImage(mi.Sprite, (float) mi.x, (float) mi.y, mi.Width,
                                            mi.Height);
                                        size = backend.MeasureString(mb.Content, mb.font);
                                        backend.DrawString(mb.Content, mb.font, mb.TextColor,
                                            ((float) mi.x) + ((mi.Width / 2) - (size.Width / 2)),
                                            ((float) mi.y) + ((mi.Height / 2) - (size.Height / 2)));
                                    }
                                    else if (mi is MenuText mt)
                                    {
                                        if (mi.x == null)
                                            x = (800 / 2) - (backend.MeasureString(mt.Content, mt.font).Width / 2);
                                        else
                                            x = (float) mi.x;
                                        if (mi.y == null)
                                            y = (600 / 2) - (backend.MeasureString(mt.Content, mt.font).Height / 2);
                                        else
                                            y = (float) mi.y;
                                        backend.DrawString(mt.Content, mt.font, mt.text_color, x, y);
                                    }
                                    else if (mi is MenuPanel)
                                        backend.DrawImage(mi.Sprite, (float) mi.x, (float) mi.y, mi.Width,
                                            mi.Height);

                                //draw backend to frontend
                                lock (gm.screen.screen_buffer)
                                {
                                    frontend.DrawImage(_backend, 0, 0, gm.Screen_Width, gm.Screen_Height);
                                }

                                gm.screen.FPS++;
                                Thread.Sleep(100);
                            }
                        }
                        catch
                        {
                            // ignored
                        }
                    else
                        Thread.Sleep(100);
            }).Start();
        }


        public new void Activate()
        {
            if (!running)
                StartRender();
            if (overlay)
            {
                lock (gm.screen.screen_buffer)
                {
                    background = _backend;
                }
            }

            Activated = true;
            gm.w.MouseDown += W_MouseDown;
        }

        public new void Deactivate()
        {
            Activated = false;
            gm.w.MouseDown -= W_MouseDown;
        }

        private void W_MouseDown(object sender, MouseButtonEventArgs e)
        {
            var p = e.GetPosition(gm.w);
            foreach (var button in items.Where(o => o.x <= p.X && o.x + o.Width >= p.X && o.y <= p.Y &&
                                                    o.y + o.Height >= p.Y))
                if (button is MenuButton menuButton)
                    menuButton.TriggerClick();
        }
    }

    public class MenuItem
    {
        public int? x;
        public int? y;
        public int Width, Height;
        public Image Sprite;
    }

    #region Items

    public class MenuButton : MenuItem
    {
        public delegate void ClickTrigger();

        public readonly string Content;
        public Font font;

        public readonly Brush TextColor;

        public MenuButton(string Content, Font font, Brush text_color, int x, int y, int Width, int Height,
            Image sprite)
        {
            this.Content = Content;
            this.TextColor = text_color;
            this.font = new Font(font.FontFamily, font.Size, font.Style, GraphicsUnit.Pixel, 0);
            this.x = x;
            this.y = y;
            this.Width = Width;
            this.Height = Height;
            Sprite = sprite;
        }

        public event ClickTrigger Clicked;

        public void TriggerClick()
        {
            Clicked?.Invoke();
        }
    }


    public class MenuPanel : MenuItem
    {
        public MenuPanel(int x, int y, int Width, int Height,
            Image sprite)
        {
            this.x = x;
            this.y = y;
            this.Width = Width;
            this.Height = Height;
            Sprite = sprite;
        }
    }

    public class MenuText : MenuItem
    {
        public readonly string Content;
        public readonly Font font;
        public readonly Brush text_color;

        public MenuText(string Content, Font font, Brush text_color, int x, int y)
        {
            this.Content = Content;
            this.font = new Font(font.FontFamily, font.Size, font.Style, GraphicsUnit.Pixel, 0);
            this.text_color = text_color;
            this.x = x;
            this.y = y;
        }

        public MenuText(string Content, Font font, Brush text_color)
        {
            this.Content = Content;
            this.font = new Font(font.FontFamily, font.Size, font.Style, GraphicsUnit.Pixel, 0);
            this.text_color = text_color;
            x = null;
            y = null;
        }
    }

    #endregion
}