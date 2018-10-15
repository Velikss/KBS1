using System.Drawing;
using System.Threading;

namespace GameEngine
{
    public class Enemy : PhysicalObject
    {
        private Enemy()
        {
        }

        private bool played = false;
        private const int Killingrange = 32;

        public Enemy(int x, int y, int stopFollowingAt)
        {
            this.stopFollowingAt = stopFollowingAt;
            X = x;
            Y = y;
            baseX = x;
            baseY = y;
            physicalType = PhysicalType.Enemy;
            Sprite = Image.FromFile("Scene/enemy.gif");
        }

        #region variables

        public int stopFollowingAt;
        private GameRenderer renderer;
        private Player player;
        public int baseX;
        public int baseY;

        #endregion

        #region Methods

        private void EnemyAI_Thread()
        {
            while (renderer.isActive())
            {
                if (player.X < X + stopFollowingAt && player.X > X - stopFollowingAt)
                {
                    if (!played)
                    {
                        played = true;
                        Invoke("outside");
                    }

                    if (player.X < X)
                        X = X - 2;

                    if (player.X > X)
                        X = X + 2;

                    if (player.Y > Y)
                        Y++;

                    if (player.Y < Y)
                        Y--;

                    if (player.X < X + Killingrange && player.X > X - Killingrange && player.Y < Y + Killingrange &&
                        player.Y > Y - Killingrange)
                    {
                        Sprite = Image.FromFile("Scene/explode.gif");
                        Invoke("kill");
                    }
                }
                else
                {
                    played = false;
                    if (X > baseX)
                        X -= 2;
                    if (X < baseX)
                        X += 2;
                    if (Y > baseY)
                        Y -= 2;
                    if (Y < baseY)
                        Y += 2;
                }

                Thread.Sleep(10);
            }

            X = baseX;
            Y = baseY;
            Sprite = Image.FromFile("Scene/enemy.gif");
        }

        public void Start(ref GameRenderer renderer, ref Player player)
        {
            this.renderer = renderer;
            this.player = player;
            new Thread(EnemyAI_Thread).Start();
        }

        #endregion
    }
}