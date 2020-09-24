using System;
using System.Collections.Generic;
using System.Text;

namespace Engine.Core
{
    public abstract class KdTree<T>
    {
        // Static variables
        private static int bucketSize = 24;

        // All types
        protected int dimensions;
        private KdTree<T> parent;

        // Root only
        private LinkedList<float[]> locationStack;
        private int sizeLimit;

        // Leaf only
        private float[][] locations;
        private Object[] data;
        private int locationCount;

        // Stem only
        private KdTree<T> left, right;
        private int splitDimension;
        private float splitValue;

        // Bounds
        private float[] minLimit, maxLimit;
        private bool singularity;

        // Temporary
        private Status status;

        // Pending Pop
        private int index;
        private KdTree<T> closestCursor;

        /**
         * Construct a KdTree with a given number of dimensions and a limit on
         * maxiumum size (after which it throws away old points)
         */
        protected KdTree(int dimensions, int sizeLimit)
        {
            this.dimensions = dimensions;

            // Init as leaf
            this.locations = new float[bucketSize][];
            this.data = new Object[bucketSize];
            this.locationCount = 0;
            this.singularity = true;

            // Init as root
            this.parent = null;
            this.sizeLimit = sizeLimit;
            if (sizeLimit != 0)
            {
                this.locationStack = new LinkedList<float[]>();
            }
            else
            {
                this.locationStack = null;
            }
        }

        /**
         * Constructor for child nodes. Internal use only.
         */
        private KdTree(KdTree<T> parent, bool right)
        {
            this.dimensions = parent.dimensions;

            // Init as leaf
            this.locations = new float[Math.Max(bucketSize, parent.locationCount)][];
            this.data = new Object[Math.Max(bucketSize, parent.locationCount)];
            this.locationCount = 0;
            this.singularity = true;

            // Init as non-root
            this.parent = parent;
            this.locationStack = null;
            this.sizeLimit = 0;
        }

        /**
         * Get the number of points in the tree
         */
        public int Size()
        {
            return locationCount;
        }

        /**
         * Add a point and associated value to the tree
         */
        public void AddPoint(float[] location, T value)
        {
            KdTree<T> cursor = this;

            while (cursor.locations == null || cursor.locationCount >= cursor.locations.Length)
            {
                if (cursor.locations != null)
                {
                    cursor.splitDimension = cursor.FindWidestAxis();
                    cursor.splitValue = (cursor.minLimit[cursor.splitDimension] + cursor.maxLimit[cursor.splitDimension]) * 0.5f;

                    // Never split on infinity or NaN
                    if (cursor.splitValue == float.PositiveInfinity)
                    {
                        cursor.splitValue = float.MaxValue;
                    }
                    else if (cursor.splitValue == float.NegativeInfinity)
                    {
                        cursor.splitValue = float.MinValue;
                    }
                    else if (float.IsNaN(cursor.splitValue))
                    {
                        cursor.splitValue = 0;
                    }

                    // Don't split node if it has no width in any axis. float the
                    // bucket size instead
                    if (cursor.minLimit[cursor.splitDimension] == cursor.maxLimit[cursor.splitDimension])
                    {
                        float[][] newLocations = new float[cursor.locations.Length * 2][];
                        System.Array.Copy(cursor.locations, 0, newLocations, 0, cursor.locationCount);
                        cursor.locations = newLocations;
                        Object[] newData = new Object[newLocations.Length];
                        System.Array.Copy(cursor.data, 0, newData, 0, cursor.locationCount);
                        cursor.data = newData;
                        break;
                    }

                    // Don't let the split value be the same as the upper value as
                    // can happen due to rounding errors!
                    if (cursor.splitValue == cursor.maxLimit[cursor.splitDimension])
                    {
                        cursor.splitValue = cursor.minLimit[cursor.splitDimension];
                    }

                    // Create child leaves
                    KdTree<T> left = new ChildNode(cursor, false);
                    KdTree<T> right = new ChildNode(cursor, true);

                    // Move locations into children
                    for (int i = 0; i < cursor.locationCount; i++)
                    {
                        float[] oldLocation = cursor.locations[i];
                        Object oldData = cursor.data[i];
                        if (oldLocation[cursor.splitDimension] > cursor.splitValue)
                        {
                            // Right
                            right.locations[right.locationCount] = oldLocation;
                            right.data[right.locationCount] = oldData;
                            right.locationCount++;
                            right.ExtendBounds(oldLocation);
                        }
                        else
                        {
                            // Left
                            left.locations[left.locationCount] = oldLocation;
                            left.data[left.locationCount] = oldData;
                            left.locationCount++;
                            left.ExtendBounds(oldLocation);
                        }
                    }

                    // Make into stem
                    cursor.left = left;
                    cursor.right = right;
                    cursor.locations = null;
                    cursor.data = null;
                }

                cursor.locationCount++;
                cursor.ExtendBounds(location);

                if (location[cursor.splitDimension] > cursor.splitValue)
                {
                    cursor = cursor.right;
                }
                else
                {
                    cursor = cursor.left;
                }
            }

            cursor.locations[cursor.locationCount] = location;
            cursor.data[cursor.locationCount] = value;
            cursor.locationCount++;
            cursor.ExtendBounds(location);

            if (this.sizeLimit != 0)
            {
                this.locationStack.AddLast(location);
                if (this.locationCount > this.sizeLimit)
                {
                    this.RemoveOld();
                }
            }
        }

