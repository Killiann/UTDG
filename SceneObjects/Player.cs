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
        private SceneObjectHandler objectHandler;
        private GameOverlay overlay;

        public Item canPickup = null;        

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

        public Player(Texture2D texture, TileMap map, SceneObjectHandler objectHandler, GameOverlay overlay)
        {
            this.texture = texture;
            position = map.GetSpawn();
            this.map = map;
            this.objectHandler = objectHandler;
            this.overlay = overlay;

            dimensions = new Vector2(texture.Width, texture.Height);
            collisionManager = new PlayerCollisionHandler(this.map);
            inputManager = new PlayerInputHandler();
            physicsManager = new PhysicsHandler(collisionManager);
            heldItemManager = new HeldItemHandler();
        }                           

        public void PickupItem(Item item)
        {
            Type itemType = item.GetType();
            Item oldItem = null;
            if (itemType == typeof(Pickup_Melee) && heldItemManager.meleeHandler.GetCurrentItem() != null)
                oldItem = heldItemManager.meleeHandler.GetCurrentItem();
            else if (itemType == typeof(Pickup_Ranged) && heldItemManager.rangedHandler.GetCurrentItem() != null)
                oldItem = heldItemManager.rangedHandler.GetCurrentItem();

            objectHandler.RemoveObject(item);
            if (oldItem != null) {
                oldItem.SetXPosition(item.GetPosition().X);
                oldItem.SetYPosition(item.GetPosition().Y);
                oldItem.UpdateCollisionBounds();
                objectHandler.AddObject(oldItem);
            }            

            //pickup new
            heldItemManager.PickupItem(item);            
            overlay.UpdateWeaponDisplay(this);            

            if (itemType == typeof(StatBoost))
            {
                StatBoost.StatType type = ((StatBoost)item).GetStatType();
                if (type == StatBoost.StatType.SPEED)
                    speedMultiplier += ((StatBoost)item).GetStatChange();
            }
        }

        public void Update(Camera camera)
        {
            this.camera = camera;
            camera.SetPosition(position);
            origin = new Vector2(dimensions.X / 2, dimensions.Y / 2);

            inputManager.Update(this);
            physicsManager.Update(this);

            if (canPickup != null && canPickup.GetType() == typeof(StatBoost))
                PickupItem(canPickup);
            canPickup = null;

            collisionManager.Update(this);
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
