using Harmony;
using System;
using System.Linq;
using System.Reflection;

namespace AzeLib
{
    class OnModLoad
    {
        // Can't just put OnLoad inside BaseOptions because it's generic.
        public static void OnLoad()
        {
            // Assume only one Options per assembly
            if (ReflectionHelpers.GetChildTypesOfGenericType(typeof(BaseOptions<>)).FirstOrDefault() is Type optionType)
                AccessTools.Method(optionType, "OnOnLoad").Invoke(null, null);

            Debug.Log("    - version: " + Assembly.GetExecutingAssembly().GetName().Version);
        }
    }
}