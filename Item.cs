using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;


namespace UTDG
{

    class Ranged : GameObject
    {
        SpriteFont font;
        string itemName;
        public readonly float damage;
        public readonly float speed;

        public Ranged(Vector2 _position, int _id, float _damage, float _speed) : base(_position, _id)
        {
            itemName = "pewpew";
            damage = _damage;
            speed = _speed;
        }

        public override void LoadContent(Game game)
        {
            texture = game.Content.Load<Texture2D>("images/gun");
            font = game.Content.Load<SpriteFont>("fonts/TestFont");
        }        

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, position, Color.White);
            spriteBatch.DrawString(font, itemName, new Vector2(position.X, position.Y + texture.Height), Color.Black);
        }
    }   
}
