using Priority_Queue;
using System.Collections.Generic;

namespace Engine.Processing
{
    public class QueueNode : FastPriorityQueueNode
    {
        public int id;
        public int PrevId { get; set; }
        public List<KeyValuePair<int, float>> Neighbors;

        public QueueNode(int id, List<KeyValuePair<int,float>> neighbors)
        {
            this.id = id;
            Neighbors = neighbors;
        }
    }
}
