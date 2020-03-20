using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Engine.Core
{
    public struct Vertex
    {

        public Vertex(int id, float x, float y, float z, float nx = 0, float ny = 0, float nz = 0)
        {
            Id = id;
            Coord = new Vector3(x,y,z);
            Normal = new Vector3(nx, ny, nz);
                
            Tris = new List<int>();
            Verts = new List<int>();
            Edges = new List<int>();
        }
        public Vertex(int id, Vector3 coord)
        {
            Id = id;
            Coord = coord;
            Normal = new Vector3(0, 0, 0);


            Tris = new List<int>();
            Verts = new List<int>();
            Edges = new List<int>();
        }
        public Vertex(int id, Vector3 coord, Vector3 normal)
        {
            Id = id;
            Coord = coord;
            Normal = normal;

            Tris = new List<int>();
            Verts = new List<int>();
            Edges = new List<int>();
        }


        public Vector3 Coord;
        public Vector3 Normal;
        public int Id;

        public List<int> Tris { get; set; }
        //public List<KeyValuePair<int, float>> Verts { get; set; }
        public List<int> Verts { get; set; }
        public List<int> Edges { get; set; }

    }

    public struct Edge
    {
        public Edge(int id, int v1, int v2, float length)
        {
            Id = id;
            Start = v1;
            End = v2;
            Length = length;
        }

        public int Id;
        public int Start;
        public int End;
        public float Length { get; set; }

    }

    public struct Triangle
    {
        public int Id;
        public int V1;
        public int V2;
        public int V3;

        public Triangle(int id, int v1, int v2, int v3)
        {
            Id = id;
            V1 = v1;
            V2 = v2;
            V3 = v3;
        }
        
    }

    public class Mesh
    {
        public string Name { get; set; }

        public Mesh(string name = "")
        {
            Name = name;
            Vertices = new List<Vertex>();
            Triangles = new List<Triangle>();
            Edges = new List<Edge>();
        }

        public List<Vertex> Vertices;
        public List<Triangle> Triangles;
        public List<Edge> Edges;

        internal void AddTriangle(int v1, int v2, int v3)
        {
            int id = Triangles.Count;

            Triangles.Add(new Triangle(id, v1, v2, v3));
            Vertices[v1].Tris.Add(id);
            Vertices[v2].Tris.Add(id);
            Vertices[v3].Tris.Add(id);

            if (!AreNeighbors(v1,v2))
            {
                AddEdge(v1, v2, (Vertices[v1].Coord - Vertices[v2].Coord).Length);
            }
            if (!AreNeighbors(v2, v3))
            {
                AddEdge(v2, v3, (Vertices[v2].Coord - Vertices[v3].Coord).Length);
            }
            if (!AreNeighbors(v3, v1))
            {
                AddEdge(v3, v1, (Vertices[v3].Coord - Vertices[v1].Coord).Length);
            }

        }
        internal void AddEdge(int v1, int v2, float length)
        {
            int id = Edges.Count;

            Edges.Add(new Edge(id, v1, v2, length));

            Vertices[v1].Edges.Add(id);
            Vertices[v2].Edges.Add(id);
        }
        internal void AddVertex(float x, float y, float z, float nx = 0, float ny = 0, float nz = 0)
        {
            int id = Vertices.Count;

            Vertices.Add(new Vertex(id, x, y, z, nx, ny, nz));
        }

        internal Vector3 CalculateTriangleNormals(Triangle tri)
        {
            var v1 = Vertices[tri.V1].Coord;
            var v2 = Vertices[tri.V2].Coord;
            var v3 = Vertices[tri.V3].Coord;

            return -Vector3.Cross(v2 - v1, v3 - v1).Normalized();
        }

        internal void CalculateVertexNormals()
        {
            for (int i = 0; i < Vertices.Count; i++)
            {
                var v = Vertices[i];

                Vector3 normal = Vector3.Zero;
                for (int j = 0; j < Vertices[i].Tris.Count; j++)
                {
                    normal += CalculateTriangleNormals(Triangles[v.Tris[j]]);
                }
                v.Normal = normal.Normalized();

                Vertices[i] = v;
            }
        }

        private float TriangleAngle(Vector3 v1,Vector3 v2, Vector3 v3)
        {

            float num = (v2.X - v1.X) * (v3.X - v1.X) +
                      (v2.Y - v1.Y) * (v3.Y - v1.Y) +
                      (v2.Z - v1.Z) * (v3.Z - v1.Z);

            double den = Math.Sqrt(Math.Pow((v2.X - v1.X), 2) +
                                   Math.Pow((v2.Y - v1.Y), 2) +
                                   Math.Pow((v2.Z - v1.Z), 2)) *
                        Math.Sqrt(Math.Pow((v3.X - v1.X), 2) +
                                   Math.Pow((v3.Y - v1.Y), 2) +
                                   Math.Pow((v3.Z - v1.Z), 2));

            double angle = Math.Acos(num / den);
                           

            return (float)angle;
        }

        internal float GetTriangleAngle(int triID, int vertexId)
        {
            Triangle tri = Triangles[triID];

            var v1 = Vertices[tri.V1].Coord;
            var v2 = Vertices[tri.V2].Coord;
            var v3 = Vertices[tri.V3].Coord;
            
            if (vertexId == tri.V1)
            {
                return TriangleAngle(v1, v2, v3);
            }
            if (vertexId == tri.V2)
            {
                return TriangleAngle(v2, v3, v1);
            }
            if (vertexId == tri.V3)
            {
                return TriangleAngle(v3, v1, v2);
            }
            return 0;
        }

        public Vector3 Center()
        {
            Vector3 center = Vector3.Zero;
            for (int i = 0; i < Vertices.Count; i++)
            {
                center += Vertices[i].Coord;
            }
            if (Vertices.Count == 0)
            {
                return center;
            }

            return center / Vertices.Count;
        }

        private bool AreNeighbors(int v1, int v2)
        {
            for (int i = 0; i < Vertices[v1].Verts.Count; i++)
            {
                if (Vertices[v1].Verts[i] == v2)
                {
                    return true;
                }
            }

            //float dist = (Vertices[v1].Coord - Vertices[v2].Coord).Length;

            Vertices[v1].Verts.Add(v2);
            Vertices[v2].Verts.Add(v1);
            return false;
        }

        public List<Vertex> GetBoundaryVertices()
        {
            var boundaries = new HashSet<Vertex>();

            for (int i = 0; i < Edges.Count; i++)
            {
                var e1 = Edges[i].Start;
                var e2 = Edges[i].End;

                int neighbor = 0;
                for (int j = 0; j < Triangles.Count; j++)
                {
                    var t = Triangles[j];
                    if (e1 == t.V1 || e1 == t.V2 || e1 == t.V3)
                    {
                        if (e2 == t.V1 || e2 == t.V2 || e2 == t.V3)
                        {
                            if (++neighbor == 2)
                            {
                                neighbor = 0;
                                break;
                            }
                        }
                    }
                }
                if (neighbor == 1)
                {
                    boundaries.Add(Vertices[e1]);
                    boundaries.Add(Vertices[e2]);
                }
            }
            return boundaries.ToList();
        }
            
    }
}
