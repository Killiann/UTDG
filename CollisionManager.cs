using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace UTDG
{
    public class CollisionManager
    {
        protected TileMap map;
        protected Rectangle bounds;

        public bool IsOutOfMap()
        {
            return bounds.X < 0 || bounds.X > map.GetWidth() || bounds.Y < 0 || bounds.Y > map.GetHeight();
        }

        public Rectangle GetBounds()
        {
            return bounds;
        }

        public Rectangle GetMapBounds()
        {
            return new Rectangle(0, 0, map.GetWidth(), map.GetHeight());
        }

        public bool IsColliding(Rectangle r2)
        {
            return bounds.Intersects(r2);
        }

        public virtual void Update(Rectangle bounds) {
            this.bounds = bounds;
        }
    }

    public class ItemCollisionManager: CollisionManager
    {
        public ItemCollisionManager(Rectangle bounds)
        {
            this.bounds = bounds;
        }
    }

    public class PlayerCollisionManager : CollisionManager
    {
        public PlayerCollisionManager(TileMap map)
        {
            this.map = map;
        }
    }
}
