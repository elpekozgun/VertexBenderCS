using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Processing
{
    public sealed class FiboHeap<TKey, TValue>
    {
        private readonly List<Node> Root = new List<Node>();
        // TODO: is this not a duplication of Root.Count? Remove it.
        private int _count;
        private Node Min;

        public int Count { get { return _count; } }
        public void Push(TKey key, TValue value)
        {
            Insert(new Node(key, value));
        }

        public KeyValuePair<TKey, TValue> Peek()
        {
            if (Min == null)
                throw new InvalidOperationException();
            return new KeyValuePair<TKey, TValue>(Min.Key, Min.Value);
        }

        public KeyValuePair<TKey, TValue> Pop()
        {
            if (Min == null)
                throw new InvalidOperationException();
            var min = ExtractMin();
            return new KeyValuePair<TKey, TValue>(min.Key, min.Value);
        }

        private void Insert(Node node)
        {
            _count++;
            Root.Add(node);
            if (Min == null)
            {
                Min = node;
            }
            else if (Comparer<TKey>.Default.Compare(node.Key, Min.Key) < 0)
            {
                Min = node;
            }
        }

        private Node ExtractMin()
        {
            var result = Min;
            if (result == null)
                return null;
            foreach (var child in result.Children)
            {
                child.Parent = null;
                // TODO: shouldn't _count be updated?? Same in Consolidate();
                Root.Add(child);
            }
            Root.Remove(result);
            if (Root.Count == 0)
            {
                Min = null;
            }
            else
            {
                Min = Root[0];
                Consolidate();
            }
            _count--;
            return result;
        }

        // Here be FibonacciHeap dragons.
        private void Consolidate()
        {
            var unlabeledBag = new Node[UpperBound()];
            for (int i = 0; i < Root.Count; i++)
            {
                Node item = Root[i];
                int itemChildren = item.Children.Count;
                while (true)
                {
                    Node child = unlabeledBag[itemChildren];
                    if (child == null)
                        break;
                    if (Comparer<TKey>.Default.Compare(item.Key, child.Key) > 0)
                    {
                        var swap = item;
                        item = child;
                        child = swap;
                    }
                    Root.Remove(child);
                    i--;
                    item.AddChild(child);
                    child.Mark = false;
                    unlabeledBag[itemChildren] = null;
                    itemChildren++;
                }
                unlabeledBag[itemChildren] = item;
            }
            Min = null;
            for (int i = 0; i < unlabeledBag.Length; i++)
            {
                var item = unlabeledBag[i];
                if (item == null)
                    continue;
                if (Min == null)
                {
                    Root.Clear();
                    Min = item;
                }
                else if (Comparer<TKey>.Default.Compare(item.Key, Min.Key) < 0)
                {
                    Min = item;
                }
                Root.Add(item);
            }
        }

        private int UpperBound()
        {
            // Here be dragons.
            // Also, if _count is NOT Root.Count, it ought to at least have a more meaningful name.
            double magicValue = Math.Log(_count, (1.0 + Math.Sqrt(5)) / 2.0);
            return (int)Math.Floor(magicValue) + 1;
        }

        private class Node
        {
            public TKey Key;
            public TValue Value;
            public Node Parent;
            public List<Node> Children = new List<Node>();
            public bool Mark;

            public Node(TKey key, TValue value)
            {
                Key = key;
                Value = value;
            }

            public void AddChild(Node child)
            {
                child.Parent = this;
                Children.Add(child);
            }
        }
    }


    //public class HeapNode<TPriority, TItem> where TPriority : IComparable<TPriority>
    //{
    //    /// <summary>
    //    /// Parent node in tree
    //    /// </summary>
    //    public HeapNode<TPriority, TItem> Parent { get; set; }

    //    /// <summary>
    //    /// Child nodes in tree
    //    /// </summary>
    //    public HashSet<HeapNode<TPriority, TItem>> Children { get; set; }

    //    /// <summary>
    //    /// Has this node had a child cut?
    //    /// </summary>
    //    public bool Marked { get; set; }

    //    /// <summary>
    //    /// Number of children of this node
    //    /// </summary>
    //    public int Rank { get { return Children.Count; } }

    //    /// <summary>
    //    /// Priority of node
    //    /// </summary>
    //    public TPriority Priority { get; internal set; }

    //    /// <summary>
    //    /// Object being stored
    //    /// </summary>
    //    public TItem Value { get; set; }

    //    /// <summary>
    //    /// A childless, parentless tree storing the given object and priority
    //    /// </summary>
    //    /// <param name="priority"></param>
    //    /// <param name="value"></param>
    //    public HeapNode(TPriority priority, TItem value)
    //    {
    //        // No children, no parent
    //        Children = new HashSet<HeapNode<TPriority, TItem>>();
    //        Parent = null;

    //        Priority = priority;
    //        Value = value;
    //    }

    //    /// <summary>
    //    /// Adds the given tree as a child of this tree
    //    /// </summary>
    //    /// <param name="node">The tree to become a child of this tree</param>
    //    public void AddChild(HeapNode<TPriority, TItem> node)
    //    {
    //        Children.Add(node);
    //        node.Parent = this;
    //    }

    //    /// <summary>
    //    /// Removes the given child from this tree's list of children
    //    /// </summary>
    //    /// <param name="child">The child to remove</param>
    //    public void RemoveChild(HeapNode<TPriority, TItem> child)
    //    {
    //        Children.Remove(child);
    //        child.Parent = null;
    //    }

    //    /// <summary>
    //    /// Unparents this child from its parent tree
    //    /// </summary>
    //    public void RemoveParent()
    //    {
    //        Parent.Children.Remove(this);
    //        Parent = null;
    //    }
    //}

    //public class Bucket<TItem>
    //{
    //    /// <summary>
    //    /// Gets or sets the item at the given index
    //    /// </summary>
    //    /// <param name="index">The index of the item</param>
    //    /// <returns>The item at index</returns>
    //    public TItem this[int index]
    //    {
    //        get
    //        {
    //            // If no item at given index, create and fill with default value
    //            if (!bucket.ContainsKey(index))
    //                bucket.Add(index, default(TItem));

    //            // Return the item at index
    //            return bucket[index];
    //        }

    //        set
    //        {
    //            // If no item at index, create entry at index storing given value
    //            if (!bucket.ContainsKey(index))
    //                bucket.Add(index, value);

    //            // Overwrite item at index
    //            else
    //                bucket[index] = value;
    //        }
    //    }

    //    private Dictionary<int, TItem> bucket;

    //    /// <summary>
    //    /// Empty bucket
    //    /// </summary>
    //    public Bucket()
    //    {
    //        bucket = new Dictionary<int, TItem>();
    //    }
    //}

    ///// <summary>
    ///// Min heap data structure that lazily defers consolidation of trees until deleteMin operation
    ///// </summary>
    //public class FiboHeap<TPriority, TItem> where TPriority : IComparable<TPriority>
    //{
    //    /// <summary>
    //    /// All trees in the heap
    //    /// </summary>
    //    public LinkedList<HeapNode<TPriority, TItem>> Trees { get; private set; }

    //    /// <summary>
    //    /// Node with the smallest priority
    //    /// </summary>
    //    public HeapNode<TPriority, TItem> Minimum { get { return minimumTreesNode.Value; } }

    //    /// <summary>
    //    /// Number of items in the heap
    //    /// </summary>
    //    public int Count { get; private set; }

    //    private LinkedListNode<HeapNode<TPriority, TItem>> minimumTreesNode;

    //    /// <summary>
    //    /// An empty collection
    //    /// </summary>
    //    public FiboHeap()
    //    {
    //        Trees = new LinkedList<HeapNode<TPriority, TItem>>();
    //        Count = 0;
    //        minimumTreesNode = null;
    //    }

    //    /// <summary>
    //    /// Removes the item with the smallest priority from the heap and returns it. O(log n) operation
    //    /// </summary>
    //    /// <returns>The item with the smallest priority</returns>
    //    public TItem Pop()
    //    {
    //        TItem min = Minimum.Value;
    //        DeleteMin();
    //        return min;
    //    }

    //    /// <summary>
    //    /// Inserts the given item into the heap maintaining the heap order property. O(1) operation
    //    /// </summary>
    //    /// <param name="priority">The priority of the item</param>
    //    /// <param name="value">The object to store</param>
    //    /// <returns>The node that the object is stored in</returns>
    //    public HeapNode<TPriority, TItem> Insert(TPriority priority, TItem value)
    //    {
    //        // Add node as first item in root nodes
    //        HeapNode<TPriority, TItem> node = new HeapNode<TPriority, TItem>(priority, value);
    //        Trees.AddFirst(node);

    //        // Update minimum
    //        if (Count == 0 ||                                       // Node is first node inserted? Its minimum
    //            node.Priority.CompareTo(Minimum.Priority) < 0)      // Node is less than current minimum? Its minimum
    //            minimumTreesNode = Trees.First;

    //        Count++;
    //        return node;
    //    }

    //    /// <summary>
    //    /// Removes the minimum item from the heap. O(log n) operation
    //    /// </summary>
    //    public void DeleteMin()
    //    {
    //        if (Count == 0)
    //            throw new InvalidOperationException("Heap is empty, cannot remove minimum priority item");

    //        // Remove minimum element from list of trees
    //        RemoveMinimum();

    //        // Consolidate trees
    //        ConsolidateTrees();

    //        // Update new minimum value
    //        FindMinimum();
    //    }

    //    /// <summary>
    //    /// Decreases the priority of the given node. O(log n) operation
    //    /// </summary>
    //    /// <param name="node">The node to decrease the value of</param>
    //    /// <param name="priority">The new priority of the node</param>
    //    public void DecreasePriority(HeapNode<TPriority, TItem> node, TPriority priority)
    //    {
    //        // If node priority is already less than given priority
    //        if (node.Priority.CompareTo(priority) < 0)
    //            throw new ArgumentException("Priority is not less than current priority");

    //        // Decrease key of node
    //        node.Priority = priority;

    //        // If node is a root node, AND its priority is less than the current minimum
    //        if (node.Parent == null &&
    //            priority.CompareTo(Minimum.Priority) < 0)
    //            minimumTreesNode = Trees.Find(node);        // Set as minimum

    //        // Is heap order violated?
    //        if (node.Parent != null &&
    //            node.Priority.CompareTo(node.Parent.Priority) < 0)
    //        {
    //            // Cut from parent and move to root (remember ref to old parent). Node becomes unmarked in the process
    //            HeapNode<TPriority, TItem> parent = MoveToRoot(node);

    //            // Is new priority less than minimum?
    //            if (priority.CompareTo(Minimum.Priority) < 0)
    //                minimumTreesNode = Trees.First;             // Node is first element in trees list

    //            // If parent is unmarked, mark it
    //            if (!parent.Marked)
    //                parent.Marked = true;

    //            else    // Parent IS already marked
    //            {
    //                // Cut parent, move to root, DO FOR ALL ANCESTORS
    //                while (parent != null &&
    //                       parent.Marked)
    //                    parent = MoveToRoot(parent);    // Move to root, check its parent
    //            }
    //        }
    //    }

    //    /// <summary>
    //    /// Cuts the given node from its parent and moves it to the list of root trees. Returns the parent of the node, prior to it 
    //    /// being cut
    //    /// </summary>
    //    private HeapNode<TPriority, TItem> MoveToRoot(HeapNode<TPriority, TItem> node)
    //    {
    //        // Remeber ref to parent because child is about to be cut
    //        HeapNode<TPriority, TItem> parent = node.Parent;

    //        // Remove ref to parent and parent ref to child
    //        node.RemoveParent();
    //        Trees.AddFirst(node);
    //        node.Marked = false;

    //        // Return old parent of node
    //        return parent;
    //    }

    //    /// <summary>
    //    /// Removes the minimum priorirty node from the list of root trees. Adds children to the list of root trees
    //    /// </summary>
    //    private void RemoveMinimum()
    //    {
    //        // Add each child as a root tree, remove ref to parent
    //        foreach (HeapNode<TPriority, TItem> child in minimumTreesNode.Value.Children)
    //        {
    //            child.Parent = null;
    //            Trees.AddFirst(child);
    //        }

    //        // Remove minimum from list of trees
    //        Trees.Remove(minimumTreesNode);
    //        Count--;
    //    }

    //    /// <summary>
    //    /// Finds the new minimum value from amongst the root tree nodes
    //    /// </summary>
    //    private void FindMinimum()
    //    {
    //        // If there is atleast one node in heap
    //        if (Count > 0)
    //        {
    //            // minimum = first tree in list
    //            // walker through remaining nodes to see if there is a smaller one
    //            minimumTreesNode = Trees.First;

    //            // Walk through root element of each root tree
    //            LinkedListNode<HeapNode<TPriority, TItem>> walker = Trees.First.Next;
    //            while (walker != null)
    //            {
    //                // Check if priority is less than current minimum, update minimum if so
    //                if (walker.Value.Priority.CompareTo(minimumTreesNode.Value.Priority) < 0)
    //                    minimumTreesNode = walker;

    //                // Keep iterating
    //                walker = walker.Next;
    //            }
    //        }

    //        // No nodes in heap, min is null
    //        else
    //            minimumTreesNode = null;
    //    }

    //    /// <summary>
    //    /// Consolidates root trees that have the same rank, so that, no two root trees end with the same rank
    //    /// </summary>
    //    private void ConsolidateTrees()
    //    {
    //        // Need atleast 2 trees in order to consolidate
    //        if (Count > 1)
    //        {
    //            // Bucket index corresponds to the rank of the stored element
    //            Bucket<LinkedListNode<HeapNode<TPriority, TItem>>> rankBucket = new Bucket<LinkedListNode<HeapNode<TPriority, TItem>>>();

    //            // Iterate over each root tree
    //            var walker = Trees.First;
    //            while (walker != null)
    //            {
    //                // Save ref to next root, because walker is going to be overwritten when there is a conflict
    //                var next = walker.Next;

    //                // Are there root trees with the same rank?
    //                int rank = walker.Value.Rank;
    //                while (rankBucket[rank] != null)
    //                {

    //                    walker = Consolidate(walker, rankBucket[rank]);     // Consolidate trees with same rank (also get ref to merged tree)
    //                    rankBucket[rank] = null;                            // Remove ref to root tree that was consolidated
    //                    rank = walker.Value.Rank;                           // Update rank
    //                }

    //                // No conflicts, store in rank bucket
    //                rankBucket[rank] = walker;

    //                walker = next;
    //            }
    //        }
    //    }

    //    /// <summary>
    //    /// Merge trees into one tree, maintaining heap property (every child is larger than parent). Returns the merged tree's node in
    //    /// root trees list
    //    /// </summary>
    //    private LinkedListNode<HeapNode<TPriority, TItem>> Consolidate(LinkedListNode<HeapNode<TPriority, TItem>> node1,
    //                                                                   LinkedListNode<HeapNode<TPriority, TItem>> node2)
    //    {
    //        var root = node1;
    //        var child = node2;

    //        // Get node with least priority
    //        if (node2.Value.Priority.CompareTo(node1.Value.Priority) < 0)
    //        {
    //            root = node2;
    //            child = node1;
    //        }

    //        // Remove tree with greater priority, add as child of other tree
    //        Trees.Remove(child);
    //        root.Value.AddChild(child.Value);

    //        // Return combined tree
    //        return root;
    //    }
    //}
}
