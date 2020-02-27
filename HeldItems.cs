using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
namespace UTDG
{
    public class HeldItemManager
    {
        private Pickup_Melee meleeItem;
        private Pickup_Ranged rangedItem;
        public Pickup_Melee GetMeleeItem() { return meleeItem; }
        public Pickup_Ranged GetRangedItem() { return rangedItem; }

        private Equiped equiped;
        public enum Equiped
        {
            None,
            Melee,
            Ranged
        }

        public HeldItemManager()
        {
            equiped = Equiped.None;
        }

        public void EquipItem(Equiped type)
        {
            equiped = type;
        }

        public Item GetEquiped()
        {
            if (equiped == Equiped.Melee)
                return meleeItem;
            else if (equiped == Equiped.Ranged)
                return rangedItem;
            else return null;
        }

        public Equiped GetEquipedType() { return equiped; }

        public void PickupItem(Item item)
        {
            if(item.GetType() == typeof(Pickup_Ranged))
            {
                rangedItem = (Pickup_Ranged)item;
                equiped = Equiped.Ranged;
            }
            else if(item.GetType() == typeof(Pickup_Melee))
            {
                meleeItem = (Pickup_Melee)item;
                equiped = Equiped.Melee;
            }
        }
    }
}
