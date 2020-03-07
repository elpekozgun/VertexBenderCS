using Engine.Core;
using OpenTK;
using System.Collections.Generic;

namespace Engine.Processing
{
    public class GraphNode
    {
        public OpenTK.Vector3 Coord;
        public Dictionary<int, float> neighbors;

        public GraphNode(Vector3 coord, Dictionary<int, float> neighbors)
        {
            Coord = coord;
            this.neighbors = neighbors;
        }
    }

    public class UndirectedGraph
    {
        public Dictionary<int, GraphNode> Nodes;

        public UndirectedGraph(Mesh mesh)
        {
            Nodes = new Dictionary<int, GraphNode>();

            foreach (var vertex in mesh.Vertices)
            {
                var neighbors = new Dictionary<int, float>();
                foreach (var n in vertex.Verts)
                {
                    neighbors.Add(n.Key, n.Value);
                }

                Nodes.Add(vertex.Id, new GraphNode(vertex.Coord, neighbors));
            }
        }

        public void DeleteNode(int key)
        {
            foreach (var item in Nodes)
            {
                var node = item.Value;

                List<int> neighbor = new List<int>();
                foreach (var n in node.neighbors)
                {
                    neighbor.Add(key);
                    break;
                }
                foreach (var n in neighbor)
                {
                    node.neighbors.Remove(n);
                }
            }
            Nodes.Remove(key);
        }

    }

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
