using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Core
{
    public static class QuaternionExtensions
    {
        public static Vector3 EulerAngles(this Quaternion q)
        {
            var eX = MathHelper.RadiansToDegrees((float)Math.Atan2(-2 * (q.Y * q.Z - q.W * q.X), q.W * q.W - q.X * q.X - q.Y * q.Y + q.Z * q.Z));
            var eY = MathHelper.RadiansToDegrees((float)Math.Asin(2 * (q.X * q.Z + q.W * q.Y)));
            var eZ = MathHelper.RadiansToDegrees((float)Math.Atan2(-2 * (q.X * q.Y - q.W * q.Z), q.W * q.W + q.X * q.X - q.Y * q.Y - q.Z * q.Z));

            return new Vector3(eX, eY, eZ);
        }

    }
}
