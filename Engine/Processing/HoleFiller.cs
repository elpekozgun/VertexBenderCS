using Engine.Core;
using Engine.GLApi;
using MathNet.Numerics;
using MathNet.Numerics.Interpolation;
using MathNet.Numerics.Optimization.LineSearch;
using OpenTK;
using OpenTK.Graphics.ES20;
using OpenTK.Graphics.OpenGL;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Schema;

namespace Engine.Processing
{
    public class HoleFiller
    {
        private class Weight
        {
            public float Area;
            public float Angle;

            public Weight()
            {
                Area = float.MaxValue;
                Angle = 180.0f;
            }

            public Weight(float area, float angle)
            {
                Area = area;
                Angle = angle;
            }

            public static Weight operator +(Weight a, Weight b)
            {
                return new Weight(a.Area + b.Area, (float)Math.Max(a.Angle, b.Angle));
            }

            public static bool operator <(Weight a, Weight b)
            {
                return (a.Angle < b.Angle) || (a.Angle == b.Angle && a.Area < b.Area);
            }

            public static bool operator >(Weight a, Weight b)
            {
                return a.Angle > b.Angle || (a.Angle == b.Angle && a.Area > b.Area);
            }
        }

        public Mesh Mesh;
        private List<Dictionary<int, int>> _allBoundaries;
        private List<Dictionary<int, int>> _allOpposites;
        private Dictionary<int, Triangle> _allBoundaryTriangles;
        private List<int> _boundaryEdges;
        private Dictionary<int, Vertex> _holeVertices;

        private Stopwatch _watch;

        public HoleFiller(Mesh mesh)
        {
            Mesh = mesh;
            _allBoundaries = new List<Dictionary<int, int>>();
            _allOpposites = new List<Dictionary<int, int>>();
            _allBoundaryTriangles = new Dictionary<int, Triangle>();
            _holeVertices = new Dictionary<int, Vertex>();

            _watch = new Stopwatch();
        }

        public void FillHoles(int iter = 10, bool isCourse = false)
        {
            _watch.Reset();
            _watch.Start();

            var boundaryEdges = Mesh.GetBoundaryEdges();
            if (boundaryEdges.Count == 0)
            {
                return;
            }
            _boundaryEdges = boundaryEdges.Select(x => x.Id).ToList();

            List<Dictionary<int, Vertex>> allGlobalBoundaries = new List<Dictionary<int, Vertex>>();

            Algorithm.RecursivelyFindAllBoundaries(Mesh.Vertices, boundaryEdges, ref allGlobalBoundaries);

            for (int i = 0; i < allGlobalBoundaries.Count; i++)
            {
                var b = allGlobalBoundaries[i];
                var c = new Dictionary<int, int>();
                for (int j = 0; j < b.Count; j++)
                {
                    c.Add(j, b.ElementAt(j).Key);
                }
                _allBoundaries.Add(c);
            }

            for (int i = 0; i < allGlobalBoundaries.Count; i++)
            {
                var boundary = allGlobalBoundaries[i];
                var opposites = new Dictionary<int, int>();

                for (int j = 0; j < boundary.Count; j++)
                {
                    var v1 = boundary.ElementAt(j % boundary.Count);
                    var v2 = boundary.ElementAt((j + 1) % boundary.Count);

                    var tris = v1.Value.Tris.Intersect(v2.Value.Tris).ToList();

                    if (tris.Count > 0)
                    {
                        var tri = tris[0];
                        var id3 = Mesh.Triangles[tri].GetThirdVertexId(v1.Key, v2.Key);
                        opposites.Add(v1.Key, id3);
                        if (!_allBoundaryTriangles.ContainsKey(tri))
                            _allBoundaryTriangles.Add(tri, Mesh.Triangles[tri]);
                    }
                }
                _allOpposites.Add(opposites);
            }

            List<List<Triangle>> newTrianglesList = new List<List<Triangle>>();

            for (int i = 0; i < _allBoundaries.Count; i++)
            {
                newTrianglesList.Add(CoarseTriangulate(i));
            }

            if (isCourse)
            {
                return;
            }

            for (int i = 0; i < newTrianglesList.Count; i++)
            {
                var triangles = newTrianglesList[i];
                var newTriangles = new List<Triangle>();

                int it = iter;
                while (it-- > 0)
                {
                    for (int j = 0; j < triangles.Count; j++)
                    {
                        Refine(triangles[j], i, ref newTriangles);
                    }
                    if (newTriangles.Count < triangles.Count)
                    {
                        break;
                    }

                    for (int k = 0; k < iter; k++)
                    {
                        for (int j = 0; j < newTriangles.Count; j++)
                        {
                            var tri = newTriangles[j];
                            var v1 = Mesh.Vertices[tri.V1];
                            var v2 = Mesh.Vertices[tri.V2];
                            var v3 = Mesh.Vertices[tri.V3];

                            Relax(v1, v2, ref newTriangles);
                            Relax(v2, v3, ref newTriangles);
                            Relax(v3, v1, ref newTriangles);
                        }
                    }

                    triangles = new List<Triangle>(newTriangles);
                    newTriangles.Clear();
                }

                for (int k = 0; k < iter; k++)
                {
                    for (int j = 0; j < triangles.Count; j++)
                    {
                        var tri = triangles[j];
                        var v1 = Mesh.Vertices[tri.V1];
                        var v2 = Mesh.Vertices[tri.V2];
                        var v3 = Mesh.Vertices[tri.V3];

                        Relax(v1, v2, ref triangles);
                        Relax(v2, v3, ref triangles);
                        Relax(v3, v1, ref triangles);
                    }
                }
            }

            _watch.Stop();
            Logger.Log($"{_allBoundaries.Count} Holes filled in {_watch.ElapsedMilliseconds} ms");

            //Fair();
        }

