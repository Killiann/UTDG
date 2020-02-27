using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace UTDG
{
    public class GameObj
    {
        protected Vector2 position;
        protected Vector2 dimensions;
        protected Texture2D texture;

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, new Rectangle((int)position.X, (int)position.Y, (int)dimensions.X, (int)dimensions.Y), Color.White);
        }

        public Vector2 GetPosition() { return position; }
        public Vector2 GetDimensions() { return dimensions; }
        public void SetXPosition(float newXPosition) { position.X = newXPosition; }
        public void SetYPosition(float newYPosition) { position.Y = newYPosition; }
    }

    public class Item : GameObj
    {
        public CollisionHandler collisionManager;
        protected ItemType itemType;

        public enum ItemType
        {
            Ranged,
            Melee,
            StatEffect
        }

        public Item(Vector2 spawnPosition, Texture2D texture)
        {
            position = spawnPosition;
            dimensions = new Vector2(64, 64); //temp (tilesize)
            this.texture = texture;
            collisionManager = new ItemCollisionHandler(new Rectangle((int)position.X, (int)position.Y, (int)dimensions.X, (int)dimensions.Y));
        }

        public Texture2D GetTexture() { return texture; }
        public ItemType GetItemType() { return itemType; }
    }

    public class Pickup_Ranged : Item
    {
        private readonly float damage;
        private readonly float speed;
        private readonly int fireRate;

        private Texture2D bulletTexture;
        //private Texture2D gunTexture;

        public Pickup_Ranged(Vector2 position, Texture2D texture, Texture2D bulletTexture, float damage, float speed, int fireRate) : base(position, texture)
        {
            itemType = ItemType.Ranged;

            this.bulletTexture = bulletTexture;
            this.damage = damage;
            this.speed = speed;
            this.fireRate = fireRate;
        }

        public float GetDamage() { return damage; }
        public float GetSpeed() { return speed; }
        public int GetFireRate() { return fireRate; }
        public Texture2D getGunTexture() { return texture; }
        public Texture2D getBulletTexture() { return bulletTexture; }
    }

    public class Pickup_Melee : Item
    {
        private readonly float damage;
        private readonly float attackSpeed;
        private readonly AttackType attackType;
        private Texture2D weaponTexture;
        
        public enum AttackType
        {
            SWING,
            STAB
        }

        public Pickup_Melee(Vector2 position, Texture2D texture, Texture2D weaponTexture, float damage, float attackSpeed, AttackType attackType) : base(position, texture)
        {
            itemType = ItemType.Melee;
            this.damage = damage;
            this.weaponTexture = weaponTexture;
            this.attackSpeed = attackSpeed;
            this.attackType = attackType;
        }

        public float GetDamage() { return damage; }
        public float GetSpeed() { return attackSpeed; }
        public AttackType GetAttackType() { return attackType; }
        public Texture2D GetWeaponTexture() { return weaponTexture; }
    }

    public class StatBoost : Item
    {
        private readonly float statChange;
        private readonly StatType statType;
        public float GetStatChange() { return statChange; }
        public StatType GetStatType() { return statType; }
        public enum StatType
        {
            SPEED,
            HEALTH
        }

        public StatBoost(Vector2 position,Texture2D texture, float statChange, StatType statType) : base(position, texture)
        {
            itemType = ItemType.StatEffect;
            this.statType = statType;
            this.statChange = statChange;
        }
    }
}
