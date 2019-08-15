using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Harmony;
using UnityEngine;

namespace SuppressNotifications
{
    //[HarmonyPatch(typeof(StatusItemRenderer), nameof(StatusItemRenderer.SetOffset))]
    //class Patch_StatusItemRenderer
    //{
    //    static void Prefix(Transform transform, Vector3 offset, StatusItemRenderer.Entry[] ___entries, Dictionary<int, int> ___handleTable)
    //    {
    //        if(transform.ToString() == "LiquidReservoirComplete (UnityEngine.Transform)")
    //        {
    //            Debug.Log("Transform: " + transform.ToString());
    //            Debug.Log("Offset: " + offset);

    //            int num = 0;
    //            if (___handleTable.TryGetValue(transform.GetInstanceID(), out num))
    //            {
    //                Debug.Log("Prev Offset: " + ___entries[num].offset);
    //            }
    //        }
    //    }
    //}
}
