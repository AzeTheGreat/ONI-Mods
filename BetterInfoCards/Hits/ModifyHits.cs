using Harmony;
using System.Collections.Generic;
using System.Reflection;

namespace BetterInfoCards
{
    public class ModifyHits
    {
        public static ModifyHits Instance { get; set; }

        public List<int> indexRedirect = new List<int>();
        public int localIndex = 0;
        public bool first = true;

        [HarmonyPatch]
        private class ChangeHits_Patch
        {
            static MethodBase TargetMethod()
            {
                return AccessTools.Method(typeof(InterfaceTool), "GetObjectUnderCursor").MakeGenericMethod(typeof(KSelectable));
            }

            static void Postfix(bool cycleSelection, ref KSelectable __result, ref int ___hitCycleCount, List<InterfaceTool.Intersection> ___intersections)
            {
                Debug.Log(1);
                if (__result == null)
                    return;

                if (cycleSelection)
                {
                    if (!Instance.first)
                    {
                        Instance.localIndex++;
                        if (Instance.localIndex > Instance.indexRedirect.Count - 1)
                            Instance.localIndex = 0;
                    }
                    Instance.first = false;
                }

                Debug.Log("Local Index: " + Instance.localIndex);
                Debug.Log("Redirects: " + Instance.indexRedirect.Count);

                int targetIndex = Instance.indexRedirect[Instance.localIndex];
                Debug.Log("Target Index: " + targetIndex);
                Debug.Log("Intersections: " + ___intersections.Count);
                __result = ___intersections[targetIndex].component as KSelectable;
                Debug.Log(2);
            }
        }
    }

    
}
