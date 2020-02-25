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
        private readonly string itemName;
        public readonly float damage;
        public readonly float speed;
        public readonly int fireRate;        

        public Ranged(Vector2 _position, int _id, string _itemName, float _damage, float _speed, int _fireRate) : base(_position, _id)
        {
            itemName = _itemName;
            damage = _damage;
            speed = _speed;
            fireRate = _fireRate;
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
