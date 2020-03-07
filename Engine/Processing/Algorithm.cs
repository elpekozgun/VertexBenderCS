using Engine.Core;
using Priority_Queue;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FibonacciHeap;


namespace Engine.Processing
{

    public static class Algorithm
    {
        public static float[][] ConstructArrayFromMesh(Mesh mesh)
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
                    graph[j][i] = verts[i].Value;
                }
            }
            return graph;
        }

        public static float DijkstraArray(float[][] graph, int src, int target, out List<int> path, bool earlyTerminate = false)
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

            path = new List<int>
            {
                target
            };
            int k = parents[target];
            while (k != -1)
            {
                path.Add(k);
                k = parents[k];
            }

            return shortestDists[target];
        }

        public static float DijkstraMinHeap(Graph graph, int src, int target, out List<int> path, bool earlyTerminate = false)
        {
            var que = new FastPriorityQueue<QueueNode>(graph.Vertices.Count);

            var distances = new Dictionary<int, float>();
            var previouses = new Dictionary<int, FastPriorityQueueNode>();
            var nodeMap = new Dictionary<int, QueueNode>();

            for (int i = 0; i < graph.Vertices.Count; i++)
            {
                var dist = i == src ? 0 : float.MaxValue;
                distances[i] = dist;
                previouses[i] = null;
                var n = new QueueNode(i, dist);
                que.Enqueue(n, dist);
                nodeMap[i] = n;
            }

            while (que.Count > 0)
            {
                var u = que.Dequeue();

                if (earlyTerminate && u.id == target)
                {
                    break;
                }

                foreach (var neighbor in graph.Vertices[u.id].Verts)
                {
                    //float val = distances[u.id] + (graph.Vertices[u.id].Coord - graph.Vertices[neighbor].Coord).Length;
                    float val = distances[u.id] + neighbor.Value;
                    if (val < distances[neighbor.Key])
                    {
                        que.UpdatePriority(nodeMap[neighbor.Key], val);
                        distances[neighbor.Key] = val;
                        previouses[neighbor.Key] = u;
                    }
                }

            }

            path = new List<int>();
            int k = target;
            path.Add(target);
            while (k != src)
            {
                k = (previouses[k] as QueueNode).id;
                path.Add(k);
            }

            return distances[target];
        }

        public static KeyValuePair<int,float> DijkstraReturnMaxPair(Graph graph, int src)
        {
            var que = new FastPriorityQueue<QueueNode>(graph.Vertices.Count);

            var distances = new Dictionary<int, float>();
            var previouses = new Dictionary<int, FastPriorityQueueNode>();
            var nodeMap = new Dictionary<int, QueueNode>();

            KeyValuePair<int, float> retVal = new KeyValuePair<int, float>();

            for (int i = 0; i < graph.Vertices.Count; i++)
            {
                var dist = i == src ? 0 : float.MaxValue;
                distances[i] = dist;
                previouses[i] = null;
                var n = new QueueNode(i, dist);
                que.Enqueue(n, dist);
                nodeMap[i] = n;
            }

            while (que.Count > 0)
            {
                var u = que.Dequeue();

                foreach (var neighbor in graph.Vertices[u.id].Verts)
                {
                    //float val = distances[u.id] + (graph.Vertices[u.id].Coord - graph.Vertices[neighbor].Coord).Length;
                    float val = distances[u.id] + neighbor.Value;
                    if (val < distances[neighbor.Key])
                    {
                        que.UpdatePriority(nodeMap[neighbor.Key], val);
                        distances[neighbor.Key] = val;
                        previouses[neighbor.Key] = u;

                        if (val > retVal.Value)
                        {
                            retVal = new KeyValuePair<int, float>(neighbor.Key, val);
                        }

                    }
                }

            }
            return retVal;
            
        }

        public static float DijkstraFibonacciHeap(Graph graph, int src, int target, out List<int> path, bool earlyTerminate = false)
        {
            FibonacciHeap<int, float> que = new FibonacciHeap<int, float>(src);

            var distances = new Dictionary<int, float>();
            var previouses = new Dictionary<int, FibonacciHeapNode<int, float>>();
            var nodeMap = new Dictionary<int, FibonacciHeapNode<int, float>>();

            for (int i = 0; i < graph.Vertices.Count; i++)
            {
                var dist = i == src ? 0 : float.MaxValue;
                distances[i] = dist;
                previouses[i] = null;
                FibonacciHeapNode<int, float> n = new FibonacciHeapNode<int, float>(i, dist);
                que.Insert(n);
                nodeMap[i] = n;
            }

            while (que.Size() > 0)
            {
                var u = que.RemoveMin();

                if (earlyTerminate && u.Data == target)
                {
                    break;
                }

                foreach (var neighbor in graph.Vertices[u.Data].Verts)
                {
                    float val = distances[u.Data] + neighbor.Value;
                    if (val < distances[neighbor.Key])
                    {
                        distances[neighbor.Key] = val;
                        que.DecreaseKey(nodeMap[neighbor.Key], val);
                        previouses[neighbor.Key] = u;
                    }
                }

            }

            path = new List<int>();
            int k = target;
            path.Add(target);
            while (k != src)
            {
                k = previouses[k].Data;
                path.Add(k);
            }

            return distances[target];
        }

        public static float[,] CreateGeodesicDistanceMatrix(Graph graph)
        {
            int n = graph.Vertices.Count;

            // Fix this suboptimal nonsense after making bottom thing work.
            float[,] matrix = new float[n,n];
            for (int y = 0; y < graph.Vertices.Count; y++)
            {
                for (int x = 0; x < graph.Vertices.Count; x++)
                {
                    matrix[x,y] = DijkstraMinHeap(graph, x, y, out List<int> path, true);
                }
            }

            return matrix;
        }

        public static float[] CreateLinearGeodesicDistanceMatrix(Graph graph)
        {
            int n = graph.Vertices.Count;

            float[] matrix = new float[n * n];
            for (int y = 0; y < graph.Vertices.Count; y++)
            {
                for (int x = 0; x < graph.Vertices.Count; x++)
                {
                    matrix[n * y + x] = DijkstraMinHeap(graph, x, y, out List<int> path, true);
                }
            }

            return matrix;
        }

        public static List<int> FarthestPointSampling(Graph graph, int sampleCount)
        {

            List<int> samples = new List<int>();

            int currentSource = 0;

            Graph copyGraph = new Graph(graph.Vertices);

            while (sampleCount > 0)
            {
                var pair = DijkstraReturnMaxPair(copyGraph, currentSource);

                currentSource = pair.Key;
                samples.Add(pair.Key);

                for (int j = 0; j < graph.Vertices[currentSource].Verts.Count; j++)
                {
                    var a = graph.Vertices[currentSource].Verts[j].Key;
                    copyGraph.Vertices.RemoveAt(a);
                }
                copyGraph.Vertices.RemoveAt(currentSource);


                sampleCount--;
            }

            return samples;











            // ARE YOU AN IDIOT? THIS IS THE REASON YOU IMPLEMENT FPS.
            // NOT TO DO THIS CRAP N2 thing...
            // Pick a node, run dijkstra, go farthest do again etc.
            //var matrix = CreateGeodesicDistanceMatrix(graph);

            //List<int> localNeighbors = new List<int>();

            //List<int> candidates = new List<int>();
            //for (int i = 0; i < graph.Vertices.Count; i++)
            //{
            //    candidates.Add(i);
            //}
            //List<int> samples = new List<int>();

            //var currentNode = 0;
            //var nextNode = 0;

            //while (sampleCount > 0)
            //{
            //    float max = 0.0f;

            //    candidates.Remove(currentNode);
            //    for (int i = 0; i < candidates.Count; i++)
            //    {
            //        var v = matrix[currentNode, candidates[i]];
            //        if (v > max)
            //        {
            //            max = v;
            //            nextNode = candidates[i];
            //        }
            //    }
            //    currentNode = nextNode;
            //    samples.Add(currentNode);
                    
            //    //candidates.AddRange(localNeighbors);
            //    //localNeighbors.Clear();
            //    for (int j = 0; j < graph.Vertices[currentNode].Verts.Count; j++)
            //    {
            //        var a = graph.Vertices[currentNode].Verts[j].Key;

            //        localNeighbors.Add(a);
            //        candidates.Remove(a);
            //    }
            //    sampleCount--;
            //}

            //return samples;

        }
    }
}
