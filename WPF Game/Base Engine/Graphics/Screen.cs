using System;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Color = System.Windows.Media.Color;
using Image = System.Windows.Controls.Image;

namespace BaseEngine
{
    public class Screen
    {
        #region Variables

        public readonly Stopwatch framerater = new Stopwatch();
        public readonly Label GameData;
        private readonly Image canvas;
        public Bitmap screen_buffer;
        public readonly Window w;
        private int refreshrate;
        public int FPS;
        public readonly int Screen_Width, Screen_Height;

        #region Win32

        [DllImport("gdi32.dll")]
        private static extern bool DeleteObject(IntPtr hObject);

        #endregion

        #endregion

        public Screen(ref Window w, int Screen_Width, int Screen_Height)
        {
            this.w = w;
            this.Screen_Width = Screen_Width;
            this.Screen_Height = Screen_Height;
            //setup Grid
            var grid = new Grid
            {
                Width = w.Width,
                Height = w.Height
            };
            //setup Window
            w.Width = Screen_Width;
            w.Height = Screen_Height;
            w.ResizeMode = ResizeMode.NoResize;
            w.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            w.Content = grid;
            CompositionTarget.Rendering += Screen_Rendering;
            w.Closing += delegate
            {
                CompositionTarget.Rendering -= Screen_Rendering;
                framerater.Stop();
                Environment.Exit(0);
            };
            //setup GameData
            GameData = new Label
            {
                Foreground = new SolidColorBrush(Colors.White),
                Background = new SolidColorBrush(new Color {A = 0}),
                Width = 100,
                Height = 100,
                FontSize = 24,
                Visibility = Visibility.Hidden,
                Margin = new Thickness(0, 0, 700, 508)
            };
            //canvas background
            grid.Children.Add(new Image
            {
                Source = new BitmapImage(new Uri(AppDomain.CurrentDomain.BaseDirectory + "Scene/back.gif")),
                Width = w.Width,
                Height = w.Height
            });
            //setup Canvas & Screen buffer
            canvas = new Image
            {
                Width = w.Width,
                Height = w.Height
            };
            screen_buffer = new Bitmap(Screen_Width, Screen_Height);
            //set View
            grid.Children.Add(canvas);
            grid.Children.Add(GameData);
        }

        #region Methods

        private BitmapSource CreateBitmapSource(ref Bitmap bitmap)
        {
            lock (bitmap)
            {
                var hBitmap = bitmap.GetHbitmap();
                try
                {
                    return Imaging.CreateBitmapSourceFromHBitmap(
                        hBitmap,
                        IntPtr.Zero,
                        Int32Rect.Empty,
                        BitmapSizeOptions.FromEmptyOptions());
                }
                finally
                {
                    DeleteObject(hBitmap);
                }
            }
        }

        private void Screen_Rendering(object sender, EventArgs e)
        {
            if (GameData.IsVisible)
                if (framerater.Elapsed.TotalSeconds > 1)
                {
                    GameData.Content = refreshrate + "Hz" + Environment.NewLine + FPS + "FPS";
                    FPS = 0;
                    refreshrate = 0;
                    framerater.Restart();
                }
                else
                {
                    refreshrate++;
                }

            //repaint canvas from screen_buffer
            canvas.Source = CreateBitmapSource(ref screen_buffer);
        }

        #endregion
    }
}