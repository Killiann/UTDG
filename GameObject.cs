using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace UTDG
{
    public class GameObject
    {
        protected Texture2D texture;
        protected Vector2 position;
        protected Rectangle bounds;
        protected int id;

        public int GetId()
        {
            return id;
        }            
           
        public GameObject(Vector2 _position, int _id)
        {
            position = _position;
            id = _id;
            bounds = new Rectangle((int)position.X, (int)position.Y, 64, 64); 
        }

        public virtual void LoadContent(Game game)
        {
            texture = game.Content.Load<Texture2D>("images/no_image");
        }

        public Rectangle GetBounds()
        {
            return bounds;
        }

        public virtual void Update(GameTime gameTime)
        {

        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, bounds, Color.White);
        }
    }
}
