using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Threading;

namespace GameEngine
{
    public class Player : PhysicalObject
    {
        #region Static

        public static readonly List<string> CharacterNames = new List<string>();
        public static int Character_index;

        public static void InitializeCharacters()
        {
            foreach (var c in Directory.GetDirectories(AppDomain.CurrentDomain.BaseDirectory + "Animations"))
                CharacterNames.Add(Path.GetFileName(c));
        }

        #endregion

        #region Variables

        private Camera camera;
        public bool Landed;
        private Image normal, normal_L, walk1, walk1_L, walk2, walk2_L, walk3, walk3_L, jump, jump_L;

        #endregion

        #region Methods

        public bool Collide(PhysicalObject po)
        {
            if (new Rectangle((int) X + 4, (int) Y, Width - 8, Height).IntersectsWith(po.collision))
            {
                po.Invoke();
                if (po.Collidable)
                    return true;
            }

            return false;
        }

        public bool Stands(PhysicalObject po)
        {
            if (new Rectangle((int) X + 4, (int) (Y + 4) + (Height - 4), Width - 8, 1).IntersectsWith(
                new Rectangle((int) po.X, (int) po.Y, po.Width, 1)))
            {
                po.Invoke();
                if (po.Collidable)
                    return true;
            }

            return false;
        }

        public void Initialize(ref Camera cam)
        {
            camera = cam;
            normal = Image.FromFile("Animations/" + CharacterNames[Character_index] + "/normal.gif");
            normal_L = Image.FromFile("Animations/" + CharacterNames[Character_index] + "/normal_L.gif");
            walk1 = Image.FromFile("Animations/" + CharacterNames[Character_index] + "/walk1.gif");
            walk1_L = Image.FromFile("Animations/" + CharacterNames[Character_index] + "/walk1_L.gif");
            walk2 = Image.FromFile("Animations/" + CharacterNames[Character_index] + "/walk2.gif");
            walk2_L = Image.FromFile("Animations/" + CharacterNames[Character_index] + "/walk2_L.gif");
            walk3 = Image.FromFile("Animations/" + CharacterNames[Character_index] + "/walk3.gif");
            walk3_L = Image.FromFile("Animations/" + CharacterNames[Character_index] + "/walk3_L.gif");
            jump = Image.FromFile("Animations/" + CharacterNames[Character_index] + "/jump.gif");
            jump_L = Image.FromFile("Animations/" + CharacterNames[Character_index] + "/jump_L.gif");
            new Thread(PlayerAnimation).Start();
        }

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

        public void Reset()
        {
            X = 75;
            Y = 330;
            Width = 32;
            Height = 32;
            lock (normal)
            {
                normal = Image.FromFile("Animations/" + CharacterNames[Character_index] + "/normal.gif");
            }

            lock (normal_L)
            {
                normal_L = Image.FromFile("Animations/" + CharacterNames[Character_index] + "/normal_L.gif");
            }

            lock (walk1)
            {
                walk1 = Image.FromFile("Animations/" + CharacterNames[Character_index] + "/walk1.gif");
            }

            lock (walk1_L)
            {
                walk1_L = Image.FromFile("Animations/" + CharacterNames[Character_index] + "/walk1_L.gif");
            }

            lock (walk2)
            {
                walk2 = Image.FromFile("Animations/" + CharacterNames[Character_index] + "/walk2.gif");
            }

            lock (walk2_L)
            {
                walk2_L = Image.FromFile("Animations/" + CharacterNames[Character_index] + "/walk2_L.gif");
            }

            lock (walk3)
            {
                walk3 = Image.FromFile("Animations/" + CharacterNames[Character_index] + "/walk3.gif");
            }

            lock (walk3_L)
            {
                walk3_L = Image.FromFile("Animations/" + CharacterNames[Character_index] + "/walk3_L.gif");
            }

            lock (jump)
            {
                jump = Image.FromFile("Animations/" + CharacterNames[Character_index] + "/jump.gif");
            }

            lock (jump_L)
            {
                jump_L = Image.FromFile("Animations/" + CharacterNames[Character_index] + "/jump_L.gif");
            }
        }

        #endregion
    }
}