        private List<Triangle> CoarseTriangulate(int boundaryId)
        {
            var nv = _allBoundaries[boundaryId].Count;

            List<Triangle> tris = new List<Triangle>();

            Weight[,] weightTable = new Weight[nv, nv];
            int[,] indexTable = new int[nv, nv];

            for (int i = 0; i < nv; i++)
            {
                for (int j = 0; j < nv; j++)
                {
                    weightTable[i, j] = new Weight();
                }
            }

            for (int i = 0; i < nv - 1; i++)
            {
                weightTable[i, i + 1] = new Weight(0, 0);
            }


            for (int j = 2; j < nv; j++)
            {
                for (int i = 0; i + j < nv; i++)
                {
                    Weight minW = new Weight();
                    int minId = -1;

                    for (int m = i + 1; m < i + j; m++)
                    {
                        Weight w = weightTable[i, m] + weightTable[m, i + j] + CalculateTriWeight(i, m, i + j, boundaryId, indexTable); ;

                        if (w < minW)
                        {
                            minW = w;
                            minId = m;
                        }
                    }
                    weightTable[i, i + j] = minW;
                    indexTable[i, i + j] = minId;
                }
            }

            AddToMesh(ref indexTable, ref tris, 0, nv - 1, boundaryId);

            return tris;
        }

