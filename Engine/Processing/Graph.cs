using Engine.Core;
using OpenTK;
using System.Collections.Generic;

namespace Engine.Processing
{

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

    public class Graph
    {
        public List<GraphNode> Nodes;

        private List<KeyValuePair<int,float>> ExtractNeighbors(Mesh mesh, Vertex vertex)
        {
            var retVal = new List<KeyValuePair<int, float>>();
            foreach (var neighbor in vertex.Verts)
            {
                retVal.Add(new KeyValuePair<int, float>(neighbor, (vertex.Coord - mesh.Vertices[neighbor].Coord).Length));
            }
            return retVal;
        }

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
                        ExtractNeighbors(mesh, mesh.Vertices[i])
                        //mesh.Vertices[i].Verts
                    )
                );    
            }
        }
    }

}
