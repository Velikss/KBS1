﻿using System.Collections.Generic;
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
        private readonly Window w;

        //holds background for lower memory_use ^change this more beautifull^
        private readonly Image background;
        private readonly List<MenuButton> buttons;
        private readonly List<MenuText> texts;

        public Menu(GameMaker gm, Window w, List<MenuText> texts, List<MenuButton> buttons, Image background) : base(gm)
        {
            Activated = true;
            (this.w = w).MouseDown += W_MouseDown;
            this.buttons = buttons;
            this.texts = texts;
            this.background = background;
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
            Activated = true;
            w.MouseDown += W_MouseDown;
        }

        public new void Deactivate()
        {
            Activated = false;
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
        public readonly Font font;

        public readonly Image Sprite;
        public readonly Brush text_color;
        public SizeF text_sizef;
        public readonly int x, y, Width, Height;

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

    public class MenuText
    {
        public readonly string Content;
        public readonly Font font;
        public readonly Brush text_color;
        public readonly int x;
        public int y;

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
}