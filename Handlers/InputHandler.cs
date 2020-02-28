using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
namespace UTDG
{
    public class PlayerInputHandler
    {
        private float baseAcceleration = 1.0f;
        private float acceleration;
        private MouseState lastMouseState;
        private KeyboardState lastKeyboardState;

        public PlayerInputHandler()
        {
            lastMouseState = Mouse.GetState();
        }
        public void Update(Player player)
        {
            acceleration = baseAcceleration;
            if (player.GetSpeedMultiplier() > 0)
                acceleration *= player.GetSpeedMultiplier();

            //handle keyboard
            KeyboardState keyboard = Keyboard.GetState();

            player.isWalkingX = false;
            player.isWalkingY = false;
            if (keyboard.IsKeyDown(Keys.W))
            {
                player.isWalkingY = true;
                player.SetYVelocity(player.GetYVelocity() - acceleration);   
            }
            if (keyboard.IsKeyDown(Keys.A))
            {
                player.isWalkingX = true;
                player.SetXVelocity(player.GetXVelocity() - acceleration);
            }
            if (keyboard.IsKeyDown(Keys.S))
            {
                player.isWalkingY = true;
                player.SetYVelocity(player.GetYVelocity() + acceleration);
            }
            if (keyboard.IsKeyDown(Keys.D))
            {
                player.isWalkingX = true;
                player.SetXVelocity(player.GetXVelocity() + acceleration);
            }

            //pickup Weapon
            if(keyboard.IsKeyDown(Keys.F) && lastKeyboardState.IsKeyUp(Keys.F) && player.canPickup != null)
            {
                player.PickupItem(player.canPickup);
            }

            //switch Weapon
            if (keyboard.IsKeyDown(Keys.E) && lastKeyboardState.IsKeyUp(Keys.E))
            {
                player.heldItemManager.SwitchEquiped();
            }

            //handle mouse
            MouseState mouse = Mouse.GetState();
            if(mouse.LeftButton == ButtonState.Pressed)
            {
                if(player.heldItemManager.GetWeaponType() == HeldItemHandler.WeaponType.Ranged
                    || player.heldItemManager.GetWeaponType() == HeldItemHandler.WeaponType.Melee && lastMouseState.LeftButton == ButtonState.Released)
                player.heldItemManager.Attack(player.camera.ScreenToWorld(new Vector2(mouse.X, mouse.Y)));
            }

            lastKeyboardState = keyboard;
            lastMouseState = mouse;
        }
    }
}
