using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading;
using GameEngine;
using WPF_Game.Base_Engine.Audio;
using WPF_Game.Game;

namespace WPF_Game
{
    public partial class MainWindow
    {
        public static MenuText PauseText;
        private readonly GameMaker gm;
        public int CoinCollection;
        public MenuText VictoryHighScoreText;

        public MainWindow()
        {
            InitializeComponent();
            ScoreController.LoadScoreBoard();
            AudioPlayer.Load("on_dead", AppDomain.CurrentDomain.BaseDirectory + "Music/on_dead.wav", false);
            AudioPlayer.Load("background", AppDomain.CurrentDomain.BaseDirectory + @"Music\temp_back.wav", true);
            AudioPlayer.Load("on_coin_collide", AppDomain.CurrentDomain.BaseDirectory + @"Music/coin.wav", false);
            AudioPlayer.Soundtrack.First(o => o.Key == "background").Value.player.Volume = 0.08;
            AudioPlayer.Play("background");
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
                    new Thread((ThreadStart) delegate
                    {
                        gm.movement.DisableKeys();
                        Dispatcher.Invoke(() => AudioPlayer.Play("on_dead"));
                        Thread.Sleep(1000);
                        gm.game_render.Deactivate();
                        gm.Menus[MenuType.Death].Activate();
                    }).Start();

                    break;
                case PhysicalType.EndFlag:
                    gm.game_render.Deactivate();
                    VictoryHighScoreText.Content = "Score: " + CoinCollection;
                    ScoreController.SaveScore(GameMaker.level, CoinCollection);
                    gm.Menus[MenuType.Completed].Activate();
                    break;
                case PhysicalType.Coin:
                    Dispatcher.Invoke(() => AudioPlayer.Play("on_coin_collide"));
                    ((Tile) po).Visible = false;
                    CoinCollection++;
                    break;
                case PhysicalType.Enemy:
                    gm.movement.DisableKeys();
                    Dispatcher.Invoke(() => AudioPlayer.Play("on_dead"));
                    gm.game_render.Deactivate();
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
            var Menus = new Dictionary<MenuType, Menu>();
            //button sprite
            var buttonsprite = Image.FromFile(AppDomain.CurrentDomain.BaseDirectory + "Scene/54b2d246e0e35be.png");
            //Buttons
            var SinglePlayerBtn = new MenuButton("Singleplayer",
                new Font("Munro", 25, System.Drawing.FontStyle.Bold), Brushes.Gainsboro,
                55, 200, 250,
                50, buttonsprite);
            var HighScoresBtn = new MenuButton("High Scores",
                new Font("Munro", 25, System.Drawing.FontStyle.Bold), Brushes.Gainsboro,
                55, 255, 250,
                50, buttonsprite);
            var HighScoreToTitleScrn = new MenuButton("Return to start",
                new Font("Munro", 25, System.Drawing.FontStyle.Bold), Brushes.Gainsboro,
                55, 475, 250,
                50, buttonsprite);
            var ExitBtn = new MenuButton("Exit", new Font("Munro", 25, System.Drawing.FontStyle.Bold),
                Brushes.Gainsboro,
                55, 310, 250,
                50, buttonsprite);
            var PauseRestart = new MenuButton("Restart", new Font("Munro", 25, System.Drawing.FontStyle.Bold),
                Brushes.Gainsboro,
                800 / 2 - 170, 120, 340,
                50, buttonsprite);
            var PauseLvlOptions = new MenuButton("Level Options",
                new Font("Munro", 25, System.Drawing.FontStyle.Bold), Brushes.Gainsboro,
                800 / 2 - 170, 175, 340,
                50, buttonsprite);
            var PauseToTitleScrn = new MenuButton("Return to start",
                new Font("Munro", 25, System.Drawing.FontStyle.Bold),
                Brushes.Gainsboro,
                800 / 2 - 170, 230, 340,
                50, buttonsprite);
            var DeathToTitleScrn = new MenuButton("Return to start",
                new Font("Munro", 25, System.Drawing.FontStyle.Bold),
                Brushes.Gainsboro,
                800 / 2 - 170, 230, 340,
                50, buttonsprite);
            var VictoryToTitleScrn = new MenuButton("Return to start",
                new Font("Munro", 25, System.Drawing.FontStyle.Bold),
                Brushes.Gainsboro,
                800 / 2 - 170, 350, 340,
                50, buttonsprite);
            var DeathLvlOptions = new MenuButton("Level Options",
                new Font("Munro", 25, System.Drawing.FontStyle.Bold), Brushes.Gainsboro,
                800 / 2 - 170, 175, 340,
                50, buttonsprite);
            var DeathRestart = new MenuButton("Restart", new Font("Munro", 25, System.Drawing.FontStyle.Bold),
                Brushes.Gainsboro,
                800 / 2 - 170, 120, 340,
                50, buttonsprite);
            var VictoryRestart = new MenuButton("Restart", new Font("Munro", 25, System.Drawing.FontStyle.Bold),
                Brushes.Gainsboro,
                800 / 2 - 170, 405, 340,
                50, buttonsprite);
            var StartGame = new MenuButton("Start Game", new Font("Munro", 25, System.Drawing.FontStyle.Bold),
                Brushes.Gainsboro,
                800 / 2 - 170, 340, 340,
                50, buttonsprite);
            var PreviousCharacter = new MenuButton("<", new Font("Calibri", 26), Brushes.Gainsboro,
                800 / 2 - 170, 250, 25,
                75, buttonsprite);
            var NextCharacter = new MenuButton(">", new Font("Calibri", 26), Brushes.Gainsboro,
                800 / 2 + 145, 250, 25,
                75, buttonsprite);
            var PreviousLevel = new MenuButton("<", new Font("Calibri", 26), Brushes.Gainsboro,
                800 / 2 - 170, 100, 25,
                75, buttonsprite);
            var NextLevel = new MenuButton(">", new Font("Calibri", 26), Brushes.Gainsboro,
                800 / 2 + 145, 100, 25,
                75, buttonsprite);
            var LevelOptionsToTitleScrn = new MenuButton("Return to start",
                new Font("Munro", 25, System.Drawing.FontStyle.Bold),
                Brushes.Gainsboro,
                800 / 2 - 170, 395, 340,
                50, buttonsprite);

            var GoToNextLevel = new MenuButton("Next Level",
                new Font("Munro", 25, System.Drawing.FontStyle.Bold),
                Brushes.Gainsboro,
                800 / 2 - 170, 295, 340,
                50, buttonsprite);
            //Panels
            var LevelSpriteBack = new MenuPanel(800 / 2 - 145, 100, 75, 75, buttonsprite);
            var LevelTitleBack = new MenuPanel(800 / 2 - 74, 100, 221, 75, buttonsprite);
            var CharacterSpriteBack = new MenuPanel(800 / 2 - 145, 250, 75, 75, buttonsprite);
            var CharacterTitleBack = new MenuPanel(800 / 2 - 74, 250, 221, 75, buttonsprite);
            var OverlayPanel = new MenuPanel(800 / 12 * 3, 0, 800 / 12 * 6, 600,
                Image.FromFile(AppDomain.CurrentDomain.BaseDirectory + "Scene/menu-background.jpg"));
            var LevelOptionsPanel = new MenuPanel(800 / 12 * 3, 0, 800 / 12 * 6, 600,
                Image.FromFile(AppDomain.CurrentDomain.BaseDirectory + "Scene/menu-background.jpg"));
            //Texts
            PauseText =
                new MenuText("Pause", new Font("ArcadeClassic", 60), Brushes.White) {y = 50};
            var DeadText =
                new MenuText("GAME  OVER", new Font("ArcadeClassic", 60), Brushes.White) {y = 50};
            var VictoryText =
                new MenuText("Victory", new Font("ArcadeClassic", 60), Brushes.White) {y = 120};
            VictoryHighScoreText =
                new MenuText("Score: " + CoinCollection, new Font("ArcadeClassic", 60), Brushes.White)
                    {y = 180};
            var CharacterName = new MenuText(Player.CharacterNames[Player.Character_index],
                new Font("Calibri", 32, System.Drawing.FontStyle.Bold),
                Brushes.DarkSlateGray, 800 / 2 - 55, 600 / 2 - 34);
            var LevelName = new MenuText(Level.Levels[Level.Level_index].Name,
                new Font("Calibri", 32, System.Drawing.FontStyle.Bold),
                Brushes.DarkSlateGray, 800 / 2 - 55, 115);
            var SelectLevel =
                new MenuText("Select  Level", new Font("ArcadeClassic", 40), Brushes.White) {y = 50};
            var SelectCharacter =
                new MenuText("Select  Character", new Font("ArcadeClassic", 40),
                        Brushes.White)
                    {y = 200};
            
            var HighScores = new MenuText(ScoreController.GetTopActive(), new Font("ArcadeClassic", 40, System.Drawing.FontStyle.Underline), Brushes.White) { y = 250 };
            
            //Images
            var CharacterSprite = new MenuImage(800 / 2 - 132, 262, 48, 48,
                Image.FromFile("Animations/" + Player.CharacterNames[Player.Character_index] + "/normal.gif"), true);
            var LevelSprite = new MenuImage(800 / 2 - 132, 112, 48, 48,
                Image.FromFile("Levels/" + Level.Levels[Level.Level_index].Name + ".gif"), true);
            var VictorySprite = new MenuImage(368, 50, 64, 64,
                Image.FromFile("Scene/trophy.png"), true);
            //Click events
            LevelOptionsToTitleScrn.Clicked += delegate
            {
                Menus[MenuType.LevelOptions].Deactivate();
                Menus[MenuType.TitleScreen].Activate();
            };
            GoToNextLevel.Clicked += delegate
            {
                if (Level.Level_index != Level.Levels.Count - 1)
                {
                    Menus[MenuType.Completed].Deactivate();
                    Level.Level_index++;
                    gm.StartLevel(true);
                }
                else
                {
                    Level.Level_index = 0;
                    Menus[MenuType.Completed].Deactivate();
                    Menus[MenuType.TitleScreen].Activate();
                }
            };
            NextLevel.Clicked += delegate
            {
                if (Level.Level_index != Level.Levels.Count - 1)
                    Level.Level_index++;
                else
                    Level.Level_index = 0;
                LevelSprite.Sprite = Image.FromFile("Levels/" + Level.Levels[Level.Level_index].Name + ".gif");
                LevelName.Content = Level.Levels[Level.Level_index].Name;
                HighScores.Content = ScoreController.GetTopActive();
            };
            PreviousLevel.Clicked += delegate
            {
                if (Level.Level_index == 0)
                    Level.Level_index = Level.Levels.Count - 1;
                else
                    Level.Level_index--;
                LevelSprite.Sprite = Image.FromFile("Levels/" + Level.Levels[Level.Level_index].Name + ".gif");
                LevelName.Content = Level.Levels[Level.Level_index].Name;
                HighScores.Content = ScoreController.GetTopActive();
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
                    Player.Character_index = Player.CharacterNames.Count - 1;
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
                CoinCollection = 0;
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
            VictoryToTitleScrn.Clicked += delegate
            {
                Menus[MenuType.Completed].Deactivate();
                Menus[MenuType.TitleScreen].Activate();
            };
            DeathRestart.Clicked += delegate
            {
                Menus[MenuType.Death].Deactivate();
                gm.StartLevel(true);
            };
            VictoryRestart.Clicked += delegate
            {
                Menus[MenuType.Completed].Deactivate();
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
            HighScoresBtn.Clicked += delegate
            {
                Menus[MenuType.TitleScreen].Deactivate();
                Menus[MenuType.HighScoreScreen].Activate();
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
            HighScoreToTitleScrn.Clicked += delegate
            {
                Menus[MenuType.HighScoreScreen].Deactivate();
                Menus[MenuType.TitleScreen].Activate();
            };
            //adds to Menu's
            Menus.Add(MenuType.TitleScreen, new Menu(ref gm.screen,
                new List<MenuItem> {SinglePlayerBtn, HighScoresBtn, ExitBtn},
                Image.FromFile(AppDomain.CurrentDomain.BaseDirectory + "Scene/Title.gif")));
            Menus.Add(MenuType.HighScoreScreen, new Menu(ref gm.screen,
                new List<MenuItem>
                {
                    HighScoreToTitleScrn, LevelSpriteBack, LevelTitleBack, PreviousLevel, NextLevel, LevelName,
                    LevelSprite, HighScores
                },
                Image.FromFile(AppDomain.CurrentDomain.BaseDirectory + "Scene/HighScores.gif")));
            Menus.Add(MenuType.Pause, new Menu(ref gm.screen,
                new List<MenuItem> {OverlayPanel, PauseText, PauseToTitleScrn, PauseRestart, PauseLvlOptions},
                null));
            Menus.Add(MenuType.Death, new Menu(ref gm.screen,
                new List<MenuItem> {OverlayPanel, DeadText, DeathToTitleScrn, DeathRestart, DeathLvlOptions},
                null));
            Menus.Add(MenuType.Completed, new Menu(ref gm.screen,
                new List<MenuItem>
                {
                    OverlayPanel, VictorySprite, VictoryText, VictoryHighScoreText, VictoryToTitleScrn, VictoryRestart, GoToNextLevel
                },
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