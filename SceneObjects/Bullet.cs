using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
namespace UTDG
{
    public class Bullet : GameObj
    {
        private readonly int LIFESPAN = 300;
        private int ticks = 0;
        public bool canBeDestroyed = false;
        public CollisionHandler collisionHandler;

        private readonly float angle;
        private readonly float speed;                               
    
        public Bullet(Vector2 _pos, float _angle, float _speed, Texture2D _texture, TileMap tileMap)
        {
            texture = _texture;            
            position = _pos;
            angle = _angle;
            speed = _speed;
            collisionHandler = new BulletCollisionHandler(tileMap);
        }

        public void Update()
        {
            //move in correct direction
            Vector2 direction = new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle));
            direction.Normalize();
            position += (direction * speed);
            collisionHandler.Update(this);

            ticks++;
            if(ticks >= LIFESPAN)            
                canBeDestroyed = true;            
        }

        public override void Draw(SpriteBatch spriteBatch)
        {            
            spriteBatch.Draw(texture, new Rectangle((int)position.X, (int)position.Y, 20, 20), null, Color.White, angle, new Vector2(texture.Width/2, texture.Height/2), SpriteEffects.None, 0); 
        }
    }
}
