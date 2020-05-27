using Engine.Core;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;

namespace Engine.Processing
{
    public static class ProcessOutputHandler
    {
        public static Color ColorPixel(float value, float max)
        {
            var ratio = value / max;

            if (ratio < 0.25f)
            {
                return Color.FromArgb(0, (int)(ratio * 255), 255);
            }
            if (ratio < 0.5f)
            {
                return Color.FromArgb(0, 255, (int)((1 - ratio) * 255));
            }
            if (ratio < 0.75f)
            {
                return Color.FromArgb((int)(ratio * 255), 255, 0);
            }
            if (ratio <= 1.0f)
            {
                return Color.FromArgb(255, (int)((1 - ratio) * 255), 0);
            }
            return Color.FromArgb(255, 0, 0);
        }

        public static Vector3 ColorPixelVector(float value, float max)
        {
            var ratio = value / max;

            if (ratio < 0.25f)
            {
                return new Vector3(0.0f, ratio , 1.0f);
            }
            if (ratio < 0.5f)
            {
                return new Vector3(0.0f, 1.0f, 1 - ratio);
            }
            if (ratio < 0.75f)
            {
                return new Vector3(ratio , 1.0f, 0.0f);
            }
            if (ratio <= 1.0f)
            {
                return new Vector3(1.0f, 1 - ratio, 0.0f);
            }
            return new Vector3(1.0f, 0.0f, 0.0f);
        }

        public static Vector3 ColorPixelVectorAGD(float value, float max)
        {
            var ratio = value / max;

            if (ratio < 0.5f)
            {
                return new Vector3(0.0f, ratio, 1.0f);
            }
            if (ratio < 0.5f)
            {
                return new Vector3(0.0f, 1.0f, 1 - ratio);
            }
            if (ratio < 0.75f)
            {
                return new Vector3(ratio, 1.0f, 0.0f);
            }
            if (ratio <= 1.0f)
            {
                return new Vector3(1.0f, 1 - ratio, 0.0f);
            }
            return new Vector3(1.0f, 0.0f, 0.0f);
        }

        public static void CreateBitmapForGraph(float[][] graph, int[] path, string file)
        {
            int n = graph.GetLength(0);
            Bitmap bitmap = new Bitmap(n, n, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
            float max = float.MinValue;
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    max = max < graph[i][j] ? graph[i][j] : max;
                }
            }

            for (int y = 0; y < n; y++)
            {
                for (int x = 0; x < n; x++)
                {
                    if (graph[y][x] == 0.0f)
                    {
                        bitmap.SetPixel(x, y, Color.LightYellow);
                    }
                    else
                    {
                        bitmap.SetPixel(x, y, Color.FromArgb(255, (int)(255 * graph[y][x] / max), 0, 0));
                    }
                    for (int k = 0; k < path.Length; k++)
                    {
                        if (x == path[k] && y == path[k])
                        {
                            bitmap.SetPixel(x, y, Color.Blue);
                        }
                    }

                }
            }
            bitmap.Save(file + ".bmp");
            bitmap.Dispose();
        }

        public static Bitmap CreateBitmapGeodesicDistance(float[][] matrix, string file)
        {
            var n = matrix.GetLength(0);
            Bitmap bitmap = new Bitmap(n, n, System.Drawing.Imaging.PixelFormat.Format24bppRgb);

            float max = 0;
            for (int y = 0; y < n; y++)
            {
                for (int x = 0; x < n; x++)
                {
                    max = max < matrix[y][x] ? matrix[y][x] : max;
                }
            }

            for (int y = 0; y < n; y++)
            {
                for (int x = 0; x < n; x++)
                {
                    bitmap.SetPixel(x, y, ColorPixel(matrix[y][x], max));
                }
            }

            bitmap.Save(file);
            return bitmap;
        }
       
        public static void SaveGeodesicDistanceToFile(float[][] matrix, string file)
        {
            var n = matrix.GetLength(0);

            using (StreamWriter writer = new StreamWriter(file))
            {
                float max = 0;
                for (int y = 0; y < n; y++)
                {
                    for (int x = 0; x < n; x++)
                    {
                        max = max < matrix[y][x] ? matrix[y][x] : max;
                    }
                }

                for (int y = 0; y < n; y++)
                {
                    for (int x = 0; x < n; x++)
                    {
                        writer.Write(matrix[y][x].ToString() + " ");
                    }
                }
            }
        }

        public static void SaveIsoCurveOutputs(Dictionary<Transform, IsoCurveOutput> outputs, string file)
        {
            using (StreamWriter writer = new StreamWriter(file))
            {
                foreach (var output in outputs)
                {
                    writer.WriteLine(output.Key.Name);
                    for (int i = 0; i < output.Value.IsoCurveDistances.Length; i++)
                    {
                        var first = i;
                        var second = output.Value.IsoCurveDistances[i];
                        var line = string.Format("{0},{1}", first, second);
                        writer.WriteLine(line);
                        writer.Flush();
                    }
                    writer.Write(Environment.NewLine);
                }
            }


        }

        public static void SaveOffFile(Mesh mesh, string file)
        {
            using (StreamWriter writer = new StreamWriter(file))
            {
                writer.WriteLine("OFF");
                writer.WriteLine($"{mesh.Vertices.Count} {mesh.Triangles.Count} 0");

                foreach (var vertex in mesh.Vertices)
                {
                    writer.WriteLine($"{vertex.Value.Coord.X} {vertex.Value.Coord.Y} {vertex.Value.Coord.Z}");
                }

                foreach (var tri in mesh.Triangles)
                {
                    writer.WriteLine($"3 {tri.Value.V1} {tri.Value.V2}  {tri.Value.V3}");
                }

                //for (int i = 0; i < mesh.Vertices.Count; i++)
                //{
                //    writer.WriteLine($"{mesh.Vertices[i].Coord.X} {mesh.Vertices[i].Coord.Y} {mesh.Vertices[i].Coord.Z}");
                //}
                //for (int i = 0; i < mesh.Triangles.Count; i++)
                //{
                //    writer.WriteLine($"3 {mesh.Triangles[i].V1} {mesh.Triangles[i].V2}  {mesh.Triangles[i].V3}");
                //}
                //writer.Write(Environment.NewLine);
            }
        }

        public static void SaveSTLFile(Mesh mesh, string file)
        {
            using (StreamWriter writer = new StreamWriter(file))
            {
                writer.WriteLine("solid");
                writer.WriteLine("\tVertex Bender Framework");
                writer.WriteLine("\tÖzgün Elpek");

                foreach (var tri in mesh.Triangles)
                {
                    var normal = mesh.CalculateTriangleNormals(tri.Value);
                    var v1 = mesh.Vertices[tri.Value.V1];
                    var v2 = mesh.Vertices[tri.Value.V2];
                    var v3 = mesh.Vertices[tri.Value.V3];
                    
                    writer.WriteLine($"\tfacet normal {normal.X} {normal.Y} {normal.Z}");
                    writer.WriteLine("\touter loop");
                    writer.WriteLine($"\t\tvertex {v1.Coord.X} {v1.Coord.Y} {v1.Coord.Z}");
                    writer.WriteLine($"\t\tvertex {v2.Coord.X} {v2.Coord.Y} {v2.Coord.Z}");
                    writer.WriteLine($"\t\tvertex {v3.Coord.X} {v3.Coord.Y} {v3.Coord.Z}");
                    writer.WriteLine("\tendloop");
                    writer.WriteLine("\tendfacet");
                }
                writer.WriteLine("endsolid");

            }
        }

        public static void SaveSTLBinaryFile(Mesh mesh, string file)
        { 
        
        }
    }

}
