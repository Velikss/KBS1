using System;
using System.Linq;
using System.Threading;

namespace GameEngine
{
    public class Camera
    {
        public Camera(ref Level lvl)
        {
            this.lvl = lvl;
        }

        #region Variables

        private Player player;
        private GameRenderer render;
        private Level lvl;
        public bool Up, Down, Left, Right;
        public float X, Y;

        #endregion

        #region EventVariables

        public delegate void _Fall();

        public static event _Fall OnFall;

        #endregion

        #region Methods

        private void CameraMovement_Thread()
        {
            while (true)
                if (render.isActive())
                {
                    try
                    {
                        if (player.Y > 650)
                            OnFall?.Invoke();

                        //because of gravity being in a diffrent thread it checks if it has to move the camera to keep focus in case of falling etc.
                        if (Math.Abs(player.Y - Y * -1) > 425 && player.Y <= 470)
                            Y -= 0.7f;
                        if (Up)
                        {
                            //check if collision is present otherwise move player to given direction
                            if (lvl.Tiles.Count(o => o.Y <= player.Y && player.Collide(o) && o.Collidable) == 0)
                            {
                                if (Math.Abs(player.Y - Y * -1) < 125)
                                    Y += 0.8f;
                                player.Y -= 0.75f;
                            }
                            else
                            {
                                player.Y += 1;
                                Up = false;
                            }
                        }

                        if (Left)
                        {
                            //check if collision is present otherwise move player to given direction
                            if (player.X > 0 &&
                                lvl.Tiles.Count(o => o.X <= player.X && player.Collide(o) && o.Collidable) == 0)
                            {
                                if (X + player.X < 250 && X < 0)
                                    X += 0.45f;
                                player.X -= 0.45f;
                            }
                            else
                            {
                                player.X += 1;
                                Left = false;
                            }
                        }

                        if (Right)
                        {
                            //check if collision is present otherwise move player to given direction
                            if (lvl.Tiles.Count(o => o.X >= player.X && player.Collide(o) && o.Collidable) == 0)
                            {
                                if (X + player.X > 350)
                                    X -= 0.45f;
                                player.X += 0.45f;
                            }
                            else
                            {
                                player.X -= 1;
                                Right = false;
                            }
                        }
                    }
                    catch
                    {
                    }

                    Thread.Sleep(1);
                }
        }

        public void Start(ref GameRenderer render, ref Player player)
        {
            this.render = render;
            this.player = player;
            new Thread(CameraMovement_Thread).Start();
        }

        public void Reset(ref Level level)
        {
            X = 0;
            Y = 0;
            Down = false;
            Up = false;
            Left = false;
            Right = false;
            lvl = level;
        }

        #endregion
    }
}