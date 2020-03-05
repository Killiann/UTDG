using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace UTDG
{
    public class CollisionHandler
    {
        protected TileMap map;
        protected Rectangle bounds;

        public virtual bool IsOutOfMap()
        {
            return bounds.X < 0 || bounds.X > map.GetWidth() || bounds.Y < 0 || bounds.Y > map.GetHeight();
        }

        public virtual Rectangle GetBounds()
        {
            return bounds;
        }

        public virtual Rectangle GetMapBounds()
        {
            return new Rectangle(0, 0, map.GetWidth(), map.GetHeight());
        }

        public virtual bool IsColliding(Rectangle r2)
        {
            return bounds.Intersects(r2);
        }

        public virtual void Update(GameObj gameObj) {
            bounds = new Rectangle((int)gameObj.GetPosition().X, (int)gameObj.GetPosition().Y, (int)gameObj.GetDimensions().X, (int)gameObj.GetDimensions().Y);
        }
    }

    public class ItemCollisionHandler: CollisionHandler
    {
        public ItemCollisionHandler(Rectangle bounds)
        {
            this.bounds = bounds;
        }
    }

    public class BulletCollisionHandler : CollisionHandler
    {
        List<Tile> collisionTiles = new List<Tile>();
        public BulletCollisionHandler(TileMap map)
        {
            collisionTiles = map.GetCollisionTiles();
        }
        public override void Update(GameObj bullet)
        {
            foreach (Tile t in collisionTiles)
            {
                if (bullet.GetBounds().Intersects(t.GetBounds()))
                    ((Bullet)bullet).canBeDestroyed = true;
            }
        }
    }

    public class EntityCollisionHandler : CollisionHandler
    {
        public enum Direction
        {
            Up,
            Right,
            Down,
            Left
        }

        private readonly List<Tile> mapTiles = new List<Tile>();
        private List<Tile> intersectingTiles = new List<Tile>();
        private DynamicObj entity;

        public EntityCollisionHandler(TileMap map)
        {
            this.map = map;
            mapTiles = map.GetCollisionTiles();
        }

        public new virtual void Update(GameObj e)
        {
            entity = (DynamicObj)e;

            //x axis tile collisions
            bounds = new Rectangle(entity.GetBounds().X + (int)entity.GetVelocity().X, entity.GetBounds().Y, entity.GetBounds().Width, entity.GetBounds().Height);
            Rectangle inflatedBounds = bounds;
            inflatedBounds.Inflate(10, 10);
            intersectingTiles = mapTiles.Where(x => inflatedBounds.Intersects(x.GetBounds())).ToList();

            if (entity.GetVelocity().X > 0)
                CheckAndHandleCollision(Direction.Right);
            else if (entity.GetVelocity().X < 0)
                CheckAndHandleCollision(Direction.Left);
            //y axis tile collisions
            inflatedBounds = bounds;
            inflatedBounds.Inflate(10, 10);
            intersectingTiles = mapTiles.Where(x => inflatedBounds.Intersects(x.GetBounds())).ToList();

            bounds = new Rectangle(entity.GetBounds().X, entity.GetBounds().Y + (int)entity.GetVelocity().Y, entity.GetBounds().Width, entity.GetBounds().Height);

            if (entity.GetVelocity().Y > 0)
                CheckAndHandleCollision(Direction.Down);
            else if (entity.GetVelocity().Y < 0)
                CheckAndHandleCollision(Direction.Up);
        }

        private void CheckAndHandleCollision(Direction direction)
        {
            foreach (Tile t in intersectingTiles)
            {
                if (bounds.Intersects(t.GetBounds()))
                {
                    switch (direction)
                    {
                        case (Direction.Up):
                            entity.SetYVelocity(0);
                            entity.SetCollidingVer(true);
                            entity.SetYPosition(t.GetBounds().Y + t.GetBounds().Height);
                            break;
                        case (Direction.Right):
                            entity.SetXVelocity(0);
                            entity.SetCollidingHor(true);
                            entity.SetXPosition(t.GetBounds().X - t.GetBounds().Width);
                            break;
                        case (Direction.Down):
                            entity.SetYVelocity(0);
                            entity.SetCollidingVer(true);
                            entity.SetYPosition(t.GetBounds().Y - entity.GetBounds().Height);
                            break;
                        case (Direction.Left):
                            entity.SetXVelocity(0);
                            entity.SetCollidingHor(true);
                            entity.SetXPosition(t.GetBounds().X + t.GetBounds().Width);
                            break;
                    }
                }
            }
        }
    }

    public class EnemyCollisionHandler : EntityCollisionHandler
    {
        private Enemy enemy;
        public EnemyCollisionHandler(TileMap map) : base(map) { }
        public override void Update(GameObj e)
        {
            base.Update(e);
            enemy = (Enemy)e;
            if (enemy.IsCollidingHor()) enemy.SetYVelocity(0);
            if (enemy.IsCollidingVer()) enemy.SetXVelocity(0);
        }
    }
}
