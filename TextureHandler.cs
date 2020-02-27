using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace UTDG
{
    public class TextureHandler
    {
        public Texture2D playerTexture;
        public Texture2D tileMapTexture;
        public Texture2D bulletTexture;

        public Texture2D gunTexture;
        public Texture2D healthTexture;
        public Texture2D no_imageTexture;
        public Texture2D swordTexture;
        public Texture2D speedTexture;

        public void LoadContent(Game game)
        {
            playerTexture = game.Content.Load<Texture2D>("images/player");
            tileMapTexture = game.Content.Load<Texture2D>("images/tileMap");
            bulletTexture = game.Content.Load<Texture2D>("images/no_image");

            gunTexture = game.Content.Load<Texture2D>("images/gun");
            healthTexture = game.Content.Load<Texture2D>("images/health");
            no_imageTexture = game.Content.Load<Texture2D>("images/no_image");
            swordTexture = game.Content.Load<Texture2D>("images/sword1");
            speedTexture = game.Content.Load<Texture2D>("images/speed");
        }
    }
}
