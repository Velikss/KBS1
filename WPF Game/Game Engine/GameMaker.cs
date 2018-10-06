using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows;
using FontStyle = System.Drawing.FontStyle;

namespace GameEngine
{
    public class GameMaker
    {
        //holds instance of camera
        internal Camera camera;
        internal Menu DeadOverlay;

        //holds menu's & Game renders
        internal Render game_render;

        //holds all tiles
        internal Level level;
        internal Menu PauseOverlay;

        //holds player instance
        internal Player player;

        //Screen
        internal Screen screen;
        internal int Screen_Height;
        internal int Screen_Width;
        private Menu TitleMenu;

        internal Window w;

        public void InitializeGame(Window w, int Width, int Height)
        {
            //screen settings
            Screen_Height = Height;
            Screen_Width = Width;
            this.w = w;
            screen = new Screen(this);
            game_render = new Render(this);
            PrepareLevel();
            PrepareMenus();
            PhysicalObject.Collided += GameMaker_Collided;
            new Movement(this);

            TitleMenu.Activate();
        }

        private void GameMaker_Collided(PhysicalObject po)
        {
            switch (po.physicalType)
            {
                case PhysicalType.Lava:
                    //add health & if
                    Console.WriteLine("lava interaction");
                    Dead();
                    break;
                default:
                    po.running = false;
                    break;
            }
        }

        private void Dead()
        {
            game_render.Deactivate();
            DeadOverlay.Activate();
        }

        private void PrepareMenus()
        {
            var buttonsprite = Image.FromFile(AppDomain.CurrentDomain.BaseDirectory + "Scene/54b2d246e0e35be.png");
            var mb = new MenuButton("Single player", new Font("Calibri", 26), Brushes.DarkSlateGray, 55, 200, 250,
                50, buttonsprite);
            var mb2 = new MenuButton("Exit", new Font("Calibri", 26), Brushes.DarkSlateGray, 55, 255, 250, 50,
                buttonsprite);
            TitleMenu = new Menu(this, new List<MenuItem> {mb, mb2},
                Image.FromFile(AppDomain.CurrentDomain.BaseDirectory + "Scene/Title.gif"));
            var Panel = new MenuPanel(800 / 12 * 3, 0, 800 / 12 * 6, 500,
                Image.FromFile(AppDomain.CurrentDomain.BaseDirectory + "Scene/pexels-photo-164005.jpeg"));
            var Text = new MenuText("Pause", new Font("Calibri", 72, FontStyle.Regular), Brushes.White);
            Text.y = 25;
            var restart = new MenuButton("Restart", new Font("Calibri", 26), Brushes.DarkSlateGray,
                800 / 2 - 100, 365, 200,
                50, buttonsprite);
            restart.Clicked += delegate
            {
                PauseOverlay.Deactivate();
                PrepareLevel(true);
            };
            var totitle = new MenuButton("Return to start", new Font("Calibri", 26), Brushes.DarkSlateGray,
                800 / 2 - 100, 420, 200,
                50, buttonsprite);
            totitle.Clicked += delegate
            {
                PauseOverlay.Deactivate();
                TitleMenu.Activate();
            };
            PauseOverlay = new Menu(this, new List<MenuItem> {Panel, Text, totitle, restart},
                null);
            var DeadText = new MenuText("Dead", new Font("Calibri", 72, FontStyle.Bold), Brushes.DarkRed) {y = 25};
            var totitle2 = new MenuButton("Return to start", new Font("Calibri", 26), Brushes.DarkSlateGray,
                800 / 2 - 100, 420, 200,
                50, buttonsprite);
            totitle2.Clicked += delegate
            {
                DeadOverlay.Deactivate();
                TitleMenu.Activate();
            };
            var restart2 = new MenuButton("Restart", new Font("Calibri", 26), Brushes.DarkSlateGray,
                800 / 2 - 100, 365, 200,
                50, buttonsprite);
            restart2.Clicked += delegate
            {
                PauseOverlay.Deactivate();
                PrepareLevel(true);
            };
            DeadOverlay = new Menu(this, new List<MenuItem> {Panel, DeadText, totitle2, restart2},
                null);
            mb.Clicked += delegate
            {
                TitleMenu.Deactivate();
                PrepareLevel(true);
            };
            mb2.Clicked += delegate { Environment.Exit(0); };
        }

        private void PrepareLevel(bool StartGame = false)
        {
            //initiate player
            player?.Dispose();
            player = new Player
            {
                X = Screen_Width / 4 - 125,
                Y = 350,
                Width = 32,
                Height = 32,
                Sprite = Image.FromFile(AppDomain.CurrentDomain.BaseDirectory + "Animations/normal.gif")
            };
            //creates Camera given reffered focus:player with collision:tiles
            camera?.Dispose();
            level = Level.Load(AppDomain.CurrentDomain.BaseDirectory + "Levels/level1.lvl");
            camera = new Camera(this);
            player.Initialize(ref camera);
            //then start camera
            camera.Start();
            //set gravity on player & enable gravity given reffered tiles
            Gravity.Dispose();
            Gravity.EnableGravity(ref level, ref game_render);
            Gravity.EnableGravityOnObject(player);
            if (StartGame)
                game_render.Activate();
        }
    }
}