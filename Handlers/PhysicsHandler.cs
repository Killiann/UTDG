using System;
namespace UTDG
{
    public class PhysicsHandler
    {
        private CollisionHandler collisionManager;
        private float maxBaseVelocity = 5.0f;
        private float maxVelocity;
        private readonly float walkfriction = 0.85f;

        public PhysicsHandler(CollisionHandler collisionManager)
        {
            this.collisionManager = collisionManager;
        }

        public void Update(Player player)
        {
            //apply speedboost
            maxVelocity = maxBaseVelocity;
            if (player.GetSpeedMultiplier() > 0)
                maxVelocity *= player.GetSpeedMultiplier();

            //cap velocity
            if (player.GetVelocity().X > maxVelocity) player.SetXVelocity(maxVelocity);
            if (player.GetVelocity().X < -maxVelocity) player.SetXVelocity(-maxVelocity);
            if (player.GetVelocity().Y > maxVelocity) player.SetYVelocity(maxVelocity);
            if (player.GetVelocity().Y < -maxVelocity) player.SetYVelocity(-maxVelocity);

            //check that player doesn't leave map bounds
            if (player.GetPosition().X + player.GetVelocity().X + player.GetOrigin().X > collisionManager.GetMapBounds().Width)
            {
                player.SetXVelocity(0);
                player.SetXPosition(collisionManager.GetMapBounds().Width - player.GetOrigin().X);
            }
            if (player.GetPosition().X + player.GetVelocity().X - player.GetOrigin().X < 0)
            {
                player.SetXVelocity(0);
                player.SetXPosition(player.GetOrigin().X);
            }
            if (player.GetPosition().Y + player.GetVelocity().Y + player.GetOrigin().Y > collisionManager.GetMapBounds().Height)
            {
                player.SetYVelocity(0);
                player.SetYPosition(collisionManager.GetMapBounds().Height - player.GetOrigin().Y);
            }
            if (player.GetPosition().Y + player.GetVelocity().Y - player.GetOrigin().Y< 0)
            {
                player.SetYVelocity(0);
                player.SetYPosition(player.GetOrigin().Y);
            }

            //apply friction
            if (!player.isWalkingX)
            {
                player.SetXVelocity(player.GetVelocity().X * walkfriction);
                if(Math.Abs((double)player.GetVelocity().X) < 0.1){
                    player.SetXVelocity(0.0f);
                }
            }
            if (!player.isWalkingY)
            {
                player.SetYVelocity(player.GetVelocity().Y * walkfriction);
                if (Math.Abs((double)player.GetVelocity().Y) < 0.1)
                {
                    player.SetYVelocity(0.0f);
                }
            }
        }
    }
}
