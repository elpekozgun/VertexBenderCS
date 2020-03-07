using Engine.Core;
using System.Collections.Generic;

namespace Engine.Processing
{
    public class Graph
    {
        public List<Vertex> Vertices;

        public Graph(Mesh mesh)
        {
            Vertices = mesh.Vertices;
        }

        public Graph(List<Vertex> vertices)
        {
            Vertices = vertices;
        }

    }
}
