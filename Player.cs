using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace UTDG
{
    public class Player
    {
        private Texture2D texture;
        private Vector2 position;
        private float walkingSpeed = 5.0f;
        private float health = 100.0f;
        private readonly float MAXHEALTH = 100.0f;
        private Vector2 dimensions;
        private Rectangle collisionBounds
        {
            get
            {
                return new Rectangle((int)position.X, (int)position.Y, (int)dimensions.X, (int)dimensions.Y);
            }
        }

        private bool canShoot = false;
        private bool hasGun = false;

        private int currentShootCount = 0;
        private int fireRate = 0;
        private float shootingSpeed = 0.0f;
        private float bulletDamage = 0.0f;

        private Texture2D bulletTexture;
        private List<Bullet> bullets;

        private enum AttackType
        {
            None,
            Ranged,
            Melee
        }

        //List<GameObject> heldItems;
        GameObject heldItem;

        public Vector2 GetPosition()
        {
            return position;
        }
        public void SetPosition(Vector2 newPos)
        {
            position = newPos;
        }

        public float getWalkingSpeed()
        {
            return walkingSpeed;
        }

        public Player(Vector2 spawnPosition)
        {
            position = spawnPosition;
            //heldItems = new List<GameObject>();
            bullets = new List<Bullet>();
        }

        public void LoadContent(Game game)
        {
            texture = game.Content.Load<Texture2D>("images/player");
            bulletTexture = game.Content.Load<Texture2D>("images/no_image");
            dimensions = new Vector2(texture.Width, texture.Height);
        }

        public void PickupObject(GameObject obj)
        {
            //heldItems.Add(obj);
            switch (obj.GetItemType())
            {
                case GameObject.ItemType.RANGED:
                    //pickup and wield
                    hasGun = true;
                    shootingSpeed = ((Ranged)obj).speed;
                    bulletDamage = ((Ranged)obj).damage;
                    fireRate = ((Ranged)obj).fireRate;

                    heldItem = obj;
                    break;
                case GameObject.ItemType.MELEE:
                    //pickup and wield 

                    heldItem = obj;
                    break;
                case GameObject.ItemType.STAT_BOOST:
                    //add effect to player
                    switch (((StatBoost)obj).statType) {
                        case StatBoost.StatType.HEALTH:
                            if(health < 100)
                            {
                                health += ((StatBoost)obj).statChange;
                                if (health > MAXHEALTH) health = MAXHEALTH;
                            }
                            break;
                        case StatBoost.StatType.SPEED:
                            walkingSpeed += ((StatBoost)obj).statChange;
                            break;
                    }
                    break;
            }
        }
        
        public bool Collides(Rectangle collidingRect)
        {
            if (collisionBounds.Intersects(collidingRect)) return true;
            else return false;
        }

        public void Attack(Vector2 target)
        {
            if(canShoot)
            {
                double dy = (target.Y - position.Y);
                double dx = (target.X - position.X);
                float angle = (float)Math.Atan2(dy, dx);
                bullets.Add(new Bullet(position, angle, 15.0f, bulletTexture));
                canShoot = false;
            }
        }

        public void MovePlayer(char key)
        {
            //temp movement            

            switch (key)
            {
                case 'W':
                    position.Y -= walkingSpeed;
                    break;
                case 'A':
                    position.X -= walkingSpeed;
                    break;
                case 'S':
                    position.Y += walkingSpeed;
                    break;
                case 'D':
                    position.X += walkingSpeed;
                    break;
            }
        }

        public void Update(GameTime gameTime)
        {
            if (!canShoot && fireRate > 0)
            {
                currentShootCount++;
                if(currentShootCount == fireRate)
                {
                    canShoot = true;
                    currentShootCount = 0;
                }
            }

            //update bullets
            for (int i = 0; i < bullets.Count; i++) { 
                bullets[i].Update(gameTime);
                if (bullets[i].canBeDestroyed)
                {
                    bullets.Remove(bullets[i]);
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, new Rectangle((int)position.X, (int)position.Y, (int)dimensions.X, (int)dimensions.Y), Color.White);
            foreach (Bullet bullet in bullets)
            {
                bullet.Draw(spriteBatch);
            }
        }
    }
}