        private Weight CalculateTriWeight(int i, int j, int k, int boundaryID, int[,] indexTable)
        {
            if (indexTable[i, j] == -1)
            {
                return new Weight();
            }
            if (indexTable[j, k] == -1)
            {
                return new Weight();
            }

            var gi = _allBoundaries[boundaryID][i];
            var gj = _allBoundaries[boundaryID][j];
            var gk = _allBoundaries[boundaryID][k];

            if (ExistsEdge(gi, gj) || ExistsEdge(gj, gk) || ExistsEdge(gk, gi))
            {
                return new Weight();
            }

            float angle = 0;

            if (i + 1 == j)
            {
                angle = Math.Max(angle, DihedralAngle(gi, gj, gk, _allOpposites[boundaryID][gi]));
            }
            else
            {
                angle = Math.Max(angle, DihedralAngle(gi, gj, gk, indexTable[i, j]));
            }

            if (j + 1 == k)
            {
                angle = Math.Max(angle, DihedralAngle(gj, gk, gi, _allOpposites[boundaryID][gj]));
            }
            else
            {
                angle = Math.Max(angle, DihedralAngle(gj, gk, gi, indexTable[j, k]));
            }

            if (i == 0 && k == _allBoundaries[boundaryID].Count - 1)
            {
                angle = Math.Max(angle, DihedralAngle(gk, gi, gj, _allOpposites[boundaryID][gk]));
            }

            var vi = Mesh.Vertices[gi].Coord;
            var vj = Mesh.Vertices[gj].Coord;
            var vk = Mesh.Vertices[gk].Coord;

            var area = Mesh.TriangleArea(vi, vj, vk);
            return new Weight(area, angle);
        }

        private float DihedralAngle(int u, int v, int a, int b)
        {
            var uu = Mesh.Vertices[u].Coord;
            var vv = Mesh.Vertices[v].Coord;
            var aa = Mesh.Vertices[a].Coord;
            var bb = Mesh.Vertices[b].Coord;

            var n0 = Vector3.Cross(vv - uu, aa - vv).Normalized();
            var n1 = Vector3.Cross(uu - vv, bb - uu).Normalized();

            return (float)Math.Acos(Vector3.Dot(n0, n1)) * 180.0f / MathHelper.Pi;
        }

        private void AddToMesh(ref int[,] indexTable, ref List<Triangle> tris, int start, int end, int boundaryID)
        {
            if (end == start + 1)
                return;

            int cur = indexTable[start, end];

            var gStart = _allBoundaries[boundaryID][start];
            var gCur = _allBoundaries[boundaryID][cur];
            var gEnd = _allBoundaries[boundaryID][end];

            var tri = Mesh.AddTriangle(gStart, gEnd, gCur);
            tris.Add(tri);

            AddToMesh(ref indexTable, ref tris, start, cur, boundaryID);
            AddToMesh(ref indexTable, ref tris, cur, end, boundaryID);

        }

        private void Refine(Triangle T, int boundaryID, ref List<Triangle> list)
        {
            var vi = Mesh.Vertices[T.V1];
            var vj = Mesh.Vertices[T.V2];
            var vk = Mesh.Vertices[T.V3];

            var c = (vi.Coord + vj.Coord + vk.Coord) / 3;

            float[] sigmaV = new float[3];
            for (int i = 0; i < vi.Verts.Count; i++)
            {
                sigmaV[0] += Mesh.Edges[vi.Verts[i]].Length;
            }
            sigmaV[0] /= vi.Verts.Count;

            for (int i = 0; i < vj.Verts.Count; i++)
            {
                sigmaV[1] += Mesh.Edges[vj.Verts[i]].Length;
            }
            sigmaV[1] /= vj.Verts.Count;

            for (int i = 0; i < vk.Verts.Count; i++)
            {
                sigmaV[2] += Mesh.Edges[vk.Verts[i]].Length;
            }
            sigmaV[2] /= vk.Verts.Count;

            float sigmaC = sigmaV.Sum() / 3;
            float scale = (float)Math.Sqrt(2);

            var di = scale * Vector3.Distance(c, vi.Coord);
            var dj = scale * Vector3.Distance(c, vj.Coord);
            var dk = scale * Vector3.Distance(c, vk.Coord);

            if (di > sigmaC && di > sigmaV[0] && dj > sigmaC && dj > sigmaV[1] && dk > sigmaC && dk > sigmaV[2])
            {
                Mesh.RemoveTriangle(T.Id);
                list.Remove(T);
                var vc = Mesh.AddVertex(c, (vi.Normal + vj.Normal + vk.Normal).Normalized());
                _holeVertices.Add(vc.Id, vc);

                list.Add(Mesh.AddTriangle(vi.Id, vj.Id, vc.Id));
                list.Add(Mesh.AddTriangle(vj.Id, vk.Id, vc.Id));
                list.Add(Mesh.AddTriangle(vk.Id, vi.Id, vc.Id));
            }
        }

