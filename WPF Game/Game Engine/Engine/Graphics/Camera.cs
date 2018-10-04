using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace GameEngine
{
    public class Camera
    {
        //holds the reffered playerdata from GameMaker.cs
        private readonly Player player;

        //holds the reffered TileData from GameMaker.cs
        private readonly List<Tile> Tiles;

        //camera movement pro loop
        public bool Up, Down, Left, Right;

        //camera position
        public float X, Y;

        //render
        public Render render;

        //set's up camera given reffered player, Tiles
        public Camera(ref Player p, ref List<Tile> Tiles, ref Render render)
        {
            player = p;
            this.Tiles = Tiles;
            this.render = render;
        }

        private void CameraMovement_Thread()
        {
            while (true)
            {
                if (render.isActive())
                {
                    //because of gravity being in a diffrent thread it checks if it has to move the camera to keep focus in case of falling etc.
                    if (Math.Abs(player.Y - Y * -1) > 425)
                        Y -= 0.7f;
                    if (Up)
                    {
                        //check if collision is present otherwise move player to given direction
                        if (Tiles.Count(o => o.Y <= player.Y && player.Collide(o)) == 0)
                        {
                            if (Math.Abs(player.Y - Y * -1) < 125)
                                Y += 0.8f;
                            player.Y -= 0.75f;
                        }
                        else
                        {
                            Gravity.EnableGravityOnObject(player);
                            Up = false;
                        }
                    }

                    if (Left)
                    {
                        //check if collision is present otherwise move player to given direction
                        if (player.X != 0 && Tiles.Count(o => o.X <= player.X && player.Collide(o)) == 0)
                        {
                            if (X + player.X < 175 && X != 0)
                                X += 0.45f;
                            player.X -= 0.45f;
                        }
                        else
                        {
                            if (!Gravity.HasGravity(player))
                                Gravity.EnableGravityOnObject(player);
                            Left = false;
                        }
                    }

                    if (Right)
                    {
                        //check if collision is present otherwise move player to given direction
                        if (Tiles.Count(o => o.X >= player.X && player.Collide(o)) == 0)
                        {
                            if (X + player.X > 625)
                                X -= 0.45f;
                            player.X += 0.45f;
                        }
                        else
                        {
                            if (!Gravity.HasGravity(player))
                                Gravity.EnableGravityOnObject(player);
                            Right = false;
                        }
                    }
                }

                Thread.Sleep(1);
            }
        }

        //start's camera
        public void Start()
        {
            new Thread(CameraMovement_Thread).Start();
        }
    }
}