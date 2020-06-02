using MathNet.Numerics;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

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
        public List<int> Verts { get; set; }
        public List<int> Edges { get; set; }

        public void UpdateVertex(Vector3 coord, Vector3 normal)
        {
            this.Normal = normal;
            this.Coord = coord;
        }

    }

    public struct Edge
    {
        public Edge(int id, int v1, int v2, float length, bool isBoundary)
        {
            Id = id;
            Start = v1;
            End = v2;
            Length = length;
            IsBoundary = isBoundary;
        }

        public int Id;
        public int Start;
        public int End;
        public float Length { get; set; }
        public bool IsBoundary;

        public int GetOppositeEnd(int id)
        {
            var ret = id == Start ? End : Start;
            return ret;
        }

        public void SetBoundary()
        {
            IsBoundary = true;
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

        public bool ContainsEdge(Edge edge)
        {
            return V1 == edge.Start && V2 == edge.End || V1 == edge.End && V2 == edge.Start ||
                   V2 == edge.Start && V3 == edge.End || V2 == edge.End && V3 == edge.Start ||
                   V3 == edge.Start && V1 == edge.End || V3 == edge.End && V1 == edge.Start;
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
            Vertices = new Dictionary<int, Vertex>();
            Triangles = new Dictionary<int, Triangle>();
            Edges = new Dictionary<int, Edge>();

            _lastV = 0;
            _lastT = 0;
            _lastE = 0;
        }

        public Dictionary<int, Vertex> Vertices;
        public Dictionary<int, Triangle> Triangles;
        public Dictionary<int, Edge> Edges;

        private int _lastV;
        private int _lastE;
        private int _lastT;

        internal void AddVertex(Vertex v)
        {
            Vertices.Add(v.Id, v);
        }

        internal Vertex AddVertex(Vector3 coord, Vector3 normal)
        {
            int id = _lastV++;
            var v = new Vertex(id, coord, normal);
            Vertices.Add(id, v);
            return v;
        }

        internal Vertex AddVertex(float x, float y, float z, float nx = 0, float ny = 0, float nz = 0)
        {
            int id = _lastV++;
            var v = new Vertex(id, x, y, z, nx, ny, nz);
            Vertices.Add(id, v);
            return v;
        }

        internal Triangle AddTriangle(int v1, int v2, int v3)
        {
            int id = _lastT++;

            var tri = new Triangle(id, v1, v2, v3);

            Triangles.Add(id, tri);
            Vertices[v1].Tris.Add(id);
            Vertices[v2].Tris.Add(id);
            Vertices[v3].Tris.Add(id);

            if (!AreNeighbors(v1, v2))
            {
                AddEdge(v1, v2, (Vertices[v1].Coord - Vertices[v2].Coord).Length, false);
            }
            if (!AreNeighbors(v2, v3))
            {
                AddEdge(v2, v3, (Vertices[v2].Coord - Vertices[v3].Coord).Length, false);
            }
            if (!AreNeighbors(v3, v1))
            {
                AddEdge(v3, v1, (Vertices[v3].Coord - Vertices[v1].Coord).Length, false);
            }

            return tri;
        }

        internal Triangle AddTriangle(int v1, int v2, int v3, int id)
        {
            var tri = new Triangle(id, v1, v2, v3);

            Triangles.Add(id, tri);
            Vertices[v1].Tris.Add(id);
            Vertices[v2].Tris.Add(id);
            Vertices[v3].Tris.Add(id);

            if (!AreNeighbors(v1, v2))
            {
                AddEdge(v1, v2, (Vertices[v1].Coord - Vertices[v2].Coord).Length, false);
            }
            if (!AreNeighbors(v2, v3))
            {
                AddEdge(v2, v3, (Vertices[v2].Coord - Vertices[v3].Coord).Length, false);
            }
            if (!AreNeighbors(v3, v1))
            {
                AddEdge(v3, v1, (Vertices[v3].Coord - Vertices[v1].Coord).Length, false);
            }

            return tri;
        }

        internal void Flip(Triangle ti, Triangle tj)
        {
            // 1 initial edge, 2 new edge

            var t1 = new List<int>() { ti.V1, ti.V2, ti.V3 };
            var t2 = new List<int>() { tj.V1, tj.V2, tj.V3 };

            var commons = t1.Intersect(t2).ToList();
            if (commons.Count < 2)
            {
                return;
            }

            var v1 = commons[0];
            var v2 = commons[1];

            var ti3 = ti.GetThirdVertexId(v1, v2);
            var tj3 = tj.GetThirdVertexId(v1, v2);

            var edge = Vertices[v1].Edges.Intersect(Vertices[v2].Edges).First();
            RemoveEdge(edge);

            AddEdge(ti3, tj3, (Vertices[ti3].Coord - Vertices[tj3].Coord).Length , false);

            ti.UpdateIndex(v2, tj3);
            tj.UpdateIndex(v1, ti3);

            Vertices[ti3].Tris.Add(tj.Id);
            Vertices[tj3].Tris.Add(ti.Id);

            Vertices[v1].Verts.Remove(v2);
            Vertices[v2].Verts.Remove(v1);

            Vertices[ti3].Verts.Add(tj3);
            Vertices[tj3].Verts.Add(ti3);

            Triangles[ti.Id] = ti;
            Triangles[tj.Id] = tj;
        }


        internal Edge AddEdge(int v1, int v2, float length, bool isBoundary)
        {
            int id = _lastE++;

            var edge = new Edge(id, v1, v2, length, isBoundary);

            Edges.Add(id, edge);

            Vertices[v1].Edges.Add(id);
            Vertices[v2].Edges.Add(id);

            return edge;
        }

        internal void RemoveVertex(int id)
        {
            var v = Vertices[id];

            var temp = new List<int>(v.Edges);
            for (int j = 0; j < temp.Count; j++)
            {
                if (Edges.ContainsKey(temp[j]))
                {
                    RemoveEdge(temp[j]);
                }
            }

            temp = new List<int>(v.Tris);
            for (int j = 0; j < temp.Count; j++)
            {
                if (Triangles.ContainsKey(temp[j]))
                {
                    RemoveTriangle(temp[j]);
                }
            }

            for (int i = 0; i < v.Verts.Count; i++)
            {
                if (Vertices.ContainsKey(v.Verts[i]))
                {
                    Vertices[v.Verts[i]].Verts.Remove(id);
                }
            }

            Vertices.Remove(id);
        }

        internal void RemoveTriangle(int id)
        {
            if (Triangles.ContainsKey(id))
            {
                var tri = Triangles[id];

                if (Vertices.TryGetValue(tri.V1, out Vertex v1))
                {
                    v1.Tris.Remove(tri.Id);
                }

                if (Vertices.TryGetValue(tri.V2, out Vertex v2))
                    v2.Tris.Remove(tri.Id);

                if (Vertices.TryGetValue(tri.V3, out Vertex v3))
                    v3.Tris.Remove(tri.Id);

                if (v1.Tris.Count == 1)
                {
                    if (v1.Edges.Count > 2)
                    {
                        for (int i = 0; i < v1.Edges.Count; i++)
                        {
                            RemoveEdge(v1.Edges[i]);
                        }
                    }
                }

                Triangles.Remove(id);
            }
        }

        internal void RemoveEdge(int id)
        {
            if (Edges.ContainsKey(id))
            {
                var edge = Edges[id];

                if (Vertices.TryGetValue(edge.Start, out Vertex v1))
                    v1.Edges.Remove(id);
                if (Vertices.TryGetValue(edge.End, out Vertex v2))
                    v2.Edges.Remove(id);

                Edges.Remove(id);
            }
        }

        internal void DivideEdge(Edge e, int id)
        {
            var vert1 = Vertices[e.Start];
            var vert2 = Vertices[e.End];


            AddEdge(e.Start, id, (vert1.Coord - Vertices[id].Coord).Length, false);
            AddEdge(id, e.End, (vert2.Coord - Vertices[id].Coord).Length, false);

            var commonTris = vert1.Tris.Intersect(vert2.Tris).ToList();

            Edges.Remove(e.Id);
            for (int i = 0; i < commonTris.Count; i++)
            {
                var tri = Triangles[commonTris[i]];
                var id3 = tri.GetThirdVertexId(e.Start, e.End);

                if (IsCCW(tri))
                {
                    AddTriangle(e.End, id3, id);
                    AddTriangle(id3, e.Start, id);
                }

                Triangles.Remove(tri.Id);
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
            var keys = Vertices.Select(x => x.Key).ToList();

            for (int i = 0; i < keys.Count; i++)
            {
                var vertex = Vertices[keys[i]];

                Vector3 normal = Vector3.Zero;
                for (int j = 0; j < vertex.Tris.Count; j++)
                {
                    //normal += CalculateTriangleNormals(Triangles[v.Tris[j]]) * TriangleArea(Triangles[v.Tris[j]]);
                    normal += CalculateTriangleNormals(Triangles[vertex.Tris[j]]) * GetTriangleAngle(vertex.Tris[j], keys[i]);
                }

                vertex.UpdateVertex(vertex.Coord, normal.Normalized());
                Vertices[keys[i]] = vertex;
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

        private float TriangleAngle(Vector3 v1, Vector3 v2, Vector3 v3)
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

        public bool IsInCircumcircle(int nx, int ia, int ib, int ic)
        {

            var a = Vertices[ia].Coord;
            var b = Vertices[ib].Coord;
            var c = Vertices[ic].Coord;

            Vector3 ab = b - a;
            Vector3 ac = c - a;


            float a00 = -2.0f * Vector3.Dot(ab, a);
            float a01 = -2.0f * Vector3.Dot(ab, b);
            float a02 = -2.0f * Vector3.Dot(ab, c);

            float b0 = a.LengthSquared - b.LengthSquared;

            float a10 = -2.0f * Vector3.Dot(ac, a);
            float a11 = -2.0f * Vector3.Dot(ac, b);
            float a12 = -2.0f * Vector3.Dot(ac, c);

            float b1 = a.LengthSquared - c.LengthSquared;

            float alpha = -(-a11 * a02 + a01 * a12 - a12 * b0 + b1 * a02 + a11 * b0 - a01 * b1)
                    / (-a11 * a00 + a11 * a02 - a10 * a02 + a00 * a12 + a01 * a10 - a01 * a12);
            float beta = (a10 * b0 - a10 * a02 - a12 * b0 + a00 * a12 + b1 * a02 - a00 * b1)
                     / (-a11 * a00 + a11 * a02 - a10 * a02 + a00 * a12 + a01 * a10 - a01 * a12);
            float gamma = (-a11 * a00 - a10 * b0 + a00 * b1 + a11 * b0 + a01 * a10 - a01 * b1)
                    / (-a11 * a00 + a11 * a02 - a10 * a02 + a00 * a12 + a01 * a10 - a01 * a12);

            Vector3 center = alpha * a + beta * b + gamma * c;

            return (Vertices[nx].Coord - center).LengthSquared < (a - center).LengthSquared;   
        }

        private bool IsCCW(Triangle t)
        {
            var v1 = Vertices[t.V1].Coord;
            var v2 = Vertices[t.V2].Coord;
            var v3 = Vertices[t.V3].Coord;

            return Vector3.CalculateAngle(v2 - v1, v3 - v1) > 0; 

        }

        public Vector3 Center()
        {
            Vector3 center = Vector3.Zero;

            foreach (var vertex in Vertices)
            {
                center += vertex.Value.Coord;
            }


            return center / Vertices.Count;
        }

        private bool AreNeighbors(int v1, int v2)
        {
            if (v1 == v2)
            {
                return false;
            }
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

            foreach (var edge in Edges)
            {
                var e1 = edge.Value.Start;
                var e2 = edge.Value.End;

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

            foreach(var edge in Edges)
            {
                var e1 = edge.Value.Start;
                var e2 = edge.Value.End;

                var commonTris = Vertices[e1].Tris.Intersect(Vertices[e2].Tris).ToList().Count;

                if (commonTris == 1)
                {
                    var e = Edges[edge.Key];
                    e.IsBoundary = true;
                    boundaries.Add(e);
                }
            }

            return boundaries.ToList();
        }

        public Mesh Copy()
        {
            Mesh mesh = new Mesh(this.Name + "-copy")
            {
                Vertices = new Dictionary<int, Vertex>(this.Vertices),
                Edges = new Dictionary<int, Edge>(this.Edges),
                Triangles = new Dictionary<int, Triangle>(this.Triangles),

                _lastE = this._lastE,
                _lastV = this._lastV,
                _lastT = this._lastT
            };

            return mesh;
        }

        public void RefreshMesh()
        {
            var tempV = new Dictionary<int,Vertex>(Vertices);
            var tempT = new Dictionary<int,Triangle>(Triangles);

            _lastT = 0;
            _lastV = 0;
            _lastE = 0;

            Vertices.Clear();
            Triangles.Clear();
            Edges.Clear();// = newMesh.Edges;

            Dictionary<int, int> vertexMap = new Dictionary<int, int>();

            int id = 0;
            foreach (var vertex in tempV)
            {
                //newMesh.AddVertex(vertex.Value.Coord, vertex.Value.Normal);
                vertexMap.Add(vertex.Key, id++);
                AddVertex(vertex.Value.Coord, vertex.Value.Normal);
            }

            foreach (var tri in tempT)
            {
                AddTriangle(vertexMap[tri.Value.V1], vertexMap[tri.Value.V2], vertexMap[tri.Value.V3]);
            }

            //Vertices = newMesh.Vertices;
            //Triangles = newMesh.Triangles;
            //Edges = newMesh.Edges;

        }

    }
}
