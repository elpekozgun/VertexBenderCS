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

        public static Mesh Tetrahedron(float size)
        {
            Mesh tetrahedron = new Mesh();

            size = 0.5f * size;

            var b = (float)Math.Sqrt(2.0 / 9.0);
            var a = (float)Math.Sqrt(8.0 / 9.0);
            var c = (float)Math.Sqrt(2.0 / 3.0);

            tetrahedron.AddVertex(a * size,  -size * (1.0f / 3.0f), 0.0f);
            tetrahedron.AddVertex(-b * size, -size * (1.0f / 3.0f), size * c);
            tetrahedron.AddVertex(-b * size, -size * (1.0f / 3.0f), -size * c);
            tetrahedron.AddVertex(0, size, 0);
                
            tetrahedron.AddTriangle(0, 1, 3);
            tetrahedron.AddTriangle(3, 1, 2);
            tetrahedron.AddTriangle(2, 0, 3);
            tetrahedron.AddTriangle(2, 1, 0);

            tetrahedron.CalculateVertexNormals();

            return tetrahedron;
        }


        public static Mesh Sphere2(float radius, int recursionLevel)
        {
            var sphere = Tetrahedron(radius);

            int n = 1;
            while (n <= recursionLevel)
            {
                DivideFace(ref sphere, radius / ( 2 * n));
                n++;
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

        
        private static void DivideFace(ref Mesh mesh, float size)
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

        private static void DivideFace(ref Mesh mesh, float size, ref Dictionary<OpenTK.Vector3, Vertex> verts)
        {
            var copyMesh = mesh.Copy();


            foreach (var item in mesh.Triangles)
            {

                Vertex v1, v2, v3;

                var c1 = (mesh.Vertices[item.V2].Coord + mesh.Vertices[item.V3].Coord) * 0.5f;
                var c2 = (mesh.Vertices[item.V1].Coord + mesh.Vertices[item.V3].Coord) * 0.5f;
                var c3 = (mesh.Vertices[item.V2].Coord + mesh.Vertices[item.V1].Coord) * 0.5f;

                //v1 = copyMesh.AddVertex(c1, OpenTK.Vector3.Zero);
                //v2 = copyMesh.AddVertex(c2, OpenTK.Vector3.Zero);
                //v3 = copyMesh.AddVertex(c3, OpenTK.Vector3.Zero);

                if (!verts.ContainsKey(c1))
                {
                    v1 = copyMesh.AddVertex(c1, OpenTK.Vector3.Zero);
                    verts.Add(v1.Coord, v1);
                }
                else
                {
                    v1 = verts[c1];
                }
                if (!verts.ContainsKey(c2))
                {
                    v2 = copyMesh.AddVertex(c2, OpenTK.Vector3.Zero);
                    verts.Add(v2.Coord, v2);
                }
                else
                {
                    v2 = verts[c2];
                }
                if (!verts.ContainsKey(c3))
                {
                    v3 = copyMesh.AddVertex(c3, OpenTK.Vector3.Zero);
                    verts.Add(v3.Coord, v3);
                }
                else
                {
                    v3 = verts[c3];
                }



                copyMesh.AddTriangle(item.V1, v3.Id, v2.Id);
                copyMesh.AddTriangle(v3.Id, item.V2, v1.Id);
                copyMesh.AddTriangle(v2.Id, v1.Id, item.V3);
                copyMesh.AddTriangle(v1.Id, v2.Id, v3.Id);

                copyMesh.Vertices[item.V1].Verts.Remove(item.V2);
                copyMesh.Vertices[item.V1].Verts.Remove(item.V3);

                copyMesh.Vertices[item.V2].Verts.Remove(item.V1);
                copyMesh.Vertices[item.V2].Verts.Remove(item.V3);

                copyMesh.Vertices[item.V3].Verts.Remove(item.V1);
                copyMesh.Vertices[item.V3].Verts.Remove(item.V2);
                
                copyMesh.RemoveTriangle(item);
            }

            mesh = copyMesh.Copy();
        }







        public static Mesh Sphere(float radius, int recursionLevel)
        {
            var sphere = Tetrahedron(radius);

            var vertDict = new Dictionary<OpenTK.Vector3, Vertex>();

            while (recursionLevel> 0)
            {
                DivideFace(ref sphere, recursionLevel, ref vertDict);
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


        private static void DivideFace2(ref Mesh mesh, float size)
        {
            var copyMesh = mesh.Copy();

            var newVertices = new List<Vertex>();
            foreach (var edge in mesh.Edges)
            {
                var start = mesh.Vertices[edge.Start];
                var end = mesh.Vertices[edge.End];

                var v = copyMesh.AddVertex((start.Coord + end.Coord) * 0.5f, OpenTK.Vector3.Zero);

                var edge1 = new Edge(copyMesh.Edges.Count, v.Id, start.Id, (v.Coord - start.Coord).Length);
                copyMesh.AddEdge(v.Id, start.Id, (v.Coord - start.Coord).Length);
                
                var edge2 = new Edge(copyMesh.Edges.Count, v.Id, end.Id, (v.Coord - end.Coord).Length);
                copyMesh.AddEdge(v.Id, end.Id, (v.Coord - end.Coord).Length);

                if (newVertices.Count > 0)
                {
                    for (int i = 0; i < newVertices.Count; i++)
                    {
                        if (OpenTK.Vector3.Distance(newVertices[i].Coord, v.Coord) <= (float)Math.Sqrt(0.75f) * size)
                        {
                            copyMesh.AddEdge(v.Id, newVertices[i].Id, (v.Coord - newVertices[i].Coord).Length);
                        }
                    }
                }

                copyMesh.Edges.Remove(edge);
                newVertices.Add(v);
            }


            mesh = copyMesh.Copy();
        }


    }

}


