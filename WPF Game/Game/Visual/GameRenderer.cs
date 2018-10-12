using System.Drawing;
using System.Linq;
using System.Threading;
using BaseEngine;

namespace GameEngine
{
    public class GameRenderer : Renderer
    {
        private readonly Player player;
        private readonly Camera camera;
        private Level level;

        public GameRenderer(ref Screen screen, ref Player player, ref Camera camera, ref Level level):base(ref screen)
        {
            this.player = player;
            this.camera = camera;
            this.level = level;
        }

        protected override void Render()
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
                                backend.DrawImage(level.Background, new Point(0, 0));
                                //draw player
                                backend.DrawImage(player.Sprite, camera.X + player.X,
                                    camera.Y + player.Y);
                                //draw all tiles within view of camera, #better performance
                                foreach (var tile in level.Tiles.Where(o => o.Visible && o.X + o.Width >= camera.X * -1 &&
                                                                               o.X <= camera.X * -1 +
                                                                               screen.Screen_Width &&
                                                                               o.Y + o.Height >= camera.Y * -1 &&
                                                                               o.Y + o.Height <=
                                                                               camera.Y * -1 + screen.Screen_Height &&
                                                                               o.Sprite != null))
                                    backend.DrawImage(tile.Sprite, camera.X + tile.X, camera.Y + tile.Y);
                                //draw backend to frontend
                                lock (screen.screen_buffer)
                                {
                                    frontend.DrawImage(_backend, 0, 0, screen.Screen_Width, screen.Screen_Height);
                                }

                                if (screen.GameData.IsVisible)
                                    screen.FPS++;
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

        public void ChangeLevelData(Level l)
        {
            lock (level)
                level = l;
        }
    }
}
