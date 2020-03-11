using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Processing
{

    public class FibonacciNode<T>
    {

        private int _degree;
        private bool _isMarked;

        public FibonacciNode<T> Next;
        public FibonacciNode<T> Prev;

        public FibonacciNode<T> Parent;
        public FibonacciNode<T> Child;
        public T Item;
        public float Priority;

        public int Degree { get { return _degree; } set { _degree = value; } }
        public bool IsMarked { get { return _isMarked; } set { _isMarked = value; } }

        public FibonacciNode(T item, float priority)
        {
            Next = this;
            Prev = this;
            Item = item;
            Priority = priority;
            _degree = 0;
            _isMarked = false;
        }
    }

    public class FibonacciHeap<T>
    {
        private FibonacciNode<T> _min;

        public int Size { get; private set; }
        public bool IsEmpty { get { return _min == null; } }

        public FibonacciNode<T> Enqueue(T value, float priority)
        {
            FibonacciNode<T> retVal = new FibonacciNode<T>(value, priority);
            _min = MergeLists(_min, retVal);
            Size++;
            return retVal;
        }

        public FibonacciNode<T> Dequeue()
        {
            if (IsEmpty)
            {
                return null;
            }

            Size--;

            FibonacciNode<T> minElem = _min;

            if (_min.Next == _min)
            { 
                _min = null;    // only element in the heap
            }
            else
            { 
                _min.Prev.Next = _min.Next;
                _min.Next.Prev = _min.Prev;
                _min = _min.Next; 
            }

            if (minElem.Child != null)
            {
                FibonacciNode<T> curr = minElem.Child;
                do
                {
                    curr.Parent = null;
                    curr = curr.Next;
                } while (curr != minElem.Child);
            }
            
            _min = MergeLists(_min, minElem.Child);

            if (_min == null) return minElem;

            List<FibonacciNode<T>> treeTable = new List<FibonacciNode<T>>();
            List<FibonacciNode<T>> toVisit = new List<FibonacciNode<T>>();

            for (FibonacciNode<T> curr = _min; toVisit.Count == 0 || toVisit[0] != curr; curr = curr.Next)
            {
                toVisit.Add(curr);
            }

            for (int i = 0; i < toVisit.Count; i++)
            {
                var curr = toVisit[i];
                while (true)
                {
                    while (curr.Degree >= treeTable.Count)
                    {
                        treeTable.Add(null);
                    }

                    if (treeTable[curr.Degree] == null)
                    {
                        treeTable[curr.Degree] = curr;
                        break;
                    }

                    FibonacciNode<T> other = treeTable[curr.Degree];
                    treeTable[curr.Degree] = null;

                    FibonacciNode<T> min = (other.Priority < curr.Priority) ? other : curr;
                    FibonacciNode<T> max = (other.Priority < curr.Priority) ? curr : other;

                    max.Next.Prev = max.Prev;
                    max.Prev.Next = max.Next;

                    max.Next = max.Prev= max;
                    min.Child = MergeLists(min.Child, max);

                    max.Parent = min;

                    max.IsMarked = false;

                    min.Degree++;

                    curr = min;
                }
                if (curr.Priority <= _min.Priority)
                {
                    _min = curr;
                }
            }
            return minElem;
        }

        public FibonacciHeap<T> Merge(FibonacciHeap<T> first, FibonacciHeap<T> second)
        {
            FibonacciHeap<T> result = new FibonacciHeap<T>();

            result._min = MergeLists(first._min, second._min);
            result.Size = first.Size + second.Size;

            first.Size = second.Size = 0;
            first._min= null;
            second._min = null;

            return result;
        }

        public void UpdateKey(FibonacciNode<T> node, float priority)
        {
            if (priority < node.Priority)
            {
                node.Priority = priority;

                if (node.Parent != null && node.Priority <= node.Parent.Priority)
                {
                    CutNode(node);
                }

                if (node.Priority <= _min.Priority)
                {
                    _min = node;
                }
            }
            
        }
        
        private FibonacciNode<T> MergeLists(FibonacciNode<T> first, FibonacciNode<T> second)
        {
            if (first == null && second == null)
            {
                return null;
            }
            else if (first != null && second == null)
            {
                return first;
            }
            else if (first == null && second != null)
            {
                return second;
            }
            else
            {
                FibonacciNode<T> oneNext = first.Next; // Cache this since we're about to overwrite it.
                first.Next = second.Next;
                first.Next.Prev = first;
                second.Next = oneNext;
                second.Next.Prev = second;

                return first.Priority < second.Priority ? first : second;
            }
        }
        
        private void CutNode(FibonacciNode<T> node)
        {
            node.IsMarked = false;

            if (node.Parent == null) return;

            if (node.Next != node)
            { 
                node.Next.Prev = node.Prev;
                node.Prev.Next = node.Next;
            }


            if (node.Parent.Child == node)
            {
                if (node.Next != node)
                {
                    node.Parent.Child = node.Next;
                }
                else
                {
                    node.Parent.Child = null;
                }
            }

            --node.Parent.Degree;

            node.Prev = node.Next = node;
            _min = MergeLists(_min, node);

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

}
