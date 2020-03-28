using Harmony;
using HarmonyAnalyzers.Interface;
using KMod;
using System.Collections.Generic;
using System.Reflection;

namespace AzeLib
{
    [HarmonyPatchMock]
    public abstract class PostLoad
    {
        protected abstract IEnumerable<MethodBase> PostLoadTargetMethods();

        [HarmonyPatch(typeof(Manager), nameof(Manager.Load))]
        private class Load_Patch
        {
            static void Postfix(Content content)
            {
                if (content != Content.Strings)
                    return;

                HarmonyInstance harmonyInstance = HarmonyInstance.Create("AzePostLoad");

                var instances = ReflectionHelpers.GetChildInstanceForType<PostLoad>();

                foreach (var instance in instances)
                {
                    foreach (var targetMethod in instance.PostLoadTargetMethods())
                    {
                        HarmonyMethod harmonyPrefix = null;
                        HarmonyMethod harmonyPostfix = null;
                        HarmonyMethod harmonyTranspiler = null;

                        // GetCustomAttributes on MethodInfo
                        if (AccessTools.Method(instance.GetType(), "Prefix") != null)
                            harmonyPrefix = new HarmonyMethod(instance.GetType(), "Prefix");
                        if (AccessTools.Method(instance.GetType(), "Postfix") != null)
                            harmonyPostfix = new HarmonyMethod(instance.GetType(), "Postfix");
                        if (AccessTools.Method(instance.GetType(), "Transpiler") != null)
                            harmonyTranspiler = new HarmonyMethod(instance.GetType(), "Transpiler");

                        harmonyInstance.Patch(targetMethod, harmonyPrefix, harmonyPostfix, harmonyTranspiler);
                    }
                }
            }
        }
    }
}
