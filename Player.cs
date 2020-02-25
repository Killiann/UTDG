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
        private readonly float walkingSpeed = 5.0f;
        private Vector2 dimensions;
        private Rectangle collisionBounds
        {
            get
            {
                return new Rectangle((int)position.X, (int)position.Y, (int)dimensions.X, (int)dimensions.Y);
            }
        }

        private bool canShoot = false;

        private int currentShootCount = 0;
        private int fireRate = 0;
        private float shootingSpeed = 0.0f;
        private float bulletDamage = 0.0f;

        private Texture2D bulletTexture;
        private List<Bullet> bullets;

        List<GameObject> heldItems;

        public Vector2 GetPosition()
        {
            return position;
        }
        public Player(Vector2 spawnPosition)
        {
            position = spawnPosition;
            heldItems = new List<GameObject>();
            bullets = new List<Bullet>();
        }

        public void LoadContent(Game game)
        {
            texture = game.Content.Load<Texture2D>("images/player");
            bulletTexture = game.Content.Load<Texture2D>("images/health");
            dimensions = new Vector2(texture.Width, texture.Height);
        }

        public void PickupObject(GameObject obj)
        {
            heldItems.Add(obj);
            if(obj.GetId() == 0) // gun
            {
                canShoot = true;
                shootingSpeed = ((Ranged)obj).speed;
                bulletDamage = ((Ranged)obj).damage;
                fireRate = ((Ranged)obj).fireRate;
            }
        }
        
        public bool Collides(Rectangle collidingRect)
        {
            if (collisionBounds.Intersects(collidingRect)) return true;
            else return false;
        }

        private void HandleMouse(Matrix transformBy)
        {
            MouseState mouseState = Mouse.GetState();
            if(mouseState.LeftButton == ButtonState.Pressed && canShoot)
            {
                Matrix invertedMatrix = Matrix.Invert(transformBy);
                Vector2 mousePosition = new Vector2(mouseState.X, mouseState.Y);
                mousePosition = Vector2.Transform(mousePosition, invertedMatrix);
                double dy = (mousePosition.Y - position.Y);
                double dx = (mousePosition.X - position.X);
                float angle = (float)Math.Atan2(dy, dx);
                bullets.Add(new Bullet(position, angle, 15.0f, bulletTexture));
                canShoot = false;
                currentShootCount = 0;
            }
        }

        private void HandleMovement()
        {
            //temp movement
            KeyboardState state = Keyboard.GetState();

            if (state.IsKeyDown(Keys.W)){
                position.Y -= walkingSpeed;
            }
            if (state.IsKeyDown(Keys.A))
            {
                position.X -= walkingSpeed;
            }
            if (state.IsKeyDown(Keys.S))
            {
                position.Y += walkingSpeed;
            }
            if (state.IsKeyDown(Keys.D))
            {
                position.X += walkingSpeed;
            }
        }

        public void Update(GameTime gameTime, Matrix transformationMatrix)
        {
            HandleMovement();
            HandleMouse(transformationMatrix);

            if (!canShoot)
            {
                currentShootCount++;
                if(currentShootCount == fireRate)
                {
                    canShoot = true;
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
