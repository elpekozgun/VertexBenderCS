using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Core
{
    public static class PrimitiveObjectFactory
    {
        public static Mesh CubeIndexed(float size)
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

        public static Mesh Cube(float size)
        {
            Mesh cube = new Mesh();

            cube.AddVertex(-size, -size, -size, 0.0f, 0.0f, -1.0f);
            cube.AddVertex(size, -size, -size, 0.0f, 0.0f, -1.0f);
            cube.AddVertex(size, size, -size, 0.0f, 0.0f, -1.0f);
            cube.AddVertex(size, size, -size, 0.0f, 0.0f, -1.0f);
            cube.AddVertex(-size, size, -size, 0.0f, 0.0f, -1.0f);
            cube.AddVertex(-size, -size, -size, 0.0f, 0.0f, -1.0f);

            cube.AddVertex(-size, -size, size, 0.0f, 0.0f, 1.0f);
            cube.AddVertex(size, -size, size, 0.0f, 0.0f, 1.0f);
            cube.AddVertex(size, size, size, 0.0f, 0.0f, 1.0f);
            cube.AddVertex(size, size, size, 0.0f, 0.0f, 1.0f);
            cube.AddVertex(-size, size, size, 0.0f, 0.0f, 1.0f);
            cube.AddVertex(-size, -size, size, 0.0f, 0.0f, 1.0f);

            cube.AddVertex(-size, size, size, -1.0f, 0.0f, 0.0f);
            cube.AddVertex(-size, size, -size, -1.0f, 0.0f, 0.0f);
            cube.AddVertex(-size, -size, -size, -1.0f, 0.0f, 0.0f);
            cube.AddVertex(-size, -size, -size, -1.0f, 0.0f, 0.0f);
            cube.AddVertex(-size, -size, size, -1.0f, 0.0f, 0.0f);
            cube.AddVertex(-size, size, size, -1.0f, 0.0f, 0.0f);

            cube.AddVertex(size, size, size, 1.0f, 0.0f, 0.0f);
            cube.AddVertex(size, size, -size, 1.0f, 0.0f, 0.0f);
            cube.AddVertex(size, -size, -size, 1.0f, 0.0f, 0.0f);
            cube.AddVertex(size, -size, -size, 1.0f, 0.0f, 0.0f);
            cube.AddVertex(size, -size, size, 1.0f, 0.0f, 0.0f);
            cube.AddVertex(size, size, size, 1.0f, 0.0f, 0.0f);

            cube.AddVertex(-size, -size, -size, 0.0f, -1.0f, 0.0f);
            cube.AddVertex(size, -size, -size, 0.0f, -1.0f, 0.0f);
            cube.AddVertex(size, -size, size, 0.0f, -1.0f, 0.0f);
            cube.AddVertex(size, -size, size, 0.0f, -1.0f, 0.0f);
            cube.AddVertex(-size, -size, size, 0.0f, -1.0f, 0.0f);
            cube.AddVertex(-size, -size, -size, 0.0f, -1.0f, 0.0f);

            cube.AddVertex(-size, size, -size, 0.0f, 1.0f, 0.0f);
            cube.AddVertex(size, size, -size, 0.0f, 1.0f, 0.0f);
            cube.AddVertex(size, size, size, 0.0f, 1.0f, 0.0f);
            cube.AddVertex(size, size, size, 0.0f, 1.0f, 0.0f);
            cube.AddVertex(-size, size, size, 0.0f, 1.0f, 0.0f);
            cube.AddVertex(-size, size, -size, 0.0f, 1.0f, 0.0f);

            return cube;
        }

        public static Mesh Pyramid(float size, float height, OpenTK.Vector2 offset)
        {
            Mesh pyramid = new Mesh();

            OpenTK.Vector3 v1 = new OpenTK.Vector3(-size, 0, size);
            OpenTK.Vector3 v2 = new OpenTK.Vector3(offset.X, height, offset.Y);
            OpenTK.Vector3 v3 = new OpenTK.Vector3(-size, 0, -size);
            var normal = OpenTK.Vector3.Cross
            (
                v3 - v2,
                v1 - v2
            ).Normalized();
            pyramid.AddVertex(v1, normal);
            pyramid.AddVertex(v2, normal);
            pyramid.AddVertex(v3, normal);


            v1 = new OpenTK.Vector3(-size, 0, -size);
            v2 = new OpenTK.Vector3(offset.X, height, offset.Y);
            v3 = new OpenTK.Vector3(size, 0, -size);
            normal = OpenTK.Vector3.Cross
            (
                v3 - v2,
                v1 - v2
            ).Normalized();
            pyramid.AddVertex(v1, normal);
            pyramid.AddVertex(v2, normal);
            pyramid.AddVertex(v3, normal);

            v1 = new OpenTK.Vector3(size, 0, -size);
            v2 = new OpenTK.Vector3(offset.X, height, offset.Y);
            v3 = new OpenTK.Vector3(size, 0, size);
            normal = OpenTK.Vector3.Cross
            (
                v3 - v2,
                v1 - v2
            ).Normalized();
            pyramid.AddVertex(v1, normal);
            pyramid.AddVertex(v2, normal);
            pyramid.AddVertex(v3, normal);

            v1 = new OpenTK.Vector3(size, 0, size);
            v2 = new OpenTK.Vector3(offset.X, height, offset.Y);
            v3 = new OpenTK.Vector3(-size, 0, size);
            normal = OpenTK.Vector3.Cross
            (
                v3 - v2,
                v1 - v2
            ).Normalized();
            pyramid.AddVertex(v1, normal);
            pyramid.AddVertex(v2, normal);
            pyramid.AddVertex(v3, normal);


            v1 = new OpenTK.Vector3(-size, 0, size);
            v2 = new OpenTK.Vector3(size, 0, size);
            v3 = new OpenTK.Vector3(size, 0, -size);
            normal = OpenTK.Vector3.Cross
            (
                v3 - v2,
                v1 - v2
            ).Normalized();
            pyramid.AddVertex(v1, normal);
            pyramid.AddVertex(v2, normal);
            pyramid.AddVertex(v3, normal);

            v1 = new OpenTK.Vector3(size, 0, -size);
            v2 = new OpenTK.Vector3(-size, 0, -size);
            v3 = new OpenTK.Vector3(-size, 0, size);
            normal = OpenTK.Vector3.Cross
            (
                v3 - v2,
                v1 - v2
            ).Normalized();
            pyramid.AddVertex(v1, normal);
            pyramid.AddVertex(v2, normal);
            pyramid.AddVertex(v3, normal);


            return pyramid;
        }

        public static Mesh PyramidNoBottom(float size, float height, OpenTK.Vector3 offset)
        {
            Mesh pyramid = new Mesh();

            var a = OpenTK.Matrix3.CreateRotationY(OpenTK.MathHelper.PiOver4);

            //var v1 = a * new OpenTK.Vector3(-size, 0, size);
            //var v2 = a * new OpenTK.Vector3(size, 0, size);
            //var v3 = a * new OpenTK.Vector3(size, 0, -size);
            //var v4 = a * new OpenTK.Vector3(-size, 0, -size);
            //var v5 = a * new OpenTK.Vector3(offset.X, height + offset.Y, offset.Z);

            //pyramid.AddVertex(v1.X, v1.Y, v1.Z);
            //pyramid.AddVertex(v2.X, v2.Y, v2.Z);
            //pyramid.AddVertex(v3.X, v3.Y, v3.Z);
            //pyramid.AddVertex(v4.X, v4.Y, v4.Z);
            //pyramid.AddVertex(v5.X, v5.Y, v5.Z);

            pyramid.AddVertex(-size, 0, size);
            pyramid.AddVertex(size, 0, size);
            pyramid.AddVertex(size, 0, -size);
            pyramid.AddVertex(-size, 0, -size);
            pyramid.AddVertex(offset.X, height + offset.Y, offset.Z);

            pyramid.AddTriangle(0, 4, 1);
            pyramid.AddTriangle(1, 4, 2);
            pyramid.AddTriangle(2, 4, 3);
            pyramid.AddTriangle(3, 4, 0);

            pyramid.CalculateVertexNormals();


            //OpenTK.Vector3 v1 = new OpenTK.Vector3(-size, 0, size);
            //OpenTK.Vector3 v2 = new OpenTK.Vector3(offset.X, height, offset.Y);
            //OpenTK.Vector3 v3 = new OpenTK.Vector3(-size, 0, -size);
            //var normal = OpenTK.Vector3.Cross
            //(
            //    v3 - v2,
            //    v1 - v2
            //).Normalized();
            //pyramid.AddVertex(v1, normal);
            //pyramid.AddVertex(v2, normal);
            //pyramid.AddVertex(v3, normal);


            //v1 = new OpenTK.Vector3(-size, 0, -size);
            //v2 = new OpenTK.Vector3(offset.X, height, offset.Y);
            //v3 = new OpenTK.Vector3(size, 0, -size);
            //normal = OpenTK.Vector3.Cross
            //(
            //    v3 - v2,
            //    v1 - v2
            //).Normalized();
            //pyramid.AddVertex(v1, normal);
            //pyramid.AddVertex(v2, normal);
            //pyramid.AddVertex(v3, normal);

            //v1 = new OpenTK.Vector3(size, 0, -size);
            //v2 = new OpenTK.Vector3(offset.X, height, offset.Y);
            //v3 = new OpenTK.Vector3(size, 0, size);
            //normal = OpenTK.Vector3.Cross
            //(
            //    v3 - v2,
            //    v1 - v2
            //).Normalized();
            //pyramid.AddVertex(v1, normal);
            //pyramid.AddVertex(v2, normal);
            //pyramid.AddVertex(v3, normal);

            //v1 = new OpenTK.Vector3(size, 0, size);
            //v2 = new OpenTK.Vector3(offset.X, height, offset.Y);
            //v3 = new OpenTK.Vector3(-size, 0, size);
            //normal = OpenTK.Vector3.Cross
            //(
            //    v3 - v2,
            //    v1 - v2
            //).Normalized();
            //pyramid.AddVertex(v1, normal);
            //pyramid.AddVertex(v2, normal);
            //pyramid.AddVertex(v3, normal);

            return pyramid;
        }

        public static Mesh CreateSphere(float radius, int recursionLevel)
        {

            float t = (float)((1.0 + Math.Sqrt(5.0f)) * 0.5f);
            var s = 1.0f;

            Dictionary<int, int> indexCache = new Dictionary<int, int>();

            var cube = Cube((float)Math.Sqrt(2 * radius * radius));

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

        public static Mesh Sphere(float radius, int recursionLevel)
        {
            var sphere = Tetrahedron(radius);

            while (recursionLevel > 0)
            {
                DivideFace(ref sphere);
                recursionLevel--;
            }

            for (int i = 0; i < sphere.Vertices.Count; i++)
            {
                var normal = sphere.Vertices[i].Coord.Normalized();

                sphere.Vertices[i] = new Vertex(sphere.Vertices[i].Id, normal * radius, normal)
                {
                    Verts = sphere.Vertices[i].Verts,
                    Edges = sphere.Vertices[i].Edges,
                    Tris = sphere.Vertices[i].Tris
                };
            }
            return sphere;
        }

        public static Mesh Tetrahedron(float size)
        {
            Mesh tetrahedron = new Mesh();

            size = 0.5f * size;

            var sqrt3over2 = (float)Math.Sin(OpenTK.MathHelper.DegreesToRadians(60));

            tetrahedron.AddVertex(0, -size * 0.5f, size);
            tetrahedron.AddVertex(-size * sqrt3over2, -size * 0.5f, -size * 0.5f);
            tetrahedron.AddVertex(size * sqrt3over2, -size * 0.5f, -size * 0.5f);
            tetrahedron.AddVertex(0, size, 0);
            
            tetrahedron.AddTriangle(0, 1, 3);
            tetrahedron.AddTriangle(3, 1, 2);
            tetrahedron.AddTriangle(2, 0, 3);
            tetrahedron.AddTriangle(2, 1, 0);

            tetrahedron.CalculateVertexNormals();

            return tetrahedron;
        }
        
        private static void DivideFace(ref Mesh mesh)
        {
            var copyMesh = mesh.Copy();

            for (int i = 0; i < mesh.Triangles.Count; i++)
            {
                var item = mesh.Triangles[i];

                copyMesh.AddVertex((mesh.Vertices[item.V1].Coord + mesh.Vertices[item.V2].Coord) * 0.5f, OpenTK.Vector3.Zero);
                copyMesh.AddVertex((mesh.Vertices[item.V2].Coord + mesh.Vertices[item.V3].Coord) * 0.5f, OpenTK.Vector3.Zero);
                copyMesh.AddVertex((mesh.Vertices[item.V3].Coord + mesh.Vertices[item.V1].Coord) * 0.5f, OpenTK.Vector3.Zero);

                copyMesh.AddTriangle(item.V1, copyMesh.Vertices.Count - 3, copyMesh.Vertices.Count - 1);
                copyMesh.AddTriangle(copyMesh.Vertices.Count - 3, item.V2, copyMesh.Vertices.Count - 2);
                copyMesh.AddTriangle(copyMesh.Vertices.Count - 2, item.V3, copyMesh.Vertices.Count - 1);
                copyMesh.AddTriangle(copyMesh.Vertices.Count - 3, copyMesh.Vertices.Count - 2, copyMesh.Vertices.Count - 1);

                copyMesh.Vertices[item.V1].Verts.Remove(item.V2);
                copyMesh.Vertices[item.V1].Verts.Remove(item.V3);

                copyMesh.Vertices[item.V2].Verts.Remove(item.V1);
                copyMesh.Vertices[item.V2].Verts.Remove(item.V3);

                copyMesh.Vertices[item.V3].Verts.Remove(item.V1);
                copyMesh.Vertices[item.V3].Verts.Remove(item.V2);
            }


            mesh = copyMesh.Copy();
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