        /**
         * Extends the bounds of this node do include a new location
         */
        private void ExtendBounds(float[] location)
        {
            if (minLimit == null)
            {
                minLimit = new float[dimensions];
                System.Array.Copy(location, 0, minLimit, 0, dimensions);
                maxLimit = new float[dimensions];
                System.Array.Copy(location, 0, maxLimit, 0, dimensions);
                return;
            }

            for (int i = 0; i < dimensions; i++)
            {
                if (float.IsNaN(location[i]))
                {
                    minLimit[i] = float.NaN;
                    maxLimit[i] = float.NaN;
                    singularity = false;
                }
                else if (minLimit[i] > location[i])
                {
                    minLimit[i] = location[i];
                    singularity = false;
                }
                else if (maxLimit[i] < location[i])
                {
                    maxLimit[i] = location[i];
                    singularity = false;
                }
            }
        }

        /**
         * Find the widest axis of the bounds of this node
         */
        private int FindWidestAxis()
        {
            int widest = 0;
            float width = (maxLimit[0] - minLimit[0]) * GetAxisWeightHint(0);
            if (float.IsNaN(width)) width = 0;
            for (int i = 1; i < dimensions; i++)
            {
                float nwidth = (maxLimit[i] - minLimit[i]) * GetAxisWeightHint(i);
                if (float.IsNaN(nwidth)) nwidth = 0;
                if (nwidth > width)
                {
                    widest = i;
                    width = nwidth;
                }
            }
            return widest;
        }

        /**
         * Remove the oldest value from the tree. Note: This cannot trim the bounds
         * of nodes, nor empty nodes, and thus you can't expect it to perfectly
         * preserve the speed of the tree as you keep adding.
         */
        private void RemoveOld()
        {
            float[] location = this.locationStack.First.Value;
            this.locationStack.RemoveFirst();

            KdTree<T> cursor = this;

            // Find the node where the point is
            while (cursor.locations == null)
            {
                if (location[cursor.splitDimension] > cursor.splitValue)
                {
                    cursor = cursor.right;
                }
                else
                {
                    cursor = cursor.left;
                }
            }

            for (int i = 0; i < cursor.locationCount; i++)
            {
                if (cursor.locations[i] == location)
                {
                    System.Array.Copy(cursor.locations, i + 1, cursor.locations, i, cursor.locationCount - i - 1);
                    cursor.locations[cursor.locationCount - 1] = null;
                    System.Array.Copy(cursor.data, i + 1, cursor.data, i, cursor.locationCount - i - 1);
                    cursor.data[cursor.locationCount - 1] = null;
                    do
                    {
                        cursor.locationCount--;
                        cursor = cursor.parent;
                    } while (cursor != null);
                    return;
                }
            }
            // If we got here... we couldn't find the value to remove. Weird...
        }

        /**
         * Enumeration representing the status of a node during the running
         */
        private enum Status
        {
            NONE, LEFTVISITED, RIGHTVISITED, ALLVISITED
        }

        /**
         * Stores a distance and value to output
         */
        public class Entry<TEntry>
        {
            public float distance;
            public TEntry value;

            internal Entry(float distance, TEntry value)
            {
                this.distance = distance;
                this.value = value;
            }
        }

