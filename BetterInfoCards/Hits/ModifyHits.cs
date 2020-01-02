using Harmony;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace BetterInfoCards
{
    public class ModifyHits
    {
        public static ModifyHits Instance { get; set; }

        private List<int> indexRedirects = new List<int>();
        private int localIndex = -1;
        private bool first = true;
        private List<MonoBehaviour> intersections = new List<MonoBehaviour>();
        private List<MonoBehaviour> priorSelectedComps = new List<MonoBehaviour>();

        [HarmonyPatch]
        private class ChangeHits_Patch
        {
            static bool Prepare() => true;

            static MethodBase TargetMethod()
            {
                return AccessTools.Method(typeof(InterfaceTool), "GetObjectUnderCursor").MakeGenericMethod(typeof(KSelectable));
            }

            static void Postfix(bool cycleSelection, ref KSelectable __result, ref int ___hitCycleCount, List<InterfaceTool.Intersection> ___intersections)
            {
                if (__result == null)
                    return;

                if (Instance.first)
                {
                    Instance.first = false;
                    Instance.intersections = ___intersections.Select(x => x.component).ToList();

                    Instance.SetNewIndex();
                }

                if (cycleSelection)
                {
                    Instance.localIndex++;
                    if (Instance.localIndex > Instance.indexRedirects.Count - 1)
                        Instance.localIndex = 0;
                }

                int targetIndex = 0;
                if (Instance.localIndex != -1)
                {
                    targetIndex = Instance.indexRedirects[Instance.localIndex];
                }

                __result = ___intersections[targetIndex].component as KSelectable;
            }
        }

        public void Reset(List<int> redirects)
        {
            if (indexRedirects.Count > 0 && localIndex != -1)
            {
                int index = indexRedirects[localIndex];
                priorSelectedComps = intersections.GetRange(index, GetRedirectCount(localIndex));
            }

            indexRedirects = redirects;
            localIndex = -1;
            first = true;
        }

        private void SetNewIndex()
        {
            int index = -1;
            foreach (MonoBehaviour comp in priorSelectedComps)
            {
                index = intersections.FindIndex(x => ReferenceEquals(x, comp));
                if (index != -1)
                    break;
            }

            if (index != -1)
            {
                for (int i = 0; i < indexRedirects.Count; i++)
                {
                    int start = indexRedirects[i];
                    int end = start + GetRedirectCount(i);

                    if (index >= start && index < end)
                        localIndex = i;
                }
            }
            else
                localIndex = -1;

            priorSelectedComps.Clear();
        }

        private int GetRedirectCount(int index)
        {
            int count = 0;
            if (index + 1 >= indexRedirects.Count)
                count = intersections.Count - 2 - indexRedirects[index];
            else
                count = indexRedirects[index + 1] - indexRedirects[index];

            return count;
        }
    }
}
