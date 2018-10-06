using System.Drawing;
using System.Threading;

namespace GameEngine
{
    public class Player : PhysicalObject
    {
        //holds reffered cameradata
        private Camera camera;

        //bool states if object is in contact with the ground
        public bool Landed;

        //set all possible sprites to lower memory_use
        Image jump = Image.FromFile("Animations/jump.gif");
        Image jump_L = Image.FromFile("Animations/jump_L.gif");
        Image walk1 = Image.FromFile("Animations/walk1.gif");
        Image walk2 = Image.FromFile("Animations/walk2.gif");
        Image walk3 = Image.FromFile("Animations/walk3.gif");
        Image walk1_L = Image.FromFile("Animations/walk1_L.gif");
        Image walk2_L = Image.FromFile("Animations/walk2_L.gif");
        Image walk3_L = Image.FromFile("Animations/walk3_L.gif");
        Image normal = Image.FromFile("Animations/normal.gif");
        Image normal_L = Image.FromFile("Animations/normal_L.gif");

        private Thread animation;

        //boolean checks collision with object
        public bool Collide(PhysicalObject po)
        {
            if (new Rectangle((int) X + 4, (int) Y + 4, Width - 8, Height - 4).IntersectsWith(po.collision))
            {
                po.Invoke();
                if (po.Collidable)
                    return true;
            }

            return false;
        }

        //boolean checks if stands on top of object
        public void Stands(PhysicalObject po)
        {
            if (new Rectangle((int) X + 4, (int) (Y + 4) + (Height - 4), Width - 8, 1).IntersectsWith(
                new Rectangle((int) po.X, (int) po.Y, po.Width, 1)))
            {
                po.Invoke();
                if (po.Collidable)
                    Landed = true;
            }
        }

        //initialises a new player given reffered camera
        public void Initialize(ref Camera cam)
        {
            camera = cam;
            (animation = new Thread(PlayerAnimation)).Start();
        }

        //changes the player sprite to give animation given movement
        private void PlayerAnimation()
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
                    switch (last_direction)
                    {
                        case 1:
                            Sprite = jump_L;
                            break;
                        case 2:
                            Sprite = jump;
                            break;
                    }
                }
                else
                {
                    if (Landed)
                    {
                        if (camera.Left)
                        {
                            last_direction = 1;
                            switch (sequence_number)
                            {
                                case 1:
                                    Sprite = walk1_L;
                                    break;
                                case 2:
                                    Sprite = walk2_L;
                                    break;
                                case 3:
                                    Sprite = walk3_L;
                                    break;
                            }
                        }

                        if (camera.Right)
                        {
                            last_direction = 2;
                            switch (sequence_number)
                            {
                                case 1:
                                    Sprite = walk1;
                                    break;
                                case 2:
                                    Sprite = walk2;
                                    break;
                                case 3:
                                    Sprite = walk3;
                                    break;
                            }
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
                    }
                    else
                    {
                        switch (last_direction)
                        {
                            case 1:
                                Sprite = jump_L;
                                break;
                            case 2:
                                Sprite = jump;
                                break;
                        }
                    }
                }

                Thread.Sleep(100);
            }
        }

        public void Dispose()
        {
            animation.Abort();
        }
    }
}