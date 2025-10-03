using System;
using System.Reflection;
using Database;
using HarmonyLib;

namespace BetterInfoCards
{
    [HarmonyPatch(typeof(MiscStatusItems))]
    class ChangeStatusItemOverlays
    {
        static MethodBase TargetMethod()
        {
            var type = typeof(MiscStatusItems);

            foreach (var name in new[]
            {
                "CreateStatusItems",
                "PopulateStatusItems",
                "InitializeStatusItems",
                "InitStatusItems",
                "ConfigureStatusItems"
            })
            {
                var method = AccessTools.DeclaredMethod(type, name);
                if (method != null)
                    return method;
            }

            var oreTempField = AccessTools.DeclaredField(type, nameof(MiscStatusItems.OreTemp));
            var unreachableField = AccessTools.DeclaredField(type, nameof(MiscStatusItems.PickupableUnreachable));
            var categoryField = AccessTools.DeclaredField(type, nameof(MiscStatusItems.ElementalCategory));

            if (oreTempField == null || unreachableField == null || categoryField == null)
                return null;

            var oreTempToken = oreTempField.MetadataToken;
            var unreachableToken = unreachableField.MetadataToken;
            var categoryToken = categoryField.MetadataToken;

            foreach (var method in AccessTools.GetDeclaredMethods(type))
            {
                if (method.IsAbstract || method.IsConstructor)
                    continue;

                var body = method.GetMethodBody();
                if (body == null)
                    continue;

                var il = body.GetILAsByteArray();
                if (il == null || il.Length == 0)
                    continue;

                if (ContainsToken(il, oreTempToken) &&
                    ContainsToken(il, unreachableToken) &&
                    ContainsToken(il, categoryToken))
                    return method;
            }

            return null;
        }

        static bool ContainsToken(byte[] il, int token)
        {
            if (il == null || il.Length < 4)
                return false;

            var tokenBytes = BitConverter.GetBytes(token);
            for (var i = 0; i <= il.Length - tokenBytes.Length; i++)
            {
                var matches = true;
                for (var j = 0; j < tokenBytes.Length; j++)
                {
                    if (il[i + j] != tokenBytes[j])
                    {
                        matches = false;
                        break;
                    }
                }

                if (matches)
                    return true;
            }

            return false;
        }

        static void Postfix(MiscStatusItems __instance)
        {
            // Prevents the duplicate temp status from being drawn in the temp overlay.
            __instance.OreTemp.status_overlays &= ~(int)StatusItem.StatusItemOverlays.Temperature;
            // Prevent the unreachable status from being drawn on any overlay.
            __instance.PickupableUnreachable.status_overlays = 0;

            if (Options.Opts.HideElementCategories)
                __instance.ElementalCategory.status_overlays = 0;
        }
    }
}
