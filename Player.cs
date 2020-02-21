using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace UTDG
{
    public class Player
    {
        private Texture2D texture;
        private Vector2 position;
        private readonly float walkingSpeed;
        private Vector2 dimensions;

        public Vector2 GetPosition()
        {
            return position;
        }
        public Player(Vector2 spawnPosition)
        {
            position = spawnPosition;
            walkingSpeed = 5.0f;
        }

        public void LoadContent(Game game)
        {
            texture = game.Content.Load<Texture2D>("images/player");

            dimensions = new Vector2(texture.Width, texture.Height);
        }

        private void HandleMovement()
        {
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
        public void Update(GameTime gameTime)
        {
            HandleMovement();
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, new Rectangle((int)position.X, (int)position.Y, (int)dimensions.X, (int)dimensions.Y), Color.White); 
        }
    }
}
