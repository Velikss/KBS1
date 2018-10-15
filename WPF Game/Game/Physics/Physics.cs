using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Xml.Serialization;

namespace GameEngine
{
    public enum PhysicalType
    {
        Lava,
        Coin,
        BeginFlag,
        EndFlag,
        Enemy,
        Ground,
        GroundSide,
        GroundSideRight
    }

    [XmlInclude(typeof(PhysicalType))]
    [Serializable]
    public class PhysicalObject
    {
        public void Invoke(string arguments = "")
        {
            if (running) return;
            running = true;
            Collided?.Invoke(this, arguments);
        }

        #region EventVariables

        public delegate void _RegisteredCollision(PhysicalObject po, string arguments);

        public static event _RegisteredCollision Collided;

        #endregion

        #region Variables

        public bool Collidable, running;
        public Rectangle collision;
        public int Height, Width;
        public PhysicalType physicalType;
        [XmlIgnore] public Image Sprite;
        public float X, Y;

        #endregion
    }

    public static class Gravity
    {
        //holds the enities to engage gravity on
        private static List<Player> entities = new List<Player>();

        //holds possible collision objects reffered by Tiles from GameMaker.cs
        private static List<Tile> objects;

        private static Thread gravity;

        //void enables gravity on parametered object
        public static void EnableGravityOnObject(Player po)
        {
            lock (entities)
            {
                entities.Add(po);
            }
        }

        //disables gravity for parametered object
        public static void DisableGravityOnObject(Player po)
        {
            lock (entities)
            {
                entities.Remove(po);
            }
        }

        //starts the gravity given reffered objects to collide with
        public static void EnableGravity(ref Level lvl, ref GameRenderer render)
        {
            objects = lvl.Tiles;
            entities = new List<Player>();
            var game = render;
            bool landed;
            (gravity = new Thread((ThreadStart) delegate
            {
                for (;;)
                    if (game.isActive())
                        try
                        {
                            //checks for each entity if it has landed, because of man-made input standard collision doesn't suffice therfore a stands boolean is used
                            foreach (var po in entities)
                            {
                                landed = false;
                                foreach (var obj in objects.Where(o =>
                                    o.X - o.Width <= po.X && o.X + o.Width >= po.X))
                                    if (po.Stands(obj))
                                        landed = true;

                                if (!(po.Landed = landed))
                                    po.Y += 0.95f;
                                else
                                    Movement.jumps = 0;
                            }

                            Thread.Sleep(1);
                        }
                        catch
                        {
                            //nothing
                        }
            })).Start();
        }

        //checks if gravity is engaged on the given player
        public static bool HasGravity(Player player)
        {
            return entities.Contains(player);
        }

        public static void Dispose()
        {
            gravity?.Abort();
            objects = null;
            entities = null;
        }
    }
}