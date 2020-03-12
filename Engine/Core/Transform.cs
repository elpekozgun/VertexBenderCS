using OpenTK;

namespace Engine.Core
{
    public class Transform
    {
        public Transform()
        {
            Position = Vector3.Zero;
            Rotation = Quaternion.Identity;
            Scale = Vector3.One;
        }

        public Vector3 Position{ get; set; }
        public Vector3 Scale { get; set; }
        public Quaternion Rotation { get; set; }

        public Matrix4 ModelMatrix
        {
            get
            {
                var rotation = Matrix4.CreateFromQuaternion(Rotation);
                var scale = Matrix4.CreateScale(Scale);
                var translation = Matrix4.CreateTranslation(Position);

                return scale * rotation * translation;
            }
        }
    }

}
