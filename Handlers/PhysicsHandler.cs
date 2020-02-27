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
            if (player.GetXVelocity() > maxVelocity) player.SetXVelocity(maxVelocity);
            if (player.GetXVelocity() < -maxVelocity) player.SetXVelocity(-maxVelocity);
            if (player.GetYVelocity() > maxVelocity) player.SetYVelocity(maxVelocity);
            if (player.GetYVelocity() < -maxVelocity) player.SetYVelocity(-maxVelocity);

            //check that player doesn't leave map bounds
            if (player.GetPosition().X + player.GetXVelocity() + player.GetOrigin().X > collisionManager.GetMapBounds().Width)
            {
                player.SetXVelocity(0);
                player.SetXPosition(collisionManager.GetMapBounds().Width - player.GetOrigin().X);
            }
            if (player.GetPosition().X + player.GetXVelocity() - player.GetOrigin().X < 0)
            {
                player.SetXVelocity(0);
                player.SetXPosition(player.GetOrigin().X);
            }
            if (player.GetPosition().Y + player.GetYVelocity() + player.GetOrigin().Y > collisionManager.GetMapBounds().Height)
            {
                player.SetYVelocity(0);
                player.SetYPosition(collisionManager.GetMapBounds().Height - player.GetOrigin().Y);
            }
            if (player.GetPosition().Y + player.GetYVelocity() - player.GetOrigin().Y< 0)
            {
                player.SetYVelocity(0);
                player.SetYPosition(player.GetOrigin().Y);
            }

            //apply friction
            if (!player.isWalkingX)
            {
                player.SetXVelocity(player.GetXVelocity() * walkfriction);
                if(Math.Abs((double)player.GetXVelocity()) < 0.1){
                    player.SetXVelocity(0.0f);
                }
            }
            if (!player.isWalkingY)
            {
                player.SetYVelocity(player.GetYVelocity() * walkfriction);
                if (Math.Abs((double)player.GetYVelocity()) < 0.1)
                {
                    player.SetYVelocity(0.0f);
                }
            }
        }
    }
}
