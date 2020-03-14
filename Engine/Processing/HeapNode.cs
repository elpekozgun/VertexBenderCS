using System.Collections.Generic;

namespace Engine.Processing
{
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
