using Engine.Core;
using Priority_Queue;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FibonacciHeap;
using OpenTK;
using PriorityQueues;

namespace Engine.Processing
{

    public struct DijkstraOutput
    {
        public float[] AllDistances;
        public float[] Path;
        public float TargetDistance;

        public DijkstraOutput(float[] allDistances, float[] path, float targetDistance)
        {
            AllDistances = allDistances;
            Path = path;
            TargetDistance = targetDistance;
        }
    }

    public struct SampleOutput
    {
        public List<GraphNode> SamplePoints;
        public List<int> SampelIndices;

        public SampleOutput(List<GraphNode> samplePoints, List<int> sampelIndices)
        {
            SamplePoints = samplePoints;
            SampelIndices = sampelIndices;
        }
    }

    public struct IsoCurveOutput
    {
        public List<List<OpenTK.Vector3>> IsoCurves;
        //public List<List<Vector3>> IsoCurves;
        public float[] IsoCurveDistances;

        public IsoCurveOutput(List<List<OpenTK.Vector3>> isoCurves, float[] isoCurveDistances)
        {
            IsoCurves = isoCurves;
            IsoCurveDistances = isoCurveDistances;
        }
    }

    public static class Algorithm
    {
        public static List<List<KeyValuePair<int, float>>> ConstructGraphFromMesh(Mesh mesh)
        {
            var graph = new List<List<KeyValuePair<int, float>>>();

            for (int i = 0; i < mesh.Vertices.Count; i++)
            {
                graph.Add(mesh.Vertices[i].Verts);
            }

            return graph;
            //int n = mesh.Vertices.Count;
            //float[][] graph = new float[n][];
            //for (int i = 0; i < n; i++)
            //{
            //    var a = new float[n];
            //    graph[i] = a;
            //}

            //for (int j = 0; j < mesh.Vertices.Count; j++)
            //{
            //    var verts = mesh.Vertices[j].Verts;

            //    for (int i = 0; i < verts.Count; i++)
            //    {
            //        var w = (mesh.Vertices[j].Coord - mesh.Vertices[verts[i].Key].Coord).Length;
            //        graph[j][verts[i].Key] = w;
            //    }
            //}
            //return graph;
        }

