using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// adapted from: http://content.gpwiki.org/index.php/C_sharp:BinaryHeapOfT

namespace Engine.Processing
{

    //public class Node
    //{
    //    /// <summary>
    //    /// Create a new graph node with no edges.
    //    /// </summary>
    //    public Node(string name, PointF position)
    //    {
    //        Name = name;
    //        Position = position;
    //    }

    //    #region General node information
    //    /// <summary>
    //    /// The name of the node, for debugging and user interface purposes.
    //    /// </summary>
    //    public string Name { get; set; }
    //    /// <summary>
    //    /// Screen position of the node.
    //    /// </summary>
    //    public PointF Position { get; set; }

    //    /// <summary>
    //    /// Edges of the node
    //    /// </summary>
    //    public readonly List<UndirectedEdge> Edges = new List<UndirectedEdge>();

    //    #endregion

    //    #region Node information specific to Dijkstra's algorithm.
    //    /// <summary>
    //    /// Position of this node in whatever priority queue it's being held in.
    //    /// This is provided to allow the BinaryHeap code to quickly find the node in the
    //    /// heap in its arrays when doing DecreaseKey.
    //    /// </summary>
    //    public int QueuePosition { get; set; }

    //    /// <summary>
    //    /// Estimated cost of getting to the node from the start point.
    //    /// </summary>
    //    public double NodeCost { get; set; }

    //    public Node Predecessor { get; set; }
    //    #endregion

    //    /// <summary>
    //    /// Creates an edge between this node and another node.
    //    /// Its cost is set to the distance between their screen positions.
    //    /// </summary>
    //    public void AddEdge(Node neighbor)
    //    {
    //        double cost = Math.Sqrt(Square(Position.X - neighbor.Position.X) + Square(Position.Y - neighbor.Position.Y));
    //        UndirectedEdge e = new UndirectedEdge(this, neighbor, cost);
    //        Edges.Add(e);
    //        neighbor.Edges.Add(e);
    //    }
    //    double Square(double x) { return x * x; }
    //}

    //public class BinaryHeap
    //{
    //    #region Instance variables

    //    readonly MinHeapNode[] data;
    //    readonly double[] priorities;
    //    int count;
    //    #endregion

    //    /// <summary>
    //    /// Creates a new, empty priority queue with the specified capacity.
    //    /// </summary>
    //    /// <param name="capacity">The maximum number of nodes that will be stored in the queue.</param>
    //    public BinaryHeap(int capacity)
    //    {
    //        data = new MinHeapNode[capacity];
    //        priorities = new double[capacity];
    //        count = 0;
    //    }

    //    /// <summary>
    //    /// Adds an item to the queue.  Is position is determined by its priority relative to the other items in the queue.
    //    /// aka HeapInsert
    //    /// </summary>
    //    /// <param name="item">Item to add</param>
    //    /// <param name="priority">Priority value to attach to this item.  Note: this is a min heap, so lower priority values come out first.</param>
    //    public void Add(MinHeapNode item, double priority)
    //    {
    //        if (count == data.Length)
    //            throw new Exception("Heap capacity exceeded");

    //        // Add the item to the heap in the end position of the array (i.e. as a leaf of the tree)
    //        int position = count++;
    //        data[position] = item;
    //        item.QueuePosition = position;
    //        priorities[position] = priority;
    //        // Move it upward into position, if necessary
    //        MoveUp(position);

    //    }

    //    /// <summary>
    //    /// Extracts the item in the queue with the minimal priority value.
    //    /// </summary>
    //    /// <returns></returns>
    //    public MinHeapNode ExtractMin() // Probably THE most important function... Got everything working
    //    {
    //        MinHeapNode minNode = data[0];
    //        Swap(0, count - 1);
    //        count--;
    //        MoveDown(0);
    //        return minNode;
    //    }

    //    /// <summary>
    //    /// Reduces the priority of a node already in the queue.
    //    /// aka DecreaseKey 
    //    /// </summary>
    //    public void DecreasePriority(MinHeapNode n, double priority)
    //    {
    //        int position = n.QueuePosition;
    //        while ((position > 0) && (priorities[Parent(position)] > priority))
    //        {
    //            int original_parent_pos = Parent(position);
    //            Swap(original_parent_pos, position);
    //            position = original_parent_pos;
    //        }
    //        priorities[position] = priority;
    //    }

    //    /// <summary>
    //    /// Moves the node at the specified position upward, it it violates the Heap Property.
    //    /// This is the while loop from the HeapInsert procedure in the slides.
    //    /// </summary>
    //    /// <param name="position"></param>
    //    void MoveUp(int position)
    //    {
    //        while ((position > 0) && (priorities[Parent(position)] > priorities[position]))
    //        {
    //            int original_parent_pos = Parent(position);
    //            Swap(position, original_parent_pos);
    //            position = original_parent_pos;
    //        }
    //    }

