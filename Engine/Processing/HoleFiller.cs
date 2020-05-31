using Engine.Core;
using MathNet.Numerics.Interpolation;
using MathNet.Numerics.Optimization.LineSearch;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Schema;

namespace Engine.Processing
{
    public class HoleFiller
    {
        public Mesh Mesh;
        public List<int> BoundaryPath;

        public HoleFiller(Mesh mesh)
        {
            Mesh = mesh;
        }

        public void FillHoles()
        {
            List<Dictionary<int, Vertex>> allBoundaries = new List<Dictionary<int, Vertex>>();

            var boundaryEdges = Mesh.GetBoundaryEdges();

            Algorithm.RecursivelyFindAllBoundaries(Mesh.Vertices, boundaryEdges, ref allBoundaries);

            for (int i = 0; i < allBoundaries.Count; i++)
            {
                var boundary = allBoundaries[i];

                CoarseTriangulate(ref boundary);
            }

        }

        public static void CoarseTriangulateWithMidVertex(ref Mesh mesh, ref Dictionary<int, Vertex> boundary)
        {
            var a = boundary.Select(x => x.Value).ToList();

            var center = Vector3.Zero;
            var normal = Vector3.Zero;
            for (int k = 0; k < a.Count; k++)
            {
                center += a[k].Coord;
                normal += a[k].Normal;
            }
            center /= a.Count;

            var c = mesh.AddVertex(center, normal.Normalized());

            for (int i = 0; i < a.Count; i++)
            {
                var vi = a[i];
                var vj = a[(i + 1) % a.Count];

                mesh.AddTriangle(vi.Id, vj.Id, c.Id);
            }

        }


        // TODO: Calculate proper weights for triangles
        public void CoarseTriangulate(ref Dictionary<int, Vertex> boundary)
        {
            List<KeyValuePair<int, int>> BoundaryPair = new List<KeyValuePair<int, int>>();


            //BoundaryPair.Add(new KeyValuePair<int, int>(0, 0));
            //BoundaryPair.Add(new KeyValuePair<int, int>(1, 1));
            //BoundaryPair.Add(new KeyValuePair<int, int>(2, 2));
            //BoundaryPair.Add(new KeyValuePair<int, int>(3, 3));
            //BoundaryPair.Add(new KeyValuePair<int, int>(4, 4));
            //BoundaryPair.Add(new KeyValuePair<int, int>(5, 5));

            int c = 0;
            foreach (var b in boundary)
            {
                BoundaryPair.Add(new KeyValuePair<int, int>(c++, b.Key));
            }


            float[,] weightTable = new float[BoundaryPair.Count, BoundaryPair.Count];

            for (int i = 0; i < BoundaryPair.Count; i++)
            {
                for (int j = 0; j < BoundaryPair.Count; j++)
                {
                    weightTable[i, j] = -1;
                }
            }

            for (int i = 0; i < BoundaryPair.Count - 1; i++)
            {
                weightTable[i, i] = 0;
                weightTable[i, i + 1] = 0;
            }
            //for (int i = 0; i < BoundaryPair.Count - 2; i++)
            //{
            //    weightTable[i, i + 2] = CalculateTriWeight(i, i + 1, i + 2);
            //}

            HashSet<Vector3> tris = new HashSet<Vector3>();

            CalculateWeight(0, BoundaryPair.Count - 1, ref weightTable, ref tris);

            var temp = tris.ToList();

            var triangulation = new List<Vector3>();

            for (int i = 0; i < BoundaryPair.Count - 2; i++)
            {
                triangulation.Add(temp[temp.Count - 1 - i]);
            }

            for (int i = 0; i < triangulation.Count; i++)
            {
                var v = triangulation[i];

                Mesh.AddTriangle(BoundaryPair[(int)v.X].Value, BoundaryPair[(int)v.Y].Value, BoundaryPair[(int)v.Z].Value);
            }


            var a = 5;
            /*
             *  05:
                01 015 15
                02 025 25
                03 035 35
                04 045 45
                
                03:
                01 013 13
                02 023 23
                
                04:
                01 014 14
                02 024 24
                03 034 34

                14:
                12 124 24 
                13 134 34

                15:
                12 125 25
                13 135 35
                14 145 45

                25:
                23 235 35
                24 245 45
             
             */

        }

        // TODO: Add proper triangles.
        private static float CalculateWeight(int i, int j, ref float[,] weightTable, ref HashSet<Vector3> tris)
        {
            if (weightTable[i, j] != -1)
            {
                return weightTable[i, j];
            }
            else
            {
                float wMin = float.MaxValue;
                int minId = -1;
                for (int x = i + 1; x < j; x++)
                {
                    float w = 0;
                    w += CalculateWeight(i, x, ref weightTable, ref tris);
                    w += CalculateTriWeight(i, x, j);
                    w += CalculateWeight(x, j, ref weightTable, ref tris);
                    if (w < wMin)
                    {
                        wMin = w;
                        minId = x;
                    }
                }
                tris.Add(new Vector3(i, minId, j));
                
                weightTable[i, j] = wMin;
                return wMin;
            }
        }

        private static float CalculateTriWeight(int v1, int v2, int v3)
        {
            return 4* v1 + 3 * v2 + 2  *v3;
        }





    }
}
