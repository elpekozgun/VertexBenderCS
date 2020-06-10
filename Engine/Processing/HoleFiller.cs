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
                Angle = MathHelper.Pi;
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
        private List<Dictionary<int, Triangle>> _allBoundaryTriangles;
        private List<int> _boundaryEdges;
        private List<Dictionary<int, Vertex>> _holeVertices;

        private Stopwatch _watch;

        public HoleFiller(Mesh mesh)
        {
            Mesh = mesh;
            _allBoundaries = new List<Dictionary<int, int>>();
            _allOpposites = new List<Dictionary<int, int>>();
            _allBoundaryTriangles = new List<Dictionary<int, Triangle>>();
            _holeVertices = new List<Dictionary<int, Vertex>>();

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
                var tris = new Dictionary<int, Triangle>();


                _holeVertices.Add(new Dictionary<int, Vertex>());

                for (int j = 0; j < boundary.Count; j++)
                {
                    var v1 = boundary.ElementAt(j % boundary.Count);
                    var v2 = boundary.ElementAt((j + 1) % boundary.Count);

                    var commonTris = v1.Value.Tris.Intersect(v2.Value.Tris).ToList();

                    if (commonTris.Count > 0)
                    {
                        var tri = commonTris[0];
                        var id3 = Mesh.Triangles[tri].GetThirdVertexId(v1.Key, v2.Key);
                        opposites.Add(v1.Key, id3);
                        if (!tris.ContainsKey(tri))
                            tris.Add(tri, Mesh.Triangles[tri]);
                    }
                }
                _allOpposites.Add(opposites);
                _allBoundaryTriangles.Add(tris);
            }

            List<Dictionary<int, Triangle>> newTrianglesList = new List<Dictionary<int, Triangle>>();

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
                var newTriangles = new Dictionary<int, Triangle>();

                int it = iter;
                while (it-- > 0)
                {
                    foreach (var tri in triangles)
                    {
                        Refine(tri.Value, i, ref newTriangles);
                    }

                    triangles = new Dictionary<int, Triangle>(newTriangles);

                    for (int j = 0; j < triangles.Count; j++)
                    {
                        var tri = triangles.ElementAt(j).Value;
                        var v1 = Mesh.Vertices[tri.V1];
                        var v2 = Mesh.Vertices[tri.V2];
                        var v3 = Mesh.Vertices[tri.V3];

                        Relax(v1, v2, i, ref triangles);
                        Relax(v2, v3, i, ref triangles);
                        Relax(v3, v1, i, ref triangles);
                    }

                    newTriangles.Clear();
                }

                newTrianglesList[i] = triangles;

            }
            Mesh.CalculateVertexNormals();

            for (int i = 0; i < _allBoundaries.Count; i++)
            {
                SmoothenHole(i);
                Rake(i, newTrianglesList[i]);
            }

            //return;

            /*
             
            ok so heres the idea.
            We get boundary vertices for a hole,
            We get the hole vertices in triangulation,
            Then we get the hole vertices that are neighboring the boundary vertices.
            We then calculate the average of triangles that neighbor hole vertices and boundary vertices create.
            We calculate their normal and check difference between the normal we want.
            Until it converges we move the point, recalculate triangle normals.
            When all hole vertices are finished, we set them as our next boundary vertices and repeat the process until no hole vertices left in sequence.
                        
             
             */



            _watch.Stop();
            Logger.Log($"{_allBoundaries.Count} Holes filled in {_watch.ElapsedMilliseconds} ms");

            //Fair();
        }

        private void Rake(int boundaryId, Dictionary<int, Triangle> newTriangles)
        {
            var boundary = _allBoundaries[boundaryId];
            var holes = _holeVertices[boundaryId];

            var border = new Dictionary<int, Vertex>();

            foreach (var b in boundary)
            {
                border.Add(b.Value, Mesh.Vertices[b.Value]);
            }

            var nextBorder = new Dictionary<int, List<int>>();

            foreach (var bv in border)
            {
                for (int i = 0; i < bv.Value.Verts.Count; i++)
                {
                    var v = bv.Value.Verts[i];
                    if (holes.ContainsKey(v) && !nextBorder.ContainsKey(v))
                    {
                        List<int> refBoundaryVertis = new List<int>();
                        for (int j = 0; j < Mesh.Vertices[v].Verts.Count; j++)
                        {
                            if (border.ContainsKey(Mesh.Vertices[v].Verts[j]))
                            {
                                refBoundaryVertis.Add(holes[v].Verts[j]);
                            }
                        }
                        nextBorder.Add(v, refBoundaryVertis);
                    }
                }
            }

            var updatedHoles = new Dictionary<int, Vertex>();

            while (holes.Count > 0)
            {
                border.Clear();
                foreach (var nbv in nextBorder)
                {
                    var tris = new HashSet<int>();
                    for (int i = 0; i < Mesh.Vertices[nbv.Key].Tris.Count; i++)
                    {
                        var tri = Mesh.Triangles[Mesh.Vertices[nbv.Key].Tris[i]];
                        for (int j = 0; j < nbv.Value.Count; j++)
                        {
                            if (tri.ContainsId(nbv.Value[j]))
                            {
                                tris.Add(tri.Id);
                            }
                        }
                    }

                    var naturalNormal = Vector3.Zero;
                    var actualNormal = Mesh.Vertices[nbv.Key].Normal.Normalized();

                    var dot = 0.0f;
                    var originalV = Mesh.Vertices[nbv.Key];
                    var prevDot = 0.0f;
                    var c = 1.0f;
                    while (dot < 0.98f)
                    {
                        foreach (var tId in tris)
                        {
                            naturalNormal += Mesh.CalculateTriangleNormals(Mesh.Triangles[tId]);
                        }
                        naturalNormal.Normalize();

                        dot = Vector3.Dot(naturalNormal, actualNormal);

                        // TODO: Solve this and you are a pretty much done.
                        if (dot <= prevDot )
                        {
                            c *= -0.5f;
                        }
                        else
                        {
                            if (c < 0 )
                            {
                                c = Math.Abs(c);
                            }
                        }

                        var curV = Mesh.Vertices[nbv.Key];

                        Mesh.Vertices[nbv.Key] = new Vertex(nbv.Key, curV.Coord + actualNormal * 0.0001f * c, actualNormal)
                        {
                            Verts = Mesh.Vertices[nbv.Key].Verts,
                            Edges = Mesh.Vertices[nbv.Key].Edges,
                            Tris = Mesh.Vertices[nbv.Key].Tris,
                        };
                        prevDot = dot;
                    }
                    updatedHoles.Add(nbv.Key, Mesh.Vertices[nbv.Key]);
                    Mesh.Vertices[nbv.Key] = originalV;
                    holes.Remove(nbv.Key);
                    border.Add(nbv.Key, Mesh.Vertices[nbv.Key]);
                }
                foreach (var v in updatedHoles)
                {
                    Mesh.Vertices[v.Key] = v.Value;
                }

                nextBorder = new Dictionary<int, List<int>>();
                foreach (var bv in border)
                {
                    for (int i = 0; i < bv.Value.Verts.Count; i++)
                    {
                        var v = bv.Value.Verts[i];
                        if (holes.ContainsKey(v) && !nextBorder.ContainsKey(v))
                        {
                            List<int> refBoundaryVertis = new List<int>();
                            for (int j = 0; j < Mesh.Vertices[v].Verts.Count; j++)
                            {
                                if (border.ContainsKey(Mesh.Vertices[v].Verts[j]))
                                {
                                    refBoundaryVertis.Add(holes[v].Verts[j]);
                                }
                            }
                            nextBorder.Add(v, refBoundaryVertis);
                        }
                    }
                }


            }
        }

        private void Rakeold(int boundaryId, Dictionary<int, Triangle> newTriangles)
        {
            var boundary = _allBoundaries[boundaryId];
            var holes = _holeVertices[boundaryId];

            var border = new Dictionary<int, Vertex>();

            foreach (var b in boundary)
            {
                border.Add(b.Key, Mesh.Vertices[b.Value]);

                var bv = Mesh.Vertices[b.Value];
                for (int j = 0; j < bv.Verts.Count; j++)
                {
                    if (holes.TryGetValue(bv.Verts[j], out Vertex v))
                    {
                        if (!border.ContainsKey(v.Id))
                        {
                            border.Add(v.Id, v);
                        }
                    }
                }
            }

            var newBorder = new Dictionary<int, Vertex>();
            foreach (var v in boundary)
            {
                newBorder.Add(v.Value, Mesh.Vertices[v.Value]);
            }

            var newHoles = new Dictionary<int, Vertex>();

            while (holes.Count > 0)
            {

                // Get common tris between boundary and next border.
                foreach (var v in border)
                {
                    var tris = new List<int>();

                    for (int j = 0; j < v.Value.Tris.Count; j++)
                    {
                        var tri = Mesh.Triangles[v.Value.Tris[j]];

                        foreach (var b in newBorder)
                        {
                            if (tri.ContainsId(b.Key))
                            {
                                tris.Add(tri.Id);
                            }
                        }
                    }


                    // Burayla ilgili problem olmalı. ne olabilir acaba.
                    //var actualNormal = Mesh.Vertices[v.Key].Normal.Normalized();
                    var actualNormal = Mesh.Vertices[v.Key].Normal.Normalized();
                    var newNormal = Vector3.Zero;

                    var newDot = Vector3.Dot(actualNormal.Normalized(), newNormal);
                    var dot = 0.0f;

                    while (true)
                    {
                        for (int j = 0; j < tris.Count; j++)
                        {
                            newNormal += Mesh.CalculateTriangleNormals(Mesh.Triangles[tris[j]]);
                        }

                        newNormal = newNormal.Normalized();

                        newDot = Vector3.Dot(actualNormal, newNormal);
                        if (Math.Abs(dot) > Math.Abs(newDot) || newDot > 0.9f)
                        {
                            break;
                        }
                        else
                        {
                            dot = newDot;
                        }

                        var curV = Mesh.Vertices[v.Key];

                        Mesh.Vertices[v.Key] = new Vertex(v.Key, curV.Coord + actualNormal * 0.01f * newDot, actualNormal)
                        {
                            Verts = Mesh.Vertices[v.Key].Verts,
                            Edges = Mesh.Vertices[v.Key].Edges,
                            Tris = Mesh.Vertices[v.Key].Tris,
                        };

                    }
                    Mesh.Vertices[v.Key] = holes[v.Key];
                    newHoles.Add(v.Key, holes[v.Key]);
                    holes.Remove(v.Key);
                }

                newBorder = new Dictionary<int, Vertex>(border);

                border.Clear();

                foreach (var b in newBorder)
                {
                    var bv = Mesh.Vertices[b.Key];
                    for (int j = 0; j < bv.Verts.Count; j++)
                    {
                        if (holes.TryGetValue(bv.Verts[j], out Vertex v))
                        {
                            if (!border.ContainsKey(v.Id))
                            {
                                border.Add(v.Id, v);
                            }
                        }
                    }
                }

            }

            foreach (var v in newHoles)
            {
                Mesh.Vertices[v.Key] = v.Value;
            }
        }



        private Dictionary<int, Triangle> CoarseTriangulate(int boundaryId)
        {
            var nv = _allBoundaries[boundaryId].Count;

            Dictionary<int, Triangle> tris = new Dictionary<int, Triangle>();

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
                angle = Math.Max(angle, DihedralAngle(gi, gj, gk, _allBoundaries[boundaryID][indexTable[i, j]]));
            }

            if (j + 1 == k)
            {
                angle = Math.Max(angle, DihedralAngle(gj, gk, gi, _allOpposites[boundaryID][gj]));
            }
            else
            {
                angle = Math.Max(angle, DihedralAngle(gj, gk, gi, _allBoundaries[boundaryID][indexTable[j, k]]));
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

            var aaa = (float)Math.Acos(Vector3.Dot(n0, n1));

            return aaa;
        }

        private void AddToMesh(ref int[,] indexTable, ref Dictionary<int, Triangle> tris, int start, int end, int boundaryID)
        {
            if (end == start + 1)
                return;

            int cur = indexTable[start, end];

            var gStart = _allBoundaries[boundaryID][start];
            var gCur = _allBoundaries[boundaryID][cur];
            var gEnd = _allBoundaries[boundaryID][end];

            var tri = Mesh.AddTriangle(gEnd, gCur, gStart);
            tris.Add(tri.Id, tri);

            AddToMesh(ref indexTable, ref tris, start, cur, boundaryID);
            AddToMesh(ref indexTable, ref tris, cur, end, boundaryID);

        }

        private void Refine(Triangle T, int boundaryID, ref Dictionary<int, Triangle> list)
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
                list.Remove(T.Id);
                var vc = Mesh.AddVertex(c, (vi.Normal + vj.Normal + vk.Normal).Normalized());
                _holeVertices[boundaryID].Add(vc.Id, vc);

                var t1 = Mesh.AddTriangle(vi.Id, vj.Id, vc.Id);
                var t2 = Mesh.AddTriangle(vj.Id, vk.Id, vc.Id);
                var t3 = Mesh.AddTriangle(vk.Id, vi.Id, vc.Id);

                list.Add(t1.Id, t1);
                list.Add(t2.Id, t2);
                list.Add(t3.Id, t3);
            }
        }

        private void Relax(Vertex v1, Vertex v2, int boundaryId, ref Dictionary<int, Triangle> tris)
        {
            var commonTris = v1.Tris.Intersect(v2.Tris).ToList();
            if (commonTris.Count < 2)
            {
                return;
            }

            if (_allBoundaryTriangles[boundaryId].ContainsKey(commonTris[0]))
            {
                return;
            }
            if (_allBoundaryTriangles[boundaryId].ContainsKey(commonTris[1]))
            {
                return;
            }

            var tri1 = Mesh.Triangles[commonTris[0]];
            var tri2 = Mesh.Triangles[commonTris[1]];

            var id13 = tri1.GetThirdVertexId(v1.Id, v2.Id);
            var id23 = tri2.GetThirdVertexId(v2.Id, v1.Id);

            if (id13 == v1.Id || v1.Id == id23 || id13 == id23 || id13 == v2.Id || id23 == v2.Id)
            {
                return;
            }

            if (Mesh.IsInCircumcircle(id13, v1.Id, v2.Id, id23) ||
                Mesh.IsInCircumcircle(id23, v1.Id, v2.Id, id13))
            {
                Mesh.Flip(tri1, tri2);

                tris[tri1.Id] = Mesh.Triangles[tri1.Id];
                tris[tri2.Id] = Mesh.Triangles[tri2.Id];

                //var aa = v1.Edges.Intersect(v2.Edges);

                //if (aa.Count() > 0)
                //{
                //    Mesh.RemoveEdge(aa.First());
                //}

                //Mesh.RemoveTriangle(tri1.Id);
                //Mesh.RemoveTriangle(tri2.Id);

                //tris.Remove(tri1.Id);
                //tris.Remove(tri2.Id);

                //tri1 = Mesh.AddTriangle(v1.Id, id23, id13, tri1.Id);
                //tri2 = Mesh.AddTriangle(v2.Id, id13, id23, tri2.Id);

                //tris.Add(tri1.Id, tri1);
                //tris.Add(tri2.Id, tri2);

            }
        }

        private void SmoothenHole(int boundaryID)
        {
            var graph = new Graph(Mesh);

            Dictionary<int, Vertex> news = new Dictionary<int, Vertex>();

            foreach (var v in _holeVertices[boundaryID])
            {
                var avgD = 0.0f;
                Vector3 normal = Vector3.Zero;

                var output = Algorithm.DijkstraMinHeapKey(graph, v.Key);

                foreach (var vb in _allBoundaries[boundaryID])
                {
                    float d = output[vb.Value];
                    avgD += d;
                    normal += (Mesh.Vertices[vb.Value].Normal) / d;
                }
                avgD /= _allBoundaries[boundaryID].Count;

                var newV = new Vertex(v.Key, v.Value.Coord, normal.Normalized())
                {
                    Verts = Mesh.Vertices[v.Key].Verts,
                    Edges = Mesh.Vertices[v.Key].Edges,
                    Tris = Mesh.Vertices[v.Key].Tris
                };
                news.Add(v.Key, newV);
                Mesh.Vertices[v.Key] = newV;
            }
            _holeVertices[boundaryID] = news;
        }

        //private void Fair()
        //{
        //    var keys = _holeVertices.Select(x => x.Key).ToList();

        //    int iteration = 4;
        //    while (iteration > 0)
        //    {
        //        for (int j = 0; j < keys.Count; j++)
        //        {
        //            var i = keys[j];
        //            var Lu = Vector3.Zero;
        //            var normal = Mesh.Vertices[i].Normal;

        //            if (Mesh.Vertices[i].Verts.Count > 0)
        //            {
        //                for (int k = 0; k < Mesh.Vertices[i].Verts.Count; k++)
        //                {
        //                    Lu += Mesh.Vertices[Mesh.Vertices[i].Verts[k]].Coord;
        //                    normal += Mesh.Vertices[Mesh.Vertices[i].Verts[k]].Normal;
        //                }
        //                Lu /= Mesh.Vertices[i].Verts.Count;
        //                Lu -= Mesh.Vertices[i].Coord;

        //                Mesh.Vertices[i] = new Vertex(Mesh.Vertices[i].Id, Mesh.Vertices[i].Coord + Lu * 0.5f, normal.Normalized())
        //                {
        //                    Verts = Mesh.Vertices[i].Verts,
        //                    Tris = Mesh.Vertices[i].Tris,
        //                    Edges = Mesh.Vertices[i].Edges
        //                };
        //                _holeVertices[i] = Mesh.Vertices[i];
        //            }
        //        }
        //        iteration--;
        //    }
        //}

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
