using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
namespace UTDG
{
    class Bullet
    {
        private Vector2 position;
        private float angle;
        private float speed;
        private int ticks;
        public bool canBeDestroyed;
        private Texture2D texture;
    
        public Bullet(Vector2 _pos, float _angle, float _speed, Texture2D _texture)
        {
            texture = _texture;
            ticks = 0;
            canBeDestroyed = false;
            position = _pos;
            angle = _angle;
            speed = _speed;
        }

        public void Update(GameTime gameTime)
        {
            Vector2 direction = new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle));
            direction.Normalize();
            position += (direction * speed);

            ticks++;
            if(ticks == 200)
            {
                canBeDestroyed = true;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {            
            spriteBatch.Draw(texture, new Rectangle((int)position.X, (int)position.Y, 20, 20), null, Color.White, angle, new Vector2(0.0f, 0.0f), SpriteEffects.None, 0); 
        }
    }
}
