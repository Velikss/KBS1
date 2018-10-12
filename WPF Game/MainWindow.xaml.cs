using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Windows.Media;
using BaseEngine;
using GameEngine;
using WPF_Game.Base_Engine.Audio;
using WPF_Game.Game;
using Brushes = System.Drawing.Brushes;

namespace WPF_Game
{
    public partial class MainWindow
    {
        private readonly GameMaker gm;
        public int CoinCollection;
        public MenuText VictoryHighScoreText;

        public MainWindow()
        {
            InitializeComponent();
            ScoreController.LoadScoreBoard();
            
            /*
            AudioPlayer.Load("background", @"C:\Users\usr\Downloads\1.wav", true);
            AudioPlayer.Play("background");
            */

            AudioPlayer.Load("fuck you", @"C:\Users\usr\Documents\GitHub\Runch\WPF Game\bin\Debug\Music\on_enemy_kill.wav", true);
            AudioPlayer.Play("fuck you");
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
                    break;
                case PhysicalType.EndFlag:
                    gm.game_render.Deactivate();
                    VictoryHighScoreText.Content = "Score: " + CoinCollection.ToString();
                    ScoreController.SaveScore(gm.level, CoinCollection);
                    gm.Menus[MenuType.Completed].Activate();
                    break;
                case PhysicalType.Coin:
                    ((Tile) po).Visible = false;
                    CoinCollection++;
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
            MenuButton SinglePlayerBtn = new MenuButton("Singleplayer",
                new Font("Munro", 25, System.Drawing.FontStyle.Bold), Brushes.Gainsboro,
                55, 200, 250,
                50, buttonsprite);
            MenuButton HighScoresBtn = new MenuButton("High Scores",
                new Font("Munro", 25, System.Drawing.FontStyle.Bold), Brushes.Gainsboro,
                55, 255, 250,
                50, buttonsprite);
            MenuButton HighScoreToTitleScrn = new MenuButton("Return to start",
                new Font("Munro", 25, System.Drawing.FontStyle.Bold), Brushes.Gainsboro,
                55, 475, 250,
                50, buttonsprite);
            MenuButton ExitBtn = new MenuButton("Exit", new Font("Munro", 25, System.Drawing.FontStyle.Bold),
                Brushes.Gainsboro,
                55, 310, 250,
                50, buttonsprite);
            MenuButton PauseRestart = new MenuButton("Restart", new Font("Munro", 25, System.Drawing.FontStyle.Bold),
                Brushes.Gainsboro,
                800 / 2 - 170, 120, 340,
                50, buttonsprite);
            MenuButton PauseLvlOptions = new MenuButton("Level Options",
                new Font("Munro", 25, System.Drawing.FontStyle.Bold), Brushes.Gainsboro,
                800 / 2 - 170, 175, 340,
                50, buttonsprite);
            MenuButton PauseToTitleScrn = new MenuButton("Return to start",
                new Font("Munro", 25, System.Drawing.FontStyle.Bold),
                Brushes.Gainsboro,
                800 / 2 - 170, 230, 340,
                50, buttonsprite);
            MenuButton DeathToTitleScrn = new MenuButton("Return to start",
                new Font("Munro", 25, System.Drawing.FontStyle.Bold),
                Brushes.Gainsboro,
                800 / 2 - 170, 230, 340,
                50, buttonsprite);
            MenuButton VictoryToTitleScrn = new MenuButton("Return to start",
                new Font("Munro", 25, System.Drawing.FontStyle.Bold),
                Brushes.Gainsboro,
                800 / 2 - 170, 350, 340,
                50, buttonsprite);
            MenuButton DeathLvlOptions = new MenuButton("Level Options",
                new Font("Munro", 25, System.Drawing.FontStyle.Bold), Brushes.Gainsboro,
                800 / 2 - 170, 175, 340,
                50, buttonsprite);
            MenuButton DeathRestart = new MenuButton("Restart", new Font("Munro", 25, System.Drawing.FontStyle.Bold),
                Brushes.Gainsboro,
                800 / 2 - 170, 120, 340,
                50, buttonsprite);
            MenuButton VictoryRestart = new MenuButton("Restart", new Font("Munro", 25, System.Drawing.FontStyle.Bold),
                Brushes.Gainsboro,
                800 / 2 - 170, 405, 340,
                50, buttonsprite);
            MenuButton StartGame = new MenuButton("Start Game", new Font("Munro", 25, System.Drawing.FontStyle.Bold),
                Brushes.Gainsboro,
                800 / 2 - 170, 340, 340,
                50, buttonsprite);
            MenuButton PreviousCharacter = new MenuButton("<", new Font("Calibri", 26), Brushes.Gainsboro,
                800 / 2 - 170, 250, 25,
                75, buttonsprite);
            MenuButton NextCharacter = new MenuButton(">", new Font("Calibri", 26), Brushes.Gainsboro,
                800 / 2 + 145, 250, 25,
                75, buttonsprite);
            MenuButton PreviousLevel = new MenuButton("<", new Font("Calibri", 26), Brushes.Gainsboro,
                800 / 2 - 170, 100, 25,
                75, buttonsprite);
            MenuButton NextLevel = new MenuButton(">", new Font("Calibri", 26), Brushes.Gainsboro,
                800 / 2 + 145, 100, 25,
                75, buttonsprite);
            MenuButton LevelOptionsToTitleScrn = new MenuButton("Return to start",
                new Font("Munro", 25, System.Drawing.FontStyle.Bold),
                Brushes.Gainsboro,
                800 / 2 - 170, 395, 340,
                50, buttonsprite);
            //Panels
            MenuPanel LevelSpriteBack = new MenuPanel(800 / 2 - 145, 100, 75, 75, buttonsprite);
            MenuPanel LevelTitleBack = new MenuPanel(800 / 2 - 74, 100, 221, 75, buttonsprite);
            MenuPanel CharacterSpriteBack = new MenuPanel(800 / 2 - 145, 250, 75, 75, buttonsprite);
            MenuPanel CharacterTitleBack = new MenuPanel(800 / 2 - 74, 250, 221, 75, buttonsprite);
            MenuPanel OverlayPanel = new MenuPanel(800 / 12 * 3, 0, 800 / 12 * 6, 600,
                Image.FromFile(AppDomain.CurrentDomain.BaseDirectory + "Scene/menu-background.jpg"));
            MenuPanel LevelOptionsPanel = new MenuPanel(800 / 12 * 3, 0, 800 / 12 * 6, 600,
                Image.FromFile(AppDomain.CurrentDomain.BaseDirectory + "Scene/menu-background.jpg"));
            //Texts
            MenuText PauseText =
                new MenuText("Pause", new Font("ArcadeClassic", 60), Brushes.White) {y = 50};
            MenuText DeadText =
                new MenuText("GAME  OVER", new Font("ArcadeClassic", 60), Brushes.White) {y = 50};
            MenuText VictoryText =
                new MenuText("Victory", new Font("ArcadeClassic", 60), Brushes.White) {y = 120};
            VictoryHighScoreText =
                new MenuText("Score: " + CoinCollection.ToString(), new Font("ArcadeClassic", 60), Brushes.White)
                    {y = 180};
            MenuText CharacterName = new MenuText(Player.CharacterNames[Player.Character_index],
                new Font("Calibri", 32, System.Drawing.FontStyle.Bold),
                Brushes.DarkSlateGray, 800 / 2 - 55, 600 / 2 - 34);
            MenuText LevelName = new MenuText(Level.Levels[Level.Level_index].Name,
                new Font("Calibri", 32, System.Drawing.FontStyle.Bold),
                Brushes.DarkSlateGray, 800 / 2 - 55, 115);
            MenuText SelectLevel =
                new MenuText("Select  Level", new Font("ArcadeClassic", 40), Brushes.White) {y = 50};
            MenuText SelectCharacter =
                new MenuText("Select  Character", new Font("ArcadeClassic", 40),
                        Brushes.White)
                    {y = 200};
       
            ScoreController.Score[] scrs = ScoreController.Scores.Where(o => o.LevelName == "Stage 1").OrderBy(i => i.score).Take(5).ToArray();
            MenuText HighScores = new MenuText(scrs.ToString(), new Font("ArcadeClassic", 40), Brushes.White) { y = 250 };
            //Images
            MenuImage CharacterSprite = new MenuImage(800 / 2 - 132, 262, 48, 48,
                Image.FromFile("Animations/" + Player.CharacterNames[Player.Character_index] + "/normal.gif"), true);
            MenuImage LevelSprite = new MenuImage(800 / 2 - 132, 112, 48, 48,
                Image.FromFile("Levels/" + Level.Levels[Level.Level_index].Name + ".gif"), true);
            MenuImage VictorySprite = new MenuImage(368, 50, 64, 64,
                Image.FromFile("Scene/trophy.png"), true);
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
                    OverlayPanel, VictorySprite, VictoryText, VictoryHighScoreText, VictoryToTitleScrn, VictoryRestart
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