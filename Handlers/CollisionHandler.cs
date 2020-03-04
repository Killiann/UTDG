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

    public class PlayerCollisionHandler : CollisionHandler
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
        private Player player;

        public PlayerCollisionHandler(TileMap map)
        {
            this.map = map;
            mapTiles = map.GetCollisionTiles();
        }

        public override void Update(GameObj p)
        {
            player = (Player)p;

            //x axis tile collisions
            bounds = new Rectangle((int)player.GetPosition().X - ((int)player.GetDimensions().X / 2) + (int)player.GetXVelocity(),
                (int)player.GetPosition().Y,
                (int)player.GetDimensions().X,
                (int)player.GetDimensions().X);

            Rectangle inflatedBounds = bounds;
            inflatedBounds.Inflate(10, 10);
            intersectingTiles = mapTiles.Where(x => inflatedBounds.Intersects(x.GetBounds())).ToList();

            if (player.GetXVelocity() > 0)
                CheckAndHandleCollision(Direction.Right);
            else if (player.GetXVelocity() < 0)
                CheckAndHandleCollision(Direction.Left);
            //y axis tile collisions

            inflatedBounds = bounds;
            inflatedBounds.Inflate(10, 10);
            intersectingTiles = mapTiles.Where(x => inflatedBounds.Intersects(x.GetBounds())).ToList();

            bounds = new Rectangle((int)player.GetPosition().X - ((int)player.GetDimensions().X / 2),
                (int)player.GetPosition().Y + (int)player.GetYVelocity(),
                (int)player.GetDimensions().X,
                (int)player.GetDimensions().X);

            if (player.GetYVelocity() > 0)
                CheckAndHandleCollision(Direction.Down);
            else if (player.GetYVelocity() < 0)
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
                            player.SetYVelocity(0);
                            player.SetYPosition(t.GetBounds().Y + t.GetBounds().Height);
                            break;
                        case (Direction.Right):
                            player.SetXVelocity(0);
                            player.SetXPosition(t.GetBounds().X - player.GetDimensions().X / 2);
                            break;
                        case (Direction.Down):
                            player.SetYVelocity(0);
                            player.SetYPosition(t.GetBounds().Y - player.GetDimensions().Y / 2);
                            break;
                        case (Direction.Left):
                            player.SetXVelocity(0);
                            player.SetXPosition(t.GetBounds().X + t.GetBounds().Width + player.GetDimensions().X / 2);
                            break;
                    }
                }
            }
        }
    }
}
