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
        private int stopFollowingAt;
        public bool activated = true;
        private GameRenderer renderer;
        private Player player;
        private bool inFOV = false;
        private bool audioPlayed = false;
        private int baseX;
        private int baseY;

        #endregion

        public Enemy(int x, int y, int stopFollowingAt)
        {
            this.stopFollowingAt = stopFollowingAt;
            X = x;
            Y = y;
            baseX = x;
            baseY = y;
            Sprite = Image.FromFile("Levels/enemy.gif");
        }

        #region Methods

        private void EnemyAI_Thread()
        {
            while (renderer.isActive())
            {
                if (X < stopFollowingAt)
                {
                
                    if ((player.X < X + 300) && (player.X > X - 300) && audioPlayed == false)
                    {
                        inFOV = true;
                        //TODO: Play "Get your ass back here"
                        audioPlayed = true;
                    }
                    
                    if(inFOV){
                        if (player.X < X)
                        {
                            X = X - 2;
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

                        if ((player.X < X + 10) && (player.X > X - 10) && (player.Y < Y + 10) && (player.Y > Y - 10))
                        {
                            Sprite = Image.FromFile("Levels/explode.gif");
                        }
                    }
                }
                else
                {
                    X = baseX;
                    Y = baseY;
                    audioPlayed = false;
                    inFOV = false;
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

        public void Reset()
        {
            
        }

        #endregion
    }
}