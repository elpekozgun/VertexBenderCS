using Engine.Core;
using Engine.GLApi;
using MathNet.Numerics.Interpolation;
using MathNet.Numerics.Optimization.LineSearch;
using OpenTK;
using OpenTK.Graphics.ES20;
using OpenTK.Graphics.OpenGL;
using System;
using System.CodeDom;
using System.Collections.Generic;
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
                return new Weight(a.Area + b.Area, Math.Max(a.Angle, b.Angle));
            }

            public static bool operator <(Weight a, Weight b)
            {
                return a.Angle < b.Angle || (a.Angle == b.Angle && a.Area < b.Area);
            }

            public static bool operator >(Weight a, Weight b)
            {
                return a.Angle > b.Angle || (a.Angle == b.Angle && a.Area > b.Area);
            }
        }

        public Mesh Mesh;
        private List<Dictionary<int, int>> _allBoundaries;
        private List<Dictionary<int, int>> _allOpposites;

        public HoleFiller(Mesh mesh)
        {
            Mesh = mesh;
            _allBoundaries = new List<Dictionary<int, int>>();
            _allOpposites = new List<Dictionary<int, int>>();
        }

        public void FillHoles()
        {
            var boundaryEdges = Mesh.GetBoundaryEdges();

            if (boundaryEdges.Count == 0)
            {
                return;
            }

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

                    var tri = v1.Value.Tris.Intersect(v2.Value.Tris).ToList()[0];
                    var id3 = Mesh.Triangles[tri].GetThirdVertexId(v1.Key, v2.Key);
                    opposites.Add(v1.Key, id3);
                }
                _allOpposites.Add(opposites);
            }

            List<List<Triangle>> newTrianglesList = new List<List<Triangle>>();

            for (int i = 0; i < _allBoundaries.Count; i++)
            {
                newTrianglesList.Add(CoarseTriangulate(i));
            }

            for (int i = 0; i < newTrianglesList.Count; i++)
            {
                var triangles = newTrianglesList[i];
                var newTriangles = new List<Triangle>();

                int it = 1;
                while (it-- > 0)
                {
                    for (int j = 0; j < triangles.Count; j++)
                    {
                        newTriangles.AddRange(Refine(triangles[j]));
                    }
                    triangles = new List<Triangle>(newTriangles);
                    newTriangles.Clear();
                }
            }
            
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
                        Weight w = weightTable[i, m] + weightTable[m, i + j] + CalculateTriWeight(i, m, i + j, boundaryId, ref indexTable); ;

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

        private Weight CalculateTriWeight(int i, int j, int k, int boundaryID, ref int[,] indexTable)
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

            return MathHelper.RadiansToDegrees((float)Math.Acos(Vector3.Dot(n0, n1)));
        }

        private void AddToMesh(ref int[,] indexTable, ref List<Triangle> tris, int start, int end, int boundaryID)
        {
            if (end == start + 1)
                return;

            int cur = indexTable[start, end];

            var gStart = _allBoundaries[boundaryID][start];
            var gCur = _allBoundaries[boundaryID][cur];
            var gEnd = _allBoundaries[boundaryID][end];

            var tri = Mesh.AddTriangle(gStart, gCur, gEnd);
            tris.Add(tri);

            AddToMesh(ref indexTable, ref tris, start, cur, boundaryID);
            AddToMesh(ref indexTable, ref tris, cur, end, boundaryID);

        }

        private List<Triangle> Refine(Triangle T)
        {
            var retVal = new List<Triangle>();

            var vi = Mesh.Vertices[T.V1];
            var vj = Mesh.Vertices[T.V2];
            var vk = Mesh.Vertices[T.V3];

            var vc = (vi.Coord + vj.Coord + vk.Coord) / 3;

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

            var di = scale * Vector3.Distance(vc, vi.Coord);
            var dj = scale * Vector3.Distance(vc, vj.Coord);
            var dk = scale * Vector3.Distance(vc, vk.Coord);

            if (di > sigmaC && di > sigmaV[0] && dj > sigmaC && dj > sigmaV[1] && dk > sigmaC && dk > sigmaV[2])
            {
                Mesh.RemoveTriangle(T.Id);
                var vn = Mesh.AddVertex(vc, (vi.Normal + vj.Normal + vk.Normal).Normalized());

                var t1 = Mesh.AddTriangle(vn.Id, vj.Id, vk.Id);
                var otherTriangle = Mesh.Triangles[vj.Tris.Intersect(vk.Tris).ToList()[0]];
                var other3rd = otherTriangle.GetThirdVertexId(vj.Id, vk.Id);

                if (Mesh.IsInCircumcircle(other3rd, vn.Id, vj.Id, vk.Id) || Mesh.IsInCircumcircle(vn.Id, vj.Id, vk.Id, other3rd))
                {
                    Mesh.RemoveTriangle(otherTriangle.Id);
                    Mesh.RemoveTriangle(t1.Id);

                    retVal.Add(Mesh.AddTriangle(other3rd, vj.Id, vn.Id));
                    retVal.Add(Mesh.AddTriangle(other3rd, vk.Id, vn.Id));
                }
                else
                {
                    retVal.Add(t1);
                }

                var t2 = Mesh.AddTriangle(vn.Id, vi.Id, vk.Id);
                var otherTriangle2 = Mesh.Triangles[vi.Tris.Intersect(vk.Tris).ToList()[0]];
                var other3rd2 = otherTriangle2.GetThirdVertexId(vi.Id, vk.Id);

                if (Mesh.IsInCircumcircle(other3rd2, vn.Id, vi.Id, vk.Id) || Mesh.IsInCircumcircle(vn.Id, vi.Id, vk.Id, other3rd2))
                {
                    Mesh.RemoveTriangle(otherTriangle2.Id);
                    Mesh.RemoveTriangle(t2.Id);

                    retVal.Add(Mesh.AddTriangle(other3rd2, vi.Id, vn.Id));
                    retVal.Add(Mesh.AddTriangle(other3rd2, vk.Id, vn.Id));
                }
                else
                {
                    retVal.Add(t2);
                }

                var t3 = Mesh.AddTriangle(vn.Id, vi.Id, vj.Id);
                var otherTriangle3 = Mesh.Triangles[vi.Tris.Intersect(vj.Tris).ToList()[0]];
                var other3rd3 = otherTriangle3.GetThirdVertexId(vi.Id, vj.Id);

                if (Mesh.IsInCircumcircle(other3rd3, vn.Id, vi.Id, vj.Id) || Mesh.IsInCircumcircle(vn.Id, vi.Id, vj.Id, other3rd3))
                {
                    Mesh.RemoveTriangle(otherTriangle3.Id);
                    Mesh.RemoveTriangle(t3.Id);

                    retVal.Add(Mesh.AddTriangle(other3rd3, vi.Id, vn.Id));
                    retVal.Add(Mesh.AddTriangle(other3rd3, vj.Id, vn.Id));
                }
                else
                {
                    retVal.Add(t3);
                }

                //retVal.Add(Mesh.AddTriangle(vn.Id, vj.Id, vk.Id));
                //retVal.Add(Mesh.AddTriangle(vi.Id, vn.Id, vk.Id));
                //retVal.Add(Mesh.AddTriangle(vi.Id, vj.Id, vn.Id));

            }

            return retVal;
        }

        private void Relax(Triangle T , Triangle T2)
        {
            var v1 = Mesh.Vertices[T.V1];
            var v2 = Mesh.Vertices[T.V2];
            var v3 = Mesh.Vertices[T.V3];

            var t1 = Mesh.Triangles[v2.Tris.Intersect(v3.Tris).ToList()[0]];
            var t2 = Mesh.Triangles[v1.Tris.Intersect(v3.Tris).ToList()[0]];
            var t3 = Mesh.Triangles[v1.Tris.Intersect(v2.Tris).ToList()[0]];

            var t13 = Mesh.Vertices[t1.GetThirdVertexId(T.V2, T.V3)];
            var t23 = Mesh.Vertices[t2.GetThirdVertexId(T.V1, T.V3)];
            var t33 = Mesh.Vertices[t3.GetThirdVertexId(T.V1, T.V2)];

            if (Mesh.IsInCircumcircle(t33.Coord, v1.Coord,v2.Coord,v3.Coord) ||
                Mesh.IsInCircumcircle(v3.Coord, v1.Coord, v2.Coord, t33.Coord))
            {
                Mesh.RemoveTriangle(t3.Id);
                Mesh.RemoveTriangle(T.Id);

                Mesh.AddTriangle(t33.Id, v1.Id, v3.Id);
                Mesh.AddTriangle(t33.Id, v2.Id, v3.Id);
            }

        }


        // isInCircumcircle(i, j, c ,x) or isInCircumCircle(i, x,j, c)
        // delete tri1, tri2
        // addTri(x,j,c)
        // addTri(x,c,i)
    }
}
