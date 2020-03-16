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
    }

}
