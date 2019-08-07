using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Harmony;

namespace SuppressNotifications
{
    [HarmonyPatch(typeof(GeneratedBuildings), "LoadGeneratedBuildings")]
    internal class Class1
    {
        private static void Postfix()
        {
            Debug.Log("Hellow World!");
        }
    }
}
