//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using Harmony;
//using UnityEngine;

//namespace SuppressNotifications
//{
//    [HarmonyPatch(typeof(Game), nameof(Game.SetStatusItemOffset))]
//    class Patch_Game_SetStatusItemOffset
//    {
//        static void Prefix(TransformerNodeEditor transform, Vector3 offset)
//        {
//            if (transform.ToString() == "LiquidReservoirComplete (UnityEngine.Transform)")
//            {
//                Debug.Log("SET STATUS OFFSET");
//                Debug.Log(transform);
//                Debug.Log(offset);
//            }
//        }
//    }
//}
