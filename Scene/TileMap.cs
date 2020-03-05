using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace UTDG
{
    public class TileMap
    {
        private readonly int[,] map;
        private readonly int tileSize = 64;
        private int roomCountW = 10;
        private int roomCountH = 10;
        private readonly int roomWidth = 9; //don't change

        Texture2D tileMap;
        private MapGenerator mapGenerator;
        private Vector2 playerSpawn;

        private List<int[,]> room1Layouts;
        private List<int[,]> room2Layouts;
        private List<int[,]> room3Layouts;
        private List<int[,]> room4Layouts;
        private List<int[,]> room5Layouts;

        public List<Tile> tiles;
        private List<Vector2> enemySpawns;

        public int GetWidth() { return map.GetLength(0) * tileSize; }
        public int GetHeight() { return map.GetLength(1) * tileSize; }
        public Vector2 GetPXPosition(Vector2 position) { return new Vector2(position.X * tileSize, position.Y * tileSize); }
        public int[,] GetTileMap() { return map; }
        public int GetTileSize() { return tileSize; }
        public Vector2 GetSpawn() { return GetPXPosition(playerSpawn); }
        public List<Vector2> GetEnemySpawns() { return enemySpawns; }

        public TileMap(Texture2D tileMap)
        {
            this.tileMap = tileMap;
            mapGenerator = new MapGenerator(roomCountW, roomCountH);
            map = new int[roomCountW * roomWidth, roomCountH * roomWidth];
            room1Layouts = new List<int[,]>();
            room2Layouts = new List<int[,]>();
            room3Layouts = new List<int[,]>();
            room4Layouts = new List<int[,]>();
            room5Layouts = new List<int[,]>();
            tiles = new List<Tile>();

            //generate map
            LoadRoomLayout();
            GenerateNewMap();
            AddWallSides();
            ConvertToTiles();
            enemySpawns = GenerateEnemySpawns();
        }

        private void LoadRoomLayout()
        {
            string path = Path.Combine(System.IO.Path.GetFullPath("../../../Scene/RoomTypes9x9.txt"));
            bool changeType = false;
            int type = 1;

            try
            {
                if (File.Exists(path))
                {
                    using (StreamReader sr = new StreamReader(path))
                    {
                        bool isAdding = false;
                        int[,] currentArr = new int[roomWidth, roomWidth];
                        int countx = 0;
                        int county = 0;
                        while (sr.Peek() >= 0)
                        {
                            char current = (char)sr.Read();
                            if (changeType)
                            {
                                type = Convert.ToInt32(current.ToString());
                                changeType = false;
                            }
                            if (current == ';')
                            {
                                isAdding = false;
                                countx = 0;
                                county = 0;
                                switch (type)
                                {
                                    case 1: room1Layouts.Add(currentArr); break;
                                    case 2: room2Layouts.Add(currentArr); break;
                                    case 3: room3Layouts.Add(currentArr); break;
                                    case 4: room4Layouts.Add(currentArr); break;
                                    case 5: room5Layouts.Add(currentArr); break;
                                }

                                currentArr = new int[roomWidth, roomWidth];
                            }
                            else if (current == '+') changeType = true;

                            if (isAdding && current != ',' && current != ' ' && current != '\n')
                            {
                                currentArr[countx, county] = Convert.ToInt32(current.ToString());
                                countx++;
                                if (countx == roomWidth)
                                {
                                    countx = 0;
                                    county++;
                                }
                            }
                            if (current == ':') isAdding = true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void GenerateNewMap()
        {
            List<Room> rooms = mapGenerator.GenerateGrid();
            Random rnd = new Random();
            for (int i = 0; i < rooms.Count; i++)
            {
                int startX = (int)rooms[i].position.X * roomWidth;
                int startY = (int)rooms[i].position.Y * roomWidth;
                int[,] currentRoom = new int[roomWidth, roomWidth];
                switch (rooms[i].GetRoomType())
                {
                    case Room.RoomType.UP: currentRoom = (int[,])RotateRoom(room1Layouts[rnd.Next(room1Layouts.Count)], 2).Clone(); break;
                    case Room.RoomType.RIGHT: currentRoom = (int[,])RotateRoom(room1Layouts[rnd.Next(room1Layouts.Count)], 3).Clone(); break;
                    case Room.RoomType.DOWN: currentRoom = (int[,])room1Layouts[rnd.Next(room1Layouts.Count)].Clone(); break;
                    case Room.RoomType.LEFT: currentRoom = (int[,])RotateRoom(room1Layouts[rnd.Next(room1Layouts.Count)], 1).Clone(); break;
                    case Room.RoomType.RIGHT_DOWN: currentRoom = (int[,])room2Layouts[rnd.Next(room2Layouts.Count)].Clone(); break;
                    case Room.RoomType.UP_RIGHT: currentRoom = (int[,])RotateRoom(room2Layouts[rnd.Next(room2Layouts.Count)], 3).Clone(); break;
                    case Room.RoomType.LEFT_UP: currentRoom = (int[,])RotateRoom(room2Layouts[rnd.Next(room2Layouts.Count)], 2).Clone(); break;
                    case Room.RoomType.DOWN_LEFT: currentRoom = (int[,])RotateRoom(room2Layouts[rnd.Next(room2Layouts.Count)], 1).Clone(); break;
                    case Room.RoomType.HALL_HORIZONTAL: currentRoom = (int[,])RotateRoom(room3Layouts[rnd.Next(room3Layouts.Count)], 1).Clone(); break;
                    case Room.RoomType.HALL_VETICAL: currentRoom = (int[,])room3Layouts[rnd.Next(room3Layouts.Count)].Clone(); break;
                    case Room.RoomType.UP_RIGHT_DOWN: currentRoom = (int[,])room4Layouts[rnd.Next(room4Layouts.Count)].Clone(); break;
                    case Room.RoomType.LEFT_UP_RIGHT: currentRoom = (int[,])RotateRoom(room4Layouts[rnd.Next(room4Layouts.Count)], 3).Clone(); break;
                    case Room.RoomType.DOWN_LEFT_UP: currentRoom = (int[,])RotateRoom(room4Layouts[rnd.Next(room4Layouts.Count)], 2).Clone(); break;
                    case Room.RoomType.RIGHT_DOWN_LEFT: currentRoom = (int[,])RotateRoom(room4Layouts[rnd.Next(room4Layouts.Count)], 1).Clone(); break;
                    case Room.RoomType.ALL: currentRoom = (int[,])room5Layouts[rnd.Next(room5Layouts.Count)].Clone();break;
                }

                if (rooms[i].roomFunction == Room.RoomFunction.Spawn) playerSpawn = new Vector2(startX + 4, startY + 4);
                for(int x = 0; x < roomWidth; x++)
                {
                    for (int y = 0; y < roomWidth; y++)
                    {
                        map[startX + x, startY + y] = currentRoom[x, y];
                    }
                }
            }
        }

        private int[,] RotateRoom(int[,] room, int rotateAmount)
        {
            int[,] rotatedRoom = (int[,])room.Clone();
            for (int i = 0; i < rotateAmount; i++)
            {
                for (int x = 0; x < roomWidth / 2; x++)
                {
                    for (int y = x; y < roomWidth - x - 1; y++)
                    {
                        int temp = rotatedRoom[x, y];
                        rotatedRoom[x, y] = rotatedRoom[y, roomWidth - 1 - x];
                        rotatedRoom[y, roomWidth - 1 - x] = rotatedRoom[roomWidth - 1 - x, roomWidth - 1 - y];
                        rotatedRoom[roomWidth - 1 - x, roomWidth - 1 - y] = rotatedRoom[roomWidth - 1 - y, x];
                        rotatedRoom[roomWidth - 1 - y, x] = temp;
                    }
                }
            }
            return rotatedRoom;   
        }

        private void AddWallSides()
        {
            for(int x = 1; x < map.GetLength(0)-1; x++)
            {
                for(int y = 1; y< map.GetLength(1)-1; y++)
                {
                    if (map[x, y] == 0 && map[x, y - 1] == 1)
                        map[x, y] = 2;
                }
            }
        }

        private void ConvertToTiles()
        {
            for(int x =0; x< map.GetLength(0); x++)
            {
                for(int y = 0; y < map.GetLength(1); y++)
                {
                    tiles.Add(new Tile(map[x,y], GetPXPosition(new Vector2(x, y)), tileSize));
                }
            }
        }

        public List<Tile> GetCollisionTiles()
        {
            return tiles.Where(x => x.CanCollide() == true).ToList();
        }

        private List<Vector2> GenerateEnemySpawns()
        {
            List<Vector2> enemySpawns = new List<Vector2>();
            Random rnd = new Random();
            for(int i=0;i<20; i++)
            {
                int xPos = rnd.Next(map.GetLength(0));
                int yPos = rnd.Next(map.GetLength(1));
                while (!(map[xPos, yPos] == 0 && (map[xPos, yPos+1] == 0 && !enemySpawns.Contains(GetPXPosition(new Vector2(xPos, yPos))) && (xPos > GetSpawn().X + 5 || xPos < GetSpawn().X - 5) && (yPos > GetSpawn().Y + 5 || yPos  < GetSpawn ().Y - 5))))
                {
                    xPos = rnd.Next(map.GetLength(0));
                    yPos = rnd.Next(map.GetLength(1));
                }
                enemySpawns.Add(GetPXPosition(new Vector2(xPos, yPos - 1)));
            }
            return enemySpawns;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach(Tile tile in tiles)
            {
                tile.Draw(spriteBatch, tileMap);
            }
        }
    }

    public class Tile
    {
        public enum Type
        {
            floor,
            wall
        }

        private int id;
        public Type type;
        private bool canCollide;
        private Vector2 position;
        private Rectangle bounds;
        private int tileSize;

        public int GetId() { return id; }
        public bool CanCollide() { return canCollide; }
        public Vector2 GetPosition() { return position; }
        public Rectangle GetBounds() { return bounds; }

        public Tile(int id, Vector2 position, int tileSize)
        {
            switch (id)
            {
                case 0: type = Type.floor; break;
                case 1: type = Type.wall;break;
                case 2: type = Type.wall; break;
                default:type = Type.floor; break;
            }

            if (type == Type.wall) canCollide = true; else canCollide = false;
            this.tileSize = tileSize;
            this.id = id;
            this.position = position;
            this.bounds = new Rectangle((int)position.X, (int)position.Y, tileSize, tileSize);
        }

        public void Draw(SpriteBatch spriteBatch, Texture2D texture) {
            spriteBatch.Draw(texture, bounds, new Rectangle(id * tileSize, 0, tileSize - 1, tileSize - 1), Color.White);
        }
    }
}
