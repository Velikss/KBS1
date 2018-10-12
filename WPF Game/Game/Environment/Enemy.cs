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
        private bool inFOV;
        private bool audioPlayed = false;

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
                if (player.X > X - 300 || audioPlayed != true)
                {
                    inFOV = true;
                    //TODO: Play "Get your ass back here"
                    audioPlayed = true;
                } 
                
                if(inFOV){
                    if (player.X < X)
                    {
                        X = X + 2;
                    }
    
                    if (player.X > X)
                    {
                        X = X + 2;
                    }
    
                    if (player.Y > Y)
                    {
                        Y++;
                    }
    
                    if (player.Y < Y)
                    {
                        Y--;
                    }
                }
                Thread.Sleep(12);
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