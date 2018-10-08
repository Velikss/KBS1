using System.Collections.Generic;
using System.Threading;
using System.Windows;
using System.Windows.Input;
using BaseEngine;

namespace GameEngine
{
    public class Movement
    {
        #region Variables

        private bool MovementEnabled;
        private readonly Player player;
        private readonly Camera camera;
        private readonly GameRenderer game_render;
        private readonly Dictionary<MenuType, Menu> Menus;
        private readonly Screen screen;
        public static int jumps;
        private static int JumpPower;
        private bool space_press;

        #endregion

        public Movement(ref Screen screen, ref Player player, ref Camera camera, ref GameRenderer game_render,
            ref Dictionary<MenuType, Menu> Menus)
        {
            this.screen = screen;
            this.Menus = Menus;
            this.game_render = game_render;
            this.player = player;
            this.camera = camera;
            screen.w.KeyDown += KeyDown;
            screen.w.KeyUp += KeyUp;
        }

        #region Methods

        #region Jump

        private void Jumper()
        {
            JumpPower = 0;
            if (Gravity.HasGravity(player))
                Gravity.DisableGravityOnObject(player);
            camera.Up = true;
            while (JumpPower < 280)
            {
                JumpPower++;
                Thread.Sleep(1);
            }

            camera.Up = false;
            if (!Gravity.HasGravity(player))
                Gravity.EnableGravityOnObject(player);
        }

        #endregion

        public void EnableKeys() => MovementEnabled = true;
        public void DisableKeys() => MovementEnabled = false;

        private void KeyDown(object sender, KeyEventArgs e)
        {
            if (MovementEnabled)
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
                        if (game_render.isActive())
                        {
                            game_render.Deactivate();
                            DisableKeys();
                            Menus[MenuType.Pause].Activate();
                        }
                        else
                        {
                            Menus[MenuType.Pause].Deactivate();
                            EnableKeys();
                            game_render.Activate();
                        }

                        break;
                    case Key.S:
                        camera.Down = true;
                        break;
                    case Key.A:
                        camera.Left = true;
                        break;
                    case Key.D:
                        camera.Right = true;
                        break;
                    case Key.F1:
                        if (screen.GameData.IsVisible)
                        {
                            screen.GameData.Visibility = Visibility.Hidden;
                            screen.framerater.Stop();
                        }
                        else
                        {
                            screen.GameData.Visibility = Visibility.Visible;
                            screen.framerater.Start();
                        }

                        break;
                }
        }

        private void KeyUp(object sender, KeyEventArgs e)
        {
            if (MovementEnabled)
                switch (e.Key)
                {
                    case Key.Space:
                        if (JumpPower < 165)
                            JumpPower = 165;
                        space_press = false;
                        jumps++;
                        break;
                    case Key.S:
                        camera.Down = false;
                        break;
                    case Key.A:
                        camera.Left = false;
                        break;
                    case Key.D:
                        camera.Right = false;
                        break;
                }
        }

        #endregion
    }
}