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
    public class Tree<T> where T : QueueNode
    { 
        public QueueNode Head;
        public QueueNode Parent;
        public List<Tree<T>> Children;
    }

    public class FibonacciHeap2
    {
        public Dictionary<int,Tree<QueueNode>> Heaps;
        private int _minIndex;

        //public QueueNode PopMin()
        //{
        //    var minTree = Heaps[_minIndex];

        //    foreach (var child in minTree.Children)
        //    {
                
        //        UpdateMin(child);
        //    }

        //}
        
        public void Insert(Tree<QueueNode> tree)
        {
            Heaps.Add(Heaps.Count, tree);
            UpdateMin();
        }

        private void UpdateMin()
        {
            int lastId = Heaps.Count - 1;
            var lastAdded = Heaps[lastId];

            if (Heaps[_minIndex].Head.Priority > lastAdded.Head.Priority)
            {
                _minIndex = lastId;
            }
        }

        private void Regen()
        {

        }

    }

    public static class Algorithm
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
                    float val = distances[u.id] + (graph.Vertices[u.id].Coord - graph.Vertices[neighbor].Coord).Length;
                    if (val < distances[neighbor])
                    {
                        nodeMap[neighbor].prevId = u.id;
                        que.UpdatePriority(nodeMap[neighbor], val);
                        distances[neighbor] = val;
                        previouses[neighbor] = u;
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

        public static float DijkstraFibonacciHeap(Graph graph, int src, int target, out List<int> path, bool earlyTerminate = false)
        {
            FibonacciHeap<QueueNode, float> que = new FibonacciHeap<QueueNode, float>(src);

            var distances = new Dictionary<int, float>();
            var previouses = new Dictionary<int, FibonacciHeapNode<QueueNode, float>>();
            var nodeMap = new Dictionary<int, FibonacciHeapNode<QueueNode, float>>();

            for (int i = 0; i < graph.Vertices.Count; i++)
            {
                var dist = i == src ? 0 : float.MaxValue;
                distances[i] = dist;
                previouses[i] = null;
                FibonacciHeapNode<QueueNode, float> n = new FibonacciHeapNode<QueueNode, float>(new QueueNode(i, dist),dist);
                que.Insert(n);
                nodeMap[i] = n;
            }

            while (que.Size() > 0)
            {
                var u = que.RemoveMin();

                if (earlyTerminate && u.Data.id == target)
                {
                    break;
                }

                foreach (var neighbor in graph.Vertices[u.Data.id].Verts)
                {
                    float val = distances[u.Data.id] + (graph.Vertices[u.Data.id].Coord - graph.Vertices[neighbor].Coord).Length;
                    if (val < distances[neighbor])
                    {
                        nodeMap[neighbor].Data.prevId = u.Data.id;
                        distances[neighbor] = val;
                        que.DecreaseKey(nodeMap[neighbor], val);
                        previouses[neighbor] = u;
                    }
                }

            }

            path = new List<int>();
            int k = target;
            path.Add(target);
            while (k != src)
            {
                k = previouses[k].Data.id;
                path.Add(k);
            }

            return distances[target];
        }

    }
}
