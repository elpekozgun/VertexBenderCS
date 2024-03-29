﻿using Engine.Core;
using MathNet.Numerics.LinearAlgebra;
using OpenTK;
using PriorityQueues;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Engine.Processing
{
    public enum eShortestPathMethod : byte
    {
        Array = 0x1,
        MinHeap = 0x2,
        Fibonacci = 0x4,
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

        #region Shortest Path, Sampling, Descriptors

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

        public static ShortestPathOutput DijkstraMinHeap(Graph graph, int src, int target, bool earlyTerminate = false)
        {
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

            //Logger.Log($"{mesh.Name}: Dijkstra Min Heap -> Source: {src}, Target: {target} Duration: {_watch.ElapsedMilliseconds} ms. Max distance -> {nodeMap[target].Item.Priority}");

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
                Logger.Log($"Farthest 2 Points -> {farthestIndices[0]} {farthestIndices[1]} ");
            }

            return new SampleOutput(farthestPoints, farthestIndices, _watch.ElapsedMilliseconds);
        }

        public static List<float> GaussianCurvature(Mesh mesh)
        {
            List<float> Curvatures = new List<float>();

            foreach (var vertex in mesh.Vertices)
            {
                float curvature = 2.0f * (float)Math.PI;
                foreach (var triId in vertex.Value.Tris)
                {
                    var ang = mesh.GetTriangleAngle(triId, vertex.Key);

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

                foreach (var tri in mesh.Triangles)
                {

                    var v1Id = tri.Value.V1;
                    var v2Id = tri.Value.V2;
                    var v3Id = tri.Value.V3;

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

                    updateProgress((int)(100 * ((float)(i * mesh.Triangles.Count + tri.Key) / (float)(mesh.Triangles.Count * k))));
                }


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

        public static Dictionary<int, float> DijkstraMinHeapKey(Graph graph, int src)
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
            var retVal = new Dictionary<int, float>();
            for (int i = 0; i < nodeMap.Count; i++)
            {
                retVal.Add(i, nodeMap[i].Item.Priority);
            }

            return retVal;
        }

        public static Dictionary<int, float> AverageGeodesicDistance2(Graph graph, int sampleCount, int startIndex, Action<int> updateProgress)
        {
            _watch.Reset();
            _watch.Start();

            Dictionary<int, float> distances = new Dictionary<int, float>();

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

            return distances;


        }


        #endregion

        #region Parameterization

        private static void RecursivelyAddNeighbor(Vertex source, ref Dictionary<int, Vertex> candidates, ref Dictionary<int, Vertex> boundary)
        {
            foreach (var neighbor in source.Verts)
            {
                if (candidates.TryGetValue(neighbor, out Vertex result))
                {
                    if (!boundary.ContainsKey(result.Id))
                    {
                        var commonEdge = source.Edges.Intersect(result.Edges).ToList();

                        boundary.Add(result.Id, result);
                        candidates.Remove(result.Id);
                        RecursivelyAddNeighbor(result, ref candidates, ref boundary);
                    }
                }
            }
        }

        private static void FillVectors(ref Vector<float> vectorX, ref Vector<float> vectorY, List<Dictionary<int, Vertex>> allBoundaries, bool fixInternals = false, bool uniformBoundary = true)
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

        private static void FillMatrix(ref Matrix<float> matrix, Mesh mesh, List<Dictionary<int, Vertex>> allBoundaries, eParameterizationMethod method, float weight, bool fixInternals = false)
        {
            int k = 0;
            foreach (var vertex in mesh.Vertices)
            {
                bool isBoundary = false;
                int boundaryIndex = 0;
                int i = vertex.Key;

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
                    matrix[k, k] = 1;
                }
                else
                {
                    if (isBoundary && fixInternals)
                    {
                        matrix[k, k] = 1;
                    }
                    else
                    {
                        for (int j = 0; j < mesh.Vertices[i].Verts.Count; j++)
                        {
                            CalculateWeight(mesh, method, i, mesh.Vertices[i].Verts[j], ref weight);
                            matrix[k, mesh.Vertices[i].Verts[j]] = weight;
                            matrix[k, k] -= weight;
                        }
                    }
                }
                k++;
            }

        }

        private static Dictionary<int, Vector2> MakeDiscTopology(Dictionary<int, Vertex> vertices, bool uniformBoundary)
        {
            Dictionary<int, Vector2> DiskPoints = new Dictionary<int, Vector2>();

            var box = Box3.CalculateBoundingBox(vertices);
            box.Top = 0;
            box.Bottom = 0;

            var initialItem = vertices.First();
            var initialOffset = Vector3.CalculateAngle
            (
                new Vector3(initialItem.Value.Coord.X - box.Center.X, 0.0f, initialItem.Value.Coord.Z - box.Center.Z),
                Vector3.UnitX
            );


            if (uniformBoundary)
            {
                var angle = MathHelper.TwoPi / vertices.Count;
                int i = 0;
                foreach (var item in vertices)
                {
                    DiskPoints.Add(item.Key, new Vector2(box.Center.X, box.Center.Z) + new Vector2((float)Math.Cos(angle * i + initialOffset), (float)Math.Sin(angle * i + initialOffset)) * box.Size);
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
            else if (method == eParameterizationMethod.MeanValue)
            {
                foreach (var tri in tris)
                {
                    var angle = mesh.GetTriangleAngle(tri.Id, i);
                    weight += (float)Math.Tan(angle * 0.5f);
                }
                weight /= (2.0f * Vector3.Distance(vi.Coord, vj.Coord));
            }
        }

        private static void RecursivelyAddEdge(Edge source, ref Dictionary<int, Edge> candidates, ref Dictionary<int, Edge> boundary)
        {

            var nexts = candidates.Where(x => x.Value.Start == source.End).ToList();
            if (nexts != null && nexts.Count > 0)
            {
                var next = nexts.First().Value;
                if (!boundary.ContainsKey(next.Id))
                {
                    boundary.Add(next.Id, next);
                    candidates.Remove(next.Id);
                    RecursivelyAddEdge(next, ref candidates, ref boundary);
                }
            }
        }

        public static void RecursivelyFindAllBoundaries(Dictionary<int, Vertex> vertices, List<Edge> candidates, ref List<Dictionary<int, Vertex>> totalBoundaries)
        {

            var candidateItems = candidates.Select(x => x).ToDictionary(x => x.Id);

            List<Dictionary<int, Edge>> totalBoundaryEdges = new List<Dictionary<int, Edge>>();

            while (candidateItems.Count > 0)
            {
                var itemList = new List<Edge>();
                var boundary = new Dictionary<int, Edge>();

                var first = candidateItems.First();
                boundary.Add(first.Key, first.Value);
                candidateItems.Remove(first.Key);
                RecursivelyAddEdge(first.Value, ref candidateItems, ref boundary);

                totalBoundaryEdges.Add(boundary);
            }

            foreach (var boundaries in totalBoundaryEdges)
            {
                HashSet<Vertex> boundaryVertices = new HashSet<Vertex>();
                var pairs = boundaries.ToList();

                foreach (var pair in pairs)
                {
                    boundaryVertices.Add(vertices[pair.Value.Start]);
                }

                boundaryVertices.Add(vertices[pairs.Last().Value.End]);

                totalBoundaries.Add(boundaryVertices.Select(x => x).ToDictionary(x => x.Id));
            }


            totalBoundaries = totalBoundaries.OrderByDescending(x => Box3.CalculateBoundingBox(x).Volume).ToList();
        }

        public static DiscParameterizeOutput ParameterizeMeshToDisc(Mesh mesh, eParameterizationMethod method, Action<int> updateProgress, float weight = 0.5f, bool fixInternals = false, bool uniformBoundary = true)
        {
            var allBoundaries = new List<Dictionary<int, Vertex>>();
            var boundaryEdges = mesh.GetBoundaryEdges();

            List<int> path = new List<int>();
            if (boundaryEdges.Count == 0)
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
                RecursivelyFindAllBoundaries(mesh.Vertices, boundaryEdges, ref allBoundaries);
                path = allBoundaries[0].Select(x => x.Key).ToList();
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

        public static SphereParameterizeOutput ParameterizeMeshToSphere(Mesh mesh, int iterationCount, Action<int> updateProgress, bool useMeshCenter = false)
        {
            var box = Box3.CalculateBoundingBox(mesh.Vertices);

            var sample = FarthestPointSampling(new Graph(mesh), 2, 0, updateProgress);
            var path = DijkstraMinHeap(mesh, sample.SampleIndices[0], sample.SampleIndices[1]);

            _watch.Reset();
            _watch.Start();

            var meshCenter = mesh.Center();
            if (!useMeshCenter)
            {
                var midId = path.Path[path.Path.Count / 2];
                var v = mesh.Vertices[midId];
                meshCenter = v.Coord - v.Normal * box.Size / 128;
            }


            List<Vector3> spherePoints = new List<Vector3>();
            List<Vector3> normals = new List<Vector3>();


            foreach (var vertex in mesh.Vertices)
            {
                var normal = (vertex.Value.Coord - meshCenter).Normalized();
                spherePoints.Add(normal * box.Size);
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

                    spherePoints[j] = newCenter.Normalized() * box.Size;
                    normals[j] = (spherePoints[j]).Normalized();
                }
                updateProgress((int)(100 * (float)(i) / iterationCount));
            }
            updateProgress(100);


            _watch.Stop();
            Logger.Log($"Sphere Parametrization completed in: {_watch.ElapsedMilliseconds} ms -> iteration Count = {iterationCount}");

            return new SphereParameterizeOutput(spherePoints, normals, new List<Vector3> { meshCenter });

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

            foreach (var vertex in mesh.Vertices)
            {
                var normal = (vertex.Value.Coord - meshCenter).Normalized();
                spherePoints.Add(meshCenter + normal * box.Size);
                normals.Add(normal);
            }

            for (int j = 0; j < iterationCount; j++)
            {

                var sphereCenter = Vector3.Zero;
                var totalTriArea = 0.0f;

                for (int i = 0; i < spherePoints.Count; i++)
                {
                    var weight = 0.0f;
                    foreach (var triId in mesh.Vertices[i].Tris)
                    {
                        var tri = mesh.Triangles[triId];
                        var triArea = mesh.TriangleArea(spherePoints[tri.V1], spherePoints[tri.V2], spherePoints[tri.V3]);
                        //var triArea = mesh.TriangleArea(tri);


                        weight += triArea;
                    }
                    totalTriArea += weight;
                    sphereCenter += (spherePoints[i] * weight);
                }
                meshCenter = sphereCenter / (totalTriArea);
                centerList.Add(meshCenter);

                for (int i = 0; i < spherePoints.Count; i++)
                {
                    var normal = (spherePoints[i] - meshCenter).Normalized();
                    spherePoints[i] = (meshCenter + normal * box.Size);
                    normals.Add(normal);
                }

            }



            _watch.Stop();
            Logger.Log($"Sphere Parametrization completed in: {_watch.ElapsedMilliseconds} ms -> iteration Count = {iterationCount}");

            return new SphereParameterizeOutput(spherePoints, normals, centerList);
        }


        // Revisit this..
        private static CutMeshOutput CutMesh(Mesh mesh)
        {
            Graph g = new Graph(mesh);

            var sample = FarthestPointSampling(g, 2, 0, (a) => { });
            var sp = DijkstraMinHeap(mesh, sample.SampleIndices[0], sample.SampleIndices[1], true);

            var boundaryVertices = new HashSet<Vertex>();

            var vertices = mesh.Vertices.Where(x => sp.Path.Contains(x.Key)).ToList();

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
                int triIndex = commonTris[0];

                var tri = mesh.Triangles[triIndex];
                if (i != 1)
                {
                    for (int j = 0; j < commonTris.Count; j++)
                    {
                        if (cutMesh.Triangles[triIndex].ContainsId(mesh.Vertices.Count + i - 2))
                        {
                            triIndex = commonTris[j];
                            tri = mesh.Triangles[triIndex];
                            break;
                        }
                    }
                }
                var i3rd = tri.GetThirdVertexId(v1.Id, vi.Id);

                vi.Tris.Add(triIndex);
                vj.Tris.Remove(triIndex);

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

            cutMesh.RefreshMesh();

            return new CutMeshOutput(cutMesh, sp, allBoundaries);
        }

        #endregion

        #region Volume Processing, Marching Cubes, Smoothing, Remeshing, Repairing

        private static List<CubicCell> MakeGrid(PointCloud output)
        {
            if (output.ImportMatrix[1,0] == -1)
            {

            }

            var x = output.XCount;
            var y = output.YCount;
            var z = output.ZCount;
            //Logger.Log(x.ToString());
            //Logger.Log(y.ToString());
            //Logger.Log(z.ToString());
            //Logger.Log(output.IntensityMap.Count().ToString());

            var retval = new List<CubicCell>();

            for (int i = 0; i < output.IntensityMap.Length; i++)
            {



                var val = output.IntensityMap[i].Key;
                if (Math.Abs(val.X) == x - 1 || Math.Abs(val.Y) == y - 1 || Math.Abs(val.Z) == z - 1)
                {
                    continue;
                }

                var corners = new KeyValuePair<OpenTK.Vector3, short>[8];

                corners[0] = output.IntensityMap[i + x * y];
                corners[1] = output.IntensityMap[i + x * y + 1];
                corners[2] = output.IntensityMap[i + 1];
                corners[3] = output.IntensityMap[i];
                corners[4] = output.IntensityMap[i + x * y + x];
                corners[5] = output.IntensityMap[i + x * y + x + 1];
                corners[6] = output.IntensityMap[i + x + 1];
                corners[7] = output.IntensityMap[i + x];

                //corners[0] = output.IntensityMap[i];
                //corners[1] = output.IntensityMap[i + 1];
                //corners[2] = output.IntensityMap[i + x];
                //corners[3] = output.IntensityMap[i + x + 1];
                //corners[4] = output.IntensityMap[i + x * y];
                //corners[5] = output.IntensityMap[i + x * y + 1];
                //corners[6] = output.IntensityMap[i + x * y + x];
                //corners[7] = output.IntensityMap[i + x * y + x + 1];

                retval.Add(new CubicCell(corners));
            }

            return retval;
        }

        private static List<Vector3[]> Polygonize(CubicCell cell, float isoLevel, bool interpolate)
        {
            int cubeIndex = 0;
            Vector3[] vertices = new Vector3[12];

            List<Vector3[]> triangles = new List<Vector3[]>();

            if (cell.Corners[0].Value >= isoLevel) cubeIndex |= 1;
            if (cell.Corners[1].Value >= isoLevel) cubeIndex |= 2;
            if (cell.Corners[2].Value >= isoLevel) cubeIndex |= 4;
            if (cell.Corners[3].Value >= isoLevel) cubeIndex |= 8;
            if (cell.Corners[4].Value >= isoLevel) cubeIndex |= 16;
            if (cell.Corners[5].Value >= isoLevel) cubeIndex |= 32;
            if (cell.Corners[6].Value >= isoLevel) cubeIndex |= 64;
            if (cell.Corners[7].Value >= isoLevel) cubeIndex |= 128;


            if (MarchingCubesTables.CubeEdgeFlags[cubeIndex] == 0)
            {
                return triangles;
            }

            if ((MarchingCubesTables.CubeEdgeFlags[cubeIndex] & 1) == 1)
            {
                vertices[0] = Interpolate(isoLevel, cell.Corners[0], cell.Corners[1], interpolate);
            }
            if ((MarchingCubesTables.CubeEdgeFlags[cubeIndex] & 2) == 2)
            {
                vertices[1] = Interpolate(isoLevel, cell.Corners[1], cell.Corners[2], interpolate);
            }
            if ((MarchingCubesTables.CubeEdgeFlags[cubeIndex] & 4) == 4)
            {
                vertices[2] = Interpolate(isoLevel, cell.Corners[2], cell.Corners[3], interpolate);
            }
            if ((MarchingCubesTables.CubeEdgeFlags[cubeIndex] & 8) == 8)
            {
                vertices[3] = Interpolate(isoLevel, cell.Corners[3], cell.Corners[0], interpolate);
            }
            if ((MarchingCubesTables.CubeEdgeFlags[cubeIndex] & 16) == 16)
            {
                vertices[4] = Interpolate(isoLevel, cell.Corners[4], cell.Corners[5], interpolate);
            }
            if ((MarchingCubesTables.CubeEdgeFlags[cubeIndex] & 32) == 32)
            {
                vertices[5] = Interpolate(isoLevel, cell.Corners[5], cell.Corners[6], interpolate);
            }
            if ((MarchingCubesTables.CubeEdgeFlags[cubeIndex] & 64) == 64)
            {
                vertices[6] = Interpolate(isoLevel, cell.Corners[6], cell.Corners[7], interpolate);
            }
            if ((MarchingCubesTables.CubeEdgeFlags[cubeIndex] & 128) == 128)
            {
                vertices[7] = Interpolate(isoLevel, cell.Corners[7], cell.Corners[4], interpolate);
            }
            if ((MarchingCubesTables.CubeEdgeFlags[cubeIndex] & 256) == 256)
            {
                vertices[8] = Interpolate(isoLevel, cell.Corners[0], cell.Corners[4], interpolate);
            }
            if ((MarchingCubesTables.CubeEdgeFlags[cubeIndex] & 512) == 512)
            {
                vertices[9] = Interpolate(isoLevel, cell.Corners[1], cell.Corners[5], interpolate);
            }
            if ((MarchingCubesTables.CubeEdgeFlags[cubeIndex] & 1024) == 1024)
            {
                vertices[10] = Interpolate(isoLevel, cell.Corners[2], cell.Corners[6], interpolate);
            }
            if ((MarchingCubesTables.CubeEdgeFlags[cubeIndex] & 2048) == 2048)
            {
                vertices[11] = Interpolate(isoLevel, cell.Corners[3], cell.Corners[7], interpolate);
            }

            for (int i = 0; MarchingCubesTables.TriangleConnectionTable[cubeIndex, i] != -1; i += 3)
            {
                var v1 = vertices[MarchingCubesTables.TriangleConnectionTable[cubeIndex, i]];
                var v2 = vertices[MarchingCubesTables.TriangleConnectionTable[cubeIndex, i + 1]];
                var v3 = vertices[MarchingCubesTables.TriangleConnectionTable[cubeIndex, i + 2]];



                triangles.Add(new Vector3[] { v1, v2, v3 });
            }

            return triangles;
        }

        private static Vector3 Interpolate(float value, KeyValuePair<Vector3, short> c1, KeyValuePair<Vector3, short> c2, bool interpolate = true)
        {
            if (!interpolate)
            {
                return (c1.Key + c2.Key) * 0.5f;
            }

            if (Math.Abs(value - c1.Value) < 0.0001f)
            {
                return c1.Key;
            }
            if (Math.Abs(value - c2.Value) < 0.0001f)
            {
                return c2.Key;
            }
            if (Math.Abs(c1.Value - c2.Value) < 0.0001f)
            {
                return c1.Key;
            }

            float mu = ((value - c1.Value) / (c2.Value - c1.Value));

            return new Vector3
            (
                c1.Key.X + mu * (c2.Key.X - c1.Key.X),
                c1.Key.Y + mu * (c2.Key.Y - c1.Key.Y),
                c1.Key.Z + mu * (c2.Key.Z - c1.Key.Z)
            );

        }

        public static PointCloud Downsample(PointCloud volumeInfo, int sampleSize)
        {
            if (sampleSize <= 1)
            {
                return volumeInfo;
            }

            var intensityMap = new List<KeyValuePair<Vector3, short>>();

            var dimX = volumeInfo.XCount;
            var dimY = volumeInfo.YCount;
            var dimZ = volumeInfo.ZCount;

            for (int z = 0; z < dimZ; z += sampleSize)
            {
                for (int y = 0; y < dimY; y += sampleSize)
                {
                    for (int x = 0; x < dimX; x += sampleSize)
                    {
                        short intensity = 0;
                        intensity += volumeInfo.IntensityMap[z * (dimX * dimY) + (y * dimX) + x].Value;

                        //intensity += volumeInfo.Intensities[Math.Min(z * (dimX * dimY) + (y * dimX) + x, volumeInfo.Intensities.Count - 1)];
                        //intensity += volumeInfo.Intensities[Math.Min(z * (dimX * dimY) + ((y + 1) * dimX) + x, volumeInfo.Intensities.Count - 1)];
                        //intensity += volumeInfo.Intensities[Math.Min(z * (dimX * dimY) + ((y + 1) * dimX) + x + 1, volumeInfo.Intensities.Count - 1)];
                        //intensity += volumeInfo.Intensities[Math.Min((z + 1) * (dimX * dimY) + (y * dimX) + x, volumeInfo.Intensities.Count - 1)];
                        //intensity += volumeInfo.Intensities[Math.Min((z + 1) * (dimX * dimY) + ((y + 1) * dimX) + x, volumeInfo.Intensities.Count - 1)];
                        //intensity += volumeInfo.Intensities[Math.Min((z + 1) * (dimX * dimY) + ((y + 1) * dimX) + x + 1, volumeInfo.Intensities.Count - 1)];
                        //intensity /= 6;


                        intensityMap.Add(new KeyValuePair<Vector3, short>(volumeInfo.ImportMatrix * new Vector3(x, y, z) / sampleSize, intensity));
                    }
                }
            }

            return new PointCloud((int)Math.Ceiling((double)dimX / sampleSize), (int)Math.Ceiling((double)dimY / sampleSize), (int)Math.Ceiling((double)dimZ / sampleSize), intensityMap.ToArray(), volumeInfo.ImportMatrix, volumeInfo.Spacing * sampleSize, volumeInfo.MaxIntensity);
        }

        public static List<Vector3[]> MarchCubesUnindexed(PointCloud output, float intensity, bool interpolate, bool isIndexed)
        {
            var grid = MakeGrid(output);

            Dictionary<Vector3d, long> vertexDict = new Dictionary<Vector3d, long>();
            HashSet<Triangle> triangleIDs = new HashSet<Triangle>();
            var triangles = new List<Vector3[]>();

            var vertices = output.IntensityMap.Select(x => x.Key).ToList();

            for (int i = 0; i < grid.Count; i++)
            {
                var tris = Polygonize(grid[i], intensity, interpolate);

                for (int j = 0; j < tris.Count; j++)
                {
                    if (tris[j][0] != tris[j][1] && tris[j][1] != tris[j][2] && tris[j][2] != tris[j][0])
                    {
                        triangles.Add(tris[j]);
                    }
                }
            }

            return triangles;
        }

        public static Mesh CreateMeshFromVolRendererOutput(Vector4[] vertices)
        {
            Dictionary<Vector3d, int> vertexDict = new Dictionary<Vector3d, int>();
            HashSet<Triangle> triangleIDs = new HashSet<Triangle>();

            var mesh = new Mesh();

            int id = 0;
            int triId = 0;

            for (int i = 0; i < vertices.Length; i += 6)
            {
                int a, b, c = -1;

                var dec = new Vector3d(Math.Round(vertices[i].X, 6), Math.Round(vertices[i].Y, 6), Math.Round(vertices[i].Z, 6));
                if (!vertexDict.ContainsKey(dec))
                {
                    a = id;
                    vertexDict.Add(dec, id++);
                }
                else
                {
                    a = vertexDict[dec];
                }

                dec = new Vector3d(Math.Round(vertices[i + 2].X, 6), Math.Round(vertices[i + 2].Y, 6), Math.Round(vertices[i + 2].Z, 6));
                if (!vertexDict.ContainsKey(dec))
                {
                    b = id;
                    vertexDict.Add(dec, id++);
                }
                else
                {
                    b = vertexDict[dec];
                }

                dec = new Vector3d(Math.Round(vertices[i + 4].X, 6), Math.Round(vertices[i + 4].Y, 6), Math.Round(vertices[i + 4].Z, 6));
                if (!vertexDict.ContainsKey(dec))
                {
                    c = id;
                    vertexDict.Add(dec, id++);
                }
                else
                {
                    c = vertexDict[dec];
                }

                if (a != b && a != c && b != c)
                {
                    triangleIDs.Add(new Triangle(triId++, a, b, c));
                }

            }

            var verts = vertexDict.Select(x => new Vector3((float)x.Key.X, (float)x.Key.Y, (float)x.Key.Z)).ToList();

            for (int i = 0; i < verts.Count; i++)
            {
                mesh.AddVertex(verts[i], Vector3.Zero);
            }

            var tris = triangleIDs.ToList();

            for (int i = 0; i < tris.Count; i++)
            {
                mesh.AddTriangle(tris[i].V1, tris[i].V2, tris[i].V3);
            }

            mesh.CalculateVertexNormals();

            return mesh;
        }

        public static void Smoothen(ref Mesh mesh, int iteration)
        {
            _watch.Reset();
            _watch.Start();
            var originalVertexCoords = new List<Vector3>(mesh.Vertices.Select(x => x.Value.Coord));

            var keys = mesh.Vertices.Select(x => x.Key).ToList();

            while (iteration > 0)
            {
                for (int j = 0; j < keys.Count; j++)
                {
                    var i = keys[j];
                    var Lu = Vector3.Zero;
                    var normal = mesh.Vertices[i].Normal;

                    if (mesh.Vertices[i].Verts.Count > 0)
                    {
                        for (int k = 0; k < mesh.Vertices[i].Verts.Count; k++)
                        {
                            Lu += mesh.Vertices[mesh.Vertices[i].Verts[k]].Coord;
                        }
                        Lu /= mesh.Vertices[i].Verts.Count;
                        Lu -= mesh.Vertices[i].Coord;

                        mesh.Vertices[i] = new Vertex(mesh.Vertices[i].Id, mesh.Vertices[i].Coord + Lu * 0.5f, normal.Normalized())
                        {
                            Verts = mesh.Vertices[i].Verts,
                            Tris = mesh.Vertices[i].Tris,
                            Edges = mesh.Vertices[i].Edges
                        };
                    }
                }
                iteration--;

            }
            mesh.RefreshMesh();
            mesh.CalculateVertexNormals();

            _watch.Stop();
            Logger.Log($" smooth: {_watch.ElapsedMilliseconds} ms");
        }

        public static void RemoveIslands(ref Mesh mesh)
        {
            _watch.Reset();
            _watch.Start();

            bool[] visited = new bool[mesh.Vertices.Count];
            Queue<int> queue = new Queue<int>(mesh.Vertices.Count);

            HashSet<int> deleteVertices = new HashSet<int>();

            int trueCount = 0;
            foreach (var vertex in mesh.Vertices)
            {
                int i = vertex.Key;
                if (!visited[i])
                {
                    queue.Enqueue(mesh.Vertices[i].Id);

                    while (queue.Count > 0)
                    {
                        int v = queue.Dequeue();
                        visited[v] = true;
                        trueCount++;

                        for (int j = 0; j < mesh.Vertices[v].Verts.Count; j++)
                        {
                            int adj = mesh.Vertices[mesh.Vertices[v].Verts[j]].Id;
                            if (!visited[adj])
                            {
                                visited[adj] = true;
                                queue.Enqueue(adj);
                                trueCount++;
                            }
                        }
                    }

                    if (trueCount < mesh.Vertices.Count * 0.75)
                    {
                        for (int j = 0; j < visited.Length; j++)
                        {
                            if (visited[j])
                            {
                                deleteVertices.Add(j);
                            }
                        }
                    }
                    else
                    {
                        for (int j = 0; j < visited.Length; j++)
                        {
                            if (!visited[j])
                            {
                                deleteVertices.Add(j);
                            }
                        }
                    }

                }

            }

            foreach (var item in deleteVertices)
            {
                mesh.RemoveVertex(item);
            }
            _watch.Stop();
            mesh.RefreshMesh();
            mesh.CalculateVertexNormals();

            Logger.Log($"island removal: {_watch.ElapsedMilliseconds} ms");

        }

        public struct HoleFillAngle
        {
            public float Angle;
            public int Next;
            public int Prev;

            public HoleFillAngle(float angle, int next, int prev)
            {
                Angle = angle;
                Next = next;
                Prev = prev;
            }
        }

        public static void PatchBoundary2(ref Mesh mesh, ref Dictionary<int, Vertex> boundary, ref Dictionary<int, HoleFillAngle> angles)
        {

            List<Vertex> newVertices = new List<Vertex>();
            for (int i = 0; i < angles.Count; i++)
            {
                var angle = angles.ElementAt(i);

                var theta = MathHelper.RadiansToDegrees(angle.Value.Angle);

                if (theta <= 75)
                {
                    mesh.AddTriangle(angle.Key, angle.Value.Next, angle.Value.Prev);
                    boundary.Remove(angle.Key);

                    var prev = angles[angle.Value.Prev];
                    var next = angles[angle.Value.Next];

                    angles[angle.Value.Prev] = new HoleFillAngle(prev.Angle, prev.Prev, angle.Value.Next);
                    angles[angle.Value.Next] = new HoleFillAngle(next.Angle, angle.Value.Prev, next.Next);
                    //angles.Remove(angle.Key);
                    //i--;
                }
                else
                {
                    var prevV = boundary[angle.Value.Prev];
                    var nextV = boundary[angle.Value.Next];
                    var start = boundary[angle.Key];

                    var midVector = (prevV.Coord + nextV.Coord) * 0.5f;

                    var length = ((prevV.Coord - start.Coord).Length + (nextV.Coord - start.Coord).Length) * 0.5f;

                    midVector = start.Coord + (midVector - start.Coord).Normalized() * length;
                    var midNormal = (nextV.Normal + prevV.Normal).Normalized();

                    //if (theta > 75 && theta <= 135)
                    {

                        Vertex newVertex = new Vertex();
                        bool found = false;
                        for (int j = 0; j < newVertices.Count; j++)
                        {
                            if ((newVertices[j].Coord - midVector).Length < length)
                            {
                                newVertex = new Vertex
                                (
                                    newVertices[j].Id,
                                    (newVertices[j].Coord + midVector) * 0.5f,
                                    midNormal
                                );
                                newVertices[j] = newVertex;
                                mesh.Vertices[newVertices[j].Id] = newVertex;
                                found = true;
                            }
                        }
                        if (!found)
                        {
                            newVertex = mesh.AddVertex(midVector, midNormal);
                            newVertices.Add(newVertex);
                        }

                        mesh.AddTriangle(prevV.Id, start.Id, newVertex.Id);
                        mesh.AddTriangle(start.Id, nextV.Id, newVertex.Id);

                        boundary.Remove(angle.Key);
                        boundary.Add(newVertex.Id, newVertex);

                        var prev = angles[angle.Value.Prev];
                        var next = angles[angle.Value.Next];

                        angles[prevV.Id] = new HoleFillAngle(prev.Angle, prev.Prev, newVertex.Id);
                        angles[nextV.Id] = new HoleFillAngle(next.Angle, newVertex.Id, next.Next);
                        //angles.Remove(angle.Key);
                        //i--;

                    }
                    //else if (theta > 135 && theta <= 180)
                    //{
                    //    continue;
                    //    var rotatedVec1 = (prevV.Coord + midVector) * 0.5f;
                    //    var dir1 = (rotatedVec1 - start.Coord).Normalized() * length;
                    //    rotatedVec1 = start.Coord + dir1;
                    //    var rotatedNormal1 = (prevV.Normal + midNormal).Normalized();

                    //    var rotatedVec2 = (nextV.Coord + midVector) * 0.5f;
                    //    var dir2 = (rotatedVec2 - start.Coord).Normalized() * length;
                    //    rotatedVec2 = start.Coord + dir2;
                    //    var rotatedNormal2 = (nextV.Normal + midNormal).Normalized();

                    //    Vertex v1 = new Vertex();
                    //    Vertex v2 = new Vertex();

                    //}
                }
            }
        }

        public static void PatchBoundary(ref Mesh mesh, ref Dictionary<int, Vertex> boundary, ref Dictionary<int, HoleFillAngle> angles)
        {

            List<int> boundaryDeletion = new List<int>();
            List<int> boundaryAddition = new List<int>();

            List<Vertex> newVertices = new List<Vertex>();
            List<Triangle> newTriangles = new List<Triangle>();

            for (int i = 0; i < angles.Count; i++)
            {
                var angle = angles.ElementAt(i);

                var theta = MathHelper.RadiansToDegrees(angle.Value.Angle);

                if (theta < 85)
                {
                    mesh.AddTriangle(angle.Key, angle.Value.Next, angle.Value.Prev);
                    //newTriangles.Add(new Triangle(0, angle.Key, angle.Value.Next, angle.Value.Prev));
                    boundaryDeletion.Add(angle.Key);
                }
                else
                {
                    var prevV = boundary[angle.Value.Prev];
                    var nextV = boundary[angle.Value.Next];
                    var start = boundary[angle.Key];

                    var midVector = (prevV.Coord + nextV.Coord) * 0.5f;
                    var length = ((prevV.Coord - start.Coord).Length + (nextV.Coord - start.Coord).Length) * 0.5f;
                    midVector = start.Coord + (midVector - start.Coord).Normalized() * length;
                    var midNormal = (nextV.Normal + prevV.Normal).Normalized();

                    if (theta > 75 && theta <= 135)
                    {
                        var newVertex = mesh.AddVertex(midVector, midNormal);
                        //var newVertex = new Vertex(mesh.Vertices.Count, midVector, midNormal);
                        newVertices.Add(newVertex);

                        mesh.AddTriangle(prevV.Id, start.Id, newVertex.Id);
                        mesh.AddTriangle(start.Id, nextV.Id, newVertex.Id);

                        //newTriangles.Add(new Triangle(mesh.Triangles.Count,prevV.Id, start.Id, newVertex.Id));
                        //newTriangles.Add(new Triangle(mesh.Triangles.Count,start.Id, nextV.Id, newVertex.Id));

                        boundaryDeletion.Add(angle.Key);
                        boundaryAddition.Add(newVertex.Id);

                    }
                    else if (theta > 135 && theta <= 180)
                    {
                        var midVector1 = (prevV.Coord + midVector) * 0.5f;
                        var length1 = ((prevV.Coord - start.Coord).Length + (midVector - start.Coord).Length) * 0.5f;
                        midVector1 = start.Coord + (midVector1 - start.Coord).Normalized() * length1;
                        var midNormal1 = (midNormal + prevV.Normal).Normalized();

                        var midVector2 = (nextV.Coord + midVector) * 0.5f;
                        var length2 = ((nextV.Coord - start.Coord).Length + (midVector - start.Coord).Length) * 0.5f;
                        midVector2 = start.Coord + (midVector2 - start.Coord).Normalized() * length2;
                        var midNormal2 = (midNormal + nextV.Normal).Normalized();

                        var newV1 = mesh.AddVertex(midVector1, midNormal1);
                        var newV2 = mesh.AddVertex(midVector2, midNormal2);

                        //var newV1 = new Vertex(mesh.Vertices.Count, midVector1, midNormal1);
                        //var newV2 = new Vertex(mesh.Vertices.Count, midVector2, midNormal2);
                        newVertices.Add(newV1);
                        newVertices.Add(newV2);

                        mesh.AddTriangle(newV1.Id, prevV.Id, start.Id);
                        mesh.AddTriangle(newV1.Id, start.Id, newV2.Id);
                        mesh.AddTriangle(newV2.Id, start.Id, nextV.Id);

                        //newTriangles.Add(new Triangle(mesh.Triangles.Count, newV1.Id, prevV.Id, start.Id));
                        //newTriangles.Add(new Triangle(mesh.Triangles.Count, newV1.Id, start.Id, newV2.Id));
                        //newTriangles.Add(new Triangle(mesh.Triangles.Count, newV2.Id, start.Id, nextV.Id));

                    }
                }
            }

            //for (int i = 0; i < newVertices.Count; i++)
            //{
            //    mesh.AddVertex(newVertices[i].Coord, newVertices[i].Normal);
            //}

            //for (int i = 0; i < newTriangles.Count; i++)
            //{
            //    mesh.AddTriangle(newTriangles[i].V1, newTriangles[i].V2, newTriangles[i].V3);
            //}

        }

        public static void FillHoles2(ref Mesh mesh)
        {
            List<Dictionary<int, Vertex>> allBoundaries = new List<Dictionary<int, Vertex>>();

            var boundaryEdges = mesh.GetBoundaryEdges();

            // Step-1: Find All Boundary vertices.
            RecursivelyFindAllBoundaries(mesh.Vertices, boundaryEdges, ref allBoundaries);

            // Step-2: calculate angle Theta between 2 adjacent boundary edges.

            List<Dictionary<int, HoleFillAngle>> anglesList = new List<Dictionary<int, HoleFillAngle>>();

            //foreach (var boundary in allBoundaries)

            for (int i = 0; i < allBoundaries.Count; i++)
            {
                var boundary = allBoundaries[i];

                var a = boundary.Select(x => x.Value.Coord).ToList();

                var center = Vector3.Zero;
                for (int k = 0; k < a.Count; k++)
                {
                    center += a[k];
                }
                center /= a.Count;

                Dictionary<int, HoleFillAngle> angles = new Dictionary<int, HoleFillAngle>();

                for (int j = 1; j <= boundary.Count; j++)
                {
                    var vi = boundary.ElementAt(j % boundary.Count);
                    var vNext = boundary.ElementAt((j + 1) % boundary.Count);
                    var vPrev = boundary.ElementAt((j - 1) % boundary.Count);

                    var d = (vi.Value.Coord - (vNext.Value.Coord + vPrev.Value.Coord) * 0.5f).Normalized();
                    var c = (vi.Value.Coord - center).Normalized();

                    var dot = Vector3.Dot(d, c);


                    float theta = Vector3.CalculateAngle(vPrev.Value.Coord - vi.Value.Coord, vNext.Value.Coord - vi.Value.Coord);

                    if (dot < 0)
                    {
                        theta = MathHelper.TwoPi - theta;
                    }

                    angles.Add(vi.Key, new HoleFillAngle(theta, vNext.Key, vPrev.Key));
                }

                anglesList.Add(angles.OrderBy(x => x.Value.Angle).ToDictionary(x => x.Key, x => x.Value));
            }

            // Step-3: Start from vertex with smallest theta angle. Apply rules such that,
            // theta <= 75 => connect, theta > 75 && theta <=135 => add new vertex, theta > 135 => add 2 new vertices

            for (int i = 0; i < allBoundaries.Count; i++)
            {
                var boundary = allBoundaries[i];
                var angles = anglesList[i];

                PatchBoundary(ref mesh, ref boundary, ref angles);
            }

            //foreach (var angles in anglesList)



            // Step-4: Compute distance between newly created vertices with related boundary vertices. if the distance is smaller than a threshhold merge them

            // Step-5: Update the front, go 2. repeat etc.

        }

        public static void FillHoles(ref Mesh mesh)
        {
            List<Dictionary<int, Vertex>> allBoundaries = new List<Dictionary<int, Vertex>>();

            var boundaryEdges = mesh.GetBoundaryEdges();

            // Step-1: Find All Boundary vertices.
            RecursivelyFindAllBoundaries(mesh.Vertices, boundaryEdges, ref allBoundaries);

            for (int i = 0; i < allBoundaries.Count; i++)
            {
                var boundary = allBoundaries[i];

                CoarseTriangulate(ref mesh, ref boundary);
            }


            //foreach (var angles in anglesList)



            // Step-4: Compute distance between newly created vertices with related boundary vertices. if the distance is smaller than a threshhold merge them

            // Step-5: Update the front, go 2. repeat etc.

        }

        public static void CoarseTriangulate(ref Mesh mesh, ref Dictionary<int, Vertex> boundary)
        {
            var a = boundary.Select(x => x.Value).ToList();

            var center = Vector3.Zero;
            var normal = Vector3.Zero;
            for (int k = 0; k < a.Count; k++)
            {
                center += a[k].Coord;
                normal += a[k].Normal;
            }
            center /= a.Count;

            var c = mesh.AddVertex(center, normal.Normalized());

            for (int i = 0; i < a.Count; i++)
            {
                var vi = a[i];
                var vj = a[(i + 1) % a.Count];

                mesh.AddTriangle(vi.Id, vj.Id, c.Id);
            }

        }


        #endregion
    }


}
