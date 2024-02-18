using AzeLib.Attributes;
using HarmonyLib;
using PeterHan.PLib.Actions;

namespace BuildMenuSearchHotkey
{
    public class SearchHotkey
    {
        private static Action action;

        [OnLoad]
        public static void InitAction()
        {
            var manager = new PActionManager();
            var pAction = manager.CreateAction("AzeTestBuildSearchHotkey", "Build Search Hotkey", new(KKeyCode.E, Modifier.Ctrl));
            action = pAction.GetKAction();
        }

        [HarmonyPatch(typeof(PlanScreen), nameof(PlanScreen.OnKeyDown))]
        class ListenForAction
        {
            static void Postfix(KButtonEvent e)
            {
                if (PlanScreen.Instance.activeCategoryInfo == null)
                    return;

                if (e.TryConsume(action))
                    BuildingGroupScreen.Instance.inputField.Select();
            }
        }
    }
}
