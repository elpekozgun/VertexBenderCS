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

        public Vertex CopyToPlaneXY()
        {
            Vertex v = new Vertex(this.Id, new Vector3(this.Coord.X, this.Coord.Y, 0), Vector3.UnitZ);
            v.Verts = this.Verts;
            v.Tris = this.Tris;
            v.Edges = this.Edges;
            return v;
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

        public int OtherEnd(int id)
        {
            var ret = id == Start ? End : Start;
            return ret;
        }

        public void UpdateIndex(int id, int val)
        {
            if (Start == id)
            {
                Start = val;
            }
            else if(End == id)
            {
                End = val;
            }
        }
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

        public int GetThirdVertexId(int i, int j)
        {
            List<int> verts = new List<int>() { V1, V2, V3 };
            verts.Remove(i);
            verts.Remove(j);
            return verts[0];
        }

        public bool ContainsId(int id)
        {
            return V1 == id || V2 == id || V3 == id;
        }

        public bool ContainsEdge(Edge e)
        {
            return (e.Start == V1 && e.End == V2 || e.Start == V2 && e.End == V1) ||
                   (e.Start == V2 && e.End == V3 || e.Start == V3 && e.End == V2) ||
                   (e.Start == V3 && e.End == V1 || e.Start == V1 && e.End == V3);
        }

        public void UpdateIndex(int id, int value)
        {
            if (V1 == id)
            {
                V1 = value;
            }
            else if (V2 == id)
            {
                V2 = value;
            }
            else if (V3 == id)
            {
                V3 = value;
            }
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

            _lastV = 0;
            _lastT = 0;
            _lastE = 0;
        }

        public List<Vertex> Vertices;
        public List<Triangle> Triangles;
        public List<Edge> Edges;

        public bool hasDirtyTriangles { get; protected set; }

        private int _lastV;
        private int _lastE;
        private int _lastT;

        internal void AddTriangle(int v1, int v2, int v3)
        {
            int id = _lastT++;
            //int id = Triangles.Count;

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
            int id = _lastE++;
            //int id = Edges.Count;
            Edges.Add(new Edge(id, v1, v2, length));

            Vertices[v1].Edges.Add(id);
            Vertices[v2].Edges.Add(id);
        }
        internal Vertex AddVertex(float x, float y, float z, float nx = 0, float ny = 0, float nz = 0)
        {
            //int id = Vertices.Count;
            int id = _lastV++;
            var v = new Vertex(id, x, y, z, nx, ny, nz);
            Vertices.Add(v);
            return v;
        }
        internal Vertex AddVertex(Vector3 coord, Vector3 normal)
        {
            //int id = Vertices.Count;
            int id = _lastV++;
            var v = new Vertex(id, coord, normal);
            Vertices.Add(v);
            return v;
        }
        internal void AddVertex(Vertex v)
        {
            Vertices.Add(v);
        }

        internal void RemoveVertex(int id)
        {
            Vertices[id].Verts.Remove(id);
            for (int i = 0; i < Vertices[id].Verts.Count; i++)
            {
                Vertices[Vertices[id].Verts[i]].Verts.Remove(id);
            }
            for (int i = 0; i < Vertices[id].Edges.Count; i++)
            {
                Edges.Remove(Edges[Vertices[id].Edges[i]]);
            }
            for (int i = 0; i < Vertices[id].Tris.Count; i++)
            {
                Triangles.Remove(Triangles[Vertices[id].Tris[i]]);
            }
            Vertices.Remove(Vertices[id]);
        }

        internal void RemoveTriangle(Triangle tri)
        {
            
            var v1 = Vertices[tri.V1];
            var v2 = Vertices[tri.V2];
            var v3 = Vertices[tri.V3];

            v1.Tris.Remove(tri.Id);
            v2.Tris.Remove(tri.Id);
            v3.Tris.Remove(tri.Id);

            Triangles.Remove(tri);
            hasDirtyTriangles = true;
        }

        public int GetTriangleIndex(int t)
        {
            if (hasDirtyTriangles)
            {
                var tri = Triangles.First(x => x.Id == t);
                return Triangles.IndexOf(tri);
            }
            return Triangles[t].Id;
        }

        internal void DivideEdge(Edge e, int id)
        {
            var vert1 = Vertices[e.Start];
            var vert2 = Vertices[e.End];

            
            AddEdge(e.Start, id, (vert1.Coord - Vertices[id].Coord).Length);
            AddEdge(id, e.End, (vert2.Coord - Vertices[id].Coord).Length);

            var commonTris = vert1.Tris.Intersect(vert2.Tris).ToList();
            
            Edges.Remove(e);
            for (int i = 0; i < commonTris.Count; i++)
            {
                var tri = Triangles[commonTris[i]];
                var id3 = tri.GetThirdVertexId(e.Start, e.End);

                if (IsCCW(tri))
                {
                    AddTriangle(e.End, id3, id);
                    AddTriangle(id3, e.Start, id);
                }

                Triangles.Remove(tri);
            }

        }

        internal Vector3 CalculateTriangleNormals(Triangle tri)
        {
            var v1 = Vertices[tri.V1].Coord;
            var v2 = Vertices[tri.V2].Coord;
            var v3 = Vertices[tri.V3].Coord;

            return -Vector3.Cross(v2 - v1, v3 - v1).Normalized();
        }

        internal Vector3 CalculateTriangleNormals(Vector3 v1, Vector3 v2, Vector3 v3)
        {
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
                    //normal += CalculateTriangleNormals(Triangles[v.Tris[j]]) * TriangleArea(Triangles[v.Tris[j]]);
                    normal += CalculateTriangleNormals(Triangles[v.Tris[j]]) * GetTriangleAngle(v.Tris[j], v.Id);
                }

                v.Normal = normal.Normalized();

                Vertices[i] = v;
            }
        }

        internal float TriangleArea(Vector3 v1, Vector3 v2, Vector3 v3)
        {
            return Vector3.Cross(v2 - v1, v2 - v3).Length / 2;
        }

        internal float TriangleArea(int id)
        {
            var tri = Triangles[id];
            return TriangleArea(Vertices[tri.V1].Coord, Vertices[tri.V2].Coord, Vertices[tri.V3].Coord);
        }

        internal float TriangleArea(Triangle tri)
        {
            return TriangleArea(Vertices[tri.V1].Coord, Vertices[tri.V2].Coord, Vertices[tri.V3].Coord);
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
       
        private bool IsCCW(Triangle t)
        {
            var v1 = Vertices[t.V1].Coord;
            var v2 = Vertices[t.V2].Coord;
            var v3 = Vertices[t.V3].Coord;

            return Vector3.CalculateAngle(v2 - v1, v3 - v1) > 0; 

        }

        internal float GetTriangleAngle(int triID, int vertexId)
        {
            Triangle tri = Triangles[GetTriangleIndex(triID)];
            //Triangle tri = Triangles[triID];

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

                var commonTris = Vertices[e1].Tris.Intersect(Vertices[e2].Tris).ToList().Count;

                if (commonTris == 1)
                {
                    boundaries.Add(Vertices[e1]);
                    boundaries.Add(Vertices[e2]);
                }
               
            }
            return boundaries.ToList();
        }

        public List<Edge> GetBoundaryEdges()
        {
            var boundaries = new HashSet<Edge>();

            for (int i = 0; i < Edges.Count; i++)
            {
                var e1 = Edges[i].Start;
                var e2 = Edges[i].End;

                var commonTris = Vertices[e1].Tris.Intersect(Vertices[e2].Tris).ToList().Count;

                if (commonTris == 1)
                {
                    boundaries.Add(Edges[i]);
                }
            }
            return boundaries.ToList();
        }

        public Mesh Copy()
        {
            Mesh mesh = new Mesh(this.Name + "-copy");
            mesh.Vertices = new List<Vertex>(this.Vertices);
            mesh.Edges = new List<Edge>(this.Edges);
            mesh.Triangles = new List<Triangle>(this.Triangles);

            mesh._lastE = this._lastE;
            mesh._lastV = this._lastV;
            mesh._lastT = this._lastT;
            mesh.hasDirtyTriangles = this.hasDirtyTriangles;

            return mesh;
        }

        public void BranchTest()
        {

        }

    }
}
