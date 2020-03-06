using Engine.Core;
using System.Collections.Generic;

namespace Engine.Processing
{
    public class Graph
    {
        public List<Vertex> Vertices;
        public List<Edge> Edges;

        public Graph(Mesh mesh)
        {
            Edges = mesh.Edges;
            Vertices = mesh.Vertices;
        }

        public Graph(List<Vertex> vertices, List<Edge> edges)
        {
            Vertices = vertices;
            Edges = edges;
        }
    }
}
