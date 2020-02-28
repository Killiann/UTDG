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
            healthBar = new ProgressBar(new Vector2(5, 5), textureHandler.progressGreen, 1.0f, textureHandler.progressBG, 2, 2);
            meleeItem = new ItemDisplay(new Vector2(5, 34), textureHandler.no_imageTexture, textureHandler.itemDisplayBg, textureHandler.itemDisplaySelected);
            rangedItem = new ItemDisplay(new Vector2(74, 34), textureHandler.no_imageTexture, textureHandler.itemDisplayBg, textureHandler.itemDisplaySelected);
        }

        public void SwitchSelectedWeapon()
        {
            if (meleeItem.IsSelected() && !rangedItem.IsEmpty())
            {
                meleeItem.UnSelect();
                rangedItem.Select();
            }
            else if (rangedItem.IsSelected() && !meleeItem.IsEmpty())
            {
                rangedItem.UnSelect();
                meleeItem.Select();
            }
        }

        public void Update(Player player)
        {
            meleeItem.UnSelect();
            rangedItem.UnSelect();
            if (player.heldItemManager.GetWeaponType() == HeldItemHandler.WeaponType.Ranged) rangedItem.Select();
            else if (player.heldItemManager.GetWeaponType() == HeldItemHandler.WeaponType.Melee) meleeItem.Select();
        }

        public void UpdateWeaponDisplay(Player player)
        {
            if(player.heldItemManager.meleeHandler.GetCurrentItem() != null)
                meleeItem.ChangeWeaponTexture(player.heldItemManager.meleeHandler.GetCurrentItem().GetTexture());
            if (player.heldItemManager.rangedHandler.GetCurrentItem() != null)
                rangedItem.ChangeWeaponTexture(player.heldItemManager.rangedHandler.GetCurrentItem().GetTexture());            
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            healthBar.Draw(spriteBatch);
            meleeItem.Draw(spriteBatch);
            rangedItem.Draw(spriteBatch);
        }
    }
}
