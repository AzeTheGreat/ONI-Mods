using System.Collections.Generic;

namespace BetterInfoCards
{
    public class ResetPool<T> where T : new()
    {
        private List<T> pool = new();
        private int currentlyUsedIndex;

        public ResetPool(ref System.Action resetOn)
        {
            resetOn += () => Reset();
        }

        public T Get()
        {
            T obj;
            if (currentlyUsedIndex < pool.Count)
                obj = pool[currentlyUsedIndex];
            else
                pool.Add(obj = new());

            currentlyUsedIndex++;
            return obj;
        }

        // TODO: Implement pool downsizing?
        private void Reset()
        {
            currentlyUsedIndex = 0;
        }
    }
}
