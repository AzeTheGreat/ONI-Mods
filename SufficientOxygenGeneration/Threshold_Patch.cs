using Harmony;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace SufficientOxygenGeneration
{
    [HarmonyPatch(typeof(Tutorial), "SufficientOxygenLastCycleAndThisCycle")]
    public class Threshold_Patch
    {
        static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            Options.ReadSettings();
            bool second = false;

            foreach (CodeInstruction i in instructions)
            {
                if(i.opcode == OpCodes.Ldc_R4)
                {
                    if (second)
                        yield return new CodeInstruction(OpCodes.Ldc_R4, Options.options.OxygenThreshold);
                    else
                    {
                        second = true;
                        yield return i;
                    } 
                }
                else
                    yield return i;
            }
        }
    }
}
