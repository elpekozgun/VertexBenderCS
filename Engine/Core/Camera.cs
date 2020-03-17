using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;

namespace Engine.Core
{
    public class Camera
    {
        public Camera(float width, float height, float fov = 45, float near = 0.1f, float far = 100.0f)
        {
            Position = Vector3.Zero;
            Front = -Vector3.UnitZ;
            //Position = new Vector3(-1, 2, -3);
            //Front = new Vector3(1, -2, 3);

            Up = Vector3.UnitY;
            Right = Vector3.UnitX;
            AspectRatio = width / height;
            OrthoSize = new Vector2(AspectRatio,1.0f);
            
            FoV = fov;
            Near = near;
            Far = far;

        }

        public Vector3 Position;
        public Vector3 Front;
        public Vector3 Up;
        public Vector3 Right;
        public Vector2 OrthoSize;

        public float Near;
        public float Far;
        public float FoV;
        public float AspectRatio;
        public bool IsOrtho;

        public Matrix4 Projection
        {
            get
            {
                if (IsOrtho)
                {
                    return Matrix4.CreateOrthographic(OrthoSize.X, OrthoSize.Y, Near, Far);
                }
                else
                {
                    return Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(FoV),AspectRatio, Near, Far);
                }
            }
        }

        public Matrix4 View
        {
            get
            {
                return Matrix4.LookAt(Position, Position + Front, Up);
            }
        }

    }
}
