using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace UTDG
{    
    public class GameOverlay
    {
        private TextureHandler textureHandler;
        public ProgressBar healthBar;
        public ItemDisplay meleeItem;
        public ItemDisplay rangedItem;

        public GameOverlay(TextureHandler textureHandler)
        {
            this.textureHandler = textureHandler;            
        }

        public void Initialize()
        {
            healthBar = new ProgressBar(new Vector2(5, 5), textureHandler.progressGreen, 1.0f, textureHandler.progressBG, 3, 3);
            meleeItem = new ItemDisplay(new Vector2(5, 45), textureHandler.no_imageTexture, textureHandler.itemDisplayBg, textureHandler.itemDisplaySelected);
            rangedItem = new ItemDisplay(new Vector2(74, 45), textureHandler.no_imageTexture, textureHandler.itemDisplayBg, textureHandler.itemDisplaySelected);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            healthBar.Draw(spriteBatch);
            meleeItem.Draw(spriteBatch);
            rangedItem.Draw(spriteBatch);
        }
    }
}