        public void PopLast()
        {
            closestCursor.locationCount--;
            var parent = closestCursor.parent;
            while (parent != null)
            {
                parent.locationCount--;
                parent = parent.parent;
            }
            if (closestCursor.locationCount > 0)
            {
                closestCursor.locations[index] = closestCursor.locations[closestCursor.locationCount];
                closestCursor.data[index] = closestCursor.data[closestCursor.locationCount];
            }
            else
            {
                //Split(closestCursor.parent);
            }
        }

        public T FindClosest(float[] location, int offset)
        {
            KdTree<T> cursor = this;
            cursor.status = Status.NONE;
            float range = float.PositiveInfinity;
            ResultHeap resultHeap = new ResultHeap(1);
            do
            {
                if (cursor.status == Status.ALLVISITED)
                {
                    // At a fully visited part. Move up the tree
                    cursor = cursor.parent;
                    continue;
                }

                if (cursor.status == Status.NONE && cursor.locations != null)
                {
                    // At a leaf. Use the data.
                    if (cursor.locationCount > 0)
                    {
                        if (cursor.singularity)
                        {
                            float dist = PointDist(cursor.locations[0], location, offset);
                            if (dist <= range)
                            {
                                for (int i = 0; i < cursor.locationCount; i++)
                                {
                                    resultHeap.AddValue(dist, cursor.data[i]);
                                }
                            }
                        }
                        else
                        {
                            for (int i = 0; i < cursor.locationCount; i++)
                            {
                                float dist = PointDist(cursor.locations[i], location, offset);
                                if (resultHeap.AddValue(dist, cursor.data[i]))
                                {
                                    closestCursor = cursor;
                                    index = i;
                                }
                            }
                        }
                        range = resultHeap.GetMaxDist();
                    }

                    if (cursor.parent == null)
                    {
                        break;
                    }
                    cursor = cursor.parent;
                    continue;
                }

                // Going to descend
                KdTree<T> nextCursor = null;
                if (cursor.status == Status.NONE)
                {
                    // At a fresh node, descend the most probably useful direction
                    if (location[cursor.splitDimension + offset] > cursor.splitValue)
                    {
                        // Descend right
                        nextCursor = cursor.right;
                        cursor.status = Status.RIGHTVISITED;
                    }
                    else
                    {
                        // Descend left;
                        nextCursor = cursor.left;
                        cursor.status = Status.LEFTVISITED;
                    }
                }
                else if (cursor.status == Status.LEFTVISITED)
                {
                    // Left node visited, descend right.
                    nextCursor = cursor.right;
                    cursor.status = Status.ALLVISITED;
                }
                else if (cursor.status == Status.RIGHTVISITED)
                {
                    // Right node visited, descend left.
                    nextCursor = cursor.left;
                    cursor.status = Status.ALLVISITED;
                }

                // Check if it's worth descending. Assume it is if it's sibling has
                // not been visited yet.
                if (cursor.status == Status.ALLVISITED)
                {
                    if (nextCursor.locationCount == 0
                            || (!nextCursor.singularity && PointRegionDist(location, offset, nextCursor.minLimit,
                                    nextCursor.maxLimit) > range))
                    {
                        continue;
                    }
                }

                // Descend down the tree
                cursor = nextCursor;
                cursor.status = Status.NONE;
            } while (cursor.parent != null || cursor.status != Status.ALLVISITED);


            return (T)resultHeap.data[0];
        }

        private float[] GetValue(int index)
        {
            if (this.locations != null && index < locations.Length)
            {
                return locations[index];
            }
            if (index < left.locationCount)
            {
                return left.GetValue(index);
            }
            else
            {
                return right.GetValue(index - left.locationCount);
            }
        }

        private void Rebalance(KdTree<T> node)
        {
            if (node.left.locationCount > 0)
            {
                //ode.
            }

        }

        private float GetNewSplitValue(KdTree<T> node)
        {
            float splitValue = 0;
            if (node.left != null && node.left.locationCount > 0)
            {
                splitValue += GetNewSplitValue(node.left);
                for (int i = 0; i < node.left.locationCount; i++)
                {

                }
            }
            else if (node.right != null && node.right.locationCount > 0)
            {

            }
            return 0;
        }

