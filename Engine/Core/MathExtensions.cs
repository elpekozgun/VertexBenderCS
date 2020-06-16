using OpenTK;
using System;
using System.Collections.Generic;

namespace Engine.Core
{
    public static class MathExtensions
    {
        public static Vector3 EulerAngles(this Quaternion q)
        {
            var eX = MathHelper.RadiansToDegrees((float)Math.Atan2(-2 * (q.Y * q.Z - q.W * q.X), q.W * q.W - q.X * q.X - q.Y * q.Y + q.Z * q.Z));
            var eY = MathHelper.RadiansToDegrees((float)Math.Asin(2 * (q.X * q.Z + q.W * q.Y)));
            var eZ = MathHelper.RadiansToDegrees((float)Math.Atan2(-2 * (q.X * q.Y - q.W * q.Z), q.W * q.W + q.X * q.X - q.Y * q.Y - q.Z * q.Z));

            return new Vector3(eX, eY, eZ);
        }

        public static Vertex GetClosestVertex(this Vertex source, List<Vertex> oneRingNeighborhood)
        {
            float minDistance = float.MaxValue;
            Vertex closest = source;

            foreach (var item in oneRingNeighborhood)
            {
                var dist = Vector3.Distance(source.Coord, item.Coord);

                if (minDistance < dist)
                {
                    minDistance = dist;
                    closest = item;
                }
            }

            return closest;
        }

        public static Vector3 RotateAroundArbitraryAxis(this Vector3 source, Vector3 start, Vector3 axis, float angle)
        {
            if (axis.Length == 0)
            {
                return source;
            }
            // align on XZ plane
            var T = Matrix4.CreateTranslation(-start);

            var projectedXZ = new Vector3(axis.X, 0, axis.Z).Normalized();

            var cosx = Math.Acos(Vector3.Dot(axis.Normalized(), projectedXZ));
            var cosy = Vector3.Dot(projectedXZ, Vector3.UnitZ);

            var RX = Matrix4.CreateRotationX((float)cosx);
            var RY = Matrix4.CreateRotationY((float)cosy);
            var RZ = Matrix4.CreateRotationZ((float)angle);

            Vector4 translated = new Vector4(source, 1.0f);

            var result = Matrix4.Invert(T) * Matrix4.Invert(RX) * Matrix4.Invert(RY) * RZ * RY * RX * T * translated;

            return result.Xyz;
        }
    }
}