        public static float DijkstraArray(List<List<KeyValuePair<int, float>>> graph, int src, int target, out List<int> path, bool earlyTerminate = false)
        {
            int n = graph.Count;
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


                for (int j = 0; j < graph[neighbor].Count; j++)
                {
                    var dist = graph[neighbor][j].Value;

                    var index = graph[neighbor][j].Key;

                    if (dist > 0 && (dist + shortestDist) < shortestDists[index])
                    {
                        parents[index] = neighbor;
                        shortestDists[index] = dist + shortestDist;
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
            var que = new FastPriorityQueue<QueueNode>(graph.Nodes.Count);

            var nodeMap = new Dictionary<int, QueueNode>();
            //var previousMap = new Dictionary<int, QueueNode>();

            for (int i = 0; i < graph.Nodes.Count; i++)
            {
                var dist = i == src ? 0 : float.MaxValue;
                var n = new QueueNode(i, graph.Nodes[i].Neighbors);
                que.Enqueue(n, dist);
                //previousMap[i] = null;
                nodeMap[i] = n;
            }

            while (que.Count > 0)
            {
                var u = que.Dequeue();

                if (earlyTerminate && u.id == target)
                {
                    break;
                }

                foreach (var neighbor in u.Neighbors)
                //foreach (var neighbor in graph.Nodes[u.id].Neighbors)
                {
                    float val = nodeMap[u.id].Priority + neighbor.Value;
                    if (val < nodeMap[neighbor.Key].Priority)
                    {
                        que.UpdatePriority(nodeMap[neighbor.Key], val);
                        nodeMap[neighbor.Key].PrevId = u.id;
                        //previousMap[neighbor.Key] = u;
                    }
                }

            }

            path = new List<int>();
            int k = target;
            path.Add(k);
            while (k != src)
            {
                k = nodeMap[k].PrevId;
                path.Add(k);
            }

            return nodeMap[target].Priority;

          
        }

        public static float DijkstraFibonacciHeap(Graph graph, int src, int target, out List<int> path, bool earlyTerminate = false)
        {
             FibonacciHeap.FibonacciHeap<int, float> que = new FibonacciHeap.FibonacciHeap<int, float>(src);

            //var previousMap = new Dictionary<int, FibonacciHeapNode<int, float>>();
            var nodeMap = new Dictionary<int, FibonacciHeapNode<int, float>>();

            for (int i = 0; i < graph.Nodes.Count; i++)
            {
                var dist = i == src ? 0 : float.MaxValue;
                //previousMap[i] = null;
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

                foreach (var neighbor in graph.Nodes[u.Data].Neighbors)
                {
                    float val = nodeMap[u.Data].Key + neighbor.Value;
                    if (val < nodeMap[neighbor.Key].Key)
                    {
                        que.DecreaseKey(nodeMap[neighbor.Key], val);
                        nodeMap[neighbor.Key].PrevId = u.Data;
                        //previousMap[neighbor.Key] = u;
                    }
                }

            }

            path = new List<int>();
            int k = target;
            path.Add(target);
            while (k != src)
            {
                //k = previousMap[k].Data;
                k = nodeMap[k].PrevId;
                path.Add(k);
            }

            return nodeMap[target].Key;
        }

        public static float[] DijkstraMinHeap(ref Graph graph, int src)
        {
            var que = new FastPriorityQueue<QueueNode>(graph.Nodes.Count);
            var nodeMap = new Dictionary<int, QueueNode>();

            for (int i = 0; i < graph.Nodes.Count; i++)
            {
                var dist = i == src ? 0 : float.MaxValue;
                var n = new QueueNode(i, graph.Nodes[i].Neighbors);
                que.Enqueue(n, dist);
                nodeMap[i] = n;
            }

            while (que.Count > 0)
            {
                var u = que.Dequeue();

                foreach (var neighbor in graph.Nodes[u.id].Neighbors)
                //foreach (var neighbor in u.Neighbors)
                {
                    float val = nodeMap[u.id].Priority + neighbor.Value;
                    if (val < nodeMap[neighbor.Key].Priority)
                    {
                        que.UpdatePriority(nodeMap[neighbor.Key], val);
                        nodeMap[neighbor.Key].PrevId = u.id;
                    }
                }

            }
            var retVal = new float[nodeMap.Count];
            for (int i = 0; i < retVal.Length; i++)
            {
                retVal[i] = nodeMap[i].Priority;
            }

            return retVal;

        }

        public static float[][] CreateLinearGeodesicDistanceMatrix(Graph graph, bool isParallel = true)
        {
            int n = graph.Nodes.Count;

            float[][] matrix = new float[n][];
            int count = 0;
            if (isParallel)
            {
                Parallel.For
                (
                    0,
                    n,
                    (i) =>
                    {
                        matrix[i] = DijkstraMinHeap(ref graph, i);
                        count++;
                    }
                );
            }
            else
            {
                for (int i = 0; i < n; i++)
                {
                    matrix[i] = DijkstraMinHeap(ref graph, i);
                }
            }


            return matrix;
        }

        public static List<float[]> CreateLinearGeodesicDistances(Graph graph, bool isParallel = true)
        {
            int n = graph.Nodes.Count;

            var matrix = new List<float[]>();
            for (int i = 0; i < n; i++)
            {
                matrix.Add(null);
            }

            if (isParallel)
            {
                Parallel.For
                (
                    0,
                    n,
                    (i) =>
                    {
                        matrix[i] = DijkstraMinHeap(ref graph, i);
                    }
                );
            }
            else
            {
                for (int i = 0; i < n; i++)
                {
                    matrix[i] = DijkstraMinHeap(ref graph, i);
                }
            }


            return matrix;
        }

        public static SampleOutput FarthestPointSampling(Graph graph, int sampleCount)
        {
            var distances = DijkstraMinHeap(ref graph, 0);
            var allDistances = new HashSet<float[]>
            {
                distances
            };

            List<GraphNode> farthestPoints = new List<GraphNode>();
            List<int> farthestIndices = new List<int>();

            for (int i = 0; i < sampleCount; i++)
            {
                var cluster = new FastPriorityQueue<QueueNode>(graph.Nodes.Count);

                foreach (var node in graph.Nodes)
                {
                    float minDist = float.MaxValue;
                    foreach (var distance in allDistances)
                    {
                        var dist = distance[node.Id];
                        if (dist < minDist)
                        {
                            minDist = dist;
                        }
                    }
                    cluster.Enqueue(new QueueNode(node.Id, node.Neighbors), -minDist);
                }

                var u = cluster.Dequeue();
                if (i == 0)
                {
                    allDistances.Clear();
                }
                allDistances.Add(DijkstraMinHeap(ref graph, u.id));

                farthestPoints.Add(graph.Nodes[u.id]);
                farthestIndices.Add(graph.Nodes[u.id].Id);
            }

            return new SampleOutput(farthestPoints, farthestIndices);
        }

        public static List<float> GaussianCurvature(Mesh mesh)
        {
            List<float> Curvatures = new List<float>();

            for (int i = 0; i < mesh.Vertices.Count; i++)
            {
                var vertex = mesh.Vertices[i];
                float curvature = 2.0f * (float)Math.PI;
                foreach (var triId in vertex.Tris)
                {
                    var ang = mesh.GetTriangleAngle(triId, vertex.Id);

                    curvature -= ang;
                }
                Curvatures.Add(curvature);
            }

            return Curvatures;
        }

        public static List<float> AverageGeodesicDistance(Graph graph)
        {
            List<float> distances = new List<float>();

            for (int i = 0; i < graph.Nodes.Count; i++)
            {
                float sum = 0.0f;
                var allDist = DijkstraMinHeap(ref graph, i);
                for (int j = 0; j < allDist.Length; j++)
                {
                    sum += allDist[j];
                }
                sum /= allDist.Length;
                distances.Add(sum);
            }

            return distances;
        }

        public static IsoCurveOutput IsoCurveSignature(Mesh mesh, int source, int sampleCount)
        {
            Graph graph = new Graph(mesh);
            var distances = DijkstraMinHeap(ref graph, source);

            /*

            Dx = { lx(r1), lx(r2),lx(r2),lx(r2),...,lx(rn)} ,      r1 < r2 < r3 < ... rn

                  p1/  
              v1---/--v2
               \  /   /   
                \/   /    lxT(ri) = |p1 - p2| , where
              p2/\  /     pj = (1 - αj)*v0 + αj*vj,
                  \/      and  αj = |ri - g0| / |gj - g0|
                  v3

             */

            int k = sampleCount;
            float maxDist = distances.Max();

            float[] isoCurveDistances = new float[k];
            List<List<Vector3>> isoCurves = new List<List<Vector3>>();

            var d = maxDist / k;

            for (int i = 0; i < k; i++)
            {
                float radius = i * d;
                float isoCurveLength = 0;

                var isoCurve = new List<Vector3>();

                for (int j = 0; j < mesh.Triangles.Count; j++)
                {
                    var tri = mesh.Triangles[j];

                    var v1Id = tri.V1;
                    var v2Id = tri.V2;
                    var v3Id = tri.V3;

                    var distV1 = distances[v1Id];
                    var distV2 = distances[v2Id];
                    var distV3 = distances[v3Id];

                    // no hit on triangle
                    if (distV1 < radius && distV2 < radius && distV3 < radius ||
                       distV1 > radius && distV2 > radius && distV3 > radius)
                    {
                        continue;
                    }

                    List<int> lower = new List<int>();
                    List<int> greater = new List<int>();
                    if (distV1 <= radius)
                    {
                        lower.Add(v1Id);
                    }
                    else
                    {
                        greater.Add(v1Id);
                    }

                    if (distV2 <= radius)
                    {
                        lower.Add(v2Id);
                    }
                    else
                    {
                        greater.Add(v2Id);
                    }

                    if (distV3 <= radius)
                    {
                        lower.Add(v3Id);
                    }
                    else
                    {
                        greater.Add(v3Id);
                    }

                    if (lower.Count == 0 || greater.Count == 0)
                    {
                        continue;
                    }
                    Vector3 p1 = new Vector3();
                    Vector3 p2 = new Vector3();

                    if (lower.Count < greater.Count)
                    {
                        var g0 = distances[lower[0]];
                        var g1 = distances[greater[0]];
                        var g2 = distances[greater[1]];
                        var alpha1 = (float)(Math.Abs(radius - g0) / Math.Abs(g1 - g0));
                        var alpha2 = (float)(Math.Abs(radius - g0) / Math.Abs(g2 - g0));
                        var v0 = mesh.Vertices[lower[0]].Coord;
                        var v1 = mesh.Vertices[greater[0]].Coord;
                        var v2 = mesh.Vertices[greater[1]].Coord;

                        p1 = (1.0f - alpha1) * v0 + alpha1 * v1;
                        p2 = (1.0f - alpha2) * v0 + alpha2 * v2;

                    }
                    else if (lower.Count > greater.Count)
                    {
                        var g0 = distances[lower[0]];
                        var g1 = distances[lower[1]];
                        var g2 = distances[greater[0]];
                        var alpha1 = Math.Abs(radius - g0) / Math.Abs(g2 - g0);
                        var alpha2 = Math.Abs(radius - g1) / Math.Abs(g2 - g1);
                        var v0 = mesh.Vertices[lower[0]].Coord;
                        var v1 = mesh.Vertices[lower[1]].Coord;
                        var v2 = mesh.Vertices[greater[0]].Coord;

                        p1 = (1.0f - alpha1) * v0 + alpha1 * v2;
                        p2 = (1.0f - alpha2) * v1 + alpha2 * v2;

                    }
                    isoCurveLength += (p1 - p2).Length;
                    isoCurve.Add(p1);
                    isoCurve.Add(p2);
                }
                isoCurveDistances[i] = isoCurveLength;

                for (int j = 0; j < isoCurve.Count - 1; j++)
                {
                    if ((isoCurve[j] - isoCurve[j + 1]).Length < 0.00005f)
                    {
                        Vector3 common = (isoCurve[j] + isoCurve[j + 1]) * 0.5f;
                        isoCurve[j] = common;
                        isoCurve[j + 1] = common;
                    }
                }

                isoCurves.Add(isoCurve);
            }
            return new IsoCurveOutput(isoCurves, isoCurveDistances);
        }



        //TODO: These are slower but more consistent in terms of usage.. Fix these methods in a more common way along with array implementation

        public static float DijkstraFibonacciHeapNew(Graph graph, int src, int target, out List<int> path, bool earlyTerminate = false)
        {
            var que = new PriorityQueues.FibonacciHeap<HeapNode, float>(PriorityQueueType.Minimum);
            var nodeMap = new Dictionary<int, IPriorityQueueEntry<HeapNode>>();

            for (int i = 0; i < graph.Nodes.Count; i++)
            {
                var dist = i == src ? 0 : float.MaxValue;

                var n = new HeapNode(i, graph.Nodes[i].Neighbors, dist);
                IPriorityQueueEntry<HeapNode> output = que.Enqueue(n, dist);
                nodeMap[i] = output;
            }

            while (que.Count > 0)
            {
                var u = que.Dequeue();

                foreach (var neighbor in graph.Nodes[u.id].Neighbors)
                {
                    float val = nodeMap[u.id].Item.Priority + neighbor.Value;
                    if (val < nodeMap[neighbor.Key].Item.Priority)
                    {
                        nodeMap[neighbor.Key].Item.Priority = val;
                        que.UpdatePriority(nodeMap[neighbor.Key], val);
                        nodeMap[neighbor.Key].Item.PrevId = u.id;
                    }
                }

            }

            path = new List<int>();
            int k = target;
            path.Add(target);
            while (k != src)
            {
                //k = previousMap[k].Data;
                k = nodeMap[k].Item.PrevId;
                path.Add(k);
            }

            return nodeMap[target].Item.Priority;
        }

        public static float DijkstraMinHeapNew(ref Graph graph, int src, int target)
        {
            var que = new PriorityQueues.BinaryHeap<HeapNode,float>(PriorityQueueType.Minimum);
            var nodeMap = new Dictionary<int, IPriorityQueueEntry<HeapNode>>();

            for (int i = 0; i < graph.Nodes.Count; i++)
            {
                var dist = i == src ? 0 : float.MaxValue;

                var n = new HeapNode(i, graph.Nodes[i].Neighbors, dist);
                IPriorityQueueEntry<HeapNode> output = que.Enqueue(n, dist);
                nodeMap[i] = output;
            }

            while (que.Count > 0)
            {
                var u = que.Dequeue();

                foreach (var neighbor in graph.Nodes[u.id].Neighbors)
                {
                    float val = nodeMap[u.id].Item.Priority + neighbor.Value;
                    if (val < nodeMap[neighbor.Key].Item.Priority)
                    {
                        nodeMap[neighbor.Key].Item.Priority = val;
                        que.UpdatePriority(nodeMap[neighbor.Key], val);
                        nodeMap[neighbor.Key].Item.PrevId = u.id;
                    }
                }

            }
            //var retVal = new float[nodeMap.Count];
            //for (int i = 0; i < retVal.Length; i++)
            //{
            //    retVal[i] = nodeMap[i].Priority;
            //}

            //return retVal;

            return nodeMap[target].Item.Priority;
        }


    }
    public class HeapNode 
    {
        public int id;
        public int PrevId { get; set; }

        public List<KeyValuePair<int, float>> Neighbors;
        public float Priority;

        public HeapNode(int id, List<KeyValuePair<int, float>> neighbors, float priority)
        {
            this.id = id;
            Neighbors = neighbors;
            Priority = priority;
        }
    }

}