        private void Split(KdTree<T> node)
        {
            var childNode = node.left.locationCount > 0 ? node.left : node.right;
            var left = node.left;
            var right = node.right;

            if (childNode.locationCount > 1)
            {
                var dim = childNode.parent.splitDimension;
                var splitValue = 0.0f;
                if (childNode.locations == null)
                {
                    node = childNode;
                }
                else
                {
                    for (int i = 0; i < childNode.locationCount; i++)
                    {
                        splitValue += childNode.locations[i][dim];
                    }
                    splitValue /= childNode.locationCount;
                    var count = childNode.locationCount;
                    childNode.locationCount = 0;
                    left.locationCount = 0;
                    right.locationCount = 0;
                    for (int i = 0; i < count; i++)
                    {
                        if (childNode.locations[i][dim] < splitValue)
                        {
                            left.locations[left.locationCount++] = childNode.locations[i];
                        }
                        else
                        {
                            right.locations[right.locationCount++] = childNode.locations[i];
                        }
                    }
                    childNode.parent.splitValue = splitValue;
                }
            }
            else
            {
                while (childNode.locations == null)
                {
                    childNode = childNode.left.locationCount > 0 ? childNode.left : childNode.right;
                }
                node.locations = childNode.locations;
                node.data = childNode.data;
                node.left = null;
                node.right = null;
            }

        }

        /**
         * Calculates the nearest 'count' points to 'location'
         */
        public List<Entry<T>> NearestNeighbor(float[] location, int offset, int count, bool sequentialSorting)
        {
            KdTree<T> cursor = this;
            cursor.status = Status.NONE;
            float range = float.PositiveInfinity;
            ResultHeap resultHeap = new ResultHeap(count);

            do
            {
                if (cursor.status == Status.ALLVISITED)
                {
                    // At a fully visited part. Move up the tree
                    cursor = cursor.parent;
                    continue;
                }

                if (cursor.status == Status.NONE && cursor.locations != null)
                {
                    // At a leaf. Use the data.
                    if (cursor.locationCount > 0)
                    {
                        if (cursor.singularity)
                        {
                            float dist = PointDist(cursor.locations[0], location, offset);
                            if (dist <= range)
                            {
                                for (int i = 0; i < cursor.locationCount; i++)
                                {
                                    resultHeap.AddValue(dist, cursor.data[i]);
                                }
                            }
                        }
                        else
                        {
                            for (int i = 0; i < cursor.locationCount; i++)
                            {
                                float dist = PointDist(cursor.locations[i], location, offset);
                                resultHeap.AddValue(dist, cursor.data[i]);
                            }
                        }
                        range = resultHeap.GetMaxDist();
                    }

                    if (cursor.parent == null)
                    {
                        break;
                    }
                    cursor = cursor.parent;
                    continue;
                }

                // Going to descend
                KdTree<T> nextCursor = null;
                if (cursor.status == Status.NONE)
                {
                    // At a fresh node, descend the most probably useful direction
                    if (location[cursor.splitDimension + offset] > cursor.splitValue)
                    {
                        // Descend right
                        nextCursor = cursor.right;
                        cursor.status = Status.RIGHTVISITED;
                    }
                    else
                    {
                        // Descend left;
                        nextCursor = cursor.left;
                        cursor.status = Status.LEFTVISITED;
                    }
                }
                else if (cursor.status == Status.LEFTVISITED)
                {
                    // Left node visited, descend right.
                    nextCursor = cursor.right;
                    cursor.status = Status.ALLVISITED;
                }
                else if (cursor.status == Status.RIGHTVISITED)
                {
                    // Right node visited, descend left.
                    nextCursor = cursor.left;
                    cursor.status = Status.ALLVISITED;
                }

                // Check if it's worth descending. Assume it is if it's sibling has
                // not been visited yet.
                if (cursor.status == Status.ALLVISITED)
                {
                    if (nextCursor.locationCount == 0
                            || (!nextCursor.singularity && PointRegionDist(location, offset, nextCursor.minLimit,
                                    nextCursor.maxLimit) > range))
                    {
                        continue;
                    }
                }

                // Descend down the tree
                cursor = nextCursor;
                cursor.status = Status.NONE;
            } while (cursor.parent != null || cursor.status != Status.ALLVISITED);

            List<Entry<T>> results = new List<Entry<T>>(resultHeap.values);
            if (sequentialSorting)
            {
                while (resultHeap.values > 0)
                {
                    resultHeap.RemoveLargest();
                    results.Add(new Entry<T>(resultHeap.removedDist, (T)resultHeap.removedData));
                }
            }
            else
            {
                for (int i = 0; i < resultHeap.values; i++)
                {
                    results.Add(new Entry<T>(resultHeap.distance[i], (T)resultHeap.data[i]));
                }
            }

            return results;
        }

