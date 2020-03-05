using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace UTDG
{
    public class Enemy : DynamicObj
    {
        public EntityCollisionHandler collisionHandler;
        public States state;
        private TileMap tileMap;

        //idle
        bool isWalking;
        int time;
        int timeBetween;
        Random rnd;
        float movingAngle;
        Vector2 direction;
        float speed = 1.5f;
        public bool colliding = false;

        public override Rectangle GetBounds()
        {
            return new Rectangle((int)position.X, (int)position.Y + (int)dimensions.X, (int)dimensions.X, (int)dimensions.Y); 
        }

        public override void SetYPosition(float newYPosition)
        {
            position.Y = newYPosition - dimensions.X;
        }

        public enum States
        {
            idle,
            charging,
            searching
        }

        public Enemy(Vector2 position, Texture2D texture, TileMap tileMap) 
        {
            collisionHandler = new EnemyCollisionHandler(tileMap);
            this.position = position;
            this.texture = texture;
            state = States.idle;
            dimensions = new Vector2(texture.Width, texture.Height);
            rnd = new Random();
            //idle
            isWalking = false;
            time = 0;
            timeBetween = rnd.Next(200);
        }

        public void Update()
        {
            switch (state)
            {
                case States.idle:
                    UpdateIdle();
                    break;
                case States.charging:

                    break;
                case States.searching:

                    break;
                default:

                    break;
            }
        }

        private void UpdateIdle()
        {
            time++;
            if(time == timeBetween)
            {
                if (isWalking) isWalking = false;
                else
                {
                    movingAngle = (float)rnd.Next((int)(2*Math.PI*10))/10;
                    direction = new Vector2((float)Math.Cos(movingAngle), (float)Math.Sin(movingAngle));
                    isWalking = true;
                }
                time = 0;
                timeBetween = rnd.Next(50, 200);
            }
            if (isWalking)
            {
                velocity = (direction * speed);
                collisionHandler.Update(this);
                position += velocity;
                if(collidingHor || collidingVer)
                {
                    isWalking = false;
                    collidingHor = false;
                    collidingVer = false;
                    time = 0;
                    timeBetween = rnd.Next(50, 200);
                }
            }
        }

        private void UpdateCharging()
        {

        }

        private void UpdateSearching()
        {

        }
    }
}
