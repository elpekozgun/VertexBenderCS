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

    public enum eCameraMovement : byte
    {
        forward = 1,
        backward = 2,
        left = 4,
        right = 8,
        up = 16,
        down = 32
    }

    public class CameraController
    {
        private readonly Camera Cam;

        public float MouseSensitivity = 0.1f;
        public float MouseSpeed;

        private float _yaw;
        private float _pitch;

        public CameraController(Camera cam)
        {
            Cam = cam;
            _yaw = -90.0f;
            _pitch = 0.0f;

            UpdateCameraVectors();
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
            Cam.Position += (Cam.Front * val * MouseSensitivity * 0.1f);
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

            xoffset *= MouseSensitivity;
            yoffset *= MouseSensitivity;

            _yaw += xoffset;
            _pitch += yoffset;

            if (_pitch > 70.0f)
            {
                _pitch = 70.0f;
            }
            if (_pitch < -70.0f)
            {
                _pitch = -70.0f;
            }

            //Quaternion q1 = new Quaternion(yoffset, -xoffset, 0);

            //Cam.Front = q1 * Cam.Front.Normalized();
            UpdateCameraVectors();
        }

        public void Reset()
        {
            Cam.Position = new Vector3(0, 0, 0);
            UpdateCameraVectors();
        }

        public void OrbitAround(float xoffset, float yoffset, Vector3 target)
        {
            xoffset *= MouseSensitivity;
            yoffset *= MouseSensitivity;

            float radius = (Cam.Position - target).Length;

            float sinY = xoffset / radius;
            float sinX = yoffset / radius;

            float angleX = (float)Math.Asin(sinX);
            float angleY = (float)Math.Asin(sinY);

            Quaternion q1 = new Quaternion(new Vector3(angleX,angleY,0));

            Cam.Position = q1 * Cam.Position;

            Cam.Front = (target - Cam.Position).Normalized();
            Cam.Right = Vector3.Cross(Cam.Front, Vector3.UnitY).Normalized();
            Cam.Up = Vector3.Cross(Cam.Right, Cam.Front).Normalized();

        }

        public void Navigate(eCameraMovement movement, float delta = 0.1f)
        {
            if ((movement & eCameraMovement.forward) == eCameraMovement.forward)
            {
                Cam.Position += Cam.Front * delta;
            }
            if ((movement & eCameraMovement.backward) == eCameraMovement.backward)
            {
                Cam.Position -= Cam.Front * delta;
            }
            if ((movement & eCameraMovement.left) == eCameraMovement.left)
            {
                Cam.Position -= Cam.Right * delta;
            }
            if ((movement & eCameraMovement.right) == eCameraMovement.right)
            {
                Cam.Position += Cam.Right * delta;
            }
            if ((movement & eCameraMovement.up) == eCameraMovement.up)
            {
                Cam.Position += Vector3.UnitY * delta;
            }
            if ((movement & eCameraMovement.down) == eCameraMovement.down)
            {
                Cam.Position -= Vector3.UnitY * delta;
            }
        }

        private void UpdateCameraVectors()
        {
            Cam.Front = new Vector3
            (
                (float)(Math.Cos(MathHelper.DegreesToRadians(_yaw)) * Math.Cos(MathHelper.DegreesToRadians(_pitch))),
                (float)Math.Sin(MathHelper.DegreesToRadians(_pitch)),
                (float)(Math.Sin(MathHelper.DegreesToRadians(_yaw)) * Math.Cos(MathHelper.DegreesToRadians(_pitch)))
            ).Normalized();

            Cam.Right = Vector3.Cross(Cam.Front, Vector3.UnitY).Normalized();
            Cam.Up = Vector3.Cross(Cam.Right, Cam.Front).Normalized();

        }
    }
}
