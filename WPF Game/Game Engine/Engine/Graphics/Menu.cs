using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using Brush = System.Drawing.Brush;
using Brushes = System.Windows.Media.Brushes;
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
                                    if (mi is MenuButton)
                                    {
                                        backend.DrawImage(mi.Sprite, mi.x, mi.y, mi.Width,
                                            mi.Height);
                                        backend.DrawString(((MenuButton) mi).Content, ((MenuButton) mi).font,
                                            ((MenuButton) mi).text_color,
                                            mi.x + (mi.Width / 2 - ((MenuButton) mi).text_sizef.Width),
                                            mi.y + (mi.Height / 2 - ((MenuButton) mi).text_sizef.Height));
                                    }
                                    else if (mi is MenuText)
                                        backend.DrawString(((MenuText) mi).Content, ((MenuText) mi).font,
                                            ((MenuText) mi).text_color,
                                            ((MenuText) mi).x,
                                            ((MenuText) mi).y);
                                    else if (mi is MenuPanel)
                                        backend.DrawImage(mi.Sprite, mi.x, mi.y, mi.Width,
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
        public int x, y, Width, Height;
        public Image Sprite;
    }

    #region Items

    public class MenuButton : MenuItem
    {
        public delegate void ClickTrigger();

        public readonly string Content;
        public readonly Font font;

        public readonly Brush text_color;
        public SizeF text_sizef;

        public MenuButton(string Content, Font font, Brush text_color, int x, int y, int Width, int Height,
            Image sprite)
        {
            this.Content = Content;
            text_sizef = MeasureString(Content, (int) font.Size, font.Name);
            this.text_color = text_color;
            this.font = font;
            this.x = x;
            this.y = y;
            this.Width = Width;
            this.Height = Height;
            Sprite = sprite;
        }

        public event ClickTrigger Clicked;

        private SizeF MeasureString(string text, int fontSize, string typeFace)
        {
            var ft = new FormattedText
            (
                text,
                CultureInfo.CurrentCulture,
                FlowDirection.LeftToRight,
                new Typeface(typeFace),
                fontSize,
                Brushes.Black
            );
            return new SizeF((float) ft.Width, (float) ft.Height);
        }

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
            this.font = font;
            this.text_color = text_color;
            this.x = x;
            this.y = y;
        }

        public MenuText(string Content, Font font, Brush text_color)
        {
            this.Content = Content;
            this.font = font;
            this.text_color = text_color;
            var size = MeasureString(Content, (int) font.Size, font.Name);
            x = (int) (800 / 2 - size.Width);
            y = (int) (600 / 2 - size.Height);
        }

        private SizeF MeasureString(string text, int fontSize, string typeFace)
        {
            var ft = new FormattedText
            (
                text,
                CultureInfo.CurrentCulture,
                FlowDirection.LeftToRight,
                new Typeface(typeFace),
                fontSize,
                Brushes.Black
            );
            return new SizeF((float) ft.Width, (float) ft.Height);
        }
    }

    #endregion
}