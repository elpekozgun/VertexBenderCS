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

        public float MouseSensitivity = 0.001f;
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

        public void Zoom(int val, bool isBoosted = false)
        {
            Cam.OrthoSize =  new Vector2(Math.Max(Cam.OrthoSize.X - val * Cam.AspectRatio * MouseSensitivity, 0.0f) ,Math.Max(Cam.OrthoSize.Y - val * MouseSensitivity, 0.0f));

            if (isBoosted)
            {
                Cam.Position += (Cam.Front * val * MouseSensitivity * 0.03f);
                return;
            }
            Cam.Position += (Cam.Front * val * MouseSensitivity * 0.001f);
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

            xoffset *= MouseSensitivity*100;
            yoffset *= MouseSensitivity*100;

            _yaw += xoffset;
            _pitch += yoffset;

            if (_pitch > 89.0f)
            {
                _pitch = 89.0f;
            }
            if (_pitch < -89.0f)
            {
                _pitch = -89.0f;
            }
            //Logger.Log(_pitch.ToString());
            //Logger.Log(_yaw.ToString());
            //Quaternion q1 = new Quaternion(yoffset, -xoffset, 0);
            //Quaternion q1 = new Quaternion(MathHelper.DegreesToRadians(_yaw), MathHelper.DegreesToRadians(_pitch), 0);

            //Cam.Front = q1 * Cam.Front.Normalized();
            //Cam.Right = Vector3.Cross(Cam.Front, Vector3.UnitY).Normalized();
            //Cam.Up = Vector3.Cross(Cam.Right, Cam.Front).Normalized();
            //Logger.Log(_yaw.ToString());

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

            Quaternion q = new Quaternion(new Vector3(angleX, angleY, 0));

            Cam.Position = q * Cam.Position;
            Cam.Front = (target - Cam.Position).Normalized();
            Cam.Right = Vector3.Cross(Cam.Front, Vector3.UnitY).Normalized();
            Cam.Up = Vector3.Cross(Cam.Right, Cam.Front).Normalized();


            var eulerRot = q.EulerAngles();
            _yaw -= eulerRot.Y;
            _pitch += eulerRot.X;

            //Logger.Log(_yaw.ToString());
        }


        public void Navigate(KeyboardState state, float delta = 0.1f)
        {
            if (state.IsKeyDown(Key.W))
            {
                Cam.Position += Cam.Front * delta;
            }
            if (state.IsKeyDown(Key.S))
            {
                Cam.Position -= Cam.Front * delta;
            }
            if (state.IsKeyDown(Key.D))
            {
                Cam.Position += Cam.Right * delta;
            }
            if (state.IsKeyDown(Key.A))
            {
                Cam.Position -= Cam.Right * delta;
            }
            if (state.IsKeyDown(Key.E))
            {
                Cam.Position += Vector3.UnitY * delta;
            }
            if (state.IsKeyDown(Key.Q))
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
