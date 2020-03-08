using System;

namespace FibonacciHeap
{
    /// <summary>
    /// Represents the one node in the Fibonacci Heap.
    /// </summary>
    /// <typeparam name="T">Type of the object to be stored.</typeparam>
    /// <typeparam name="TKey">Type of the key to be used for the stored object. 
    /// Has to implement the <see cref="IComparable"/> interface.</typeparam>
    public class FibonacciHeapNode<T, TKey> where TKey : IComparable<TKey>
    {
        public FibonacciHeapNode(T data, TKey key)
        {
            Right = this;
            Left = this;
            Data = data;
            Key = key;
        }

        public int PrevId { get; set; }

        /// <summary>
        /// Gets or sets the node data object.
        /// </summary>
        public T Data { get; set; }

        /// <summary>
        /// Gets or sets the reference to the first child node.
        /// </summary>
        public FibonacciHeapNode<T, TKey> Child { get; set; }

        /// <summary>
        /// Gets or sets the reference to the left node neighbour.
        /// </summary>
        public FibonacciHeapNode<T, TKey> Left { get; set; }

        /// <summary>
        /// Gets or sets the reference to the node parent.
        /// </summary>
        public FibonacciHeapNode<T, TKey> Parent { get; set; }

        /// <summary>
        /// Gets or sets the reference to the right node neighbour.
        /// </summary>
        public FibonacciHeapNode<T, TKey> Right { get; set; }

        /// <summary>
        /// Gets or sets the value indicating whatever node is marked (visited).
        /// </summary>
        public bool Mark { get; set; }

        /// <summary>
        /// Gets or sets the value of the node key.
        /// </summary>
        public TKey Key { get; set; }

        /// <summary>
        /// Gets or sets the value of the node degree.
        /// </summary>
        public int Degree { get; set; }
    }

    internal class FibonacciHeapNode<T> : IComparable where T : IComparable
    {
        internal T Value { get; set; }

        internal int Id;
        internal int Degree;
        internal FibonacciHeapNode<T> ChildrenHead { get; set; }

        internal FibonacciHeapNode<T> Parent { get; set; }
        internal bool LostChild { get; set; }

        internal FibonacciHeapNode<T> Previous;
        internal FibonacciHeapNode<T> Next;

        internal FibonacciHeapNode(T value)
        {
            Value = value;
        }

        public int CompareTo(object obj)
        {
            return Value.CompareTo(((FibonacciHeapNode<T>)obj).Value);
        }
    }

}