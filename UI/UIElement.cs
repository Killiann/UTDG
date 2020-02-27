using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace UTDG
{
    public class UIElement
    {
        protected Vector2 position;
        protected Texture2D texture;
        
        public UIElement(Vector2 position, Texture2D texture)
        {
            this.position = position;
            this.texture = texture;
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, new Vector2(position.X, position.Y), Color.White);
        }
    }

    public class ProgressBar : UIElement
    {
        private Texture2D backgroundTexture;
        private bool hasBackground;
        private int bgOffsetX;
        private int bgOffsetY;

        private float progressVal;

        public ProgressBar(Vector2 position, Texture2D texture, float progressVal): base(position, texture)
        {
            hasBackground = false;
            if (progressVal <= 1.0f && progressVal >= 0f)
                this.progressVal = progressVal;
            else progressVal = 1;
        }
        public ProgressBar(Vector2 position, Texture2D texture, float progressVal, Texture2D backgroundTexture, int bgOffsetX, int bgOffsetY) : base(position, texture)
        {
            hasBackground = true;
            if (progressVal <= 1.0f && progressVal >= 0f)
                this.progressVal = progressVal;
            else progressVal = 1;

            this.backgroundTexture = backgroundTexture;
            this.bgOffsetX = bgOffsetX;
            this.bgOffsetY = bgOffsetY;
        }

        public float GetProgress() { return progressVal; }
        public void UpdateProgress(float progressVal)
        {
            if(progressVal <= 1.0f && progressVal >= 0f)
                this.progressVal = progressVal;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            var texturePosition = position;
            if (hasBackground)
            {
                spriteBatch.Draw(backgroundTexture, new Vector2(position.X, position.Y), Color.White);
                texturePosition = new Vector2(position.X + bgOffsetX, position.Y + bgOffsetY);
            }
            spriteBatch.Draw(texture, texturePosition, null, Color.White, 0f, new Vector2(0, 0), new Vector2(progressVal, 1.0f), SpriteEffects.None, 1.0f);
        }
    }

    public class ItemDisplay : UIElement
    {
        private Texture2D backgroundTexture;
        private Texture2D selectedTexture;

        bool isSelected;
        bool isEmpty;
        private readonly int bgOffset = 2;
        private readonly int size = 64;
        public bool IsSelected() { return isSelected; }
        public bool IsEmpty() { return isEmpty; }

        public ItemDisplay(Vector2 position, Texture2D texture, Texture2D backgroundTexture, Texture2D selectedTexture): base(position, texture)
        {
            isSelected = false;
            isEmpty = true;
            this.backgroundTexture = backgroundTexture;
            this.selectedTexture = selectedTexture;
        }

        public void Select() { isSelected = true; }
        public void UnSelect() { isSelected = false; }
        public void ChangeWeaponTexture(Texture2D texture){ this.texture = texture; isEmpty = false; }
        public void DropWeapon() { isEmpty = true; }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(backgroundTexture, new Rectangle((int)position.X, (int)position.Y, size, size), Color.White);
            if (!isEmpty) spriteBatch.Draw(texture, new Rectangle((int)position.X + bgOffset, (int)position.Y + bgOffset, size - bgOffset * 2, size - bgOffset*2), Color.White);
            if (isSelected) spriteBatch.Draw(selectedTexture, new Rectangle((int)position.X, (int)position.Y, size, size), Color.White);
        }
    }
}