        // Override in subclasses
        protected abstract float PointDist(float[] p1, float[] p2, int offset);

        protected abstract float PointRegionDist(float[] point, int offset, float[] min, float[] max);

        protected float GetAxisWeightHint(int i)
        {
            return 1.0f;
        }
        internal class ChildNode : KdTree<T>
        {
            internal ChildNode(KdTree<T> parent, bool right) : base(parent, right)
            {
            }

            // Distance measurements are always called from the root node

            protected override float PointDist(float[] p1, float[] p2, int offset)
            {
                throw new Exception();
            }

            protected override float PointRegionDist(float[] point, int offset, float[] min, float[] max)
            {
                throw new Exception();
            }
        }
    }
    /**
     * Internal class for child nodes
     */


    /**
     * Class for tree with Weighted Squared Euclidean distancing
     */
    public class WeightedSqrEuclid<T> : KdTree<T>
    {
        internal float[] weights;

        public WeightedSqrEuclid(int dimensions, int sizeLimit) : base(dimensions, sizeLimit)
        {
            this.weights = new float[dimensions];
            for (int i = 0; i < weights.Length; i++)
            {
                weights[i] = 1.0f;
            }
        }

        public void SetWeights(float[] weights)
        {
            this.weights = weights;
        }

        protected new float GetAxisWeightHint(int i)
        {
            return weights[i];
        }

        protected override float PointDist(float[] p1, float[] p2, int offset)
        {
            float d = 0;

            for (int i = 0; i < p1.Length; i++)
            {
                float diff = (p1[i] - p2[i + offset]) * weights[i];
                if (!float.IsNaN(diff))
                {
                    d += diff * diff;
                }
            }

            return d;
        }

        protected override float PointRegionDist(float[] point, int offset, float[] min, float[] max)
        {
            float d = 0;

            for (int i = 0; i < dimensions; i++)
            {
                float diff = 0;
                if (point[i + offset] > max[i])
                {
                    diff = (point[i + offset] - max[i]) * weights[i];
                }
                else if (point[i + offset] < min[i])
                {
                    diff = (point[i + offset] - min[i]) * weights[i];
                }

                if (!float.IsNaN(diff))
                {
                    d += diff * diff;
                }
            }

            return d;
        }
    }

    /**
     * Class for tree with Unweighted Squared Euclidean distancing
     */
    public class SqrEuclid<T> : KdTree<T>
    {
        public SqrEuclid(int dimensions, int sizeLimit) : base(dimensions, sizeLimit)
        {
        }

        protected override float PointDist(float[] p1, float[] p2, int offset)
        {
            float d = 0;

            for (int i = 0; i < p1.Length; i++)
            {
                float diff = (p1[i] - p2[i + offset]);
                if (!float.IsNaN(diff))
                {
                    d += diff * diff;
                }
            }

            return d;
        }

        protected override float PointRegionDist(float[] point, int offset, float[] min, float[] max)
        {
            float d = 0;

            for (int i = 0; i < dimensions; i++)
            {
                float diff = 0;
                if (point[i + offset] > max[i])
                {
                    diff = (point[i + offset] - max[i]);
                }
                else if (point[i + offset] < min[i])
                {
                    diff = (point[i + offset] - min[i]);
                }

                if (!float.IsNaN(diff))
                {
                    d += diff * diff;
                }
            }

            return d;
        }
    }

    /**
     * Class for tree with Weighted Manhattan distancing
     */
    public class WeightedManhattan<T> : KdTree<T>
    {
        private float[] weights;

        public WeightedManhattan(int dimensions, int sizeLimit) : base(dimensions, sizeLimit)
        {
            this.weights = new float[dimensions];
            //Array.Fill(this.weights, 1.0);
            for (int i = 0; i < weights.Length; i++)
            {
                weights[i] = 1.0f;
            }
        }

        public void SetWeights(float[] weights)
        {
            this.weights = weights;
        }

