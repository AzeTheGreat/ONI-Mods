using AzeLib.Attributes;
using HarmonyLib;
using PeterHan.PLib.Actions;
using System.Linq;

namespace BuildMenuSearchHotkey
{
    public class SearchHotkey
    {
        private static Action action;

        [OnLoad]
        public static void InitAction()
        {
            var manager = new PActionManager();
            var pAction = manager.CreateAction(typeof(SearchHotkey).FullName, MYSTRINGS.BUILDSEARCHHOTKEY.NAME, new(KKeyCode.E, Modifier.Ctrl));
            action = pAction.GetKAction();
        }

        [HarmonyPatch(typeof(PlanScreen), nameof(PlanScreen.OnKeyDown))]
        class ListenForAction
        {
            static void Postfix(KButtonEvent e)
            {    
                if (e.TryConsume(action))
                {
                    if (PlanScreen.Instance.activeCategoryInfo == null)
                    {
                        if (Options.Opts.HotkeyWorksWhenBuildMenuClosed)
                            PlanScreen.Instance.OpenCategoryPanel(PlanScreen.Instance.toggleEntries.First().toggleInfo);
                        else
                            return;
                    }

                    BuildingGroupScreen.Instance.inputField.Select();
                }
            }
        }
    }
}
