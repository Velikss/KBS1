using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading;

namespace GameEngine
{
    public class Enemy : PhysicalObject
    {
        #region variables

        private int
            stopFollowingAt; //Location Y from visible -until player.Y + (stopFollowingAt + remaining space to the right of the screen)

//        private int circleSize;
        public bool activated = true;
        private GameRenderer renderer;
        private Player player;

        #endregion

        public Enemy(int x, int y, int stopFollowingAt)
        {
            Console.WriteLine("abc1");
            this.stopFollowingAt = stopFollowingAt;
            X = x;
            Y = y;
            Sprite = Image.FromFile("Levels/enemy.gif");
//            circleSize = 300;
        }

        #region Methods

        private void EnemyAI_Thread()
        {
            while (renderer.isActive())
            {
                Console.WriteLine("Active");
                X++;
                Thread.Sleep(100);
            }
        }

        public void Start(ref GameRenderer render, ref Player player)
        {
            this.renderer = render;
            this.player = player;
            new Thread(EnemyAI_Thread).Start();
        }

        #endregion
    }
}