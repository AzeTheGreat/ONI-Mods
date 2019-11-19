using Harmony;
using System.Collections.Generic;

namespace BetterInfoCards
{
    [HarmonyPatch(typeof(InterfaceTool), nameof(InterfaceTool.GetSelectablesUnderCursor))]
    class ChangeHits_Patch
    {
        static void Postfix(List<KSelectable> hits)
        {
            hits.Sort((x,y) => x.entityName.CompareTo(y.entityName));
        }
    }
}
