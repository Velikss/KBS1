using System.Collections.Generic;
using System.Drawing;
using System.Threading;

namespace GameEngine
{
    public class Enemy
    {

        #region variables

        private int x;
        public int y;
        private Image sprite;
        private int stopFollowingAt; //Location Y from visible -until player.Y + (stopFollowingAt + remaining space to the right of the screen)
        private bool visible;
        private Camera camera;
        private Image normal, normal_L;

        #endregion

        public Enemy(int x, int y, Image sprite, int stopFollowingAt)
        {
            this.x = x;
            this.y = y;
            this.sprite = sprite;
            this.stopFollowingAt = stopFollowingAt;
        }


        public void Initialize(ref Camera cam)
        {
            camera = cam;
            normal = Image.FromFile("Animations/enemy/enemy.gif");
            normal_L = Image.FromFile("Animations/enemy/enemy_L.gif");
            new Thread(EnemyAnimation).Start();
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

        public Image Sprite { get; set; }
    }
}
