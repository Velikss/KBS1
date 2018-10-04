using System;
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
        //holds background for lower memory_use ^change this more beautifull^
        private readonly Image background = Image.FromFile(AppDomain.CurrentDomain.BaseDirectory + "Scene/Title.gif");
        private bool Active;
        public List<MenuButton> buttons = new List<MenuButton>();
        public List<MenuText> texts = new List<MenuText>();
        private readonly Window w;

        public Menu(GameMaker gm, Window w, List<MenuText> texts, List<MenuButton> buttons) : base(gm)
        {
            Active = true;
            (this.w = w).MouseDown += W_MouseDown;
            this.buttons = buttons;
            this.texts = texts;
        }

        public new void StartRender()
        {
            new Thread((ThreadStart) delegate
            {
                for (;;)
                    if (Active)
                        using (backend = Graphics.FromImage(_backend))
                        {
                            //draw background
                            backend.DrawImage(background, new Point(0, 0));
                            //draw buttons
                            foreach (var mb in buttons)
                                backend.DrawImage(mb.Sprite, mb.x, mb.y, mb.Width,
                                    mb.Height);
                            foreach (var mb in buttons)
                                backend.DrawString(mb.Content, mb.font,
                                    mb.text_color,
                                    mb.x + (mb.Width / 2 - mb.text_sizef.Width),
                                    mb.y + (mb.Height / 2 - mb.text_sizef.Height));

                            foreach (var text in texts)
                                backend.DrawString(text.Content, text.font,
                                    text.text_color,
                                    text.x,
                                    text.y);

                            //draw backend to frontend
                            lock (gm.screen.screen_buffer)
                            {
                                frontend.DrawImage(_backend, 0, 0, gm.Screen_Width, gm.Screen_Height);
                            }

                            gm.screen.FPS++;
                            Thread.Sleep(100);
                        }
            }).Start();
        }

        public void Activate()
        {
            Active = true;
            w.MouseDown += W_MouseDown;
        }

        public void Deactivate()
        {
            Active = false;
            w.MouseDown -= W_MouseDown;
        }

        private void W_MouseDown(object sender, MouseButtonEventArgs e)
        {
            var p = e.GetPosition(w);
            foreach (var button in buttons.Where(o =>
                o.x <= p.X && o.x + o.Width >= p.X && o.y <= p.Y &&
                o.y + o.Height >= p.Y))
                button.TriggerClick();
        }
    }

    public class MenuButton
    {
        public delegate void ClickTrigger();

        public readonly string Content;
        public Font font;

        public Image Sprite;
        public Brush text_color;
        public SizeF text_sizef;
        public int x, y, Width, Height;

        public MenuButton(string Content, Font font, Brush text_color, int x, int y, int Width, int Height,
            ref Image sprite)
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

        public SizeF MeasureString(string text, int fontSize, string typeFace)
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
            Clicked();
        }
    }

    public class MenuText
    {
        public string Content;
        public Font font;
        public Brush text_color;
        public int x, y;

        public MenuText(string Content, Font font, Brush text_color, int x, int y)
        {
            this.Content = Content;
            this.font = font;
            this.text_color = text_color;
            this.x = x;
            this.y = y;
        }
    }
}