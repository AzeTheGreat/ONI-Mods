using AzeLib.Attributes;
using HarmonyLib;
using KMod;
using PeterHan.PLib.Core;
using System.Linq;
using System.Reflection;

namespace AzeLib
{
    public static class AzeMod
    {
        public static UserMod2 UserMod { get; set; }

        // Only one class per assembly may inherit from UserMod2 or the game will crash.
        // The game can't find a private constructor, do not instantiate this manually.
        private sealed class AzeUserMod : UserMod2
        {
            // TODO: Benchmark and make sure this isn't too bad for load times.
            public override void OnLoad(Harmony harmony)
            {
                UserMod = this;
                Debug.Log("    - version: " + UserMod.assembly.GetName().Version);
                PUtil.InitLibrary(false);

                var onLoadMethods = assembly.GetTypes()
                    .SelectMany(x => x.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static))
                    .Where(x => x.GetCustomAttribute<OnLoadAttribute>() != null)
                    .ToList();

                foreach (var method in onLoadMethods)
                    method.Invoke(null, null);

                base.OnLoad(harmony);
            }
        }
    }
}
