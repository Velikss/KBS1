using System;
using System.Collections.Generic;
using System.Windows;
using BaseEngine;

namespace GameEngine
{
    public class GameMaker
    {
        public GameMaker(Window w, int _w, int _h)
        {
            Player.InitializeCharacters();
            Level.LoadLevels(AppDomain.CurrentDomain.BaseDirectory + "Levels");
            level = Level.Load(Level.Level_index);
            camera = new Camera(ref level);
            player = new Player();
            player.Initialize(ref camera);
            screen = new Screen(ref w, _w, _h);
            game_render = new GameRenderer(ref screen, ref player, ref camera, ref level);
            camera.Start(ref game_render, ref player);
        }

        #region Variables

        public Dictionary<MenuType, Menu> Menus = new Dictionary<MenuType, Menu>();
        private Camera camera;
        public Movement movement;
        public GameRenderer game_render;
        public Level level;
        private Player player;
        public Screen screen;

        #endregion

        #region Methods

        public void InitializeGame(Dictionary<MenuType, Menu> Menus)
        {
            this.Menus = Menus;
            movement = new Movement(ref screen, ref player, ref camera, ref game_render, ref Menus);
            Menus[MenuType.TitleScreen].Activate();
        }

        public void StartLevel(bool StartGame = false)
        {
            player.Reset();
            camera.Reset();
            movement.EnableKeys();
            game_render.ChangeLevelData(level = Level.Load(Level.Level_index));
            Gravity.Dispose();
            Gravity.EnableGravity(ref level, ref game_render);
            Gravity.EnableGravityOnObject(player);
            if (StartGame)
            {
                game_render.Activate();
                foreach (var enemy in level.Enemies)
                    enemy.Start(ref game_render, ref player);
            }
        }

        #endregion
    }
}