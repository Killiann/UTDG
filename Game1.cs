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

        //scene objects
        private TileMap tileMap;
        private Player player;
        private Camera camera;
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
            player = new Player(tileMap.GetPXPosition(new Vector2(10, 10)), textureHandler.playerTexture, tileMap);

            sceneObjectHandler = new SceneObjectHandler(tileMap);
            sceneObjectHandler.AddObject(new Pickup_Ranged(tileMap.GetPXPosition(new Vector2(3, 3)), textureHandler.gunTexture, textureHandler.bulletTexture, 20.0f, 20.0f, 6));
            sceneObjectHandler.AddObject(new Pickup_Melee(tileMap.GetPXPosition(new Vector2(5, 3)), textureHandler.healthTexture, textureHandler.swordTexture, 20.0f, 15f, Pickup_Melee.AttackType.STAB));                       
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
            camera.SetPosition(player.GetPosition());
            gameOverlay.meleeItem.UnSelect();
            gameOverlay.rangedItem.UnSelect();
            if (player.heldItemManager.GetEquipedType() == HeldItemHandler.Equiped.Melee) gameOverlay.meleeItem.Select();
            else if (player.heldItemManager.GetEquipedType() == HeldItemHandler.Equiped.Ranged) gameOverlay.rangedItem.Select();


            for(int i = 0; i < sceneObjectHandler.GetSceneObjects().Count; i++)
            {
                GameObj obj = sceneObjectHandler.GetSceneObjects()[i];

                //picking up items (temp)
                if (player.collisionManager.IsColliding(((Item)obj).collisionManager.GetBounds())){
                    player.PickupItem((Item)obj);
                    if (obj.GetType() == typeof(Pickup_Melee)) gameOverlay.meleeItem.ChangeWeaponTexture(((Item)obj).GetTexture());
                    else if (obj.GetType() == typeof(Pickup_Ranged)) gameOverlay.rangedItem.ChangeWeaponTexture(((Item)obj).GetTexture());
                    sceneObjectHandler.RemoveObject(obj);
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
            sceneObjectHandler.Draw(_spriteBatch);
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
