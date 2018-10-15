using System.Drawing;
using System.Threading;

namespace GameEngine
{
    public class Enemy : PhysicalObject
    {
        #region variables

        private readonly int stopFollowingAt;
        private GameRenderer renderer;
        private Player player;
        private readonly int baseX;
        private readonly int baseY;

        #endregion

        public Enemy(int x, int y, int stopFollowingAt)
        {
            this.stopFollowingAt = stopFollowingAt;
            X = x;
            Y = y;
            baseX = x;
            baseY = y;
            Sprite = Image.FromFile("Levels/enemy.gif");
            physicalType = PhysicalType.Enemy;
        }

        #region Methods

        private void EnemyAI_Thread()
        {
            while (renderer.isActive())
            {
                if ((player.X < X + stopFollowingAt) && (player.X > X - stopFollowingAt))
                {
                    //TODO: Play "Get your ass back here"
                    if (player.X < X)
                        X = X - 2;

                    if (player.X > X)
                        X = X + 2;

                    if (player.Y > Y)
                        Y++;

                    if (player.Y < Y)
                        Y--;

                    if ((player.X < X + 10) && (player.X > X - 10) && (player.Y < Y + 10) && (player.Y > Y - 10))
                    {
                        Sprite = Image.FromFile("Levels/explode.gif");
                        Invoke();
                    }
                }
                else
                {
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