    //    /// <summary>
    //    /// Moves the node at the specified position down, if it violates the Heap Property
    //    /// aka Heapify
    //    /// </summary>
    //    /// <param name="position"></param>
    //    void MoveDown(int position)
    //    {
    //        int lchild = LeftChild(position);
    //        int rchild = RightChild(position);
    //        int largest = 0;
    //        if ((lchild < count) && (priorities[lchild] < priorities[position]))
    //        {
    //            largest = lchild;
    //        }
    //        else
    //        {
    //            largest = position;
    //        }
    //        if ((rchild < count) && (priorities[rchild] < priorities[largest]))
    //        {
    //            largest = rchild;
    //        }
    //        if (largest != position)
    //        {
    //            Swap(position, largest);
    //            MoveDown(largest);
    //        }
    //    }

    //    /// <summary>
    //    /// Number of items waiting in queue
    //    /// </summary>
    //    public int Count
    //    {
    //        get
    //        {
    //            return count;
    //        }
    //    }

    //    #region Utilities
    //    /// <summary>
    //    /// Swaps the nodes at the respective positions in the heap
    //    /// Updates the nodes' QueuePosition properties accordingly.
    //    /// </summary>
    //    void Swap(int position1, int position2)
    //    {
    //        Node temp = data[position1];
    //        data[position1] = data[position2];
    //        data[position2] = temp;
    //        data[position1].QueuePosition = position1;
    //        data[position2].QueuePosition = position2;

    //        double temp2 = priorities[position1];
    //        priorities[position1] = priorities[position2];
    //        priorities[position2] = temp2;
    //    }

    //    /// <summary>
    //    /// Gives the position of a node's parent, the node's position in the queue.
    //    /// </summary>
    //    static int Parent(int position)
    //    {
    //        return (position - 1) / 2;
    //    }

    //    /// <summary>
    //    /// Returns the position of a node's left child, given the node's position.
    //    /// </summary>
    //    static int LeftChild(int position)
    //    {
    //        return 2 * position + 1;
    //    }

    //    /// <summary>
    //    /// Returns the position of a node's right child, given the node's position.
    //    /// </summary>
    //    static int RightChild(int position)
    //    {
    //        return 2 * position + 2;
    //    }

    //    /// <summary>
    //    /// Checks all entries in the heap to see if they satisfy the heap property.
    //    /// </summary>
    //    public void TestHeapValidity()
    //    {
    //        for (int i = 1; i < count; i++)
    //            if (priorities[Parent(i)] > priorities[i])
    //                throw new Exception("Heap violates the Heap Property at position " + i);
    //    }
    //    #endregion
    //}






    public class BinaryHeap<T> : ICollection<T> where T : IComparable<T>
    {
        // Constants
        private const int DEFAULT_SIZE = 4;
        // Fields
        private T[] _data = new T[DEFAULT_SIZE];
        private int _count = 0;
        private int _capacity = DEFAULT_SIZE;
        private bool _sorted;

        // Properties

        /// <summary>
        /// Gets the number of values in the heap. 
        /// </summary>
        public int Count
        {
            get { return _count; }
        }
        /// <summary>
        /// Gets or sets the capacity of the heap.
        /// </summary>
        public int Capacity
        {
            get { return _capacity; }
            set
            {
                int previousCapacity = _capacity;
                _capacity = Math.Max(value, _count);
                if (_capacity != previousCapacity)
                {
                    T[] temp = new T[_capacity];
                    Array.Copy(_data, temp, _count);
                    _data = temp;
                }
            }
        }

        // Methods

        /// <summary>
        /// Initializes a new binary heap with no arguments passed.
        /// </summary>
        public BinaryHeap()
        {
        }

        /// <summary>
        /// Initializes a new binary heap with some data passed.
        /// </summary>
        private BinaryHeap(T[] data, int count)
        {
            this.Capacity = count;
            this._count = count;
            Array.Copy(data, this._data, count);
        }

        /// <summary>
        /// Gets the first value in the heap without removing it.
        /// </summary>
        /// <returns>The lowest value of type TValue.</returns>
        public T Peek()
        {
            return this._data[0];
        }

        /// <summary>
        /// Removes all items from the heap.
        /// </summary>
        public void Clear()
        {
            this._count = 0;
            this._data = new T[this._capacity];
        }
        /// <summary>
        /// Adds a key and value to the heap.
        /// </summary>
        /// <param name="item">The item to add to the heap.</param>
        public void Add(T item)
        {
            if (this._count == this._capacity)
            {
                this.Capacity *= 2;
            }

            this._data[this._count] = item;
            this.UpHeap();
            this._count++;
        }

