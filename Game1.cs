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

        private readonly TileMap tileMap;
        private readonly Camera camera;
        private readonly Player player;

        Random rnd;

        private List<GameObject> gameObjects;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            tileMap = new TileMap();
            camera = new Camera();
            player = new Player(new Vector2(320, 320));

            rnd = new Random();

            gameObjects = new List<GameObject>();

            gameObjects.Add(new Ranged(tileMap.GetPXPosition(new Vector2(3, 10)), 0 , 20.0f, 20.0f));                       
        }

        protected override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            tileMap.LoadContent(this);
            player.LoadContent(this);

            foreach(GameObject obj in gameObjects){
                obj.LoadContent(this);
            }

            camera.viewportWidth = GraphicsDevice.Viewport.Width;
            camera.viewportHeight = GraphicsDevice.Viewport.Height;
        }

        protected override void Update(GameTime gameTime)
        {
            camera.SetPosition(player.GetPosition());
            player.Update(gameTime, camera.TranslationMatrix);
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
            null, null, null, null, camera.TranslationMatrix);

            tileMap.Draw(_spriteBatch);
            foreach (GameObject obj in gameObjects)
            {
                obj.Draw(_spriteBatch);
            }
            player.Draw(_spriteBatch);

            _spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
