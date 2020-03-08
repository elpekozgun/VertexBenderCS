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
    public interface IPriorityQueue<TItem, TPriority> : IEnumerable<TItem>
    {
        int Count { get; }
        TItem Peek { get; }
        TPriority PeekPriority { get; }
        IPriorityQueueEntry<TItem> Enqueue(TItem item, TPriority priority);
        TItem Dequeue();
        void UpdatePriority(IPriorityQueueEntry<TItem> entry, TPriority priority);
        void Remove(IPriorityQueueEntry<TItem> entry);
        void Clear();
    }
    public interface IPriorityQueueEntry<TItem>
    {
        TItem Item { get; }
    }

    public enum PriorityQueueType
    {
        Minimum,
        Maximum
    }
    public sealed class FibonacciHeap2<TItem, TPriority> : IPriorityQueue<TItem, TPriority>
    {
        private sealed class FibonacciNode : IPriorityQueueEntry<TItem>
        {
            public FibonacciNode Parent = null;
            public FibonacciNode Left;
            public FibonacciNode Right;
            public FibonacciNode FirstChild = null;
            public int Degree = 0;
            public bool IsMarked = false;
            public TItem Item { get; internal set; }
            public TPriority Priority { get; internal set; }
            public Guid HeapIdentifier { get; set; }

            public FibonacciNode(TItem item, TPriority priority, Guid heapIdentifier)
            {
                Item = item;
                Priority = priority;
                HeapIdentifier = heapIdentifier;
            }
        }

        private readonly Guid identifier;

        private readonly Func<TPriority, TPriority, int> Compare;

        private FibonacciNode head;

        public TItem Peek
        {
            get
            {
                if (Count == 0)
                {
                    throw new InvalidOperationException("Heap contains no elements");
                }
                return head.Item;
            }
        }

        public TPriority PeekPriority
        {
            get
            {
                if (Count == 0)
                {
                    throw new InvalidOperationException("Binary heap does not contain elements");
                }
                return head.Priority;
            }
        }

        public int Count { get; private set; }

        public FibonacciHeap2(PriorityQueueType type, IComparer<TPriority> comparer = null)
        {
            if (comparer == null)
            {
                throw new ArgumentNullException("comparer");
            }
            switch (type)
            {
                case PriorityQueueType.Minimum:
                    Compare = (x, y) => comparer.Compare(x, y);
                    break;
                case PriorityQueueType.Maximum:
                    Compare = (x, y) => comparer.Compare(y, x);
                    break;
                default: throw new ArgumentException(string.Format("Unknown priority queue type: {0}", type));
            }
            identifier = Guid.NewGuid();
        }

        public FibonacciHeap2(PriorityQueueType type)
            : this(type, Comparer<TPriority>.Default)
        {
        }

        public IEnumerator<TItem> GetEnumerator()
        {
            foreach (var entry in Enumerate())
            {
                yield return entry.Item;
            }
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        public IPriorityQueueEntry<TItem> Enqueue(TItem item, TPriority priority)
        {
            if (item == null)
            {
                throw new ArgumentNullException("item");
            }
            if (priority == null)
            {
                throw new ArgumentNullException("priority");
            }
            FibonacciNode node = new FibonacciNode(item, priority, identifier);

            if (Count == 0)
            {
                node.Left = node.Right = head = node;
            }
            else
            {
                Paste(head, node);

                if (Compare(head.Priority, priority) > 0)
                {
                    head = node;
                }
            }
            Count++;
            return node;
        }

        public void UpdatePriority(IPriorityQueueEntry<TItem> entry, TPriority priority)
        {
            if (entry == null)
            {
                throw new ArgumentNullException("entry");
            }
            if (priority == null)
            {
                throw new ArgumentNullException("priority");
            }
            FibonacciNode node = entry as FibonacciNode;
            if (node == null)
            {
                throw new InvalidCastException("Invalid heap entry format!");
            }
            if (node.HeapIdentifier != identifier)
            {
                throw new ArgumentException("Heap does not contain this node!");
            }
            if (node.Parent != null && Compare(node.Parent.Priority, priority) > 0)
            {
                CutNode(node);
            }
            node.Priority = priority;

            if (Compare(head.Priority, priority) > 0)
            {
                head = node;
            }
        }

        public TItem Dequeue()
        {
            if (Count == 0)
            {
                throw new InvalidOperationException("Heap is empty!");
            }
            FibonacciNode h = head;

            if (Count == 1)
            {
                head = null;
                Count--;
                h.HeapIdentifier = Guid.Empty;
                return h.Item;
            }
            while (head.Degree > 0)
            {
                CutNode(head.FirstChild);
            }
            Concatenate();
            head.Left.Right = head.Right;
            head.Right.Left = head.Left;
            head = SearchNewMinimum();
            Count--;
            h.HeapIdentifier = Guid.Empty;
            return h.Item;
        }

        public void Remove(IPriorityQueueEntry<TItem> entry)
        {
            if (entry == null)
            {
                throw new ArgumentNullException("entry");
            }

            FibonacciNode temp = entry as FibonacciNode;
            if (temp == null)
            {
                throw new InvalidCastException("Invalid heap entry format!");
            }

            if (temp.HeapIdentifier != identifier)
            {
                throw new ArgumentException("Heap does not contain this node!");
            }
            CutNode(temp);
            head = temp;
            Dequeue();
        }

        public void Clear()
        {
            foreach (var entry in Enumerate())
            {
                entry.HeapIdentifier = Guid.Empty;
            }
            Count = 0;
            head = null;
        }

        private IEnumerable<FibonacciNode> Enumerate()
        {
            if (head == null)
            {
                yield break;
            }
            var current = head;
            do
            {
                foreach (var node in EnumerateBranch(current))
                {
                    yield return node;
                }
                current = current.Right;
            }
            while (current != head);
        }

        private IEnumerable<FibonacciNode> EnumerateBranch(FibonacciNode root)
        {
            if (root.FirstChild != null)
            {
                var current = root.FirstChild;
                do
                {
                    foreach (var node in EnumerateBranch(current))
                    {
                        yield return node;
                    }
                    current = current.Right;
                }
                while (current != root.FirstChild);
            }
            yield return root;
        }

        private FibonacciNode SearchNewMinimum()
        {
            FibonacciNode h = head.Right;
            for (FibonacciNode node = head.Right.Right; node != head.Right; node = node.Right)
            {
                if (Compare(h.Priority, node.Priority) > 0)
                {
                    h = node;
                }
            }
            return h;
        }

        private void Concatenate()
        {
            IDictionary<int, FibonacciNode> concat = new Dictionary<int, FibonacciNode>();

            for (FibonacciNode node = head.Right; node != head;)
            {
                FibonacciNode next = node.Right;
                bool cont = true;
                do
                {
                    cont = true;
                    if (!concat.ContainsKey(node.Degree))
                    {
                        concat.Add(node.Degree, node);
                        cont = false;
                    }
                    else
                    {
                        FibonacciNode n = concat[node.Degree];
                        concat.Remove(node.Degree);
                        if (Compare(n.Priority, node.Priority) > 0)
                        {
                            Merge(node, n);
                        }
                        else
                        {
                            Merge(n, node);
                            node = n;
                        }
                    }
                }
                while (cont);
                node = next;
            }
        }

        private void Merge(FibonacciNode root, FibonacciNode child)
        {
            child.Parent = root;
            child.IsMarked = false;
            child.Left.Right = child.Right;
            child.Right.Left = child.Left;
            if (root.Degree == 0)
            {
                root.FirstChild = child;
                child.Left = child.Right = child;
            }
            else
            {
                Paste(root.FirstChild, child);
            }
            root.Degree++;
        }

        private void Paste(FibonacciNode prev, FibonacciNode next)
        {
            next.Left = prev;
            next.Right = prev.Right;
            prev.Right.Left = next;
            prev.Right = next;
        }

        private void CutNode(FibonacciNode node)
        {
            if (node.Parent == null)
            {
                return;
            }
            else if (node.Parent.Degree == 1)
            {
                node.Parent.FirstChild = null;
            }
            else if (node.Parent.FirstChild == node)
            {
                node.Parent.FirstChild = node.Right;
                node.Right.Left = node.Left;
                node.Left.Right = node.Right;
            }
            else
            {
                node.Right.Left = node.Left;
                node.Left.Right = node.Right;
            }
            Paste(head, node);
            node.Parent.Degree--;
            if (node.Parent.IsMarked)
            {
                CutNode(node.Parent);
            }
            else
            {
                node.Parent.IsMarked = true;
            }
            node.Parent = null;
        }
    }

    public static class Algorithm
    {

        public static List<List<KeyValuePair<int,float>>> ConstructGraphFromMesh(Mesh mesh)
        {
            var graph = new List<List<KeyValuePair<int,float>>>();

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
      
        public static float DijkstraArray(List<List<KeyValuePair<int,float>>> graph, int src, int target, out List<int> path, bool earlyTerminate = false)
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

            //var que = new FastPriorityQueue<QueueNode>(graph.Vertices.Count);

            //var distances = new Dictionary<int, float>();
            //var previouses = new Dictionary<int, FastPriorityQueueNode>();
            //var nodeMap = new Dictionary<int, QueueNode>();

            //for (int i = 0; i < graph.Vertices.Count; i++)
            //{
            //    var dist = i == src ? 0 : float.MaxValue;
            //    distances[i] = dist;
            //    previouses[i] = null;
            //    var n = new QueueNode(i, dist);
            //    que.Enqueue(n, dist);
            //    nodeMap[i] = n;
            //}

            //while (que.Count > 0)
            //{
            //    var u = que.Dequeue();

            //    if (earlyTerminate && u.id == target)
            //    {
            //        break;
            //    }

            //    foreach (var neighbor in graph.Vertices[u.id].Verts)
            //    {
            //        //float val = distances[u.id] + (graph.Vertices[u.id].Coord - graph.Vertices[neighbor].Coord).Length;
            //        float val = distances[u.id] + neighbor.Value;
            //        if (val < distances[neighbor.Key])
            //        {
            //            que.UpdatePriority(nodeMap[neighbor.Key], val);
            //            distances[neighbor.Key] = val;
            //            previouses[neighbor.Key] = u;
            //        }
            //    }

            //}

            //path = new List<int>();
            //int k = target;
            //path.Add(target);
            //while (k != src)
            //{
            //    k = (previouses[k] as QueueNode).id;
            //    path.Add(k);
            //}

            //return distances[target];
        }

        public static float DijkstraFibonacciHeap(Graph graph, int src, int target, out List<int> path, bool earlyTerminate = false)
        {
            FibonacciHeap<int, float> que = new FibonacciHeap<int, float>(src);

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
                var n = new QueueNode(i,graph.Nodes[i].Neighbors);
                que.Enqueue(n, dist);
                nodeMap[i] = n;
            }

            while (que.Count > 0)
            {
                var u = que.Dequeue();

                //foreach (var neighbor in graph.Nodes[u.id].Neighbors)
                foreach (var neighbor in u.Neighbors)
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


        public static List<GraphNode> FarthestPointSampling(Graph graph, int sampleCount)
        {
            List<GraphNode> samples = new List<GraphNode>();

            var matrix = CreateLinearGeodesicDistances(graph, true);
            //var matrix = CreateLinearGeodesicDistanceMatrix(graph, false);

            int startIndex = 0;
            
            
            int maxDistIndex = -1;
            float maxVal = float.MinValue;
            for (int i = 0; i < graph.Nodes.Count; ++i)
            {
                if (matrix[startIndex][i] > maxVal)
                {
                    maxVal = matrix[startIndex][i];
                    maxDistIndex = i;
                }
            }

            samples.Add(graph.Nodes[maxDistIndex]);
            while (samples.Count < sampleCount)
            {
                List<KeyValuePair<int, int>> associations = new List<KeyValuePair<int, int>>();

                for (int i = 0; i < graph.Nodes.Count; i++)
                {
                    float minDist = float.MaxValue;
                    int minIndex = -1;
                    for (int j = 0; j < samples.Count; j++)
                    {
                        if (i == samples[j].Id)
                        {
                            minIndex = -1;
                            minDist = float.MaxValue;
                            break;
                        }
                        if (matrix[i][samples[j].Id] < minDist)
                        {
                            minDist = matrix[i][ samples[j].Id];
                            minIndex = samples[j].Id;
                        }
                    }
                    if (minIndex != -1)
                    {
                        associations.Add(new KeyValuePair<int, int>(i, minIndex));
                    }
                }

                float maxDist = float.MinValue;
                int maxIndex = -1;
                for (int i = 0; i < associations.Count; ++i)
                {
                    var assoc = associations[i];
                    if (matrix[assoc.Key][ assoc.Value] > maxDist)
                    {
                        maxIndex = assoc.Key;
                        maxDist = matrix[assoc.Key][ assoc.Value];
                    }
                }
                samples.Add(graph.Nodes[maxIndex]);
            }

            return samples;
        }
    }
}
