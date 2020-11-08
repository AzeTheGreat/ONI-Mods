using Harmony;
using KMod;
using PeterHan.PLib;
using PeterHan.PLib.Options;
using PeterHan.PLib.UI;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Reflection;
using UnityEngine;

namespace RebalancedTilesTesting
{
    [HarmonyPatch(typeof(ModsScreen), nameof(ModsScreen.BuildDisplay))]
    class AddOptionButton
    {
        static void Postfix(List<ModsScreen.DisplayedMod> ___displayedMods)
        {
            foreach (var modEntry in ___displayedMods)
            {
                var mod = Global.Instance.modManager.mods[modEntry.mod_index];
                if (IsThisMod(mod))
                    AddButton(modEntry, mod);
            }
                
        }

        private static bool IsThisMod(Mod mod) => Path.GetFileName(mod.file_source.GetRoot()) 
            == Path.GetFileName(POptions.GetModDir(Assembly.GetExecutingAssembly()));

        private static void AddButton(ModsScreen.DisplayedMod modEntry, Mod mod)
        {
            new PButton()
            {
                FlexSize = Vector2.up,
                OnClick = (GameObject realized) => new EmbeddedOptions().GetDialog().Activate(),
                ToolTip = PUIStrings.DIALOG_TITLE.text.F(new object[] { mod.title }),
                Text = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(PUIStrings.BUTTON_OPTIONS.text.ToLower())
            }
            .SetKleiPinkStyle()
            .AddTo(modEntry.rect_transform.gameObject, 3); // POptions.AddModOptions 
        }
    }
}
