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
        private SpriteBatch _overlaySpriteBatch;

        private SceneObjectHandler sceneObjectHandler;
        private readonly TextureHandler textureHandler;
        private GameOverlay gameOverlay;
        private FrameCounter _frameCounter = new FrameCounter();

        //scene objects
        private TileMap tileMap;
        private Player player;
        private Camera camera;

        public List<Enemy> enemies;

        //private List<GameObj> gameObjects;

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
            gameOverlay = new GameOverlay(textureHandler);
            gameOverlay.Initialize();

            tileMap = new TileMap(textureHandler.tileMapTexture);
            camera = new Camera(GraphicsDevice.Viewport, tileMap);
            sceneObjectHandler = new SceneObjectHandler(tileMap);
            player = new Player(textureHandler.playerTexture, tileMap, sceneObjectHandler, gameOverlay);

            enemies = new List<Enemy>();
            List<Vector2> enemySpawns = tileMap.GetEnemySpawns();

            foreach(Vector2 pos in enemySpawns)
            {
                enemies.Add(new Enemy(pos, textureHandler.enemyTexture, tileMap));
            }

            sceneObjectHandler.AddObject(new Pickup_Ranged(tileMap.GetPXPosition(new Vector2(3, 3)), textureHandler.gunTexture,textureHandler.bulletTexture,textureHandler.tempGun, 20.0f, 20.0f, 6));
            sceneObjectHandler.AddObject(new Pickup_Melee(tileMap.GetPXPosition(new Vector2(5, 3)), textureHandler.healthTexture, textureHandler.swordTexture, 20.0f, 15f, Pickup_Melee.AttackType.SWING));
            sceneObjectHandler.AddObject(new Pickup_Melee(tileMap.GetPXPosition(new Vector2(5, 5)), textureHandler.no_imageTexture, textureHandler.swordTexture, 20.0f, 15f, Pickup_Melee.AttackType.STAB));
            sceneObjectHandler.AddObject(new StatBoost(tileMap.GetPXPosition(new Vector2(7, 3)), textureHandler.speedTexture, 1.5f, StatBoost.StatType.SPEED));            
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            _overlaySpriteBatch = new SpriteBatch(GraphicsDevice);
            textureHandler.LoadContent(this);
        }

        protected override void Update(GameTime gameTime)
        {
            player.Update(camera);
            gameOverlay.Update(player);
            sceneObjectHandler.Update(player, gameOverlay);     

            foreach(Enemy enemy in enemies)
            {
                enemy.Update();
            }

            //camera.SetPosition(enemies[0].GetPosition());

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            var deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            _frameCounter.Update(deltaTime);

            var fps = string.Format("FPS: {0}", _frameCounter.AverageFramesPerSecond);

            Console.Clear();
            Console.Write(fps);

            //world
            _spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
            SamplerState.PointClamp, null, null, null, camera.TranslationMatrix);

            tileMap.Draw(_spriteBatch);
            sceneObjectHandler.Draw(_spriteBatch);

            foreach(Enemy enemy in enemies)
            {
                enemy.Draw(_spriteBatch);
            }

            player.Draw(_spriteBatch);

            _spriteBatch.End();

            //UI
            _overlaySpriteBatch.Begin();
            gameOverlay.Draw(_overlaySpriteBatch);
            _overlaySpriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
