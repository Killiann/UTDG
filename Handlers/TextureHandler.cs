using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace UTDG
{
    public class TextureHandler
    {
        //dynamic
        public Texture2D playerTexture;
        public Texture2D tileMapTexture;
        public Texture2D bulletTexture;

        //UI
        public Texture2D progressBG;
        public Texture2D progressGreen;
        public Texture2D progressRed;
        public Texture2D itemDisplayBg;
        public Texture2D itemDisplaySelected;

        //pickup objects
        public Texture2D gunTexture;
        public Texture2D healthTexture;
        public Texture2D no_imageTexture;
        public Texture2D swordTexture;
        public Texture2D speedTexture;

        public void LoadContent(Game game)
        {
            //dynamic
            playerTexture = game.Content.Load<Texture2D>("images/player");
            tileMapTexture = game.Content.Load<Texture2D>("images/tileMap");
            bulletTexture = game.Content.Load<Texture2D>("images/no_image");

            //UI
            progressBG = game.Content.Load<Texture2D>("images/progressBg");
            progressGreen = game.Content.Load<Texture2D>("images/progressGreen");
            progressRed = game.Content.Load<Texture2D>("images/progressRed");
            itemDisplayBg = game.Content.Load<Texture2D>("images/itemDisplayBg");
            itemDisplaySelected = game.Content.Load<Texture2D>("images/itemDisplayselected");

            //pickup objects
            gunTexture = game.Content.Load<Texture2D>("images/gun");
            healthTexture = game.Content.Load<Texture2D>("images/health");
            no_imageTexture = game.Content.Load<Texture2D>("images/no_image");
            swordTexture = game.Content.Load<Texture2D>("images/sword1");
            speedTexture = game.Content.Load<Texture2D>("images/speed");
        }
    }
}
