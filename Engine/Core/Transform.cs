using OpenTK;
using System.Collections.Generic;

namespace Engine.Core
{
    public class Transform
    {
        private Transform _parent;

        public string Name;
        public Transform Parent
        {
            set
            {
                _parent = value;
                _parent.Children.Add(this);
            }
            get
            {
                return _parent;
            }
        }
        public readonly List<Transform> Children;

        public Vector3 Position { get; set; }
        public Vector3 Scale { get; set; }
        public Quaternion Rotation { get; set; }

        public bool IsEnabled { get; set; }

        public Transform(string name = "entity")
        {
            Position = Vector3.Zero;
            Rotation = Quaternion.Identity;
            Scale = Vector3.One;
            Name = name;
            Children = new List<Transform>();
        }

        public virtual Matrix4 ModelMatrix
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
