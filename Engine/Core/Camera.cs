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
        public Camera(float width, float height, float fov = 45, float near = 0.01f, float far = 100.0f)
        {
            Position = Vector3.Zero;
            Front = -Vector3.UnitZ;
            //Position = new Vector3(-1, 2, -3);
            //Front = new Vector3(1, -2, 3);

            Up = Vector3.UnitY;
            Right = Vector3.UnitX;
            _aspectRatio = width / height;
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

        private float _aspectRatio;

        public float Near;
        public float Far;
        public float FoV;
        public bool IsOrtho;

        public float AspectRatio
        {
            get
            {
                return _aspectRatio;
            }
            set
            {
                _aspectRatio = value;
                OrthoSize = new Vector2(_aspectRatio, 1.0f );
            }
        }

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
                    return Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(FoV),_aspectRatio, Near, Far);
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
