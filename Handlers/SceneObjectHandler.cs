using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace UTDG
{
    class SceneObjectHandler
    {
        private List<GameObj> sceneObjects;
        private TileMap tileMap;
        private int[,] occupiedPlaces;

        public List<GameObj> GetSceneObjects() { return sceneObjects; }

        public SceneObjectHandler(TileMap tileMap)
        {
            this.tileMap = tileMap;
            sceneObjects = new List<GameObj>();
            occupiedPlaces = new int[tileMap.GetTileMap().GetLength(0), tileMap.GetTileMap().GetLength(1)];
        }

        public void AddObject(GameObj newObject)
        {
            int xPos = (int)Math.Round(newObject.GetPosition().X / tileMap.GetTileSize());
            int yPos = (int)Math.Round(newObject.GetPosition().Y / tileMap.GetTileSize());

            if (occupiedPlaces[xPos, yPos] == 0)
            {
                sceneObjects.Add(newObject);
                occupiedPlaces[xPos, yPos] = 1;
            }
            else
            {
                int checkWidth = 1;
                int checkHeight = 1;
                while (occupiedPlaces[xPos, yPos] != 0)
                {                    
                    checkWidth += 2;
                    checkHeight += 2;
                    if (xPos >= 1) xPos--;
                    if (yPos >= 1) yPos--;
                    for (int i = 0; i < checkWidth; i++)
                    {                        
                        for (int j = 0; j < checkHeight; j++)
                        {
                            if (occupiedPlaces[xPos + i, yPos + j] == 0)
                            {
                                xPos = xPos + i;
                                yPos = yPos + j;
                                break;
                            }
                        }
                        if (occupiedPlaces[xPos, yPos] == 0) break;
                    }
                }
                newObject.SetXPosition(xPos * tileMap.GetTileSize());
                newObject.SetYPosition(yPos * tileMap.GetTileSize());
                sceneObjects.Add(newObject);
                occupiedPlaces[xPos, yPos] = 1;
            }
        }

        public void RemoveObject(GameObj obj)
        {
            if (sceneObjects.Contains(obj))
            {
                occupiedPlaces[(int)(obj.GetPosition().X / tileMap.GetTileSize()), (int)(obj.GetPosition().Y / tileMap.GetTileSize())] = 0;
                sceneObjects.Remove(obj);
            }
        }

        public void Update(Player player, GameOverlay gameOverlay)
        {
            for(int i = 0; i < sceneObjects.Count; i++)
            {
                GameObj obj = sceneObjects[i];
                if (player.collisionManager.IsColliding(((Item)obj).collisionManager.GetBounds()))
                {
                    player.PickupItem((Item)obj);
                    if (obj.GetType() == typeof(Pickup_Melee)) gameOverlay.meleeItem.ChangeWeaponTexture(((Item)obj).GetTexture());
                    else if (obj.GetType() == typeof(Pickup_Ranged)) gameOverlay.rangedItem.ChangeWeaponTexture(((Item)obj).GetTexture());
                    RemoveObject(obj);
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            for(int i = 0; i < sceneObjects.Count; i++)
            {
                sceneObjects[i].Draw(spriteBatch);
            }
        }
    }
}
