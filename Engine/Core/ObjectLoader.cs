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
            mesh.Center();
            return mesh;
        }

        public static Mesh LoadObj(string path)
        {
            return null;
        }

        public static Mesh LoadDae(string path)
        {
            return null;
        }

        public static Mesh LoadVol(string path)
        {
            return null;
        }

        public static Mesh LoadDicom(string path)
        {
            return null;
        }

    }
}
