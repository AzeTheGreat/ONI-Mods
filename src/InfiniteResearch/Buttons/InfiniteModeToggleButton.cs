using Harmony;
using KSerialization;

namespace InfiniteResearch
{
    [SerializationConfig(MemberSerialization.OptIn)]
    abstract class InfiniteModeToggleButton : KMonoBehaviour, ISaveLoadable
    {
        [Serialize] public bool isInfiniteMode = false;

        protected override void OnPrefabInit() => Subscribe((int)GameHashes.RefreshUserMenu, OnRefreshUserMenuDelegate);

        private void OnRefreshUserMenu()
        {
            if (isInfiniteMode)
            {
                string iconName = "action_building_disabled";
                string text = "Disable Learning";
                void on_click()
                {
                    isInfiniteMode = false;
                    UpdateState();
                }
                string tooltipText = "Stop dupes from training their Science Attribute here.";

                Game.Instance.userMenu.AddButton(base.gameObject, new KIconButtonMenu.ButtonInfo(iconName, text, on_click, tooltipText: tooltipText));
                return;
            }
            else
            {
                string iconName = "action_building_enabled";
                string text = "Enable Learning";
                void on_click()
                {
                    isInfiniteMode = true;
                    UpdateState();
                }
                string tooltipText = "Allow dupes to train their Science Attribute here.";

                Game.Instance.userMenu.AddButton(base.gameObject, new KIconButtonMenu.ButtonInfo(iconName, text, on_click, tooltipText: tooltipText));
                return;
            }
        }

        protected abstract void UpdateState();

        private static readonly EventSystem.IntraObjectHandler<InfiniteModeToggleButton> OnRefreshUserMenuDelegate =
            new EventSystem.IntraObjectHandler<InfiniteModeToggleButton>(Handler);

        private static void Handler(InfiniteModeToggleButton component, object data) => component.OnRefreshUserMenu();
    }
}
