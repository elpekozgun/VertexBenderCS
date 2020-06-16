using OpenTK;
using System.Collections.Generic;

namespace Engine.GLApi
{
    public class Material
    {
        public Shader Shader;
        public List<Texture> Textures;
        public Vector4 Color;
        public float Smoothness;
        public float SpecularIntensity;

        public Material(Shader shader)
        {
            Textures = new List<Texture>();

        }

    }
}
