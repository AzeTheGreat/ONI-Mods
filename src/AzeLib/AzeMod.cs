using HarmonyLib;
using KMod;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AzeLib
{
    public sealed class AzeMod : UserMod2
    {
        public static UserMod2 UserMod { get; private set; }

        public override void OnAllModsLoaded(Harmony harmony, IReadOnlyList<Mod> mods)
        {
            base.OnAllModsLoaded(harmony, mods);
        }

        public override void OnLoad(Harmony harmony)
        {
            UserMod = this;
            OnOnLoad();
            harmony.PatchAll(UserMod.assembly);
            //base.OnLoad(harmony);
        }

        private void OnOnLoad()
        {
            // Assume only one Options per assembly
            if (ReflectionHelpers.GetChildTypesOfGenericType(typeof(BaseOptions<>)).FirstOrDefault() is Type optionType)
                AccessTools.Method(optionType, "OnLoad").Invoke(null, null);

            Debug.Log("    - version: " + UserMod.assembly.GetName().Version);
        }
    }
}
