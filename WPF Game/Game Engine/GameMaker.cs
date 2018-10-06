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

        //holds menu's & Game renders
        internal Render game_render;
        internal Menu PauseOverlay;
        private Menu TitleMenu;
        internal Menu DeadOverlay;

        //holds player instance
        internal Player player;

        //Screen
        internal Screen screen;
        internal int Screen_Height;
        internal int Screen_Width;

        //holds all tiles
        internal Level level;

        internal Window w;

        public void InitializeGame(Window w, int Width, int Height)
        {
            //screen settings
            Screen_Height = Height;
            Screen_Width = Width;
            this.w = w;
            //Leveling ^Level class loader
            Image i = Image.FromFile(AppDomain.CurrentDomain.BaseDirectory + "Scene/ground.gif");
            i.Tag = "ground";
            Image beginpoint = Image.FromFile(AppDomain.CurrentDomain.BaseDirectory + "Scene/begin-point-sprite.gif");
            beginpoint.Tag = "beginpoint";
            Image endpoint = Image.FromFile(AppDomain.CurrentDomain.BaseDirectory + "Scene/end-point-sprite.gif");
            endpoint.Tag = "endpoint";
            Image lava = Image.FromFile(AppDomain.CurrentDomain.BaseDirectory + "Scene/lava.gif");
            lava.Tag = "lava";

            Image groundside = Image.FromFile(AppDomain.CurrentDomain.BaseDirectory + "Scene/ground-side.gif");
            groundside.Tag = "groundside";
            Image groundsideright =
                Image.FromFile(AppDomain.CurrentDomain.BaseDirectory + "Scene/ground-side-right.gif");
            groundsideright.Tag = "groundsideright";

            level = new Level("level1");
            level.Tiles.Add(new Tile(ref beginpoint, PhysicalType.Block, 10, 404, 2, 96));
            level.Tiles.Add(new Tile(ref endpoint, PhysicalType.Block, 1470, 254, 2, 96));

            level.Tiles.Add(new Tile(ref lava, PhysicalType.Lava, 224, 500, 3));
            level.Tiles.Add(new Tile(ref i, PhysicalType.Block, 0, 500, 6, 32, true));
            level.Tiles.Add(new Tile(ref i, PhysicalType.Block, 320, 500, 10, 32, true));

            level.Tiles.Add(new Tile(ref groundside, PhysicalType.Block, 192, 500, 1, 32, true));
            level.Tiles.Add(new Tile(ref groundsideright, PhysicalType.Block, 320, 500, 1, 32, true));
            level.Tiles.Add(new Tile(ref i, PhysicalType.Block, 224, 532, 3, 32, true));

            level.Tiles.Add(new Tile(ref i, PhysicalType.Block, 750, 432, 10, 32, true));
            level.Tiles.Add(new Tile(ref i, PhysicalType.Block, 1200, 350, 10, 32, true));
            //
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
                    Dead();
                    break;
                default:
                    po.running = false;
                    break;
            }
        }

        public void Dead()
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
            var DeadText = new MenuText("Dead", new Font("Calibri", 72, FontStyle.Bold), Brushes.DarkRed);
            DeadText.y = 25;
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