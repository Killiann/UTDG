using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace UTDG
{
    public class Camera
    {
        private Vector2 position;
        private readonly float scale = 0.8f;
        private readonly int viewportWidth;
        private readonly int viewportHeight;

        private bool isScreenShaking;
        private float xOffset;
        private float yOffset;

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
                return Matrix.CreateTranslation(-(int)position.X + xOffset, -(int)position.Y + yOffset, 0)
                    * Matrix.CreateScale(scale)
                    * Matrix.CreateTranslation(new Vector3(ViewPortCentre, 0));
            }
        }
        
        //public void ScreenShake()
        //{            
        //    xOffset = 2;
        //    yOffset = 2;
        //}

        public Camera(Viewport viewPort)
        {
            viewportWidth = viewPort.Width;
            viewportHeight = viewPort.Height;
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
            xOffset *= -0.95f;
            yOffset *= -0.95f;
            if (xOffset < 0.1f && xOffset > -0.1f) xOffset = 0;
            if (yOffset < 0.1f && yOffset > -0.1f) yOffset = 0;

            position = newPosition;
        }
    }
}
