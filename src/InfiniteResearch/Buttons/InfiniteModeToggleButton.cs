using KSerialization;

namespace InfiniteResearch
{
    [SerializationConfig(MemberSerialization.OptIn)]
    abstract class InfiniteModeToggleButton : KMonoBehaviour, ISaveLoadable
    {
        [Serialize] public bool isInfiniteMode = false;

        public override void OnPrefabInit() => Subscribe((int)GameHashes.RefreshUserMenu, (object data) => OnRefreshUserMenu());

        private void OnRefreshUserMenu()
        {
            if (isInfiniteMode)
            {
                string iconName = "action_building_disabled";
                string text = BUTTONS.DISABLELEARN.NAME;
                void on_click()
                {
                    isInfiniteMode = false;
                    UpdateState();
                }
                string tooltipText = BUTTONS.DISABLELEARN.TOOLTIP;

                Game.Instance.userMenu.AddButton(gameObject, new KIconButtonMenu.ButtonInfo(iconName, text, on_click, tooltipText: tooltipText));
                return;
            }
            else
            {
                string iconName = "action_building_disabled";
                string text = BUTTONS.ENABLELEARN.NAME;
                void on_click()
                {
                    isInfiniteMode = true;
                    UpdateState();
                }
                string tooltipText = BUTTONS.ENABLELEARN.TOOLTIP;

                Game.Instance.userMenu.AddButton(base.gameObject, new KIconButtonMenu.ButtonInfo(iconName, text, on_click, tooltipText: tooltipText), 0f);
                return;
            }
        }

        protected abstract void UpdateState();
    }
}
