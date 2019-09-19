using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Harmony;
using UnityEngine;

namespace SuppressNotifications.Patches
{
    [HarmonyPatch(typeof(PlayerController), "OnPrefabInit")]
    public static class PlayerController_OnPrefabInit_Patch
    {
        static void Postfix(PlayerController __instance)
        {
            var interfaceTools = new List<InterfaceTool>(__instance.tools);
            var critterCopyTool = new GameObject(typeof(CopyCritterSettingsTool).Name);
            critterCopyTool.AddComponent<CopyCritterSettingsTool>();

            // Reparent tool to the player controller, then enable/disable to load it
            critterCopyTool.transform.SetParent(__instance.gameObject.transform);
            critterCopyTool.gameObject.SetActive(true);
            critterCopyTool.gameObject.SetActive(false);

            interfaceTools.Add(critterCopyTool.GetComponent<InterfaceTool>());
            __instance.tools = interfaceTools.ToArray();
        }
    }
}
