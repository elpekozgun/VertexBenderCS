using Engine.Core;
using OpenTK.Graphics;
using System.Collections.Generic;
using System.Drawing;

namespace Engine.Processing
{
    public static class Dijkstra
    {
        public static float[,] ConstructGraphFromMesh(Mesh mesh)
        {
            int n = mesh.Vertices.Count;
            float[,] graph = new float[n, n];

            for (int j = 0; j < mesh.Vertices.Count; j++)
            {
                var verts = mesh.Vertices[j].Verts;

                for (int i = 0; i < verts.Count; i++)
                {
                    var w = (mesh.Vertices[j].Coord - mesh.Vertices[verts[i]].Coord).Length;
                    graph[j, verts[i]] = w;
                }   
            }
            return graph;
        }

        public static float FindShortestPath(float[,] graph, int n, int srcIndex, int target, out int[] path)
        {
            var p = new List<int>();

            float[] dist = new float[n];
            bool[] spt = new bool[n];

            for (int i = 0; i < n; i++)
            {
                dist[i] = float.MaxValue;
                spt[i] = false;
            }

            dist[srcIndex] = 0;

            for (int i = 0; i < n - 1; i++)
            {
                int u = MinDistance(dist, spt, n);
                spt[u] = true;

                for (int j = 0; j < n; j++)
                {
                    if (!spt[j] && graph[u,j] != 0 && dist[u] != float.MaxValue && dist[u] + graph[u,j] < dist[j])
                    {
                        dist[j] = dist[u] + graph[u, j];
                        if (i == target)
                        {
                            p.Add(j);
                        }
                    }
                }
            }

            path = p.ToArray();
            return dist[target];
        }

        public static int MinDistance(float[] distances, bool[] spt, int n)
        {
            float min = float.MaxValue;
            int minIndex = -1;

            for (int i = 0; i < n; i++)
            {
                if (spt[i] == false && distances[i] <= min)
                {
                    min = distances[i];
                    minIndex = i;
                }
            }

            return minIndex;
        }

        public static void CreateBitmap(float[,] graph, int n ,string file)
        {
            Bitmap bitmap = new Bitmap(n, n);
            for (int y = 0; y<bitmap.Height; y++)
            {
                for (int x = 0; x<bitmap.Width; x++)
                {
                    if (graph[x, y] == 0)
                    {
                        bitmap.SetPixel(x, y, Color.White);
                    }
                    else
                    {
                        bitmap.SetPixel(x, y, (Color)(new Color4(0.0f, 0.0f, graph[x, y], 1.0f)));
                    }
                }
            }
            bitmap.Save(file);
            bitmap.Dispose();
        }
    }
}
