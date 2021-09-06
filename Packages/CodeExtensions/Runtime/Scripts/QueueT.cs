using System.Collections.Generic;

namespace CodeExtensions
{
    public static class QueueT
    {
        public static bool IsEmpty<T>(this Queue<T> queue) => queue.Count == 0;
        
        public static bool IsNotEmpty<T>(this Queue<T> queue) => !IsEmpty(queue);

        public static void EnqueueRange<T>(this Queue<T> queue, IEnumerable<T> collection)
        {
            foreach (var item in collection)
                queue.Enqueue(item);
        }
    }
}