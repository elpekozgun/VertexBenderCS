using Engine.Core;
using OpenTK.Graphics;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace Engine.Processing
{

    public class MinHeapNode : IComparable<MinHeapNode>
    {
        public int Id;
        public float Distance;
        public MinHeapNode Parent;
        public List<MinHeapNode> Neighbors;

        public MinHeapNode(int id, float distance, List<MinHeapNode> neighbors, MinHeapNode previous)
        {
            Id = id;
            Distance = distance;
            Parent = previous;
            Neighbors = neighbors;
        }

        public int CompareTo(MinHeapNode other)
        {
            return (int)(this.Distance - other.Distance);
        }
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

        public static float DijkstraArray(float[][] graph, int src, int target, out int[] path)
        {
            int n = graph.GetLength(0);
            float[] shortestDists = new float[n];
            bool[] added = new bool[n];

            if (src >= n || src < 0 || target >= n || target < 0)
            {
                path = null;
                return float.MaxValue;
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
                    if (!added[j] && shortestDists[j] < shortestDist )
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

            a.Add(target);
            int k = parents[target];
            while (k != -1)
            {
                a.Add(k);
                k = parents[k];
            }
            path = a.ToArray();
            return shortestDists[target];
        }

        public static List<MinHeapNode> DijkstraHeap(Mesh mesh, int src, int target, out int[] path)
        {
            List<MinHeapNode> graph = new List<MinHeapNode>();

            for (int i = 0; i < mesh.Vertices.Count; i++)
            {
                graph.Add(new MinHeapNode(i, 1000 ,null, null));
            }
            graph[src].Distance = 0;
            graph[src].Parent = null;

            for (int i = 0; i < graph.Count; i++)
            {
                graph[i].Neighbors = new List<MinHeapNode>();
                for (int j = 0; j < mesh.Vertices[i].Verts.Count; j++)
                {
                    graph[i].Neighbors.Add(graph[mesh.Vertices[i].Verts[j]]);
                }
            }

            var heap = new BinaryHeap<MinHeapNode>();

            for (int i = 0; i < graph.Count; i++)
            {
                heap.Add(graph[i]);
            }


            while (heap.Count > 0)
            {
                var u = heap.Remove();
                foreach (var neighbor in u.Neighbors)
                {
                    Relax(u, neighbor , mesh.Vertices);
                }
            }

            var sp = new List<MinHeapNode>();

            bool hasparent = true;
            int t = target;
            while (hasparent)
            {
                var node = graph[t];

                sp.Add(node);
                if (node.Parent != null)
                {
                    t = node.Parent.Id;
                    continue;
                }
                hasparent = false;
            }

            path = new int[sp.Count];

            for (int i = 0; i < sp.Count; i++)
            {
                path[i] = sp[i].Id;
            }

            return sp;
        }

        public static List<MinHeapNode> DijkstraHeap2(Mesh mesh, int src ,int target, out int[] path)
        {
            //List<MinHeapNode> visitedNodes = new List<MinHeapNode>();
            List<MinHeapNode> graph = new List<MinHeapNode>();

            for (int i = 0; i < mesh.Vertices.Count; i++)
            {
                graph.Add(new MinHeapNode(i, 100000, null, null));
            }

            graph[src].Distance = 0;

            for (int i = 0; i < graph.Count; i++)
            {
                graph[i].Neighbors = new List<MinHeapNode>();
                var ns = mesh.Vertices[i].Verts;
                for (int j = 0; j < ns.Count; j++)
                {
                    graph[i].Neighbors.Add(graph[ns[j]]);
                }
            }

            var heap = new MinHeap();

            for (int i = 0; i < graph.Count; i++)
            {
                heap.Add(graph[i]);
            }


            while (heap.Size > 0)
            {
                var u = heap.Peek(); 
             
                for (int i = 0; i < u.Neighbors.Count; i++)
                {
                    Relax(u, u.Neighbors[i], mesh.Vertices);
                }
                heap.Poll();
            }

            var sp = new List<MinHeapNode>();

            bool hasparent = true;
            int t = target;
            while (hasparent)
            {
                var node = graph[t];

                sp.Add(node);
                if (node.Parent != null)
                {
                    t = node.Parent.Id;
                    continue;
                }
                hasparent = false;
            }

            path = new int[sp.Count];

            for (int i = 0; i < sp.Count; i++)
            {
                path[i] = sp[i].Id;
            }

            return sp;
        }

        private static void Relax(MinHeapNode start, MinHeapNode end, List<Vertex> meshVertices)
        {
            var neighborIndex = start.Neighbors.IndexOf(end);

            float dist = (meshVertices[start.Id].Coord - meshVertices[start.Neighbors[neighborIndex].Id].Coord).Length;

            //var dist =  start.Neighbors[neighborIndex].Distance;
            if (end.Distance > start.Distance + dist)
            {
                end.Distance = start.Distance + dist;
                end.Parent = start;
            }
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

        private static int MinimumDistance(float[] distance, bool[] shortestPathTreeSet, int verticesCount)
        {
            float min = 100000;
            int minIndex = 0;

            for (int v = 0; v < verticesCount; ++v)
            {
                if (shortestPathTreeSet[v] == false && distance[v] <= min)
                {
                    min = distance[v];
                    minIndex = v;
                }
            }

            return minIndex;
        }

        public static float DijkstraAlgo(float[][] graph, int src, int trg, out int[] path)
        {

            int n = graph.GetLength(0);
            float[] distance = new float[n];
            bool[] shortestPathTreeSet = new bool[n];

            for (int i = 0; i < n; ++i)
            {
                distance[i] = int.MaxValue;
                shortestPathTreeSet[i] = false;
            }

            distance[src] = 0;
            
            int[] parents = new int[n];
            distance[src] = 0;
            parents[src] = -1;

            for (int count = 0; count < n - 1; ++count)
            {
                int u = MinimumDistance(distance, shortestPathTreeSet, n);
                shortestPathTreeSet[u] = true;

                for (int v = 0; v < n; ++v)
                {
                    if (!shortestPathTreeSet[v] && Convert.ToBoolean(graph[u][v]) && distance[u] != int.MaxValue && distance[u] + graph[u][v] < distance[v])
                    {
                        //parents[j] = neighbor;
                        distance[v] = distance[u] + graph[u][v];
                    }
                }
            }

            //int k = parents[target];
            //a.Add(k);
            //while (k != -1)
            //{
            //    a.Add(k);
            //    k = parents[k];
            //}
            //path = a.ToArray();
            //return 5.0f;
            path = new int[0];
            return distance[trg];

        }

        public static float[][] ConstructHeapGraphFromMesh(Mesh mesh)
        {
            int n = mesh.Vertices.Count;
            float[][] graph = new float[n][];

            for (int j = 0; j < mesh.Vertices.Count; j++)
            {
                var verts = mesh.Vertices[j].Verts;

                List<float> neighbors = new List<float>();

                for (int i = 0; i < verts.Count; i++)
                {
                    var w = (mesh.Vertices[j].Coord - mesh.Vertices[verts[i]].Coord).Length;
                    neighbors.Add(w);
                }
                graph[j] = neighbors.ToArray();
            }
            return graph;
        }


    }
}
