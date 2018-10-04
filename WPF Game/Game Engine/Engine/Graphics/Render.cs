using System;
using System.Drawing;
using System.Linq;
using System.Threading;

namespace GameEngine
{
    public class Render
    {
        protected readonly Bitmap _backend;

        //holds background for lower memory_use ^change this more beautifull^
        private readonly Image background = Image.FromFile(AppDomain.CurrentDomain.BaseDirectory + "Scene/back.gif");

        //holds the frontend Graphics drawer -> Screen.screen_buffer
        protected readonly Graphics frontend;

        protected bool Activated;

        //holds the backend Graphics drawer -> backend buffer
        protected Graphics backend;
        protected GameMaker gm;
        protected bool running;

        public Render(GameMaker gm)
        {
            this.gm = gm;
            //creates a new bitmap given screen size
            _backend = new Bitmap(gm.Screen_Width, gm.Screen_Height);
            //setup Graphics from buffer's
            backend = Graphics.FromImage(_backend);
            frontend = Graphics.FromImage(gm.screen.screen_buffer);
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
                                //draw all tiles within view of camera, #better performance
                                foreach (var tile in gm.level.Tiles.Where(o =>
                                    o.X + o.Width >= gm.camera.X * -1 && o.X <= gm.camera.X * -1 + gm.Screen_Width &&
                                    o.Y + o.Height >= gm.camera.Y * -1 &&
                                    o.Y + o.Height <= gm.camera.Y * -1 + gm.Screen_Height &&
                                    o.Sprite != null))
                                    backend.DrawImage(tile.Sprite, gm.camera.X + tile.X, gm.camera.Y + tile.Y);
                                //draw player
                                backend.DrawImage(gm.player.Sprite, gm.camera.X + gm.player.X,
                                    gm.camera.Y + gm.player.Y);
                                //draw backend to frontend
                                lock (gm.screen.screen_buffer)
                                {
                                    frontend.DrawImage(_backend, 0, 0, gm.Screen_Width, gm.Screen_Height);
                                }

                                if (gm.screen.GameData.IsVisible)
                                    gm.screen.FPS++;
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


        public void Activate()
        {
            if (!running)
                StartRender();
            Activated = true;
        }

        public void Deactivate()
        {
            Activated = false;
        }

        public bool isActive()
        {
            return Activated;
        }
    }
}