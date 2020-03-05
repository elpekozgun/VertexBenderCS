using Engine.Core;
using OpenTK.Graphics;
using System.Collections.Generic;
using System.Drawing;

namespace Engine.Processing
{
    public class Node
    {
        public int Dest;
        public float Value;
        public Node Next;

        public Node(int dest, float value, Node next)
        {
            Dest = dest;
            Value = value;
            Next = next;
        }
    }

    public class NodeList
    {
        Node Head;
    }

    public class Graph
    {
        int Index;
        NodeList List;
    }

    public static class Dijkstra
    {
        public static float[][] ConstructGraphFromMesh(Mesh mesh)
        {


            int n = mesh.Vertices.Count;
            float[][] graph = new float[n][];
            for (int i = 0; i < n; i++)
            {
                var a = new float[n];
                graph[i] = a; 
            }

            for (int j = 0; j < mesh.Vertices.Count; j++)
            {
                var verts = mesh.Vertices[j].Verts;

                for (int i = 0; i < verts.Count; i++)
                {
                    var w = (mesh.Vertices[j].Coord - mesh.Vertices[verts[i]].Coord).Length;
                    graph[j][verts[i]] = w;
                }   
            }
            return graph;
        }

        public static void DijkstraArray(float[][] graph, int src, int target, out int[] path)
        {
            int n = graph.GetLength(0);
            float[] shortestDists = new float[n];
            bool[] added = new bool[n];

            if (src >= n || src < 0 || target >= n || target < 0)
            {
                path = null;
                return;
            }

            for (int i = 0; i < n; i++)
            {
                shortestDists[i] = float.MaxValue;
                added[i] = false;
            }
            int[] parents = new int[n];

            shortestDists[src] = 0;
            parents[src] = -1;

            for (int i = 0; i < n; i++)
            {
                int neighbor = -1;
                float shortestDist = float.MaxValue;

                for (int j = 0; j < n; j++)
                {
                    if (!added[j] && shortestDists[j] < shortestDist)
                    {
                        neighbor = j;
                        shortestDist = shortestDists[j];
                    }
                }

                added[neighbor] = true;

                for (int j = 0; j < n; j++)
                {
                    float edgeDist = graph[neighbor][j];

                    if (edgeDist > 0 && (shortestDist + edgeDist) < shortestDists[j])
                    {
                        parents[j] = neighbor;
                        shortestDists[j] = shortestDist + edgeDist;
                    }
                }
            }
            var a = new List<int>();

            int k = parents[target];
            a.Add(k);
            while (k != -1)
            {
                a.Add(k);
                k = parents[k];
            }

            path = a.ToArray();
        }

        public static void DijkstraHeap()
        {


        }



        public static void CreateBitmap(float[][] graph,int[] path, string file)
        {
            int n = graph.GetLength(0);
            Bitmap bitmap = new Bitmap(n, n,System.Drawing.Imaging.PixelFormat.Format24bppRgb);
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
    }
}
