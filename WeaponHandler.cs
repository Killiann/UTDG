using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace UTDG
{
    public class WeaponHandler
    {
        protected bool isActive;
        protected Vector2 position;
        protected float angle;
        protected Texture2D weaponTexture;
        public CollisionManager collisionManager;

        protected bool IsActive() { return isActive; }

        public WeaponHandler()
        {
            collisionManager = new CollisionManager();
            isActive = false;
        }

        public virtual void ChangeWeapon(Item item) { }
        public virtual void Attack(Vector2 target) { }
        public virtual void Update(GameObj gameObj) { }
        public virtual void Draw(SpriteBatch spriteBatch) { }
    }

    public class RangedHandler : WeaponHandler
    {
        private int fireRate;
        private int fireCount;
        private float shootSpeed;
        private Texture2D bulletTexture;
        private List<Bullet> bullets;
            
        public RangedHandler()
        {
            bullets = new List<Bullet>();
        }
        public override void ChangeWeapon(Item newGun)
        {
            fireRate = ((Pickup_Ranged)newGun).GetFireRate();
            shootSpeed = ((Pickup_Ranged)newGun).GetSpeed();
            weaponTexture = ((Pickup_Ranged)newGun).getGunTexture();
            bulletTexture = ((Pickup_Ranged)newGun).getBulletTexture();
            isActive = true;
        }
        public override void Attack(Vector2 target)
        {
            if (isActive && fireCount > fireRate)
            {
                double dx = target.X - position.X;
                double dy = target.Y - position.Y;
                angle = (float)Math.Atan2(dy, dx);

                bullets.Add(new Bullet(position, angle, shootSpeed, bulletTexture));
                fireCount = 0;
            }
        }
        public override void Update(GameObj gameObj)
        {
            position = gameObj.GetPosition();
            for(int i = 0; i < bullets.Count; i++)
            {
                bullets[i].Update();
                if (bullets[i].canBeDestroyed)
                    bullets.Remove(bullets[i]);
            }
            fireCount++;
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            for(int i = 0; i < bullets.Count; i++)
            {
                bullets[i].Draw(spriteBatch);
            }
        }
    }

    public class MeleeHandler : WeaponHandler
    {
        private readonly float MAXSTAB = 200.0f;
        private readonly float MAXROTATION = 1.0f;

        private float swingRate;
        private float rotation;
        private float stabDistance;

        private bool isAttacking;

        private Vector2 dimensions;
        private Pickup_Melee.AttackType attackType;        

        public override void ChangeWeapon(Item item)
        {
            swingRate = ((Pickup_Melee)item).GetSpeed();
            swingRate = ((Pickup_Melee)item).GetSpeed();
            weaponTexture = ((Pickup_Melee)item).GetWeaponTexture();
            attackType = ((Pickup_Melee)item).GetAttackType();
            dimensions = new Vector2(weaponTexture.Width, weaponTexture.Height);
            rotation = 0.0f;
            stabDistance = 0.0f;
            isActive = true;
            isAttacking = false;
        }
        public override void Attack(Vector2 target)
        {
            if (isActive)
            {
                double dx = target.X - position.X;
                double dy = target.Y - position.Y;
                angle = (float)Math.Atan2(dy, dx);
                isAttacking = true;
            }
        }
        public override void Update(GameObj gameObj)
        {
            position = gameObj.GetPosition();
            if (isAttacking)
            {                
                if (attackType == Pickup_Melee.AttackType.STAB)
                {
                    if(stabDistance < MAXSTAB)
                    {
                        stabDistance += swingRate;
                        position.X += (float)Math.Cos(angle) * stabDistance;
                        position.Y += (float)Math.Sin(angle) * stabDistance;                        
                    }
                    else
                    {
                        stabDistance = 0.0f;
                        isAttacking = false;
                    }
                }else if(attackType == Pickup_Melee.AttackType.SWING)
                {
                    if (rotation < MAXROTATION)
                    {
                        rotation += swingRate;
                        angle = angle + rotation - (MAXROTATION / 2);
                    }
                    else
                    {
                        rotation = 0.0f;
                        isAttacking = false;
                    }
                }
            }
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            if (isAttacking)
            {
                spriteBatch.Draw(weaponTexture, new Rectangle((int)position.X, (int)position.Y, (int)dimensions.X, (int)dimensions.Y), null, Color.White, angle, position, SpriteEffects.None, 1.0f);
            }
        }
    }
}
