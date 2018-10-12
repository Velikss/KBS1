using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading;

namespace GameEngine
{
    public class Enemy : PhysicalObject
    {

        #region variables

        private int stopFollowingAt; //Location Y from visible -until player.Y + (stopFollowingAt + remaining space to the right of the screen)
//        private int circleSize;
        public bool activated;

        #endregion

        public Enemy(int x, int y, int stopFollowingAt)
        {
            Console.WriteLine("abc1");
            this.stopFollowingAt = stopFollowingAt;
            X = x;
            Y = y;
            Sprite = Image.FromFile("Levels/enemy.gif");
//            circleSize = 300;
            new Thread(EnemyAI_Thread).Start();
        }
       
        #region Methods
        
        private void EnemyAI_Thread()
        {
         
            if(activated) {
                while (true)
                {
                    Console.WriteLine("abc2");
                    
                    Thread.Sleep(100);
                }
            }
        }

        public void Start(ref GameRenderer render)
        {
            new Thread(EnemyAI_Thread).Start();
        }
        
        #endregion
        
    }
}
