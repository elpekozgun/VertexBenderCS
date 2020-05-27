using OpenTK;

namespace Engine.GLApi
{
    public struct ComputeTriangle
    {
        public Vector4 v0;
        public Vector4 v1;
        public Vector4 v2;
    };

    public struct GpuVertex
    {
        public Vector3 Coord;
        public Vector3 Normal;
        public Vector3 Color;
        public Vector2 TexCoord;

        public static int Size
        {
            get
            {
                return Vector3.SizeInBytes * 3 + Vector2.SizeInBytes;
            }
        }
    }
}
