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
                        bitmap.SetPixel(x, y, Color.FromArgb(255, (int)(255 * graph[x][y] / max), 0, 0));
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

        public static void CreateBitmapGeodesicDistance()
        {

        }
    }


}
