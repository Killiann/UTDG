using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace UTDG
{
    public class Game1 : Game
    {
        Random rnd;
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        //objects
        private TileMap tileMap;
        private Camera camera;
        private Player player;

        private List<GameObject> gameObjects;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;         
        }

        protected override void Initialize()
        {
            rnd = new Random();
            camera = new Camera(GraphicsDevice.Viewport);
            tileMap = new TileMap();
            player = new Player(new Vector2(320, 320));
            gameObjects = new List<GameObject>();

            base.Initialize();           
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            //adding objects to scene
            gameObjects.Add(new Ranged(tileMap.GetPXPosition(new Vector2(3, 3)), "pewpew", 20.0f, 20.0f, 6));
            gameObjects.Add(new Melee(tileMap.GetPXPosition(new Vector2(5, 3)), "stabstab", 20.0f, Melee.AttackType.STAB, 5.0f));
            gameObjects.Add(new StatBoost(tileMap.GetPXPosition(new Vector2(7, 3)), "speedspeed",StatBoost.StatType.SPEED, 5.0f));

            //loading content
            tileMap.LoadContent(this);
            player.LoadContent(this);           
            foreach (GameObject obj in gameObjects){
                obj.LoadContent(this);
            }
        }

        protected override void Update(GameTime gameTime)
        {
            camera.SetPosition(player.GetPosition());
            player.Update(gameTime, camera.TranslationMatrix);

            for(int i=0;i<gameObjects.Count;i++)
            {
                if (player.Collides(gameObjects[i].GetBounds())){
                    player.PickupObject(gameObjects[i]);
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
