using OpenTK;
using OpenTK.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Engine.Core
{
    public struct CameraControlInput
    {
        public int xOffset;
        public int yOffset;
        public MouseButtons Button;
        public int Wheel;

        public CameraControlInput(int xOffset, int yOffset, MouseButtons button, int wheel)
        {
            this.xOffset = xOffset;
            this.yOffset = yOffset;
            Button = button;
            Wheel = wheel;
        }
    }


    public class CameraController
    {
        private readonly Camera Cam;

        public float MouseSensitivity = 0.001f;
        public float MouseSpeed;

        public CameraController(Camera cam)
        {
            Cam = cam;
        }

        public void ProcessInput(CameraControlInput input)
        {
            if (input.Button == MouseButtons.Middle)
            {
                Pan(input.xOffset, input.yOffset);
            }
            if (input.Wheel != 0)
            {
                Zoom(input.Wheel);
            }

        }

        public void Zoom(int val)
        {
            Cam.Position += (Cam.Front * val * MouseSensitivity);
        }

        public void Pan(float xoffset, float yoffset)
        {
            xoffset *= MouseSensitivity;
            yoffset *= MouseSensitivity;

            Cam.Position -= Cam.Right * xoffset;
            Cam.Position += Cam.Up * yoffset;

        }

        public void Rotate(float xoffset, float yoffset)
        {
            Quaternion q1 = new Quaternion(yoffset, -xoffset, 0);

            Cam.Front = q1 * Cam.Front.Normalized();
            Cam.Right = Vector3.Cross(Cam.Front, Vector3.UnitY).Normalized();
            Cam.Up = Vector3.Cross(Cam.Right, Cam.Front).Normalized();
        }

        public void OrbitAround(float xoffset, float yoffset, Vector3 target)
        {
            float radius = (Cam.Position - target).Length;

            float sinY = xoffset ;
            float sinX = yoffset ;

            float angleX = (float)Math.Asin(sinX);
            float angleY = (float)Math.Asin(sinY);

            Quaternion q1 = new Quaternion(new Vector3(angleX,angleY,0));

            Cam.Position = q1 * Cam.Position;

            Cam.Front = (target - Cam.Position).Normalized();
            Cam.Right = Vector3.Cross(Cam.Front, Vector3.UnitY).Normalized();
            Cam.Up = Vector3.Cross(Cam.Right, Cam.Front).Normalized();

        }
    }
}
