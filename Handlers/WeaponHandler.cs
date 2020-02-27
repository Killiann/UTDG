using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace UTDG
{
    public class WeaponHandler
    {
        public bool IsEmpty = true;
        protected bool isEquiped;
        protected Vector2 position;
        protected float angle;
        protected Texture2D weaponTexture;
        public CollisionHandler collisionManager;

        protected bool IsActive() { return isEquiped; }

        public WeaponHandler()
        {
            collisionManager = new CollisionHandler();
            isEquiped = false;
        }

        public void Equip() { isEquiped = true; }
        public void UnEquip() { isEquiped = false; }
        public bool IsEquiped() { return isEquiped; }
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
            isEquiped = true;
            IsEmpty = false;
        }
        public override void Attack(Vector2 target)
        {
            if (isEquiped && fireCount > fireRate)
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
            if(isEquiped)
                spriteBatch.Draw(weaponTexture, new Rectangle((int)position.X, (int)position.Y, weaponTexture.Width, weaponTexture.Height), null, Color.White, angle, new Vector2(weaponTexture.Width / 2, weaponTexture.Height / 2), SpriteEffects.None, 1.0f);
        }
    }

    public class MeleeHandler : WeaponHandler
    {
        private readonly float MAXSTAB = 100.0f;
        private readonly float MAXROTATION = 2.0f;

        private float swingRate;
        private float rotation;
        private float swingAngle;
        private float stabDistance;
        private Vector2 origin;

        private bool isAttacking;
        private bool swingingRight;

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
            isEquiped = true;
            isAttacking = false;
            origin = new Vector2((int)dimensions.X / 2, (int)dimensions.Y / 2);
            IsEmpty = false;
        }

        public override void Attack(Vector2 target)
        {
            if (isEquiped && !isAttacking)
            {
                if (target.X > position.X)
                    swingingRight = true;
                else swingingRight = false;

                stabDistance = 0.0f;
                rotation = 0.0f;

                double dx = target.X - position.X;
                double dy = target.Y - position.Y;
                angle = (float)Math.Atan2(dy, dx);
                isAttacking = true;                
            }
        }

        public override void Update(GameObj gameObj)
        {
            position = new Vector2(gameObj.GetPosition().X, gameObj.GetPosition().Y);            
            if (isAttacking)
            {                
                if (attackType == Pickup_Melee.AttackType.STAB)
                {
                    origin.X = dimensions.X / 2;
                    swingAngle = angle;
                    if(stabDistance < MAXSTAB)
                        stabDistance += swingRate;                    
                    else
                        isAttacking = false;
                }
                else if(attackType == Pickup_Melee.AttackType.SWING)
                {
                    origin.X = 0;

                    if (rotation < MAXROTATION)
                    {
                        rotation += swingRate / 100;
                        if (swingingRight)
                            swingAngle = angle + rotation - (MAXROTATION / 2);
                        else if (!swingingRight)
                            swingAngle = angle - rotation + (MAXROTATION / 2);
                    }
                    else
                    {
                        angle = 0.0f;
                        isAttacking = false;
                    }
                }
            }
            else
            {
                if (stabDistance > 0 && attackType == Pickup_Melee.AttackType.STAB)                
                    stabDistance -= swingRate;                
                else stabDistance = 0f;
            }

            if (attackType == Pickup_Melee.AttackType.STAB)
            {
                position.X += (float)Math.Cos(angle) * stabDistance;
                position.Y += (float)Math.Sin(angle) * stabDistance;
            }
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            if (isAttacking || (stabDistance > 0 && attackType == Pickup_Melee.AttackType.STAB))
            {
                if (attackType == Pickup_Melee.AttackType.SWING) origin.X = -30;
                spriteBatch.Draw(weaponTexture, new Rectangle((int)position.X, (int)position.Y, (int)dimensions.X, (int)dimensions.Y), null, Color.White, swingAngle, origin, SpriteEffects.None, 1.0f);
            }
            else if (isEquiped) 
            {
                origin.X = dimensions.X / 2;
                spriteBatch.Draw(weaponTexture, new Rectangle((int)position.X, (int)position.Y, (int)dimensions.X, (int)dimensions.Y), null, Color.White, 0f, origin, SpriteEffects.None, 1.0f);
            }
        }
    }
}
