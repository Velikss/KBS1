using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading;
using GameEngine;

namespace WPF_Game
{
    public partial class MainWindow
    {
        private readonly GameMaker gm;

        public MainWindow()
        {
            InitializeComponent();
            gm = new GameMaker(this, 800, 600);
            gm.InitializeGame(PrepareMenus());
            Camera.OnFall += Player_Fell;
            PhysicalObject.Collided += ObjectInteraction;
        }
        
        private void ObjectInteraction(PhysicalObject po)
        {
            switch (po.physicalType)
            {
                case PhysicalType.Lava:
                    gm.movement.DisableKeys();
                    new Thread((ThreadStart) delegate
                    {
                        Thread.Sleep(150);
                        gm.game_render.Deactivate();
                        gm.Menus[MenuType.Death].Activate();
                    }).Start();
                    break;
                case PhysicalType.EndFlag:
                    gm.game_render.Deactivate();
                    //Next Level Menu//TODO
                    gm.Menus[MenuType.Death].Activate();
                    break;
                default:
                    po.running = false;
                    break;
            }
        }

        private void Player_Fell()
        {
            gm.game_render.Deactivate();
            gm.Menus[MenuType.Death].Activate();
        }

        private Dictionary<MenuType, Menu> PrepareMenus()
        {
            Dictionary<MenuType, Menu> Menus = new Dictionary<MenuType, Menu>();
            //button sprite
            Image buttonsprite = Image.FromFile(AppDomain.CurrentDomain.BaseDirectory + "Scene/54b2d246e0e35be.png");
            //Buttons
            MenuButton SinglePlayerBtn = new MenuButton("Single player", new Font("Calibri", 26), Brushes.DarkSlateGray,
                55, 200, 250,
                50, buttonsprite);
            MenuButton ExitBtn = new MenuButton("Exit", new Font("Calibri", 26), Brushes.DarkSlateGray, 55, 255, 250,
                50,
                buttonsprite);
            MenuButton PauseRestart = new MenuButton("Restart", new Font("Calibri", 26), Brushes.DarkSlateGray,
                800 / 2 - 170, 120, 340,
                50, buttonsprite);
            MenuButton PauseLvlOptions = new MenuButton("Level Options", new Font("Calibri", 26), Brushes.DarkSlateGray,
                800 / 2 - 170, 175, 340,
                50, buttonsprite);
            MenuButton PauseToTitleScrn = new MenuButton("Return to start", new Font("Calibri", 26),
                Brushes.DarkSlateGray,
                800 / 2 - 170, 230, 340,
                50, buttonsprite);
            MenuButton DeathToTitleScrn = new MenuButton("Return to start", new Font("Calibri", 26),
                Brushes.DarkSlateGray,
                800 / 2 - 170, 230, 340,
                50, buttonsprite);
            MenuButton DeathLvlOptions = new MenuButton("Level Options", new Font("Calibri", 26), Brushes.DarkSlateGray,
                800 / 2 - 170, 175, 340,
                50, buttonsprite);
            MenuButton DeathRestart = new MenuButton("Restart", new Font("Calibri", 26), Brushes.DarkSlateGray,
                800 / 2 - 170, 120, 340,
                50, buttonsprite);
            MenuButton StartGame = new MenuButton("Start Game", new Font("Calibri", 26), Brushes.DarkSlateGray,
                800 / 2 - 170, 340, 340,
                50, buttonsprite);
            MenuButton PreviousCharacter = new MenuButton("<", new Font("Calibri", 26), Brushes.DarkSlateGray,
                800 / 2 - 170, 250, 25,
                75, buttonsprite);
            MenuButton NextCharacter = new MenuButton(">", new Font("Calibri", 26), Brushes.DarkSlateGray,
                800 / 2 + 145, 250, 25,
                75, buttonsprite);
            MenuButton PreviousLevel = new MenuButton("<", new Font("Calibri", 26), Brushes.DarkSlateGray,
                800 / 2 - 170, 100, 25,
                75, buttonsprite);
            MenuButton NextLevel = new MenuButton(">", new Font("Calibri", 26), Brushes.DarkSlateGray,
                800 / 2 + 145, 100, 25,
                75, buttonsprite);
            MenuButton LevelOptionsToTitleScrn = new MenuButton("Return to start", new Font("Calibri", 26),
                Brushes.DarkSlateGray,
                800 / 2 - 170, 395, 340,
                50, buttonsprite);
            //Panels
            MenuPanel LevelSpriteBack = new MenuPanel(800 / 2 - 145, 100, 75, 75, buttonsprite);
            MenuPanel LevelTitleBack = new MenuPanel(800 / 2 - 74, 100, 221, 75, buttonsprite);
            MenuPanel CharacterSpriteBack = new MenuPanel(800 / 2 - 145, 250, 75, 75, buttonsprite);
            MenuPanel CharacterTitleBack = new MenuPanel(800 / 2 - 74, 250, 221, 75, buttonsprite);
            MenuPanel OverlayPanel = new MenuPanel(800 / 12 * 3, 0, 800 / 12 * 6, 320,
                Image.FromFile(AppDomain.CurrentDomain.BaseDirectory + "Scene/pexels-photo-164005.jpeg"));
            MenuPanel LevelOptionsPanel = new MenuPanel(800 / 12 * 3, 0, 800 / 12 * 6, 465,
                Image.FromFile(AppDomain.CurrentDomain.BaseDirectory + "Scene/pexels-photo-164005.jpeg"));
            //Texts
            MenuText PauseText =
                new MenuText("Pause", new Font("Calibri", 72, System.Drawing.FontStyle.Regular),
                    Brushes.White) {y = 25};
            MenuText DeadText =
                new MenuText("Dead", new Font("Calibri", 72, System.Drawing.FontStyle.Bold), Brushes.DarkRed) {y = 25};
            MenuText CharacterName = new MenuText(Player.CharacterNames[Player.Character_index],
                new Font("Calibri", 32, System.Drawing.FontStyle.Bold),
                Brushes.DarkSlateGray, 800 / 2 - 55, 600 / 2 - 34);
            MenuText LevelName = new MenuText(Level.Levels[Level.Level_index].Name,
                new Font("Calibri", 32, System.Drawing.FontStyle.Bold),
                Brushes.DarkSlateGray, 800 / 2 - 55, 115);
            MenuText SelectLevel =
                new MenuText("Select Level", new Font("Calibri", 40, System.Drawing.FontStyle.Regular), Brushes.White)
                {
                    y = 40
                };
            MenuText SelectCharacter =
                new MenuText("Select Character", new Font("Calibri", 40, System.Drawing.FontStyle.Regular),
                    Brushes.White) {y = 190};
            //Images
            MenuImage CharacterSprite = new MenuImage(800 / 2 - 132, 262, 48, 48,
                Image.FromFile("Animations/" + Player.CharacterNames[Player.Character_index] + "/normal.gif"), true);
            MenuImage LevelSprite = new MenuImage(800 / 2 - 132, 112, 48, 48,
                Image.FromFile("Levels/" + Level.Levels[Level.Level_index].Name + ".gif"), true);
            //Click events
            LevelOptionsToTitleScrn.Clicked += delegate
            {
                Menus[MenuType.LevelOptions].Deactivate();
                Menus[MenuType.TitleScreen].Activate();
            };
            NextLevel.Clicked += delegate
            {
                if (Level.Level_index != Level.Levels.Count - 1)
                    Level.Level_index++;
                else
                    Level.Level_index = 0;
                LevelSprite.Sprite = Image.FromFile("Levels/" + Level.Levels[Level.Level_index].Name + ".gif");
                LevelName.Content = Level.Levels[Level.Level_index].Name;
            };
            PreviousLevel.Clicked += delegate
            {
                if (Level.Level_index == 0)
                    Level.Level_index = (Level.Levels.Count - 1);
                else
                    Level.Level_index--;
                LevelSprite.Sprite = Image.FromFile("Levels/" + Level.Levels[Level.Level_index].Name + ".gif");
                LevelName.Content = Level.Levels[Level.Level_index].Name;
            };
            NextCharacter.Clicked += delegate
            {
                if (Player.Character_index != Player.CharacterNames.Count - 1)
                    Player.Character_index++;
                else
                    Player.Character_index = 0;
                CharacterSprite.Sprite =
                    Image.FromFile("Animations/" + Player.CharacterNames[Player.Character_index] + "/normal.gif");
                CharacterName.Content = Player.CharacterNames[Player.Character_index];
            };
            PreviousCharacter.Clicked += delegate
            {
                if (Player.Character_index == 0)
                    Player.Character_index = (Player.CharacterNames.Count - 1);
                else
                    Player.Character_index -= 1;
                CharacterSprite.Sprite =
                    Image.FromFile("Animations/" + Player.CharacterNames[Player.Character_index] + "/normal.gif");
                CharacterName.Content = Player.CharacterNames[Player.Character_index];
            };
            StartGame.Clicked += delegate
            {
                Menus[MenuType.LevelOptions].Deactivate();
                gm.StartLevel(true);
            };
            DeathLvlOptions.Clicked += delegate
            {
                Menus[MenuType.Death].Deactivate();
                Menus[MenuType.LevelOptions].Activate();
            };
            DeathToTitleScrn.Clicked += delegate
            {
                Menus[MenuType.Death].Deactivate();
                Menus[MenuType.TitleScreen].Activate();
            };
            DeathRestart.Clicked += delegate
            {
                Menus[MenuType.Death].Deactivate();
                gm.StartLevel(true);
            };
            SinglePlayerBtn.Clicked += delegate
            {
                Menus[MenuType.TitleScreen].Deactivate();
                Menus[MenuType.LevelOptions].Activate();
            };
            ExitBtn.Clicked += delegate { Environment.Exit(0); };
            PauseRestart.Clicked += delegate
            {
                Menus[MenuType.Pause].Deactivate();
                gm.StartLevel(true);
            };
            PauseLvlOptions.Clicked += delegate
            {
                Menus[MenuType.Pause].Deactivate();
                Menus[MenuType.LevelOptions].Activate();
            };
            PauseToTitleScrn.Clicked += delegate
            {
                Menus[MenuType.Pause].Deactivate();
                Menus[MenuType.TitleScreen].Activate();
            };
            //adds to Menu's
            Menus.Add(MenuType.TitleScreen, new Menu(ref gm.screen, new List<MenuItem> {SinglePlayerBtn, ExitBtn},
                Image.FromFile(AppDomain.CurrentDomain.BaseDirectory + "Scene/Title.gif")));
            Menus.Add(MenuType.Pause, new Menu(ref gm.screen,
                new List<MenuItem> {OverlayPanel, PauseText, PauseToTitleScrn, PauseRestart, PauseLvlOptions},
                null));
            Menus.Add(MenuType.Death, new Menu(ref gm.screen,
                new List<MenuItem> {OverlayPanel, DeadText, DeathToTitleScrn, DeathRestart, DeathLvlOptions},
                null));
            Menus.Add(MenuType.LevelOptions, new Menu(ref gm.screen,
                new List<MenuItem>
                {
                    LevelOptionsPanel,
                    StartGame,
                    PreviousCharacter,
                    CharacterSpriteBack,
                    CharacterTitleBack,
                    CharacterSprite,
                    NextCharacter,
                    CharacterName,
                    LevelSpriteBack,
                    LevelTitleBack,
                    PreviousLevel,
                    NextLevel,
                    LevelName,
                    LevelSprite,
                    SelectCharacter,
                    SelectLevel,
                    LevelOptionsToTitleScrn
                },
                Image.FromFile(AppDomain.CurrentDomain.BaseDirectory + "Scene/back.gif")));
            return Menus;
        }
    }
}