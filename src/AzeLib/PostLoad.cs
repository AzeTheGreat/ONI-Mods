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
            private static HarmonyInstance harmonyInstance;

            public static void PrePatch(HarmonyInstance instance) => harmonyInstance = instance;

            static void Postfix(Content content)
            {
                if (content != Content.Strings)
                    return;

                var instances = ReflectionHelpers.GetChildTypesOfType<PostLoad>().CreateAndGetInstances<PostLoad>();

                foreach (var instance in instances)
                {
                    var instanceType = instance.GetType();
                    foreach (var targetMethod in instance.PostLoadTargetMethods())
                    {
                        HarmonyMethod harmonyPrefix = null;
                        HarmonyMethod harmonyPostfix = null;
                        HarmonyMethod harmonyTranspiler = null;

                        if (AccessTools.Method(instanceType, "Prefix") != null)
                            harmonyPrefix = new HarmonyMethod(instanceType, "Prefix");
                        if (AccessTools.Method(instanceType, "Postfix") != null)
                            harmonyPostfix = new HarmonyMethod(instanceType, "Postfix");
                        if (AccessTools.Method(instanceType, "Transpiler") != null)
                            harmonyTranspiler = new HarmonyMethod(instanceType, "Transpiler");

                        harmonyInstance.Patch(targetMethod, harmonyPrefix, harmonyPostfix, harmonyTranspiler);
                    }
                }
            }
        }
    }
}
