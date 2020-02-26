using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace UTDG
{
    public class Attack
    {
        private Item equipedItem;

        public void SetEquiped(Item newItem){ equipedItem = newItem; }

        public Attack()
        {
        }

        public void AttackTarget(Vector2 target)
        {

        }
    }
}
