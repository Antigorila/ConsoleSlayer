using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleSlayer_02
{
    //This is blasphemy...
    public class Camera
    {
        public Vector2 Position { get; private set; }
        public Matrix Transform { get; private set; } //This matrix is no fun...
        private int viewportWidth;
        private int viewportHeight;

        public Camera(int viewportWidth, int viewportHeight)
        {
            this.viewportWidth = viewportWidth;
            this.viewportHeight = viewportHeight;
        }

        public void Follow(Vector2 targetPosition)
        {
            // Center the camera on the target position
            Position = targetPosition - new Vector2(viewportWidth / 2f, viewportHeight / 2f);

            // Update the transformation matrix to reflect the new camera position
            Transform = Matrix.CreateTranslation(new Vector3(-Position, 0));
        }
    }
}
