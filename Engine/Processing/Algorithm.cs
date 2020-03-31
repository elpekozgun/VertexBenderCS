using Engine.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OpenTK;
using PriorityQueues;
using System.Diagnostics;
using System.Threading;
using MathNet.Numerics.LinearAlgebra;

namespace Engine.Processing
{
    public enum eShortestPathMethod : byte
    {
        Array = 0x1,
        MinHeap = 0x2,
        Fibonacci= 0x4,
        Astar = 0x8
    }

    public enum eParameterizationMethod : byte
    {
        Uniform = 0,
        Harmonic,
        MeanValue
    }

    public static class Algorithm
    {
        private static readonly Stopwatch _watch = new Stopwatch();

        #region Public Methods

        public static ShortestPathOutput ShortestPath(Mesh mesh, int src, int target, eShortestPathMethod type, bool earlyTerminate = false)
        {
            switch (type)
            {
                case eShortestPathMethod.Array:
                    return DijkstraArray(mesh, src, target, earlyTerminate);
                case eShortestPathMethod.MinHeap:
                    return DijkstraMinHeap(mesh, src, target, earlyTerminate);
                case eShortestPathMethod.Fibonacci:
                    return DijkstraFibonacciHeap(mesh, src, target, earlyTerminate);
                case eShortestPathMethod.Astar:
                    return AStarMinHeap(mesh, src, target);
                default:
                    return DijkstraMinHeap(mesh, src, target, earlyTerminate);
            }
        }

