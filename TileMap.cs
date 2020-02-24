using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace UTDG
{
    public class TileMap
    {
        private readonly int[,] map;
        private readonly int[,] occupied;
        private readonly int tileSize = 64;
        Texture2D tileMap;

        public TileMap()
        {
            map = new int[20,20];
            occupied = new int[map.GetLength(0), map.GetLength(1)];
        }

        public void LoadContent(Game game)
        {
            tileMap = game.Content.Load<Texture2D>("images/tileMap");
        }

        public int GetWidth()
        {
            return map.GetLength(0) * tileSize;
        }

        public int GetHeight()
        {
            return map.GetLength(1) * tileSize;
        }

        public Vector2 GetPXPosition(Vector2 position)
        {
            return new Vector2(position.X * tileSize, position.Y*tileSize);
        }

        public void AddToOccupied(int x, int y)
        {
            if (occupied[x, y] == 0)
            {
                occupied[x, y] = 1;
            }
        }
        
        public void RemoveFromOccupied(int x, int y)
        {
            if (occupied[x, y] == 1)
            {
                occupied[x, y] = 0;
            }
        }

        public bool IsOccupied(int x, int y)
        {
            if (occupied[x, y] == 1)
            {
                return true;
            }
            return false;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            for (int x = 0; x < map.GetLength(0); x++)
            {
                for (int y = 0; y < map.GetLength(1); y++)
                {
                    spriteBatch.Draw(tileMap, new Rectangle(x * tileSize, y*tileSize, tileSize, tileSize), Color.White);
                }
            }
        }
    }
}
