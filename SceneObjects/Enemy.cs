using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace UTDG
{
    public class Enemy : DynamicObj
    {
        public EntityCollisionHandler collisionHandler;
        public States state;
        private TileMap tileMap;

        //idle
        bool isWalking;
        int time;
        int timeBetween;
        Random rnd;
        float movingAngle;
        Vector2 direction;
        float speed = 1.5f;
        public bool colliding = false;

        public bool canSeePlayer = false;

        List<Vector2> path;
        List<Vector2> lineOfSight;
        private readonly int viewDistance = 10;

        public override Rectangle GetBounds()
        {
            return new Rectangle((int)position.X, (int)position.Y + (int)dimensions.X, (int)dimensions.X, (int)dimensions.Y);
        }

        public override void SetYPosition(float newYPosition)
        {
            position.Y = newYPosition - dimensions.X;
        }

        public enum States
        {
            idle,
            charging,
            searching
        }

        public Enemy(Vector2 position, Texture2D texture, TileMap tileMap)
        {
            collisionHandler = new EnemyCollisionHandler(tileMap);
            this.tileMap = tileMap;
            this.position = position;
            this.texture = texture;
            state = States.idle;
            dimensions = new Vector2(texture.Width, texture.Height);
            path = new List<Vector2>();
            rnd = new Random();
            //idle
            isWalking = false;
            time = 0;
            timeBetween = rnd.Next(200);

        }

        public void Update(Player player)
        {
            canSeePlayer = IsInLineOfSight(player);

            if (canSeePlayer)
            {
                path = SearchAlgorithm(position, player.GetPosition(), tileMap);
                state = States.charging;
            }
            
            switch (state)
            {
                case States.idle:
                    UpdateIdle();
                    break;
                case States.charging:
                    UpdateCharging(player);
                    break;
                case States.searching:

                    break;
                default:

                    break;
            }
        }

        private void UpdateIdle()
        {
            time++;
            if (time == timeBetween)
            {
                if (isWalking) isWalking = false;
                else
                {
                    movingAngle = (float)rnd.Next((int)(2 * Math.PI * 10)) / 10;
                    direction = new Vector2((float)Math.Cos(movingAngle), (float)Math.Sin(movingAngle));
                    isWalking = true;
                }
                time = 0;
                timeBetween = rnd.Next(50, 200);
            }
            if (isWalking)
            {
                velocity = (direction * speed);
                collisionHandler.Update(this);
                position += velocity;
                if (collidingHor || collidingVer)
                {
                    isWalking = false;
                    collidingHor = false;
                    collidingVer = false;
                    time = 0;
                    timeBetween = rnd.Next(50, 200);
                }
            }
        }

        private void UpdateCharging(Player player)
        {
            Vector2 nextPos = new Vector2();
            if (path!= null)
                nextPos = new Vector2(path.First().X * 64, path.First().Y * 64);
            if (position == nextPos)
            {
                path.Remove(path.First());
                if(path.Count > 0)
                    nextPos = new Vector2(path.First().X * 64, path.First().Y * 64);
            }
            double dx = nextPos.X - position.X;
            double dy = nextPos.Y - position.Y;
            float angle = (float)Math.Atan2(dy, dx);
            direction = new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle));

            speed = 5f;
            velocity = (direction * speed);
            //collisionHandler.Update(this);
            position += velocity;

        }

        private void UpdateSearching()
        {

        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);

            //if (path!= null)
            //{
            //    foreach (Vector2 pos in path)
            //    {
            //        spriteBatch.Draw(texture, new Rectangle((int)pos.X * 64, (int)pos.Y * 64, 64, 64), Color.Red);
            //    }
            //}

            //if(lineOfSight != null && canSeePlayer)
            //{
            //    foreach(Vector2 pos in lineOfSight)
            //    {
            //        spriteBatch.Draw(texture, new Rectangle((int)pos.X * 64, (int)pos.Y * 64, 64, 64), Color.Red);
            //    }
            //}
        }

        private bool CheckLineLow(int x0, int y0, int x1, int y1)
        {
            int[,] map = tileMap.GetTileMap();
            int dx = x1 - x0;
            int dy = y1 - y0;
            int yi = 1;
            if(dy < 0)
            {
                yi = -1;
                dy = -dy;
            }
            int D = 2 * dy - dx;
            int y = y0;
            for(int x = x0; x < x1; x++)
            {
                if (lineOfSight.Count < viewDistance)
                {
                    if (x > 0 && x < map.GetLength(0) && y > 0 && y < map.GetLength(1))
                    {
                        if (map[x, y] == 0)
                        {
                            lineOfSight.Add(new Vector2(x, y));
                            if (D > 0)
                            {
                                y += yi;
                                D -= 2 * dx;
                            }
                            D += 2 * dy;
                        }
                        else return false;
                    }
                    else return false;
                }
                else return false;
            }
            return true;
        }

        private bool CheckLineHigh(int x0, int y0, int x1, int y1)
        {
            int[,] map = tileMap.GetTileMap();
            int dx = x1 - x0;
            int dy = y1 - y0;
            int xi = 1;
            if (dx < 0)
            {
                xi = -1;
                dx = -dx;
            }
            int D = 2 * dx - dy;
            int x = x0;
            for (int y = y0; y < y1; y++)
            {
                if (lineOfSight.Count < viewDistance)
                {
                    if (x > 0 && x < map.GetLength(0) && y > 0 && y < map.GetLength(1))
                    {
                        if (map[x, y] == 0)
                        {
                            lineOfSight.Add(new Vector2(x, y));
                            if (D > 0)
                            {
                                x += xi;
                                D -= 2 * dy;
                            }
                            D += 2 * dx;
                        }
                        else return false;
                    }
                    else return false;
                }
                else return false;
            }
            return true;
        }

        private bool IsInLineOfSight(Player player)
        {
            int x0 = (int)Math.Round((decimal)position.X / 64);
            int y0 = (int)Math.Round((decimal)position.Y / 64);
            int x1 = (int)Math.Round((decimal)player.GetPosition().X / 64);
            int y1 = (int)Math.Round((decimal)player.GetPosition().Y / 64);

            lineOfSight = new List<Vector2>();
            if(Math.Abs(y1 - y0) < Math.Abs(x1 - x0))
            {
                if (x0 > x1)
                    return CheckLineLow(x1, y1, x0, y0);
                else
                    return CheckLineLow(x0, y0, x1, y1);
            }
            else
            {
                if (y0 > y1)
                    return CheckLineHigh(x1, y1, x0, y0);
                else
                    return CheckLineHigh(x0, y0, x1, y1);
            }
        }

        private List<Vector2> SearchAlgorithm(Vector2 startPos, Vector2 endPos, TileMap tileMap)
        {
            int[,] map = tileMap.GetTileMap();
            List<Node> openList = new List<Node>();
            List<Node> closedList = new List<Node>();
            //add head
            startPos.X = (int)Math.Round(startPos.X / 64);
            startPos.Y = (int)Math.Round(startPos.Y / 64);

            endPos.X = (int)Math.Round(endPos.X / 64);
            endPos.Y = (int)Math.Round(endPos.Y / 64);

            Node head = new Node(startPos);
            head.g = 0;
            head.f = head.g + Heuristic(startPos, endPos);
            openList.Add(head);

            Node openNeighbor;
            Node current;
            while (openList.Count > 0)
            {
                current = openList.OrderBy(x => x.f).First();
                if (current.position == endPos)
                    return ConstructPath(current);
                closedList.Add(current);
                openList.Remove(current);
                List<Node> neighbors = Neighbors(current, map, endPos);
                foreach (Node neighbor in neighbors)
                {
                    if (!closedList.Any(x=>x.position == neighbor.position))
                    {
                        neighbor.f = neighbor.g + Heuristic(neighbor.position, endPos);
                        if (!openList.Any(x=>x.position == neighbor.position))
                        {
                            openList.Add(neighbor);
                        }
                        else
                        {
                            openNeighbor = openList.Where(x=>x.position == neighbor.position).First();
                            if (neighbor.g < openNeighbor.g)
                            {
                                openNeighbor.g = neighbor.g;
                                openNeighbor.prevNode = neighbor.prevNode;
                            }
                        }
                    }
                }
            }
            return null;
        }

        private List<Node> Neighbors(Node node, int[,] tileMap, Vector2 endPos)
        {
            List<Node> returnList = new List<Node>();
            for(int x = -1; x < 2; x++)
            {
                for (int y = -1; y < 2; y++)
                {
                    int xpos = (int)node.position.X + x;
                    int ypos = (int)node.position.Y + y;
                    if (xpos >= 0 && xpos < tileMap.GetLength(0) && ypos > 0 && ypos < tileMap.GetLength(1))
                    {
                        if (tileMap[(int)node.position.X + x, (int)node.position.Y + y] == 0)
                        {
                            Node newNode = new Node(new Vector2((int)node.position.X + x, (int)node.position.Y + y));
                            newNode.g = node.g + 1;
                            newNode.prevNode = node;
                            returnList.Add(newNode);
                        }
                    }
                }
            }
            return returnList;
        }

        private List<Vector2> ConstructPath(Node node)
        {
            List<Vector2> path = new List<Vector2>();
            path.Add(node.position);
            while(node.prevNode != null)
            {
                node = node.prevNode;
                path.Add(node.position);
            }
            return path;
        }

        private float Heuristic(Vector2 start, Vector2 end)
        {
            return (float)Math.Sqrt(Math.Pow(start.X - end.X, 2) + Math.Pow(start.Y - end.Y, 2));
        }

        private class Node
        {
            public Vector2 position;
            public Node nextNode;
            public Node prevNode;

            public float g;
            private float gSquared;
            public float h;
            public float f;
            //for start
            public Node(Vector2 position)
            {
                this.position = position;
            }

            public Node(Vector2 position, Node prevNode,Vector2 endPos)
            {
                this.prevNode = prevNode;
                g = prevNode.g + 1;
                gSquared = (float)Math.Pow(g, 2);
                h = (float)Math.Pow(position.X - endPos.X, 2) + (float)Math.Pow(position.Y - endPos.Y, 2);
                f = g + h;
            }
        }
    }
}