        public static ShortestPathOutput DijkstraArray(Mesh mesh, int src, int target, bool earlyterminate = false)
        {
            var graph = ConstructGraphFromMesh(mesh);

            _watch.Reset();
            _watch.Start();
            int n = graph.Count;
            float[] shortestDists = new float[n];
            bool[] added = new bool[n];

            if (src >= n || src < 0 || target >= n || target < 0)
            {
                return new ShortestPathOutput(eShortestPathMethod.Array, -1, null, 0);
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

            var path = new List<int>
            {
                target
            };
            int k = parents[target];
            while (k != -1)
            {
                path.Add(k);
                k = parents[k];
            }

            _watch.Stop();
            Logger.Log($"{mesh.Name}: Dijkstra Array -> Source: {src}, Target: {target} Duration: {_watch.ElapsedMilliseconds} ms. Max distance -> {shortestDists[target]}");


            return new ShortestPathOutput(eShortestPathMethod.Array, shortestDists[target], path, _watch.ElapsedMilliseconds);
        }

        public static ShortestPathOutput DijkstraFibonacciHeap(Mesh mesh, int src, int target, bool earlyTerminate = false)
        {
            var graph = new Graph(mesh);
            _watch.Reset();
            _watch.Start();

            var que = new FibonacciHeap<HeapNode, float>(PriorityQueueType.Minimum);
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

                if (earlyTerminate && u.id == target)
                {
                    break;
                }

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

            var path = new List<int>();
            int k = target;
            path.Add(target);
            while (k != src)
            {
                //k = previousMap[k].Data;
                k = nodeMap[k].Item.PrevId;
                path.Add(k);
            }

            _watch.Stop();
            Logger.Log($"{mesh.Name}: Dijkstra Fibonacci Heap -> Source: {src}, Target: {target} Duration: {_watch.ElapsedMilliseconds} ms. Max distance -> {nodeMap[target].Item.Priority}");

            return new ShortestPathOutput(eShortestPathMethod.Fibonacci, nodeMap[target].Item.Priority, path, _watch.ElapsedMilliseconds);
        }

        public static ShortestPathOutput DijkstraMinHeap(Mesh mesh, int src, int target, bool earlyTerminate = false)
        {
            var graph = new Graph(mesh);

            _watch.Reset();
            _watch.Start();

            var que = new BinaryHeap<HeapNode, float>(PriorityQueueType.Minimum);
            var nodeMap = new Dictionary<int, IPriorityQueueEntry<HeapNode>>();

            for (int i = 0; i < graph.Nodes.Count; i++)
            {
                var dist = i == src ? 0 : float.MaxValue;
                IPriorityQueueEntry<HeapNode> output = que.Enqueue(new HeapNode(i, graph.Nodes[i].Neighbors, dist), dist);
                nodeMap[i] = output;
            }

            while (que.Count > 0)
            {
                var u = que.Dequeue();

                if (earlyTerminate && u.id == target)
                {
                    break;
                }

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

            var path = new List<int>();
            int k = target;
            path.Add(k);
            while (k != src)
            {
                k = nodeMap[k].Item.PrevId;
                path.Add(k);
            }

            _watch.Stop();

            Logger.Log($"{mesh.Name}: Dijkstra Min Heap -> Source: {src}, Target: {target} Duration: {_watch.ElapsedMilliseconds} ms. Max distance -> {nodeMap[target].Item.Priority}");

            return new ShortestPathOutput(eShortestPathMethod.MinHeap, nodeMap[target].Item.Priority, path, _watch.ElapsedMilliseconds);
        }

        public static ShortestPathOutput AStarMinHeap(Mesh mesh, int src, int target)
        {
            var graph = new Graph(mesh);
            _watch.Reset();
            _watch.Start();
            var que = new BinaryHeap<HeapNode, float>(PriorityQueueType.Minimum);
            var nodeMap = new Dictionary<int, IPriorityQueueEntry<HeapNode>>();

            for (int i = 0; i < graph.Nodes.Count; i++)
            {
                var dist = i == src ? (graph.Nodes[src].Coord - graph.Nodes[target].Coord).Length : float.MaxValue;

                IPriorityQueueEntry<HeapNode> output = que.Enqueue(new HeapNode(i, graph.Nodes[i].Neighbors, dist), dist);
                nodeMap[i] = output;
            }

            while (que.Count > 0)
            {
                var u = que.Dequeue();

                if (u.id == target)
                {
                    break;
                }

                foreach (var neighbor in graph.Nodes[u.id].Neighbors)
                {
                    var euclidDist = (graph.Nodes[neighbor.Key].Coord - graph.Nodes[target].Coord).Length;
                    float val = nodeMap[u.id].Item.Priority + neighbor.Value + euclidDist;

                    if (val < nodeMap[neighbor.Key].Item.Priority)
                    {
                        nodeMap[neighbor.Key].Item.Priority = val - euclidDist;
                        que.UpdatePriority(nodeMap[neighbor.Key], val - euclidDist);

                        nodeMap[neighbor.Key].Item.PrevId = u.id;
                    }
                }

            }

            var path = new List<int>();
            int k = target;
            var targetDistance = 0.0f;
            path.Add(k);
            while (k != src)
            {
                targetDistance += (graph.Nodes[k].Coord - graph.Nodes[nodeMap[k].Item.PrevId].Coord).Length;
                k = nodeMap[k].Item.PrevId;

                path.Add(k);
            }
            _watch.Stop();

            Logger.Log($"{mesh.Name}: A* Min Heap -> Source: {src}, Target: {target} Duration: {_watch.ElapsedMilliseconds} ms. Max distance -> {targetDistance}");

            return new ShortestPathOutput(eShortestPathMethod.Astar, targetDistance, path, _watch.ElapsedMilliseconds);
        }

        public static SampleOutput FarthestPointSampling(Graph graph, int sampleCount, int startIndex, Action<int> updateProgress)
        {
            _watch.Reset();
            _watch.Start();
            var distances = DijkstraMinHeap(graph, startIndex);
            var allDistances = new HashSet<float[]>
            {
                distances
            };

            List<GraphNode> farthestPoints = new List<GraphNode>();
            List<int> farthestIndices = new List<int>();

            for (int i = 0; i < sampleCount; i++)
            {
                var storage = new BinaryHeap<HeapNode, float>(PriorityQueueType.Minimum);

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
                    storage.Enqueue(new HeapNode(node.Id, node.Neighbors, -minDist), -minDist);
                }


                var u = storage.Dequeue();
                if (i == 0)
                {
                    allDistances.Clear();
                }
                allDistances.Add(DijkstraMinHeap(graph, u.id));

                farthestPoints.Add(graph.Nodes[u.id]);
                farthestIndices.Add(graph.Nodes[u.id].Id);
                updateProgress((int)(100 * ((float)i / (float)sampleCount)));
            }
            _watch.Stop();
            updateProgress(100);

            Logger.Log($"Farthest point sample -> Duration: {_watch.ElapsedMilliseconds} ms. SampleCount -> {farthestIndices.Count} ");
            if (farthestIndices.Count > 1)
            {
                Logger.Log($"Farthest 2 Points -> Duration: {farthestIndices[0]} {farthestIndices[1]} ");
            }

            return new SampleOutput(farthestPoints, farthestIndices, _watch.ElapsedMilliseconds);
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

        public static AverageGeodesicOutput AverageGeodesicDistance(Graph graph, int sampleCount, int startIndex, Action<int> updateProgress)
        {
            _watch.Reset();
            _watch.Start();

            float[] distances = new float[graph.Nodes.Count];

            var samples = FarthestPointSampling(graph, sampleCount, startIndex, updateProgress);

            for (int i = 0; i < samples.SampleIndices.Count; i++)
            {
                var dist = DijkstraMinHeap(graph, samples.SampleIndices[i]);
                for (int j = 0; j < dist.Length; j++)
                {
                    distances[j] += (dist[j] / samples.SamplePoints.Count);
                }
                updateProgress((int)(100 * ((float)i / (float)samples.SampleIndices.Count)));
            }

            _watch.Stop();

            Logger.Log($"Average geodesic distance -> Duration: {_watch.ElapsedMilliseconds} ms");

            return new AverageGeodesicOutput(distances, _watch.ElapsedMilliseconds);
        }

        public static IsoCurveOutput IsoCurveSignature(Mesh mesh, int source, int sampleCount, Action<int> updateProgress)
        {
            _watch.Reset();
            _watch.Start();

            Graph graph = new Graph(mesh);
            var distances = DijkstraMinHeap(graph, source);

            /*

            Dx = { lx(r1), lx(r2),lx(r2),lx(r2),...,lx(rn)} ,      r1 < r2 < r3 < ... rn

                  p1/  
              v1---/--v2
               \  /   /   
                \/   /    lxT(ri) = |p1 - p2| , where
              p2/\  /     pj = (1 - αj) * v0 + αj * vj,
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

                    updateProgress((int)(100 * ((float)(i * mesh.Triangles.Count + j) / (float)(mesh.Triangles.Count * k))));
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
            updateProgress(100);
            _watch.Stop();

            Logger.Log($"Iso-Curve Signature -> Duration: {_watch.ElapsedMilliseconds} ms for {k} samples");


            return new IsoCurveOutput(isoCurves, isoCurveDistances, source, _watch.ElapsedMilliseconds);
        }

        public static GeodesicMatrixOutput CreateGeodesicDistanceMatrix(Mesh mesh, Action<int> progress, bool isParallel = true)
        {
            var graph = new Graph(mesh);
            _watch.Reset();
            _watch.Start();

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
                        matrix[i] = DijkstraMinHeap(graph, i);
                        progress((int)(100.0f * Interlocked.Increment(ref count) / (n)));
                    }
                );

            }
            else
            {
                for (int i = 0; i < n; i++)
                {
                    matrix[i] = DijkstraMinHeap(graph, i);
                    progress((int)(100.0f * i / (n)));

                }
            }
            _watch.Stop();
            progress(100);

            Logger.Log($"Geodesic Distance Matrix -> Duration: {_watch.ElapsedMilliseconds} ms");


            return new GeodesicMatrixOutput(matrix, _watch.ElapsedMilliseconds);
        }


        internal static float[] DijkstraFibonacciHeap(Mesh mesh, int src)
        {
            var graph = new Graph(mesh);
            return DijkstraFibonacciHeap(graph, src);
        }

        internal static float[] DijkstraMinHeap(Mesh mesh, int src)
        {
            var graph = new Graph(mesh);
            return DijkstraMinHeap(graph, src);
        }

        #endregion

        #region Private Methods

        private static List<List<KeyValuePair<int, float>>> ConstructGraphFromMesh(Mesh mesh)
        {
            var graph = new Graph(mesh);

            var retVal = new List<List<KeyValuePair<int, float>>>();

            for (int i = 0; i < graph.Nodes.Count; i++)
            {
                retVal.Add(graph.Nodes[i].Neighbors);
            }

            return retVal;

        }

        private static float[] DijkstraFibonacciHeap(Graph graph, int src)
        {
            var heap = new FibonacciHeap<HeapNode, float>(PriorityQueueType.Minimum);
            var nodeMap = new Dictionary<int, IPriorityQueueEntry<HeapNode>>();

            for (int i = 0; i < graph.Nodes.Count; i++)
            {
                var dist = i == src ? 0 : float.MaxValue;

                var n = new HeapNode(i, graph.Nodes[i].Neighbors, dist);
                IPriorityQueueEntry<HeapNode> output = heap.Enqueue(n, dist);
                nodeMap[i] = output;
            }

            while (heap.Count > 0)
            {
                var u = heap.Dequeue();

                foreach (var neighbor in graph.Nodes[u.id].Neighbors)
                {
                    float val = nodeMap[u.id].Item.Priority + neighbor.Value;
                    if (val < nodeMap[neighbor.Key].Item.Priority)
                    {
                        nodeMap[neighbor.Key].Item.Priority = val;
                        heap.UpdatePriority(nodeMap[neighbor.Key], val);
                        nodeMap[neighbor.Key].Item.PrevId = u.id;
                    }
                }

            }
            var retVal = new float[nodeMap.Count];
            for (int i = 0; i < retVal.Length; i++)
            {
                retVal[i] = nodeMap[i].Item.Priority;
            }

            return retVal;
        }

        private static float[] DijkstraMinHeap(Graph graph, int src)
        {
            var que = new PriorityQueues.BinaryHeap<HeapNode, float>(PriorityQueueType.Minimum);
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
            var retVal = new float[nodeMap.Count];
            for (int i = 0; i < retVal.Length; i++)
            {
                retVal[i] = nodeMap[i].Item.Priority;
            }

            return retVal;
        }

        private static void RecursivelyAddNeighbor(Vertex source, ref Dictionary<int,Vertex> candidates, ref Dictionary<int,Vertex> boundary)
        {
            foreach (var neighbor in source.Verts)
            {
                if (candidates.TryGetValue(neighbor, out Vertex result))
                {
                    if (!boundary.ContainsKey(result.Id))
                    {
                        boundary.Add(result.Id, result);
                        candidates.Remove(result.Id);
                        RecursivelyAddNeighbor(result, ref candidates, ref boundary);
                    }
                }
            }
        }

        private static void FillVectors(ref Vector<float> vectorX, ref Vector<float> vectorY, List<Dictionary<int,Vertex>> allBoundaries, bool fixInternals = false, bool uniformBoundary = true)
        {
            if (allBoundaries.Count > 0)
            {

                var disc = MakeDiscTopology(allBoundaries[0], uniformBoundary); //only make outmost boundary disk.

                for (int i = 0; i < vectorX.Count; i++)
                {
                    int boundaryIndex = -1;

                    for (int j = 0; j < allBoundaries.Count; j++)
                    {
                        if (allBoundaries[j].ContainsKey(i))
                        {
                            boundaryIndex = j;
                            break;
                        }
                    }

                    if (boundaryIndex == 0)
                    {
                        vectorX[i] = disc[i].X;
                        vectorY[i] = disc[i].Y;
                    }
                    else
                    if (boundaryIndex >= 0)
                    {
                        if (fixInternals)
                        {
                            vectorX[i] = allBoundaries[boundaryIndex][i].Coord.X;
                            vectorY[i] = allBoundaries[boundaryIndex][i].Coord.Z;
                            continue;
                        }
                        vectorX[i] = 0;
                        vectorY[i] = 0;
                    }
                    else
                    {
                        vectorX[i] = 0;
                        vectorY[i] = 0;
                    }
                }
            }
            
        }

        private static void FillMatrix(ref Matrix<float> matrix, Mesh mesh, List<Dictionary<int,Vertex>> allBoundaries, eParameterizationMethod method, float weight, bool fixInternals = false)
        {
            for (int i = 0; i < mesh.Vertices.Count; i++)
            {
                bool isBoundary = false;
                int boundaryIndex = 0;

                for (int j = 0; j < allBoundaries.Count; j++)
                {
                    if (allBoundaries[j].ContainsKey(i))
                    {
                        isBoundary = true;
                        boundaryIndex = j;
                        break;
                    }
                }

                if (isBoundary && boundaryIndex == 0)
                {
                    matrix[i, i] = 1;
                }
                else
                {
                    if (isBoundary && fixInternals)
                    {
                        matrix[i, i] = 1;
                    }
                    else
                    {
                        for (int j = 0; j < mesh.Vertices[i].Verts.Count; j++)
                        {
                            CalculateWeight(mesh, method, i, mesh.Vertices[i].Verts[j], ref weight);
                            matrix[i, mesh.Vertices[i].Verts[j]] = weight;
                            matrix[i, i] -= weight;
                        }
                    }
                }
            }
        }

        private static Dictionary<int, Vector2> MakeDiscTopology(Dictionary<int,Vertex> vertices, bool uniformBoundary)
        {
            Dictionary<int, Vector2> DiskPoints = new Dictionary<int, Vector2>();

            var box = Box3.CalculateBoundingBox(vertices);
            box.Top = 0;
            box.Bottom = 0;

            if (uniformBoundary)
            {
                var angle = MathHelper.TwoPi / vertices.Count;
                int i = 0;
                foreach (var item in vertices)
                {
                    DiskPoints.Add(item.Key, new Vector2(box.Center.X, box.Center.Z) + new Vector2((float)Math.Cos(angle * i), (float)Math.Sin(angle * i)) * box.Size);
                    i++;
                }
            }
            else
            {
                foreach (var item in vertices)
                {
                    var v = RayCaster.Cast(box.Center, new Vector3(item.Value.Coord.X, 0, item.Value.Coord.Z), box.Size, box.Size);
                    DiskPoints.Add(item.Key, new Vector2(v.X, v.Z));
                }
            }

            return DiskPoints;
        }

        private static void CalculateWeight(Mesh mesh, eParameterizationMethod method, int i, int j, ref float weight)
        {
            if (method == eParameterizationMethod.Uniform)
            {
                return;
            }

            var vi = mesh.Vertices[i];
            var vj = mesh.Vertices[j];

            var tris = new List<Triangle>();
            foreach (var triId in vi.Tris)
            {
                var tri = mesh.Triangles[triId];
                if (tri.V1 == j || tri.V2 == j || tri.V3 == j)
                {
                    tris.Add(tri);
                }
            }

            weight = 0.0f;
            if (method == eParameterizationMethod.Harmonic)
            {
                foreach (var tri in tris)
                {
                    var third = tri.GetThirdVertexId(i, j);
                    var angle = mesh.GetTriangleAngle(tri.Id, third);
                    weight += (float)(1.0 / Math.Tan(angle));
                }
                weight *= 0.5f;
            }
            else if(method == eParameterizationMethod.MeanValue)
            {
                foreach (var tri in tris)
                {
                    var angle = mesh.GetTriangleAngle(tri.Id, i);
                    weight += (float)Math.Tan(angle * 0.5f);
                }
                weight /= (2.0f * Vector3.Distance(vi.Coord, vj.Coord));
            }
        }

        #endregion

        public static void RecursivelyFindAllBoundaries(List<Vertex> candidates, ref List<Dictionary<int,Vertex>> totalBoundaries)
        {

            var candidateItems = candidates.Select(x => x).ToDictionary(x=>x.Id);

            while (candidateItems.Count > 0)
            {
                var itemList = new List<Vertex>();
                var boundary = new Dictionary<int, Vertex>();

                var first = candidateItems.First();
                boundary.Add(first.Key, first.Value);
                candidateItems.Remove(first.Key);
                RecursivelyAddNeighbor(first.Value, ref candidateItems, ref boundary);

                totalBoundaries.Add(boundary);
            }
            totalBoundaries = totalBoundaries.OrderByDescending(x => Box3.CalculateBoundingBox(x).Volume).ToList();
        }

        public static DiscParameterizeOutput ParameterizeMeshToDisc(Mesh mesh, eParameterizationMethod method, Action<int> updateProgress, float weight = 0.5f, bool fixInternals = false, bool uniformBoundary = true)
        {
            var allBoundaries = new List<Dictionary<int, Vertex>>();
            var boundaryVertices = mesh.GetBoundaryVertices();

            boundaryVertices = boundaryVertices.OrderByDescending(x => x.Coord.X).ToList();




            List<int> path = new List<int>();
            if (boundaryVertices.Count == 0)
            {
                string extra = "";
                if (!uniformBoundary)
                {
                    extra = "Uniformly distributing boundaries.";
                    uniformBoundary = true;
                }
                Logger.Log($"No holes detected, cutting {mesh.Name}. {extra}");
                var cutOutput = CutMesh(mesh);
                allBoundaries = cutOutput.boundary;
                mesh = cutOutput.Cutmesh;
                path = cutOutput.ShortestPath.Path;
            }
            else
            {
                RecursivelyFindAllBoundaries(boundaryVertices, ref allBoundaries);
                path = allBoundaries[0].Select(x=>x.Key).ToList();
            }


            updateProgress(20);

            MathNet.Numerics.Control.UseNativeOpenBLAS();
            Matrix<float> W = Matrix<float>.Build.Dense(mesh.Vertices.Count, mesh.Vertices.Count, 0.0f);
            Vector<float> Bx = Vector<float>.Build.Dense(mesh.Vertices.Count, 0.0f);
            Vector<float> By = Vector<float>.Build.Dense(mesh.Vertices.Count, 0.0f);


            FillMatrix(ref W, mesh, allBoundaries, method, weight, fixInternals);
            FillVectors(ref Bx, ref By, allBoundaries, fixInternals, uniformBoundary);

            _watch.Reset();
            _watch.Start();


            var Xx = W.Solve(Bx);
            var Xy = W.Solve(By);

            updateProgress(80);

            _watch.Stop();
            Logger.Log($"Matrix of size {W.ColumnCount} x {W.ColumnCount} Solved in: {_watch.ElapsedMilliseconds} ms");

            var list = new List<Vector2>();
            for (int i = 0; i < Xx.Count; i++)
            {
                list.Add(new Vector2(Xx[i], Xy[i]));
            }
            updateProgress(100);

            return new DiscParameterizeOutput(list, mesh, path);
        }




        public static SphereParameterizeOutput ParameterizeMeshToSphere(Mesh mesh, int iterationCount, Action<int> updateProgress)
        {
            _watch.Reset();
            _watch.Start();

            var box = Box3.CalculateBoundingBox(mesh.Vertices);

            var sample = FarthestPointSampling(new Graph(mesh), 2, 0, (a)=>{ });

            var path = DijkstraMinHeap(mesh, sample.SampleIndices[0], sample.SampleIndices[1]);

            var midId = path.Path[path.Path.Count / 2];
            var v = mesh.Vertices[midId];


            var meshCenter = v.Coord - v.Normal * box.Size  / 128;


            //meshCenter = mesh.Center();

            

            List<Vector3> spherePoints = new List<Vector3>();
            List<Vector3> normals = new List<Vector3>();



            for (int i = 0; i < mesh.Vertices.Count; i++)
            {
                var normal = (mesh.Vertices[i].Coord - meshCenter).Normalized();
                spherePoints.Add(normal);
                normals.Add(normal);
            }


            for (int i = 0; i < iterationCount; i++)
            {
                for (int j = 0; j < spherePoints.Count; j++)
                {
                    var neighbors = mesh.Vertices[j].Verts;

                    Vector3 newCenter = Vector3.Zero;
                    for (int k = 0; k < neighbors.Count; k++)
                    {
                        newCenter += spherePoints[neighbors[k]];
                    }
                    newCenter /= neighbors.Count;

                    spherePoints[j] = newCenter.Normalized();
                    normals[j] = (spherePoints[j]).Normalized();
                }
                updateProgress((int)(100 * (float)(i) / iterationCount));
            }
            updateProgress(100);


            _watch.Stop();
            Logger.Log($"Sphere Parametrization completed in: {_watch.ElapsedMilliseconds} ms -> iteration Count = {iterationCount}");

            return new SphereParameterizeOutput(spherePoints, normals, new List<Vector3> {  meshCenter });

        }

        public static SphereParameterizeOutput ParameterizeMeshToSphereOld(Mesh mesh, int iterationCount, Action<int> updateProgress)
        {
            _watch.Reset();
            _watch.Start();

            var meshCenter = mesh.Center();

            List<Vector3> vertices = mesh.Vertices.Select(x => x.Coord).ToList();

            for (int j = 0; j < mesh.Vertices.Count; j++)
            {
                var neighbors = mesh.Vertices[j].Verts;

                Vector3 newCenter = Vector3.Zero;
                for (int k = 0; k < neighbors.Count; k++)
                {
                    newCenter += vertices[neighbors[k]];
                }
                newCenter /= neighbors.Count;

                vertices[j] = newCenter;
            }

            List<Vector3> spherePoints = new List<Vector3>();
            List<Vector3> normals = new List<Vector3>();
            for (int i = 0; i < vertices.Count; i++)
            {
                var normal = (vertices[i] - meshCenter).Normalized();
                spherePoints.Add(RayCaster.Cast(meshCenter, normal, 0.5f, 0.5f).Normalized());
                normals.Add(normal);
            }

            for (int i = 0; i < iterationCount; i++)
            {
                for (int j = 0; j < spherePoints.Count; j++)
                {
                    var neighbors = mesh.Vertices[j].Verts;

                    Vector3 newCenter = Vector3.Zero;
                    for (int k = 0; k < neighbors.Count; k++)
                    {
                        newCenter += spherePoints[neighbors[k]];
                    }
                    newCenter /= neighbors.Count;

                    spherePoints[j] = newCenter.Normalized();
                    normals[j] = (spherePoints[j] - meshCenter).Normalized();
                }
                updateProgress((int)(100 * (float)(i) / iterationCount));
            }
            updateProgress(100);


            _watch.Stop();
            Logger.Log($"Sphere Parametrization completed in: {_watch.ElapsedMilliseconds} ms -> iteration Count = {iterationCount}");

            return new SphereParameterizeOutput(spherePoints, normals, new List<Vector3>{ meshCenter });

        }


        public static SphereParameterizeOutput SphereTest(Mesh mesh, int iterationCount)
        {
            _watch.Reset();
            _watch.Start();

            var meshCenter = mesh.Center();

            var box = Box3.CalculateBoundingBox(mesh.Vertices);
            List<Vector3> spherePoints = new List<Vector3>();
            List<Vector3> normals = new List<Vector3>();
            List<Vector3> centerList = new List<Vector3>();

            var radius = box.LongestDimension * 0.5f;
            
            var dist = float.MinValue;
            for (int i = 0; i < mesh.Vertices.Count; i++)
            {
                dist = (mesh.Vertices[i].Coord - meshCenter).Length > dist ? (mesh.Vertices[i].Coord - meshCenter).Length : dist;
            }

            for (int j = 0; j < iterationCount; j++)
            {
                spherePoints.Clear();
                normals.Clear();


                for (int i = 0; i < mesh.Vertices.Count; i++)
                {
                    var normal = (mesh.Vertices[i].Coord - meshCenter).Normalized();
                    spherePoints.Add((meshCenter + normal * dist));
                    normals.Add(normal);
                }

                if (j < iterationCount - 1)
                {
                    var sphereCenter = Vector3.Zero;
                    var weight = 0;
                    for (int i = 0; i < spherePoints.Count; i++)
                    {
                        sphereCenter += spherePoints[i];
                        weight += mesh.Vertices[i].Verts.Count;
                    }
                    sphereCenter /= spherePoints.Count;

                    //meshCenter = (sphereCenter - meshCenter) / radius;

                    meshCenter = sphereCenter;
                    centerList.Add(meshCenter);
                }


            }


            _watch.Stop();
            Logger.Log($"Sphere Parametrization completed in: {_watch.ElapsedMilliseconds} ms -> iteration Count = {iterationCount}");

            return new SphereParameterizeOutput(spherePoints, normals, centerList );
        }

        public static CutSeamParameterizeOutput ParameterizeMeshCutSeam3(Mesh mesh, Action<int> updateProgress)
        {
            //TODO: this doesnt do any process regarding edges.
            Graph g = new Graph(mesh);

            var sample = FarthestPointSampling(g, 2, 0, (a) => { });
            var sp = DijkstraMinHeap(mesh, sample.SampleIndices[0], sample.SampleIndices[1], true);

            var boundaryVertices = new HashSet<Vertex>();

            var vertices = mesh.Vertices.Where(x => sp.Path.Contains(x.Id)).ToList();

            var cutMesh = mesh.Copy();

            for (int i = 1; i < sp.Path.Count - 1; i++)
            {
                var newVertex = mesh.Vertices[sp.Path[i]];
                newVertex.Id = cutMesh.Vertices.Count;
                cutMesh.AddVertex(newVertex);
            }

            for (int i = 1; i < sp.Path.Count - 1; i++)
            {
                var vi = cutMesh.Vertices[sp.Path[i]];
                var vj = cutMesh.Vertices[mesh.Vertices.Count + i - 1];
                var v1 = mesh.Vertices[sp.Path[i - 1]];

                vi.Verts = new List<int>(vi.Verts);
                vi.Tris = new List<int>(vi.Tris);

                // This is problematish
                vj.Verts = new List<int>(); //{ v1.Id, sp.Path[i + 1] };
                vj.Tris = new List<int>();

                var commonTris = vi.Tris.Intersect(v1.Tris).ToList();
                var tri = mesh.Triangles[commonTris[0]];
                int triIndex = tri.Id;
                if (i != 1)
                {
                    for (int j = 0; j < commonTris.Count; j++)
                    {
                        if (cutMesh.Triangles[commonTris[j]].ContainsId(mesh.Vertices.Count + i - 2))
                        {
                            tri = mesh.Triangles[commonTris[j]];
                            triIndex = tri.Id;
                            break;
                        }
                    }
                }
                var i3rd = tri.GetThirdVertexId(v1.Id, vi.Id);

                vj.Tris.Add(tri.Id);
                vi.Tris.Remove(tri.Id);

                tri.UpdateIndex(vi.Id, vj.Id);
                cutMesh.Triangles[triIndex] = tri;

                while (i3rd != sp.Path[i + 1])
                {
                    vi.Verts.Remove(i3rd);
                    vj.Verts.Add(i3rd);

                    cutMesh.Vertices[i3rd].Verts.Remove(vi.Id);
                    cutMesh.Vertices[i3rd].Verts.Add(vj.Id);

                    commonTris = vi.Tris.Intersect(mesh.Vertices[i3rd].Tris).ToList();
                    commonTris.Remove(tri.Id);
                    var next = commonTris[0];

                    tri = cutMesh.Triangles[next];
                    i3rd = tri.GetThirdVertexId(vi.Id, i3rd);
                    

                    vi.Tris.Remove(next);
                    vj.Tris.Add(next);
                }

                for (int j = 0; j < vj.Tris.Count; j++)
                {
                    tri = cutMesh.Triangles[vj.Tris[j]];
                    tri.UpdateIndex(vi.Id, vj.Id);
                    cutMesh.Triangles[vj.Tris[j]] = tri;
                }

                cutMesh.Vertices[vi.Id] = vi;
                cutMesh.Vertices[vj.Id] = vj;

            }
            cutMesh.Vertices[sp.Path.Last()].Verts.Add(cutMesh.Vertices[cutMesh.Vertices.Count - 1].Id);
            cutMesh.Vertices[sp.Path[0]].Verts.Add(mesh.Vertices.Count);

            for (int i = 1; i < sp.Path.Count - 1; i++)
            {
                var v = cutMesh.Vertices[mesh.Vertices.Count + i - 1];

                if (i == 1)
                {
                    v.Verts.Add(sp.Path[0]);
                    v.Verts.Add(cutMesh.Vertices[mesh.Vertices.Count + i].Id);
                }
                else if (i == sp.Path.Count - 2)
                {
                    v.Verts.Add(cutMesh.Vertices[mesh.Vertices.Count + i - 2].Id);
                    v.Verts.Add(sp.Path.Last());
                }
                else
                {
                    v.Verts.Add(cutMesh.Vertices[mesh.Vertices.Count + i - 2].Id);
                    v.Verts.Add(cutMesh.Vertices[mesh.Vertices.Count + i].Id);
                }

                foreach (var item in v.Tris)
                {
                    var tri = cutMesh.Triangles[item];
                    tri.UpdateIndex(sp.Path[i], mesh.Vertices.Count + i - 1);

                    cutMesh.Triangles[item] = tri;
                }

                cutMesh.Vertices[mesh.Vertices.Count + i - 1] = v;
            }

            for (int i = 0; i < sp.Path.Count; i++)
            {
                boundaryVertices.Add(cutMesh.Vertices[sp.Path[i]]);
            }

            for (int i = cutMesh.Vertices.Count - 1; i > mesh.Vertices.Count - 1; i--)
            {
                boundaryVertices.Add(cutMesh.Vertices[i]);
            }

            var allBoundaries = new List<Dictionary<int, Vertex>>
            {
                boundaryVertices.ToDictionary(x => x.Id)
            };


            var discOutput = ParameterizeMeshToDisc(cutMesh, eParameterizationMethod.Harmonic, updateProgress);


            return new CutSeamParameterizeOutput(cutMesh, discOutput, sp, boundaryVertices);
        }

        public static CutSeamParameterizeOutput ParameterizeMeshCutSeam(Mesh mesh, Action<int> updateProgress)
        {
            //TODO: this doesnt do any process regarding edges.
            Graph g = new Graph(mesh);

            var sample = FarthestPointSampling(g, 2, 0, (a) => { });
            var sp = DijkstraMinHeap(mesh, sample.SampleIndices[0], sample.SampleIndices[1], true);

            var boundaryVertices = new HashSet<Vertex>();

            var vertices = mesh.Vertices.Where(x => sp.Path.Contains(x.Id)).ToList();

            var cutMesh = mesh.Copy();

            for (int i = 1; i < sp.Path.Count - 1; i++)
            {
                var newVertex = mesh.Vertices[sp.Path[i]];
                newVertex.Id = cutMesh.Vertices.Count;
                cutMesh.AddVertex(newVertex);
            }

            for (int i = 1; i < sp.Path.Count - 1; i++)
            {
                var vi = cutMesh.Vertices[sp.Path[i]];
                var vj = cutMesh.Vertices[mesh.Vertices.Count + i - 1];
                var v1 = mesh.Vertices[sp.Path[i - 1]];

                vi.Verts = new List<int>(vi.Verts);
                vi.Tris = new List<int>(vi.Tris);

                // This is problematish
                vj.Verts = new List<int>(); //{ v1.Id, sp.Path[i + 1] };
                vj.Tris = new List<int>();

                var commonTris = vi.Tris.Intersect(v1.Tris).ToList();
                var tri = mesh.Triangles[commonTris[0]];
                int triIndex = tri.Id;
                if (i != 1)
                {
                    for (int j = 0; j < commonTris.Count; j++)
                    {
                        if (cutMesh.Triangles[commonTris[j]].ContainsId(mesh.Vertices.Count + i - 2))
                        {
                            tri = mesh.Triangles[commonTris[j]];
                            triIndex = tri.Id;
                            break;
                        }
                    }
                }
                var i3rd = tri.GetThirdVertexId(v1.Id, vi.Id);

                vj.Tris.Add(tri.Id);
                vi.Tris.Remove(tri.Id);

                tri.UpdateIndex(vi.Id, vj.Id);
                cutMesh.Triangles[triIndex] = tri;

                while (i3rd != sp.Path[i + 1])
                {
                    vi.Verts.Remove(i3rd);
                    vj.Verts.Add(i3rd);

                    cutMesh.Vertices[i3rd].Verts.Remove(vi.Id);
                    cutMesh.Vertices[i3rd].Verts.Add(vj.Id);

                    commonTris = vi.Tris.Intersect(mesh.Vertices[i3rd].Tris).ToList();
                    commonTris.Remove(tri.Id);
                    var next = commonTris[0];

                    tri = cutMesh.Triangles[next];
                    i3rd = tri.GetThirdVertexId(vi.Id, i3rd);


                    vi.Tris.Remove(next);
                    vj.Tris.Add(next);
                }

                for (int j = 0; j < vj.Tris.Count; j++)
                {
                    tri = cutMesh.Triangles[vj.Tris[j]];
                    tri.UpdateIndex(vi.Id, vj.Id);
                    cutMesh.Triangles[vj.Tris[j]] = tri;
                }

                cutMesh.Vertices[vi.Id] = vi;
                cutMesh.Vertices[vj.Id] = vj;

            }
            cutMesh.Vertices[sp.Path.Last()].Verts.Add(cutMesh.Vertices[cutMesh.Vertices.Count - 1].Id);
            cutMesh.Vertices[sp.Path[0]].Verts.Add(mesh.Vertices.Count);

            for (int i = 1; i < sp.Path.Count - 1; i++)
            {
                var v = cutMesh.Vertices[mesh.Vertices.Count + i - 1];

                if (i == 1)
                {
                    v.Verts.Add(sp.Path[0]);
                    v.Verts.Add(cutMesh.Vertices[mesh.Vertices.Count + i].Id);
                }
                else if (i == sp.Path.Count - 2)
                {
                    v.Verts.Add(cutMesh.Vertices[mesh.Vertices.Count + i - 2].Id);
                    v.Verts.Add(sp.Path.Last());
                }
                else
                {
                    v.Verts.Add(cutMesh.Vertices[mesh.Vertices.Count + i - 2].Id);
                    v.Verts.Add(cutMesh.Vertices[mesh.Vertices.Count + i].Id);
                }

                foreach (var item in v.Tris)
                {
                    var tri = cutMesh.Triangles[item];
                    tri.UpdateIndex(sp.Path[i], mesh.Vertices.Count + i - 1);

                    cutMesh.Triangles[item] = tri;
                }

                cutMesh.Vertices[mesh.Vertices.Count + i - 1] = v;
            }

            for (int i = 0; i < sp.Path.Count; i++)
            {
                boundaryVertices.Add(cutMesh.Vertices[sp.Path[i]]);
            }

            for (int i = cutMesh.Vertices.Count - 1; i > mesh.Vertices.Count - 1; i--)
            {
                boundaryVertices.Add(cutMesh.Vertices[i]);
            }

            var allBoundaries = new List<Dictionary<int, Vertex>>
            {
                boundaryVertices.ToDictionary(x => x.Id)
            };


            var discOutput = ParameterizeMeshToDisc(cutMesh, eParameterizationMethod.Harmonic, updateProgress);


            return new CutSeamParameterizeOutput(cutMesh, discOutput, sp, boundaryVertices);
        }

        private static CutMeshOutput CutMesh(Mesh mesh)
        {
            Graph g = new Graph(mesh);

            var sample = FarthestPointSampling(g, 2, 0, (a) => { });
            var sp = DijkstraMinHeap(mesh, sample.SampleIndices[0], sample.SampleIndices[1], true);

            var boundaryVertices = new HashSet<Vertex>();

            var vertices = mesh.Vertices.Where(x => sp.Path.Contains(x.Id)).ToList();

            var cutMesh = mesh.Copy();

            for (int i = 1; i < sp.Path.Count - 1; i++)
            {
                var newVertex = mesh.Vertices[sp.Path[i]];
                newVertex.Id = cutMesh.Vertices.Count;
                cutMesh.AddVertex(newVertex);
            }

            for (int i = 1; i < sp.Path.Count - 1; i++)
            {
                var vi = cutMesh.Vertices[sp.Path[i]];
                var vj = cutMesh.Vertices[mesh.Vertices.Count + i - 1];
                var v1 = mesh.Vertices[sp.Path[i - 1]];

                vi.Verts = new List<int>(vi.Verts);
                vi.Tris = new List<int>(vi.Tris);

                // This is problematish
                vj.Verts = new List<int>(); //{ v1.Id, sp.Path[i + 1] };
                vj.Tris = new List<int>();

                var commonTris = vi.Tris.Intersect(v1.Tris).ToList();
                var tri = mesh.Triangles[commonTris[0]];
                int triIndex = tri.Id;
                if (i != 1)
                {
                    for (int j = 0; j < commonTris.Count; j++)
                    {
                        if (cutMesh.Triangles[commonTris[j]].ContainsId(mesh.Vertices.Count + i - 2))
                        {
                            tri = mesh.Triangles[commonTris[j]];
                            triIndex = tri.Id;
                            break;
                        }
                    }
                }
                var i3rd = tri.GetThirdVertexId(v1.Id, vi.Id);

                vj.Tris.Add(tri.Id);
                vi.Tris.Remove(tri.Id);

                tri.UpdateIndex(vi.Id, vj.Id);
                cutMesh.Triangles[triIndex] = tri;

                while (i3rd != sp.Path[i + 1])
                {
                    vi.Verts.Remove(i3rd);
                    vj.Verts.Add(i3rd);

                    cutMesh.Vertices[i3rd].Verts.Remove(vi.Id);
                    cutMesh.Vertices[i3rd].Verts.Add(vj.Id);

                    commonTris = vi.Tris.Intersect(mesh.Vertices[i3rd].Tris).ToList();
                    commonTris.Remove(tri.Id);
                    var next = commonTris[0];

                    tri = cutMesh.Triangles[next];
                    i3rd = tri.GetThirdVertexId(vi.Id, i3rd);


                    vi.Tris.Remove(next);
                    vj.Tris.Add(next);
                }

                for (int j = 0; j < vj.Tris.Count; j++)
                {
                    tri = cutMesh.Triangles[vj.Tris[j]];
                    tri.UpdateIndex(vi.Id, vj.Id);
                    cutMesh.Triangles[vj.Tris[j]] = tri;
                }

                cutMesh.Vertices[vi.Id] = vi;
                cutMesh.Vertices[vj.Id] = vj;

            }
            cutMesh.Vertices[sp.Path.Last()].Verts.Add(cutMesh.Vertices[cutMesh.Vertices.Count - 1].Id);
            cutMesh.Vertices[sp.Path[0]].Verts.Add(mesh.Vertices.Count);

            for (int i = 1; i < sp.Path.Count - 1; i++)
            {
                var v = cutMesh.Vertices[mesh.Vertices.Count + i - 1];

                if (i == 1)
                {
                    v.Verts.Add(sp.Path[0]);
                    v.Verts.Add(cutMesh.Vertices[mesh.Vertices.Count + i].Id);
                }
                else if (i == sp.Path.Count - 2)
                {
                    v.Verts.Add(cutMesh.Vertices[mesh.Vertices.Count + i - 2].Id);
                    v.Verts.Add(sp.Path.Last());
                }
                else
                {
                    v.Verts.Add(cutMesh.Vertices[mesh.Vertices.Count + i - 2].Id);
                    v.Verts.Add(cutMesh.Vertices[mesh.Vertices.Count + i].Id);
                }

                foreach (var item in v.Tris)
                {
                    var tri = cutMesh.Triangles[item];
                    tri.UpdateIndex(sp.Path[i], mesh.Vertices.Count + i - 1);

                    cutMesh.Triangles[item] = tri;
                }

                cutMesh.Vertices[mesh.Vertices.Count + i - 1] = v;
            }

            for (int i = 0; i < sp.Path.Count; i++)
            {
                boundaryVertices.Add(cutMesh.Vertices[sp.Path[i]]);
            }

            for (int i = cutMesh.Vertices.Count - 1; i > mesh.Vertices.Count - 1; i--)
            {
                boundaryVertices.Add(cutMesh.Vertices[i]);
            }

            var allBoundaries = new List<Dictionary<int, Vertex>>
            {
                boundaryVertices.ToDictionary(x => x.Id)
            };


            return new CutMeshOutput(cutMesh, sp, allBoundaries);
        }

        private static void DetectOverlaps(List<Triangle> triangles, List<Vertex> vertices)
        {
            for (int i = 0; i < triangles.Count; i++)
            {
                var v1 = vertices[triangles[i].V1];
                var v2 = vertices[triangles[i].V2];
                var v3 = vertices[triangles[i].V3];
            }
        }
    }


}
