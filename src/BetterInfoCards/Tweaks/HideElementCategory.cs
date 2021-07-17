using AzeLib.Extensions;
using HarmonyLib;
using System.Collections.Generic;
using System.Linq;

namespace BetterInfoCards
{
    public class HideElementCategory
    {
        public static IEnumerable<CodeInstruction> ChildTranspiler(IEnumerable<CodeInstruction> codes)
        {
            var getMaterialCategory_method = AccessTools.Method(typeof(Element), nameof(Element.GetMaterialCategoryTag));
            var isVacuum_getter = AccessTools.PropertyGetter(typeof(Element), nameof(Element.IsVacuum));

            var target = codes.Last(i => i.Calls(getMaterialCategory_method))
                .FindPrior(codes, i => i.Calls(isVacuum_getter));

            return codes.Manipulator(
                i => i == target,
                i => new[]
                {
                    i,
                    CodeInstruction.Call(typeof(HideElementCategory), nameof(Splice))
                });
        }

        private static bool Splice(bool isVacuum) => Options.Opts.HideElementCategories ?  true : isVacuum;
    }
}
