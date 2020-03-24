using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