        /// <summary>
        /// Removes and returns the first item in the heap.
        /// </summary>
        /// <returns>The next value in the heap.</returns>
        public T Remove()
        {
            if (this._count == 0)
            {
                throw new InvalidOperationException("Cannot remove item, heap is empty.");
            }

            T v = _data[0];

            this._count--;
            this._data[0] = this._data[this._count];
            this._data[this._count] = default(T); // Clears the Last Node
            this.DownHeap();
            return v;
        }

        /// <summary>
        /// helper function that performs up-heap bubbling
        /// </summary>
        private void UpHeap()
        {
            _sorted = false;
            int p = this._count;
            T item = this._data[p];
            int par = Parent(p);
            while (par > -1 && item.CompareTo(this._data[par]) < 0)
            {
                this._data[p] = this._data[par]; // Swap nodes
                p = par;
                par = Parent(p);
            }

            this._data[p] = item;
        }


        /// <summary>
        /// helper function that performs down-heap bubbling
        /// </summary>
        private void DownHeap()
        {
            this._sorted = false;
            int n;
            int p = 0;
            T item = this._data[p];

            while (true)
            {
                int ch1 = Child1(p);

                if (ch1 >= this._count)
                {
                    break;
                }

                int ch2 = Child2(p);

                if (ch2 >= this._count)
                {
                    n = ch1;
                }
                else
                {
                    n = _data[ch1].CompareTo(_data[ch2]) < 0 ? ch1 : ch2;
                }

                if (item.CompareTo(this._data[n]) > 0)
                {
                    this._data[p] = this._data[n]; //Swap nodes
                    p = n;
                }
                else
                {
                    break;
                }
            }

            this._data[p] = item;
        }

        private void EnsureSort()
        {
            if (_sorted) return;
            Array.Sort(_data, 0, _count);
            _sorted = true;
        }

        /// <summary>
        /// helper function that calculates the parent of a node
        /// </summary>
        /// <param name="index">Index of the node of which we want the parent</param>
        /// <returns>Index of the parent node</returns>
        private static int Parent(int index)
        {
            return (index - 1) >> 1;
        }

        /// <summary>
        /// helper function that calculates the first child of a node
        /// </summary>
        /// <param name="index">Index of the node of which we want the child</param>
        /// <returns>Index of the left child</returns>
        private static int Child1(int index)
        {
            return (index << 1) + 1;
        }

        /// <summary>
        /// helper function that calculates the second child of a node
        /// </summary>
        /// <param name="index">Index of the node of which we want the child</param>
        /// <returns>Index of the right child</returns>
        private static int Child2(int index)
        {
            return (index << 1) + 2;
        }

        /// <summary>
        /// Creates a new instance of an identical binary heap.
        /// </summary>
        /// <returns>A BinaryHeap.</returns>
        public BinaryHeap<T> Copy()
        {
            return new BinaryHeap<T>(this._data, this._count);
        }

        /// <summary>
        /// Gets an enumerator for the binary heap.
        /// </summary>
        /// <returns>An IEnumerator of type T.</returns>
        public IEnumerator<T> GetEnumerator()
        {
            EnsureSort();
            for (int i = 0; i < this._count; i++)
            {
                yield return this._data[i];
            }
        }


        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <summary>
        /// Checks to see if the binary heap contains the specified item.
        /// </summary>
        /// <param name="item">The item to search the binary heap for.</param>
        /// <returns>A boolean, true if binary heap contains item.</returns>
        public bool Contains(T item)
        {
            this.EnsureSort();
            return Array.BinarySearch<T>(this._data, 0, this._count, item) >= 0;
        }

        /// <summary>
        /// Copies the binary heap to an array at the specified index.
        /// </summary>
        /// <param name="array">One dimensional array that is the destination of the copied elements.</param>
        /// <param name="arrayIndex">The zero-based index at which copying begins.</param>
        public void CopyTo(T[] array, int arrayIndex)
        {
            EnsureSort();
            Array.Copy(this._data, array, this._count);
        }

        /// <summary>
        /// Gets whether or not the binary heap is readonly.
        /// </summary>
        public bool IsReadOnly
        {
            get { return false; }
        }

        /// <summary>
        /// Removes an item from the binary heap. This utilizes the type T's Comparer and will not remove duplicates.
        /// </summary>
        /// <param name="item">The item to be removed.</param>
        /// <returns>Boolean true if the item was removed.</returns>
        public bool Remove(T item)
        {
            this.EnsureSort();

            int i = Array.BinarySearch<T>(this._data, 0, this._count, item);
            if (i < 0)
            {
                return false;
            }

            Array.Copy(this._data, i + 1, this._data, i, this._count - i);
            this._data[this._count] = default(T);
            this._count--;
            return true;
        }

    }
}
