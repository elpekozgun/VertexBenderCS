using Priority_Queue;

namespace Engine.Processing
{
    public class QueueNode : FastPriorityQueueNode
    {

        public int id;
        public float distance;

        public QueueNode(int id, float distance)
        {
            this.id = id;
            this.distance = distance;
        }
    }
}
