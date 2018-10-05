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

        //holds player instance
        internal Player player;

        //Screen
        internal Screen screen;
        internal int Screen_Height;
        internal int Screen_Width;

        //holds all tiles
        internal Level level;
        private Menu TitleMenu;

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
            level = new Level("level1");
            level.Tiles.Add(new Tile(ref i, 0, 532, 20, 32, true));
            level.Tiles.Add(new Tile(ref i, 750, 332, 10, 32, true));
            level.Tiles.Add(new Tile(ref i, 1250, 150, 10, 32, true));
            //
            screen = new Screen(this);
            game_render = new Render(this);
            PrepareLevel();
            PrepareMenus();
            new Movement(this);
            TitleMenu.Activate();
        }

        private void PrepareMenus()
        {
            var buttonsprite = Image.FromFile(AppDomain.CurrentDomain.BaseDirectory + "Scene/54b2d246e0e35be.png");
            var mb = new MenuButton("Start Game", new Font("Calibri", 16), Brushes.DarkSlateGray, 55, 200, 250,
                50, buttonsprite);
            var mb2 = new MenuButton("Exit Game", new Font("Calibri", 16), Brushes.DarkSlateGray, 55, 255, 250, 50,
                buttonsprite);
            TitleMenu = new Menu(this, new List<MenuItem> {mb, mb2},
                Image.FromFile(AppDomain.CurrentDomain.BaseDirectory + "Scene/Title.gif"));
            var Panel = new MenuPanel(800 / 10 * 3, 0, 800 / 12 * 4, 500,
                Image.FromFile(AppDomain.CurrentDomain.BaseDirectory + "Scene/threeRock.gif"));
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
            PauseOverlay = new Menu(this, new List<MenuItem> {Panel, Text, totitle},
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
            Gravity.EnableGravity(ref level, ref game_render);
            Gravity.EnableGravityOnObject(player);
            if (StartGame)
                game_render.Activate();
        }
    }
}