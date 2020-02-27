using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
namespace UTDG
{
    public class HeldItemHandler
    {
        public RangedHandler rangedHandler;
        public MeleeHandler meleeHandler;

        public enum WeaponType
        {
            None,
            Ranged,
            Melee
        }

        public HeldItemHandler()
        {
            rangedHandler = new RangedHandler();
            meleeHandler = new MeleeHandler();
        }

        public void SwitchEquiped()
        {
            if (rangedHandler.IsEquiped() && !meleeHandler.IsEmpty)
            {
                meleeHandler.Equip();
                rangedHandler.UnEquip();
            }

            else if (meleeHandler.IsEquiped() && !rangedHandler.IsEmpty)
            {
                rangedHandler.Equip();
                meleeHandler.UnEquip();
            }
        }

        public WeaponType GetWeaponType() {
            if (rangedHandler.IsEquiped()) return WeaponType.Ranged;
            else if (meleeHandler.IsEquiped()) return WeaponType.Melee;
            else return WeaponType.None;
        }

        public void Attack(Vector2 target)
        {
            if (rangedHandler.IsEquiped()) rangedHandler.Attack(target);
            else if (meleeHandler.IsEquiped()) meleeHandler.Attack(target);
        }

        public void Update(Player player)
        {
            meleeHandler.Update(player);
            rangedHandler.Update(player);
        }

        public void PickupItem(Item item)
        {
            if(item.GetType() == typeof(Pickup_Ranged))
            {
                rangedHandler.ChangeWeapon(item);
                meleeHandler.UnEquip();
            }
            else if(item.GetType() == typeof(Pickup_Melee))
            {
                meleeHandler.ChangeWeapon(item);
                rangedHandler.UnEquip();
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            rangedHandler.Draw(spriteBatch);
            meleeHandler.Draw(spriteBatch);
        }
    }
}
