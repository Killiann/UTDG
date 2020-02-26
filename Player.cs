using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace UTDG
{
    public class Player : GameObj
    {        
        private float speed;

        public CollisionManager collisionManager;
        private PlayerInputManager inputManager;
        private PhysicsManager physicsManager;
        private HeldItemManager heldItemManager;

        private float speedBoost = 0.0f;

        private TileMap map;
        private float xVelocity;
        private float yVelocity;

        public bool isWalkingX;
        public bool isWalkingY;

        public float GetXVelocity() { return xVelocity; }
        public float GetYVelocity() { return yVelocity; }
        public float GetSpeedBoost() { return speedBoost; }
        public void SetXVelocity(float newVel){ xVelocity = newVel; }
        public void SetYVelocity(float newVel) { yVelocity = newVel; }

        public Player(Vector2 position, Texture2D texture, TileMap map)
        {
            this.texture = texture;
            this.position = position;
            this.map = map;

            dimensions = new Vector2(texture.Width, texture.Height);
            collisionManager = new PlayerCollisionManager(this.map);
            inputManager = new PlayerInputManager();
            physicsManager = new PhysicsManager(collisionManager);
            heldItemManager = new HeldItemManager();
        }

        public void PickupItem(Item item)
        {
            if(item.GetType() == typeof(Pickup_Melee) || item.GetType() == typeof(Pickup_Ranged))
                heldItemManager.PickupItem(item);
            else if(item.GetType() == typeof(StatBoost))
            {
                StatBoost.StatType type = ((StatBoost)item).GetStatType();
                if (type == StatBoost.StatType.SPEED)
                    speedBoost += ((StatBoost)item).GetStatChange();
            }
        }

        public void Update()
        {
            collisionManager.Update(this);
            inputManager.Update(this);
            physicsManager.Update(this);

            position.X += xVelocity;
            position.Y += yVelocity;
        }
    }
}
