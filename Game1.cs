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

        private readonly TextureHandler textureHandler;
        private GameOverlay gameOverlay;

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
            gameOverlay = new GameOverlay(textureHandler);
            gameOverlay.Initialize();

            tileMap = new TileMap(textureHandler.tileMapTexture);
            camera = new Camera(GraphicsDevice.Viewport, tileMap);
            player = new Player(tileMap.GetPXPosition(new Vector2(10, 10)), textureHandler.playerTexture, tileMap);

            gameObjects = new List<GameObj>();
            gameObjects.Add(new Pickup_Ranged(tileMap.GetPXPosition(new Vector2(3, 3)), textureHandler.gunTexture, textureHandler.bulletTexture, 20.0f, 20.0f, 6));
            gameObjects.Add(new Pickup_Melee(tileMap.GetPXPosition(new Vector2(5, 3)), textureHandler.healthTexture, textureHandler.swordTexture,20.0f, 15f, Pickup_Melee.AttackType.SWING));
            gameObjects.Add(new StatBoost(tileMap.GetPXPosition(new Vector2(7, 3)), textureHandler.speedTexture, 1.5f, StatBoost.StatType.SPEED));
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
            camera.SetPosition(player.GetPosition());
            gameOverlay.meleeItem.UnSelect();
            gameOverlay.rangedItem.UnSelect();
            if (player.heldItemManager.GetEquipedType() == HeldItemHandler.Equiped.Melee) gameOverlay.meleeItem.Select();
            else if (player.heldItemManager.GetEquipedType() == HeldItemHandler.Equiped.Ranged) gameOverlay.rangedItem.Select();

            for (int i = 0; i < gameObjects.Count; i++)
            {
                if (((Item)gameObjects[i]).collisionManager.IsColliding(player.collisionManager.GetBounds()))
                {
                    player.PickupItem((Item)gameObjects[i]);

                    //swap weapon display UI
                    if (gameObjects[i].GetType() == typeof(Pickup_Melee))                    
                        gameOverlay.meleeItem.ChangeWeaponTexture(((Item)gameObjects[i]).GetTexture());                    
                    else if (gameObjects[i].GetType() == typeof(Pickup_Ranged))
                        gameOverlay.rangedItem.ChangeWeaponTexture(((Item)gameObjects[i]).GetTexture());

                    gameObjects.Remove(gameObjects[i]);
                }
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            //world
            _spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend,
            null, null, null, null, camera.TranslationMatrix);
            tileMap.Draw(_spriteBatch);
            foreach (GameObj obj in gameObjects)
            {
                obj.Draw(_spriteBatch);
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
