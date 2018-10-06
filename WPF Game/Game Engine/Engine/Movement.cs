using System.Threading;
using System.Windows;
using System.Windows.Input;

namespace GameEngine
{
    public class Movement
    {
        public Movement(GameMaker gm)
        {
            this.gm = gm;
            gm.w.KeyDown += KeyDown;
            gm.w.KeyUp += KeyUp;
        }

        #region Jump

        private void Jumper()
        {
            JumpPower = 0;
            if (Gravity.HasGravity(gm.player))
                Gravity.DisableGravityOnObject(gm.player);
            gm.camera.Up = true;
            while (JumpPower < 280)
            {
                JumpPower++;
                Thread.Sleep(1);
            }

            gm.camera.Up = false;
            if (!Gravity.HasGravity(gm.player))
                Gravity.EnableGravityOnObject(gm.player);
        }

        #endregion

        #region Variables

        public static int jumps;
        private static int JumpPower;
        private bool space_press;
        private readonly GameMaker gm;

        #endregion

        #region Keys

        private void KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Space:
                    if (jumps < 2 && !space_press)
                    {
                        space_press = true;
                        new Thread(Jumper).Start();
                    }

                    break;
                case Key.Escape:
                    if (gm.game_render.isActive())
                    {
                        gm.game_render.Deactivate();
                        gm.PauseOverlay.Activate();
                    }
                    else
                    {
                        gm.PauseOverlay.Deactivate();
                        gm.game_render.Activate();
                    }

                    break;
                case Key.S:
                    gm.camera.Down = true;
                    break;
                case Key.A:
                    gm.camera.Left = true;
                    break;
                case Key.D:
                    gm.camera.Right = true;
                    break;
                case Key.F1:
                    if (gm.screen.GameData.IsVisible)
                    {
                        gm.screen.GameData.Visibility = Visibility.Hidden;
                        gm.screen.framerater.Stop();
                    }
                    else
                    {
                        gm.screen.GameData.Visibility = Visibility.Visible;
                        gm.screen.framerater.Start();
                    }

                    break;
            }
        }

        private void KeyUp(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Space:
                    if (JumpPower < 165)
                        JumpPower = 165;
                    space_press = false;
                    jumps++;
                    break;
                case Key.S:
                    gm.camera.Down = false;
                    break;
                case Key.A:
                    gm.camera.Left = false;
                    break;
                case Key.D:
                    gm.camera.Right = false;
                    break;
            }
        }

        #endregion
    }
}