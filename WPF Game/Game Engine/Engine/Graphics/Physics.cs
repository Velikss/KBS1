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
        Water,
        Spike,
        Block,
        Coin
    }

    [XmlInclude(typeof(PhysicalType))]
    [Serializable]
    public class PhysicalObject
    {
        public delegate void _RegisteredCollision(PhysicalObject po);

        //boolean represents if the Tile is a standable Tile
        internal bool Collidable;

        //rectangle inside object which which contains the collision points
        public Rectangle collision;

        public int Height, Width;
        public PhysicalType physicalType;

        public bool running;

        //contains the sprite for the object
        [XmlIgnore] public Image Sprite;

        //oriental info
        public float X, Y;
        public static event _RegisteredCollision Collided;

        public void Invoke()
        {
            if (!running && physicalType != PhysicalType.Block)
            {
                running = true;
                Collided?.Invoke(this);
            }
        }
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
        public static void EnableGravity(ref Level lvl, ref Render render)
        {
            objects = lvl.Tiles;
            entities = new List<Player>();
            var game = render;
            (gravity = new Thread((ThreadStart) delegate
            {
                for (;;)
                    if (game.isActive())
                        try
                        {
                            //checks for each entity if it has landed, because of man-made input standard collision doesn't suffice therfore a stands boolean is used
                            foreach (var po in entities)
                            {
                                po.Landed = false;
                                foreach (var obj in objects.Where(o =>
                                    o.X - o.Width <= po.X && o.X + o.Width >= po.X))
                                    po.Stands(obj);

                                if (!po.Landed)
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