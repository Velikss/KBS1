using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Media;
using Brushes = System.Windows.Media.Brushes;
using FlowDirection = System.Windows.FlowDirection;
using Point = System.Drawing.Point;

namespace GameEngine
{
    public class Menu : Render
    {
        //holds background for lower memory_use ^change this more beautifull^
        private readonly Image background = Image.FromFile(AppDomain.CurrentDomain.BaseDirectory + "Scene/Title.gif");
        private bool Active;
        public List<MenuButton> buttons = new List<MenuButton>();
        private Window w;

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
                                backend.DrawString(mb.Content, new Font("Calibri", mb.text_size),
                                    System.Drawing.Brushes.DarkSlateGray,
                                    mb.x + (mb.Width / 2 - mb.text_sizef.Width),
                                    mb.y + (mb.Height / 2 - mb.text_sizef.Height));

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

        public Menu(GameMaker gm, Window w, List<MenuButton> buttons) : base(gm)
        {
            Active = true;
            (this.w = w).MouseDown += W_MouseDown;
            this.buttons = buttons;
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

        private void W_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            System.Windows.Point p = e.GetPosition(w);
            foreach (var button in buttons.Where(o =>
                o.x <= p.X && o.x + o.Width >= p.X && o.y <= p.Y &&
                o.y + o.Height >= p.Y))
                button.TriggerClick();
        }
    }

    public class MenuButton
    {
        public int x, y, Width, Height = 0;
        public event ClickTrigger Clicked;
        public SizeF text_sizef;
        public int text_size;
        public readonly string Content;

        public delegate void ClickTrigger();

        public Image Sprite;

        public MenuButton(string Content, int text_size, int x, int y, int Width, int Height, ref Image sprite)
        {
            this.Content = Content;
            text_sizef = MeasureString(Content, text_size, "Calibri");
            this.text_size = text_size;
            this.x = x;
            this.y = y;
            this.Width = Width;
            this.Height = Height;
            Sprite = sprite;
        }

        public SizeF MeasureString(string text, int fontSize, string typeFace)
        {
            FormattedText ft = new FormattedText
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

        public void TriggerClick() => Clicked();
    }
}