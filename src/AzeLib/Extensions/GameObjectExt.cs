using Harmony;
using UnityEngine;

namespace AzeLib.Extensions
{
    public static class GameObjectExt
    {
        public static Component GetReflectionComp(this GameObject go, string typeString)
        {
            var type = AccessTools.TypeByName(typeString);
            if (type == null)
                return null;
            else
                return go.GetComponent(type);
        }
    }
}
