using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace UTDG
{
    public class Camera
    {
        private Vector2 position;
        private readonly float scale;
        public int viewportWidth;
        public int viewportHeight;

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

        public Camera()
        {
            scale = 0.8f;
        }

        public void SetPosition(Vector2 newPosition)
        {
            position = newPosition;
        }
    }
}
