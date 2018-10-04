using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Threading;
using System.Windows;
using System.Windows.Input;
using FontStyle = System.Drawing.FontStyle;

namespace GameEngine
{
    public class GameMaker
    {
        //jumpdata
        public static int jumps;
        private static int JumpPower;
        private bool space_press;

        //holds instance of camera
        internal Camera camera;

        //holds menu's & Game renders
        private Render game_render;
        private bool jump_active;
        private Menu PauseOverlay;

        //holds player instance
        internal Player player;

        //holds reffered screen
        internal Screen screen;

        //holds screen prefferences
        public int Screen_Height;
        public int Screen_Width;

        //holds all tiles
        internal Level level;
        private Menu TitleMenu;

        public void InitializeGame(Window w, int Width, int Height)
        {
            //gets preferred Screen size
            Screen_Height = Height;
            Screen_Width = Width;

            //let's do something with a TileLoader in ? Tile Class: static List<> return?
            //load groud tile sprite to lower memory use ^better place
            Image i = Image.FromFile(AppDomain.CurrentDomain.BaseDirectory + "Scene/ground.gif");
            i.Tag = "ground";
            //let's do a Level class which loads level data
            //create level data ^more nice
            level = new Level("level1");
            level.Tiles.Add(new Tile(ref i, 0, 532, 20, 32, true));
            level.Tiles.Add(new Tile(ref i, 750, 332, 1, 32, true));
            level.Tiles.Add(new Tile(ref i, 1250, 150, 1, 32, true));

            //creates a new screen given screen preferences
            screen = new Screen(this, w);
            //new Render
            game_render = new Render(this);
            PrepareLevel();
            //new Menu
            PrepareMenus(ref w);
            //setup Input events ^nicer place
            w.KeyDown += KeyDown;
            w.KeyUp += KeyUp;
            //start TitleMenu Render
            TitleMenu.Activate();
        }

        private void PrepareMenus(ref Window w)
        {
            var buttonsprite = Image.FromFile(AppDomain.CurrentDomain.BaseDirectory + "Scene/54b2d246e0e35be.png");
            var mb = new MenuButton("Start Game", new Font("Calibri", 16), Brushes.DarkSlateGray, 55, 200, 250,
                50, buttonsprite);
            var mb2 = new MenuButton("Exit Game", new Font("Calibri", 16), Brushes.DarkSlateGray, 55, 255, 250, 50,
                buttonsprite);
            TitleMenu = new Menu(this, w, new List<MenuText>(), new List<MenuButton> {mb, mb2},
                Image.FromFile(AppDomain.CurrentDomain.BaseDirectory + "Scene/Title.gif"));
            var Panel = new MenuButton("", new Font("Calibri", 16), Brushes.DarkSlateGray, 800 / 10 * 3, 0, 800 / 12 * 4,
                500,
                Image.FromFile(AppDomain.CurrentDomain.BaseDirectory + "Scene/pexels-photo-164005.jpeg"));
            var Text = new MenuText("Pause", new Font("Calibri", 48, FontStyle.Regular), Brushes.White);
            Text.y = 25;
            var totitle = new MenuButton("Return to start", new Font("Calibri", 16), Brushes.DarkSlateGray,
                800 / 2 - 250 / 2, 420, 200,
                50, buttonsprite);
            totitle.Clicked += delegate
            {
                PauseOverlay.Deactivate();
                TitleMenu.Activate();
            };
            PauseOverlay = new Menu(this, w, new List<MenuText> {Text}, new List<MenuButton> {Panel, totitle},
                null);
            mb.Clicked += delegate
            {
                TitleMenu.Deactivate();
                PrepareLevel(true);
            };
            mb2.Clicked += delegate { Environment.Exit(0); };
        }

        private void KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Space:
                    if (!jump_active && jumps < 2 && !space_press)
                    {
                        space_press = true;
                        new Thread(Jump_Thread).Start();
                    }

                    break;
                case Key.Escape:
                    if (game_render.isActive())
                    {
                        game_render.Deactivate();
                        PauseOverlay.Activate();
                    }
                    else
                    {
                        PauseOverlay.Deactivate();
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
            switch (e.Key)
            {
                case Key.Space:
                    if (JumpPower < 165)
                        JumpPower = 165;
                    space_press = false;
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

        private void Jump_Thread()
        {
            jump_active = true;
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
            jumps++;
            jump_active = false;
        }

        private void PrepareLevel(bool StartGame = false)
        {
            //initiate player
            player?.Dispose();
            player = new Player
            {
                X = Screen_Width / 4 - 50,
                Y = 350,
                Width = 32,
                Height = 32,
                Sprite = Image.FromFile(AppDomain.CurrentDomain.BaseDirectory + "Animations/normal.gif")
            };
            //creates Camera given reffered focus:player with collision:tiles
            camera?.Dispose();
            camera = new Camera(ref player, ref level, ref game_render);
            player.Initialize(ref camera);
            //then start camera
            camera.Start();
            //set gravity on player & enable gravity given reffered tiles
            Gravity.Dispose();
            Gravity.EnableGravityOnObject(player);
            Gravity.EnableGravity(ref level, ref game_render);
            if (StartGame)
                game_render.Activate();
        }
    }
}