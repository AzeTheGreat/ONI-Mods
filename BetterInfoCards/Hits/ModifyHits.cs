using Harmony;
using System.Collections.Generic;
using System.Reflection;

namespace BetterInfoCards
{
    public class ModifyHits
    {
        public static ModifyHits Instance { get; set; }

        public List<int> indexRedirect = new List<int>();
        private int lastIndex = 0;
        public int localIndex = 0;

        [HarmonyPatch]
        private class ChangeHits_Patch
        {
            static MethodBase TargetMethod()
            {
                return AccessTools.Method(typeof(InterfaceTool), "GetObjectUnderCursor").MakeGenericMethod(typeof(KSelectable));
            }

            static void Postfix(ref KSelectable __result, ref int ___hitCycleCount, List<InterfaceTool.Intersection> ___intersections)
            {
                if (__result == null)
                    return;
                
                int index = ___hitCycleCount % ___intersections.Count;
                if (index != Instance.lastIndex)
                {
                    Instance.localIndex++;
                }
                if (index == 0)
                    Instance.localIndex = 0;

                int targetIndex = Instance.indexRedirect[Instance.localIndex];
                Instance.lastIndex = ___hitCycleCount = targetIndex;

                __result = ___intersections[targetIndex].component as KSelectable;
            }
        }
    }

    
}