        protected new float GetAxisWeightHint(int i)
        {
            return weights[i];
        }

        protected override float PointDist(float[] p1, float[] p2, int offset)
        {
            float d = 0;

            for (int i = 0; i < p1.Length; i++)
            {
                float diff = (p1[i] - p2[i + offset]);
                if (!float.IsNaN(diff))
                {
                    d += ((diff < 0) ? -diff : diff) * weights[i];
                }
            }

            return d;
        }

        protected override float PointRegionDist(float[] point, int offset, float[] min, float[] max)
        {
            float d = 0;

            for (int i = 0; i < dimensions; i++)
            {
                float diff = 0;
                if (point[i + offset] > max[i])
                {
                    diff = (point[i + offset] - max[i]);
                }
                else if (point[i + offset] < min[i])
                {
                    diff = (min[i] - point[i + offset]);
                }

                if (!float.IsNaN(diff))
                {
                    d += diff * weights[i];
                }
            }

            return d;
        }
    }


    /**
     * Class for tree with Manhattan distancing
     */
    public class Manhattan<T> : KdTree<T>
    {
        public Manhattan(int dimensions, int sizeLimit) : base(dimensions, sizeLimit)
        {

        }



        protected override float PointDist(float[] p1, float[] p2, int offset)
        {
            float d = 0;

            for (int i = 0; i < p1.Length; i++)
            {
                float diff = (p1[i] - p2[i + offset]);
                if (!float.IsNaN(diff))
                {
                    d += (diff < 0) ? -diff : diff;
                }
            }

            return d;
        }

        protected override float PointRegionDist(float[] point, int offset, float[] min, float[] max)
        {
            float d = 0;

            for (int i = 0; i < dimensions; i++)
            {
                float diff = 0;
                if (point[i + offset] > max[i])
                {
                    diff = (point[i + offset] - max[i]);
                }
                else if (point[i + offset] < min[i])
                {
                    diff = (min[i] - point[i + offset]);
                }

                if (!float.IsNaN(diff))
                {
                    d += diff;
                }
            }

            return d;
        }

    }
    /**
     * Class for tracking up to 'size' closest values
     */
    internal class ResultHeap
    {
        internal Object[] data;
        internal float[] distance;
        internal int size;
        internal int values;
        public Object removedData;
        public float removedDist;

        public ResultHeap(int size)
        {
            this.data = new Object[size];
            this.distance = new float[size];
            this.size = size;
            this.values = 0;
        }

        public bool AddValue(float dist, Object value)
        {
            // If there is still room in the heap
            if (values < size)
            {
                // Insert new value at the end
                data[values] = value;
                distance[values] = dist;
                UpHeapify(values);
                values++;
                return true;
            }
            // If there is no room left in the heap, and the new entry is lower
            // than the max entry
            else if (dist < distance[0])
            {
                // Replace the max entry with the new entry
                data[0] = value;
                distance[0] = dist;
                DownHeapify(0);
                return true;
            }
            return false;
        }

        public void RemoveLargest()
        {
            if (values == 0)
            {
                throw new Exception();
            }

            removedData = data[0];
            removedDist = distance[0];
            values--;
            data[0] = data[values];
            distance[0] = distance[values];
            DownHeapify(0);
        }

        private void UpHeapify(int c)
        {
            for (int p = (c - 1) / 2; c != 0 && distance[c] > distance[p]; c = p, p = (c - 1) / 2)
            {
                Object pData = data[p];
                float pDist = distance[p];
                data[p] = data[c];
                distance[p] = distance[c];
                data[c] = pData;
                distance[c] = pDist;
            }
        }

        private void DownHeapify(int p)
        {
            for (int c = p * 2 + 1; c < values; p = c, c = p * 2 + 1)
            {
                if (c + 1 < values && distance[c] < distance[c + 1])
                {
                    c++;
                }
                if (distance[p] < distance[c])
                {
                    // Swap the points
                    Object pData = data[p];
                    float pDist = distance[p];
                    data[p] = data[c];
                    distance[p] = distance[c];
                    data[c] = pData;
                    distance[c] = pDist;
                }
                else
                {
                    break;
                }
            }
        }

        public float GetMaxDist()
        {
            if (values < size)
            {
                return float.PositiveInfinity;
            }
            return distance[0];
        }
    }
}

