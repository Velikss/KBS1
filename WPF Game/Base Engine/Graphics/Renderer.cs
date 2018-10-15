using System.Drawing;

namespace BaseEngine
{
    public abstract class Renderer
    {
        protected Renderer(ref Screen screen)
        {
            this.screen = screen;
            //creates a new bitmap given screen size
            _backend = new Bitmap(screen.Screen_Width, screen.Screen_Height);
            //setup Graphics from buffer's
            backend = Graphics.FromImage(_backend);
            frontend = Graphics.FromImage(screen.screen_buffer);
        }

        #region Variables

        protected readonly Bitmap _backend;
        protected readonly Graphics frontend;
        protected Graphics backend;
        protected bool Activated, running;
        protected readonly Screen screen;

        #endregion

        #region Methods

        protected abstract void Render();

        public void Activate()
        {
            if (!running)
                Render();
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

        #endregion
    }
}