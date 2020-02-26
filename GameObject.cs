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
        //SpriteFont font;
        private readonly string itemName;
        protected Texture2D texture;
        protected Vector2 position;
        protected Rectangle bounds;
        protected ItemType itemType;

        public enum ItemType
        {
            RANGED,
            MELEE,
            STAT_BOOST
        }

        public ItemType GetItemType()
        {
            return itemType;
        }            
           
        public GameObject(Vector2 _position, string _itemName)
        {
            position = _position;
            itemName = _itemName;
            bounds = new Rectangle((int)position.X, (int)position.Y, 64, 64); 
        }

        public virtual void LoadContent(Game game)
        {
            //font = game.Content.Load<SpriteFont>("fonts/TestFont");
        }

        public Rectangle GetBounds()
        {
            return bounds;
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, bounds, Color.White);
            //spriteBatch.DrawString(font, itemName, new Vector2(position.X, position.Y + texture.Height), Color.Black);
        }
    }

    class Ranged : GameObject
    {
        public readonly float damage;
        public readonly float speed;
        public readonly int fireRate;

        public Ranged(Vector2 _position, string _itemName, float _damage, float _speed, int _fireRate) : base(_position, _itemName)
        {
            itemType = ItemType.RANGED;
            damage = _damage;
            speed = _speed;
            fireRate = _fireRate;
        }
        public override void LoadContent(Game game)
        {
            base.LoadContent(game);
            texture = game.Content.Load<Texture2D>("images/gun");
        }
    }

    class Melee : GameObject
    {
        public readonly float damage;
        public readonly AttackType attackType;
        public readonly float attackSpeed;
        public enum AttackType
        {
            SWING,
            STAB
        }
        public Melee(Vector2 _position, string _itemName, float _damage, AttackType _attackType, float _attackSpeed) : base(_position, _itemName)
        {
            itemType = ItemType.MELEE;
            damage = _damage;
            attackType = _attackType;
            attackSpeed = _attackSpeed;
        }
        public override void LoadContent(Game game)
        {
            base.LoadContent(game);
            texture = game.Content.Load<Texture2D>("images/sword");
        }
    }

    class StatBoost : GameObject
    {

        public readonly StatType statType;
        public readonly float statChange;
        public enum StatType
        {
            SPEED,
            HEALTH
        }
        public StatBoost(Vector2 _position, string _itemName, StatType _statType, float _statChange) : base(_position, _itemName)
        {
            itemType = ItemType.STAT_BOOST;
            statType = _statType;
            statChange = _statChange;
        }
        public override void LoadContent(Game game)
        {
            base.LoadContent(game);
            if (statType == StatType.SPEED)
                texture = game.Content.Load<Texture2D>("images/speed");
            if (statType == StatType.HEALTH)
                texture = game.Content.Load<Texture2D>("images/health");
        }
    }
}
