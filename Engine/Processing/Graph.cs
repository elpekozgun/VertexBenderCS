using Engine.Core;
using OpenTK;
using System.Collections.Generic;

namespace Engine.Processing
{

    public class Graph
    {
        public List<GraphNode> Nodes;

        public Graph(Mesh mesh)
        {
            Nodes = new List<GraphNode>();
            for (int i = 0; i < mesh.Vertices.Count; i++)
            {
                Nodes.Add
                (
                    new GraphNode
                    (
                        i,
                        mesh.Vertices[i].Coord,
                        mesh.Vertices[i].Verts
                    )
                );    
            }
        }
    }

    public struct GraphNode
    {

        public int Id;
        public Vector3 Coord;
        public List<KeyValuePair<int, float>> Neighbors;

        public GraphNode(int id, Vector3 coord, List<KeyValuePair<int, float>> neighbors)
        {
            Id = id;
            Coord = coord;
            Neighbors = neighbors;
        }
    }

    //public class Graph
    //{
    //    public List<Vertex> Vertices;

    //    public Graph(Mesh mesh)
    //    {
    //        Vertices = mesh.Vertices;
    //    }

    //    public Graph(List<Vertex> vertices)
    //    {
    //        Vertices = vertices;
    //    }

    //}
}
