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
        private Camera camera;
        private Image normal, normal_L;

        #endregion

        public Enemy(int x, int y, int stopFollowingAt, ref Camera cam)
        {
            this.stopFollowingAt = stopFollowingAt;
            X = x;
            Y = y;
            camera = cam;
            normal = Image.FromFile("Animations/enemy/enemy.gif");
            Sprite = normal;
            normal_L = Image.FromFile("Animations/enemy/enemy_L.gif");
            //new Thread(EnemyAnimation).Start();
        }

        private void EnemyAnimation()
        {
            //movement sprite changer to give walking animation
            var sequence_number = 1;
            //keeps last direction for jump_direction
            var last_direction = 2;
            //checks if to switch sequence number
            var switching = true;
            while (true)
            {
                if (sequence_number == 1)
                    switching = true;
                if (sequence_number == 3)
                    switching = false;
                if (switching)
                    sequence_number++;
                else
                    sequence_number--;
                if (camera.Up)
                {
                    if (camera.Left)
                        last_direction = 1;
                    if (camera.Right)
                        last_direction = 2;
                }

                if (!camera.Left && !camera.Right && !camera.Up && !camera.Down)
                    switch (last_direction)
                    {
                        case 1:
                            Sprite = normal_L;
                            break;
                        case 2:
                            Sprite = normal;
                            break;
                    }
                
                Thread.Sleep(100);
            }
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
