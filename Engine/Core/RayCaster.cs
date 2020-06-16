using OpenTK;

namespace Engine.Core
{
    public struct RayCaster
    {
        public static Vector3 Cast(Vector3 origin, Vector3 direction, float targetDistance, float step)
        {
            var d = (direction - origin).Normalized();

            var t = 0.0f;
            while (t < targetDistance)
            {
                t += step;
            }
            return origin + t * d;
        }

    }

}
