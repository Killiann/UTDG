using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace UTDG
{
    public class Camera
    {
        private Vector2 position;
        private readonly float scale = 0.75f;
        private readonly int viewportWidth;
        private readonly int viewportHeight;
        private readonly TileMap map;

        public Vector2 ViewPortCentre
        {
            get
            {
                return new Vector2(viewportWidth * 0.5f, viewportHeight * 0.5f);
            }
        }

        public Matrix TranslationMatrix
        {
            get
            {
                return Matrix.CreateTranslation(-(int)position.X, -(int)position.Y, 0)
                    * Matrix.CreateScale(scale)
                    * Matrix.CreateTranslation(new Vector3(ViewPortCentre, 0));
            }
        }

        public Camera(Viewport viewPort, TileMap _map)
        {
            viewportWidth = viewPort.Width;
            viewportHeight = viewPort.Height;
            map = _map;
        }

        public Vector2 WorldToScreen(Vector2 coord)
        {
            return Vector2.Transform(coord, TranslationMatrix);
        }

        public Vector2 ScreenToWorld(Vector2 coord)
        {
            return Vector2.Transform(coord, Matrix.Invert(TranslationMatrix));
        }

        public void SetPosition(Vector2 newPosition)
        {
            position.Y = newPosition.Y;
            //xpos
            if ((newPosition.X - (viewportWidth / 2 / scale)) > 0
                && newPosition.X + (viewportWidth / 2 / scale) < map.GetWidth())
                position.X = newPosition.X;
            else
            {
                if ((newPosition.X - (viewportWidth / 2 / scale)) < 0)
                    position.X = viewportWidth / 2 / scale;
                else if (newPosition.X + (viewportWidth / 2 / scale) > map.GetWidth())
                    position.X = map.GetWidth() - viewportWidth / 2 / scale;
            }

            //ypos
            if((newPosition.Y - (viewportHeight / 2 / scale)) > 0
                && newPosition.Y + (viewportHeight / 2 / scale) < map.GetHeight())
                position.Y = newPosition.Y;
            else
            {
                if ((newPosition.Y - (viewportHeight / 2 / scale)) < 0)
                    position.Y = viewportHeight / 2 / scale;
                else if (newPosition.Y + (viewportHeight / 2 / scale) > map.GetHeight())
                    position.Y = map.GetHeight() - viewportHeight / 2 / scale;
            }
        }
    }
}
