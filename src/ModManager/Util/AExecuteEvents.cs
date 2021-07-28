using UnityEngine;
using UnityEngine.EventSystems;

namespace ModManager
{
    public static class AExecuteEvents
    {
        public static void ExecuteOnEntireHierarchy<T>(GameObject go, System.Action<T> functor) where T : IEventSystemHandler => ExecuteOnEntireHierarchy<T>(go, null, (x, d) => functor(x));
        public static void ExecuteOnEntireHierarchy<T>(GameObject go, BaseEventData data, ExecuteEvents.EventFunction<T> functor) where T : IEventSystemHandler
        {
            var rootGO = go.transform.root.gameObject;
            ExecuteOnChildren(rootGO);

            void ExecuteOnChildren(GameObject go)
            {
                foreach (Transform tf in go.transform)
                {
                    var child = tf.gameObject;
                    ExecuteEvents.Execute(child, data, functor);
                    ExecuteOnChildren(child);
                }
            }
        }

        public static void ExecuteHierarchy<T>(GameObject go, System.Action<T> functor) where T : IEventSystemHandler => ExecuteEvents.ExecuteHierarchy<T>(go, null, (x, d) => functor(x));
    }
}
