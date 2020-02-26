using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace UTDG
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private readonly TextureHandler textureHandler;

        //scene objects
        private TileMap tileMap;
        private Player player;
        private Camera camera;
        private List<GameObj> gameObjects;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            textureHandler = new TextureHandler();
        }

        protected override void Initialize()
        {
            base.Initialize();

            tileMap = new TileMap(textureHandler.tileMapTexture);
            camera = new Camera(GraphicsDevice.Viewport, tileMap);
            player = new Player(new Vector2(320, 320), textureHandler.playerTexture, tileMap);

            gameObjects = new List<GameObj>();
            gameObjects.Add(new Pickup_Ranged(tileMap.GetPXPosition(new Vector2(3, 3)), textureHandler.gunTexture, 20.0f, 20.0f, 6));
            gameObjects.Add(new Pickup_Melee(tileMap.GetPXPosition(new Vector2(5, 3)), textureHandler.swordTexture, 20.0f, 20.0f, Pickup_Melee.AttackType.STAB));
            gameObjects.Add(new StatBoost(tileMap.GetPXPosition(new Vector2(7, 3)), textureHandler.speedTexture, 1.5f, StatBoost.StatType.SPEED));
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            textureHandler.LoadContent(this);
        }

        protected override void Update(GameTime gameTime)
        {
            player.Update();
            camera.SetPosition(new Vector2(player.GetPosition().X + 32, player.GetPosition().Y + 64));

            for (int i = 0; i < gameObjects.Count; i++)
            {
                if (((Item)gameObjects[i]).collisionManager.IsColliding(player.collisionManager.GetBounds()))
                {
                    player.PickupItem((Item)gameObjects[i]);
                    gameObjects.Remove(gameObjects[i]);
                }
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
            null, null, null, null, camera.TranslationMatrix);

            tileMap.Draw(_spriteBatch);
            foreach (GameObj obj in gameObjects)
            {
                obj.Draw(_spriteBatch);
            }
            player.Draw(_spriteBatch);

            _spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
