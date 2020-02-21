using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace UTDG
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        private readonly TileMap tileMap;
        private readonly Camera camera;
        private readonly Player player;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            tileMap = new TileMap();
            camera = new Camera();
            player = new Player(new Vector2(320, 320));
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

            camera.viewportWidth = GraphicsDevice.Viewport.Width;
            camera.viewportHeight = GraphicsDevice.Viewport.Height;
        }

        protected override void Update(GameTime gameTime)
        {
            camera.SetPosition(player.GetPosition());

            player.Update(gameTime);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend,
            null, null, null, null, camera.TranslationMatrix);

            tileMap.Draw(_spriteBatch);
            player.Draw(_spriteBatch);

            _spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
