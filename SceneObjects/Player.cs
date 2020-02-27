﻿using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace UTDG
{
    public class Player : GameObj
    {        
        public CollisionHandler collisionManager;
        private PlayerInputHandler inputManager;
        private PhysicsHandler physicsManager;
        public HeldItemHandler heldItemManager;

        public TileMap map;
        public Camera camera;
        private Vector2 origin;

        private float xVelocity;
        private float yVelocity;
        private float speedMultiplier = 0.0f;
        public bool isWalkingX;
        public bool isWalkingY;

        public float GetXVelocity() { return xVelocity; }
        public float GetYVelocity() { return yVelocity; }
        public Vector2 GetOrigin() { return origin; }
        public float GetSpeedMultiplier() { return speedMultiplier; }
        public void SetXVelocity(float newVel){ xVelocity = newVel; }
        public void SetYVelocity(float newVel) { yVelocity = newVel; }

        public Player(Vector2 position, Texture2D texture, TileMap map)
        {
            this.texture = texture;
            this.position = new Vector2(position.X + texture.Width/2, position.Y+texture.Height/2);
            this.map = map;

            dimensions = new Vector2(texture.Width, texture.Height);
            collisionManager = new PlayerCollisionHandler(this.map);
            inputManager = new PlayerInputHandler();
            physicsManager = new PhysicsHandler(collisionManager);
            heldItemManager = new HeldItemHandler();
        }

        public void PickupItem(Item item)
        {
            heldItemManager.PickupItem(item);

            if (item.GetType() == typeof(StatBoost))
            {
                StatBoost.StatType type = ((StatBoost)item).GetStatType();
                if (type == StatBoost.StatType.SPEED)
                    speedMultiplier += ((StatBoost)item).GetStatChange();
            }
        }

        public void Update(Camera camera)
        {
            this.camera = camera;
            origin = new Vector2(dimensions.X / 2, dimensions.Y / 2);

            collisionManager.Update(new Rectangle((int)position.X - (int)origin.X, (int)position.Y - (int)origin.Y, (int)dimensions.X, (int)dimensions.Y));
            inputManager.Update(this);
            physicsManager.Update(this);

            position.X += xVelocity;
            position.Y += yVelocity;

            heldItemManager.Update(this);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, new Rectangle((int)position.X, (int)position.Y, (int)dimensions.X, (int)dimensions.Y), null, Color.White, 0f, origin, SpriteEffects.None, 1f);
            heldItemManager.Draw(spriteBatch);
        }
    }
}