        private void Relax(Vertex v1, Vertex v2, ref List<Triangle> tris)
        {
            var commonTris = v1.Tris.Intersect(v2.Tris).ToList();
            if (commonTris.Count < 2)
            {
                return;
            }

            if (_allBoundaryTriangles.ContainsKey(commonTris[0]))
            {
                return;
            }
            if (_allBoundaryTriangles.ContainsKey(commonTris[1]))
            {
                return;
            }

            var tri1 = Mesh.Triangles[commonTris[0]];
            var tri2 = Mesh.Triangles[commonTris[1]];

            var id13 = tri1.GetThirdVertexId(v1.Id, v2.Id);
            var id23 = tri2.GetThirdVertexId(v1.Id, v2.Id);

            if (Mesh.IsInCircumcircle(id13, v1.Id, v2.Id, id23) ||
                Mesh.IsInCircumcircle(id23, v1.Id, v2.Id, id13))
            {

                //tris.Remove(tri1);
                //tris.Remove(tri2);

                //Mesh.Flip(tri1, tri2);

                //tris.Add(Mesh.Triangles[tri1.Id]);
                //tris.Add(Mesh.Triangles[tri2.Id]);

                //return;
                //var edge = v1.Edges.Intersect(v2.Edges).FirstOrDefault();
                //Mesh.RemoveEdge(edge);

                Mesh.RemoveTriangle(tri1.Id);
                Mesh.RemoveTriangle(tri2.Id);

                var T1 = Mesh.AddTriangle(id13, v1.Id, id23, tri1.Id);
                var T2 = Mesh.AddTriangle(id23, v2.Id, id13, tri2.Id);

                tris.Remove(tri1);
                tris.Remove(tri2);
                tris.Add(T1);
                tris.Add(T2);

                


                
            }
        }

        private void Fair()
        {
            var keys = _holeVertices.Select(x => x.Key).ToList();

            int iteration = 4;
            while (iteration > 0)
            {
                for (int j = 0; j < keys.Count; j++)
                {
                    var i = keys[j];
                    var Lu = Vector3.Zero;
                    var normal = Mesh.Vertices[i].Normal;

                    if (Mesh.Vertices[i].Verts.Count > 0)
                    {
                        for (int k = 0; k < Mesh.Vertices[i].Verts.Count; k++)
                        {
                            Lu += Mesh.Vertices[Mesh.Vertices[i].Verts[k]].Coord;
                            normal += Mesh.Vertices[Mesh.Vertices[i].Verts[k]].Normal;
                        }
                        Lu /= Mesh.Vertices[i].Verts.Count;
                        Lu -= Mesh.Vertices[i].Coord;

                        Mesh.Vertices[i] = new Vertex(Mesh.Vertices[i].Id, Mesh.Vertices[i].Coord + Lu * 0.5f, normal.Normalized())
                        {
                            Verts = Mesh.Vertices[i].Verts,
                            Tris = Mesh.Vertices[i].Tris,
                            Edges = Mesh.Vertices[i].Edges
                        };
                        _holeVertices[i] = Mesh.Vertices[i];
                    }
                }
                iteration--;
            }
        }

        private bool ExistsEdge(int i, int j)
        {
            var v1 = Mesh.Vertices[i];
            var v2 = Mesh.Vertices[j];

            var a = v1.Edges.Intersect(v2.Edges).ToList();

            if (a.Count > 0)
            {
                if (!_boundaryEdges.Contains(a[0]))
                {
                    return true;
                }
            }
            return false;
            
        }

    }
}
