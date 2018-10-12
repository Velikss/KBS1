using System.Collections.Generic;
using System.Drawing;
using System.Threading;

namespace GameEngine
{
    public class Enemy : PhysicalObject
    {

        #region variables

        private int stopFollowingAt; //Location Y from visible -until player.Y + (stopFollowingAt + remaining space to the right of the screen)
        private bool visible;
        private int circleSize;

        #endregion

        public Enemy(int x, int y, int stopFollowingAt)
        {
            this.stopFollowingAt = stopFollowingAt;
            X = x;
            Y = y;
            Sprite = Image.FromFile("Levels/enemy.gif");
            circleSize = 300;
        }
        
    }
    
    public class EnemyAi : PhysicalObject
    {

        #region Methods
        
        private void EnemyAI_Thread()
        {
         
            while (true)
            {
                
                
                Thread.Sleep(100);
            }
        }

        public void Start(ref GameRenderer render)
        {
            new Thread(EnemyAI_Thread).Start();
        }
        
        #endregion
        
    }
}
