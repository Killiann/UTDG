using System;
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
        public RangedHandler rangedHandler;
        public MeleeHandler meleeHandler;

        private float speedMultiplier = 0.0f;

        public TileMap map;
        public Camera camera;
        private float xVelocity;
        private float yVelocity;

        private Vector2 origin;

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
            rangedHandler = new RangedHandler();
            meleeHandler = new MeleeHandler();
        }

        public void PickupItem(Item item)
        {
            if (item.GetType() == typeof(Pickup_Melee))
            {
                heldItemManager.PickupItem(item);
                meleeHandler.ChangeWeapon(item);
            }            
            else if (item.GetType() == typeof(Pickup_Ranged))
            {
                heldItemManager.PickupItem(item);
                rangedHandler.ChangeWeapon(item);
            }
            else if (item.GetType() == typeof(StatBoost))
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
            rangedHandler.Update(this);
            meleeHandler.Update(this);

            position.X += xVelocity;
            position.Y += yVelocity;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, new Rectangle((int)position.X, (int)position.Y, (int)dimensions.X, (int)dimensions.Y), null, Color.White, 0f, origin, SpriteEffects.None, 1f);
            rangedHandler.Draw(spriteBatch);
            meleeHandler.Draw(spriteBatch);
        }
    }
}
