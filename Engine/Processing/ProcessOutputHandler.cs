using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public static OpenTK.Vector3 ColorPixelVector(float value, float max)
        {
            var ratio = value / max;

            if (ratio < 0.25f)
            {
                return new OpenTK.Vector3(0.0f, ratio , 1.0f);
            }
            if (ratio < 0.5f)
            {
                return new OpenTK.Vector3(0.0f, 1.0f, 1 - ratio);
            }
            if (ratio < 0.75f)
            {
                return new OpenTK.Vector3(ratio , 1.0f, 0.0f);
            }
            if (ratio <= 1.0f)
            {
                return new OpenTK.Vector3(1.0f, 1 - ratio, 0.0f);
            }
            return new OpenTK.Vector3(1.0f, 0.0f, 0.0f);
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

        public static void CreateBitmapGeodesicDistance(float[][] matrix, string file)
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

            bitmap.Save(file + ".bmp");
            bitmap.Dispose();
        }
    }


}
