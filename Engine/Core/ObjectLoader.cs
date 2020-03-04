using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Core
{
    public static class ObjectLoader
    {
        public static Mesh LoadOff(string path)
        {
            string[] a = new string[1];
            try
            {
                a = File.ReadLines(path).ToArray();
            }
            catch (Exception)
            {
                throw new FileNotFoundException("file not found");
            }

            if (a[0].ToUpper() != "OFF")
            {
                throw new FileLoadException("Incorrect OFF format");
            }
            var line = a[1].Split(new char[]{ ' '}, StringSplitOptions.RemoveEmptyEntries);

            Mesh mesh = new Mesh();

            int nVerts =  int.Parse(line[0]);
            int nTris =  int.Parse(line[1]);
            int useless = int.Parse(line[2]);
            int i = 2;

            while (i < nVerts + 2)
            {
                line = a[i].Split(new char[]{ ' '}, StringSplitOptions.RemoveEmptyEntries);

                var v1 = float.Parse(line[0]);
                var v2 = float.Parse(line[2]);
                var v3 = float.Parse(line[1]);

                mesh.AddVertex(v1 / 100, v2 / 100, v3 / 100);
                i++;
            }

            while (i < nVerts + nTris + 2)
            {
                line = a[i].Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                var v1 = int.Parse(line[1]);
                var v2 = int.Parse(line[2]);
                var v3 = int.Parse(line[3]);

                mesh.AddTriangle(v1, v2, v3);
                i++;
            }

            mesh.CalculateVertexNormals();

            return mesh;
        }

        public static Mesh CreateCube(float size)
        {
            Mesh cube = new Mesh();

            float[] flb = new float[3] { 0, 0, 0 };
            float deltaX = 0;
            float deltaY = 0;
            float deltaZ = 0;

            for (int v = 0; v < 8; v++)
            {
                switch (v)
                {
                    case 1:
                        deltaX = size;
                        break;
                    case 2:
                        deltaZ = -size;
                        break;
                    case 3:
                        deltaX = 0;
                        break;
                    case 4:
                        deltaZ = 0;
                        deltaY = size;
                        break;
                    case 5:
                        deltaX = size;
                        break;
                    case 6:
                        deltaZ = -size;
                        break;
                    default:
                        deltaX = 0; ;
                        break;
                }
                cube.AddVertex(flb[0] + deltaX, flb[1] + deltaY, flb[2] + deltaZ);
            }

            cube.AddTriangle(0, 2, 1);
            cube.AddTriangle(0, 3, 2);
            cube.AddTriangle(1, 2, 5);
            cube.AddTriangle(2, 6, 5);
            cube.AddTriangle(2, 3, 6);
            cube.AddTriangle(3, 7, 6);
            cube.AddTriangle(3, 4, 7);
            cube.AddTriangle(3, 0, 4);
            cube.AddTriangle(4, 5, 6);
            cube.AddTriangle(4, 6, 7);
            cube.AddTriangle(0, 1, 5);
            cube.AddTriangle(0, 5, 4);

            return cube;
        }
    }
}
