using Engine.Core;
using System.Collections.Generic;

namespace Engine.Processing
{
    public class Graph
    {
        public List<Vertex> Vertices;
        public List<Edge> Edges;
        public List<float> EdgeLengths;

        public Graph(Mesh mesh)
        {
            Edges = mesh.Edges;
            Vertices = mesh.Vertices;
        }
    }
}
