using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Core
{
    public static class PrimitiveObjectFactory
    {
        public static Mesh CreateCube(float size)
        {
            Mesh cube = new Mesh();

            size = 0.5f * size;

            cube.AddVertex(size, size, size);
            cube.AddVertex(-size, size, size);
            cube.AddVertex(-size, size, -size);
            cube.AddVertex(size, size, -size);
            cube.AddVertex(size, -size, size);
            cube.AddVertex(-size, -size, size);
            cube.AddVertex(-size, -size, -size);
            cube.AddVertex(size, -size, -size);

            cube.AddTriangle(1, 2, 0);
            cube.AddTriangle(2, 3, 0);
            cube.AddTriangle(5, 2, 1);
            cube.AddTriangle(5, 6, 2);
            cube.AddTriangle(6, 3, 2);
            cube.AddTriangle(6, 7, 3);
            cube.AddTriangle(7, 4, 3);
            cube.AddTriangle(4, 0, 3);
            cube.AddTriangle(6, 5, 4);
            cube.AddTriangle(7, 6, 4);
            cube.AddTriangle(5, 1, 0);
            cube.AddTriangle(4, 5, 0);

            cube.CalculateVertexNormals();

            return cube;
        }

        public static Mesh CreateSphere(float radius, int recursionLevel)
        {

            float t = (float)((1.0 + Math.Sqrt(5.0f)) * 0.5f);
            var s = 1.0f;

            Dictionary<int, int> indexCache = new Dictionary<int, int>();

            var cube = CreateCube((float)Math.Sqrt(2 * radius * radius));

            for (int i = 0; i < recursionLevel; i++)
            {
                var newTris = new List<Triangle>();
                foreach (var tri in cube.Triangles)
                {
                    int a = GetMiddlePointIndex(cube.Vertices[tri.V1], cube.Vertices[tri.V2], ref cube, ref indexCache);
                    int b = GetMiddlePointIndex(cube.Vertices[tri.V2], cube.Vertices[tri.V3], ref cube, ref indexCache);
                    int c = GetMiddlePointIndex(cube.Vertices[tri.V3], cube.Vertices[tri.V1], ref cube, ref indexCache);

                    newTris.Add(new Triangle(cube.Triangles.Count, tri.V1, a, c));
                    newTris.Add(new Triangle(cube.Triangles.Count + 1, tri.V2, b, a));
                    newTris.Add(new Triangle(cube.Triangles.Count + 2, tri.V3, c, b));
                    newTris.Add(new Triangle(cube.Triangles.Count + 3, a, b, c));
                }
                cube.Triangles = newTris;
            }


            return cube;
        }

        private static int GetMiddlePointIndex(Vertex v1, Vertex v2, ref Mesh sphere, ref Dictionary<int, int> cache)
        {
            var i1 = sphere.Vertices.IndexOf(v1);
            var i2 = sphere.Vertices.IndexOf(v2);

            var firstSmaller = i1 < i2;

            int smallerIndex = firstSmaller ? i1 : i2;
            int greaterIndex = firstSmaller ? i2 : i1;

            int key = (smallerIndex << 32) + greaterIndex;

            if (cache.TryGetValue(key, out int ret))
            {
                return ret;
            }

            var coord = new OpenTK.Vector3((v1.Coord + v2.Coord) * 0.5f);

            sphere.AddVertex(coord.X, coord.Y, coord.Z);

            cache.Add(key, sphere.Vertices.Count - 1);

            return sphere.Vertices.Count - 1;

        }

    }
}
