using KSerialization;
using UnityEngine;

namespace InfiniteResearch
{
    [SerializationConfig(MemberSerialization.OptIn)]
    abstract class InfiniteModeToggleButton : KMonoBehaviour, ISaveLoadable
    {
        [Serialize] public bool isInfiniteMode = false;

        public override void OnPrefabInit()
        {
            Subscribe((int)GameHashes.RefreshUserMenu, (object data) => OnRefreshUserMenu());
            Subscribe((int)GameHashes.CopySettings, (object data) => OnCopySettings(data));
        }

        private void OnRefreshUserMenu()
        {
            if (isInfiniteMode)
            {
                string iconName = "action_building_disabled";
                string text = MYSTRINGS.BUTTONS.DISABLELEARN.NAME;
                void on_click()
                {
                    isInfiniteMode = false;
                    UpdateState();
                }
                string tooltipText = MYSTRINGS.BUTTONS.DISABLELEARN.TOOLTIP;

                Game.Instance.userMenu.AddButton(gameObject, new KIconButtonMenu.ButtonInfo(iconName, text, on_click, tooltipText: tooltipText));
                return;
            }
            else
            {
                string iconName = "action_building_disabled";
                string text = MYSTRINGS.BUTTONS.ENABLELEARN.NAME;
                void on_click()
                {
                    isInfiniteMode = true;
                    UpdateState();
                }
                string tooltipText = MYSTRINGS.BUTTONS.ENABLELEARN.TOOLTIP;

                Game.Instance.userMenu.AddButton(gameObject, new KIconButtonMenu.ButtonInfo(iconName, text, on_click, tooltipText: tooltipText));
                return;
            }
        }

        private void OnCopySettings(object sourceObj)
        {
            if((sourceObj as GameObject).GetComponent<InfiniteModeToggleButton>() is var comp)
                isInfiniteMode = comp.isInfiniteMode;
        }

        protected abstract void UpdateState();
    }
}
