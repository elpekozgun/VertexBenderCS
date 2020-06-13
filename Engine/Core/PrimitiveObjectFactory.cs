using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Core
{
    public enum eSphereGenerationType
    {
        Tetrahedron,
        Cube,
        icosahedron
    }

    public static class PrimitiveObjectFactory
    {
        private static Mesh CubeIndexed(float size)
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

        public static Mesh Pyramid(float size, float height, OpenTK.Vector2 offset, bool hasBottom = true)
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

        public static Mesh Icosahedron(float size)
        {
            Mesh icosahedron = new Mesh();

            var t = (float)(1.0 + Math.Sqrt(5.0)) / 2.0f;

            icosahedron.AddVertex(-1, t, 0);
            icosahedron.AddVertex(1, t, 0);
            icosahedron.AddVertex(-1, -t, 0);
            icosahedron.AddVertex(1, -t, 0);

            icosahedron.AddVertex(0, -1, t);
            icosahedron.AddVertex(0, 1, t);
            icosahedron.AddVertex(0, -1, -t);
            icosahedron.AddVertex(0, 1, -t);

            icosahedron.AddVertex(t, 0, -1);
            icosahedron.AddVertex(t, 0, 1);
            icosahedron.AddVertex(-t, 0, -1);
            icosahedron.AddVertex(-t, 0, 1);

            icosahedron.AddTriangle(5, 11, 0);
            icosahedron.AddTriangle(1, 5, 0);
            icosahedron.AddTriangle(7, 1, 0);
            icosahedron.AddTriangle(10, 7, 0);
            icosahedron.AddTriangle(11, 10, 0);

            icosahedron.AddTriangle(9, 5, 1);
            icosahedron.AddTriangle(4, 11, 5);
            icosahedron.AddTriangle(2, 10, 11);
            icosahedron.AddTriangle(6, 7, 10);
            icosahedron.AddTriangle(8, 1, 7);

            icosahedron.AddTriangle(4, 9, 3);
            icosahedron.AddTriangle(2, 4, 3);
            icosahedron.AddTriangle(6, 2, 3);
            icosahedron.AddTriangle(8, 6, 3);
            icosahedron.AddTriangle(9, 8, 3);

            icosahedron.AddTriangle(5, 9, 4);
            icosahedron.AddTriangle(11, 4, 2);
            icosahedron.AddTriangle(10, 2, 6);
            icosahedron.AddTriangle(7, 6, 8);
            icosahedron.AddTriangle(1, 8, 9);

            return icosahedron;

        }

        private static void DivideFace(ref Mesh mesh, float size, ref Dictionary<OpenTK.Vector3, Vertex> verts)
        {
            var copyMesh = mesh.Copy();

            foreach (var item in mesh.Triangles)
            {

                Vertex v1, v2, v3;

                var c1 = (mesh.Vertices[item.Value.V2].Coord + mesh.Vertices[item.Value.V3].Coord) * 0.5f;
                var c2 = (mesh.Vertices[item.Value.V1].Coord + mesh.Vertices[item.Value.V3].Coord) * 0.5f;
                var c3 = (mesh.Vertices[item.Value.V2].Coord + mesh.Vertices[item.Value.V1].Coord) * 0.5f;

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



                copyMesh.AddTriangle(item.Value.V1, v3.Id, v2.Id);
                copyMesh.AddTriangle(v3.Id, item.Value.V2, v1.Id);
                copyMesh.AddTriangle(v2.Id, v1.Id, item.Value.V3);
                copyMesh.AddTriangle(v1.Id, v2.Id, v3.Id);

                copyMesh.Vertices[item.Value.V1].Verts.Remove(item.Value.V2);
                copyMesh.Vertices[item.Value.V1].Verts.Remove(item.Value.V3);

                copyMesh.Vertices[item.Value.V2].Verts.Remove(item.Value.V1);
                copyMesh.Vertices[item.Value.V2].Verts.Remove(item.Value.V3);
                
                copyMesh.Vertices[item.Value.V3].Verts.Remove(item.Value.V1);
                copyMesh.Vertices[item.Value.V3].Verts.Remove(item.Value.V2);
                
                copyMesh.RemoveTriangle(item.Key);
            }

            mesh = copyMesh.Copy();
        }

        public static Sphere Sphere(float radius, int recursionLevel, eSphereGenerationType type)
        {
            Mesh sphere = null;
            var subdivision = recursionLevel;
            if (type == eSphereGenerationType.Cube)
            {
                sphere = CubeIndexed(radius);
            }
            else if (type == eSphereGenerationType.Tetrahedron)
            {
                sphere = Tetrahedron(radius);
            }
            else
            {
                sphere = Icosahedron(radius);
            }

            var vertDict = new Dictionary<OpenTK.Vector3, Vertex>();

            while (recursionLevel> 0)
            {
                DivideFace(ref sphere, recursionLevel, ref vertDict);
                recursionLevel--;
            }

            var keys = sphere.Vertices.Select(x => x.Key).ToList();

            for (int i = 0; i < keys.Count; i++)
            {
                var v = sphere.Vertices[keys[i]];

                var normal = v.Coord.Normalized();

                sphere.Vertices[keys[i]] = new Vertex(keys[i], normal * radius, normal)
                {
                    Verts = v.Verts,
                    Edges = v.Edges,
                    Tris = v.Tris
                };
            }

            //for (int i = 0; i < sphere.Vertices.Count; i++)
            //{
            //    var normal = sphere.Vertices[i].Coord.Normalized();

            //    sphere.Vertices[i] = new Vertex(sphere.Vertices[i].Id, normal * radius, normal)
            //    {
            //        Verts = sphere.Vertices[i].Verts,
            //        Edges = sphere.Vertices[i].Edges,
            //        Tris = sphere.Vertices[i].Tris
            //    };
            //}


            return new Sphere(radius, subdivision, type, sphere);
        }

        public static Mesh Cylinder(float radius, float baseOffset, float length, int division)
        {
            Mesh arrow3D = new Mesh();

            // 2 div + 2 vertex;

            //bottom 0 - 11
            for (int i = 0; i < division; i++)
            {
                arrow3D.AddVertex
                (
                    radius * (float)Math.Cos(i * (2 * Math.PI / division)),
                    baseOffset,                                          
                    radius * (float)Math.Sin(i * (2 * Math.PI / division))
                );
            }

            //top 12 - 23
            for (int i = 0; i < division; i++)
            {
                arrow3D.AddVertex
                (
                    radius * (float)Math.Cos(i * (2 * Math.PI / division)), 
                    length,                                     
                    radius * (float)Math.Sin(i * (2 * Math.PI / division))
                );
            }
            
            // base 24, top 25
            arrow3D.AddVertex(0.0f, baseOffset, 0.0f);
            arrow3D.AddVertex(0.0f,  length , 0.0f);

            int a, b, c, d;
            // base
            for (int i = 0; i <= division; i++)
            {
                a = i % division;
                b = (i + 1) % division;
                c = a + division;
                d = b + division;

                // bottom, top
                arrow3D.AddTriangle(a, b, 2 * division);
                arrow3D.AddTriangle(c, d, 2 * division + 1);
                // sides
                arrow3D.AddTriangle(a, b, c);
                arrow3D.AddTriangle( c, b, d);
            }

            arrow3D.CalculateVertexNormals();

            return arrow3D;
        }

        public static Mesh Grid (int sizeX, int sizeY, int sizeZ, float gridDistance)
        {
            var grid = new Mesh();

            for (int y = 0; y <= sizeY; y++)
            {
                for (int x = 0; x <= sizeX; x++)
                {
                    var v1 = grid.AddVertex(x, y, 0);
                    var v2 = grid.AddVertex(x, y, sizeZ);
                    grid.AddEdge(v1.Id, v2.Id, sizeZ, false);
                }
            }

            for (int y = 0; y <= sizeY; y++)
            {
                for (int z = 0; z <= sizeX; z++)
                {
                    var v1 = grid.AddVertex(0, y, z);
                    var v2 = grid.AddVertex(sizeX, y, z);
                    grid.AddEdge(v1.Id, v2.Id, sizeX, false);
                }
            }

            for (int z = 0; z <= sizeY; z++)
            {
                for (int x = 0; x <= sizeX; x++)
                {
                    var v1 = grid.AddVertex(x, 0, z);
                    var v2 = grid.AddVertex(x, sizeY, z);
                    grid.AddEdge(v1.Id, v2.Id, sizeY, false);
                }
            }

            return grid;
        }
    }

    public class Sphere : Mesh
    {
        public float Size { get; set; }
        public int Subdivision { get; set; }
        public eSphereGenerationType Type { get; set; }

        public Sphere(float size, int subdivision, eSphereGenerationType type, Mesh mesh):base("asd")
        {
            this.Vertices = mesh.Vertices;
            this.Triangles = mesh.Triangles;
            this.Edges = mesh.Edges;
            this.Name = mesh.Name;

            Size = size;
            Subdivision = subdivision;
            Type = type;
        }
    }
}


