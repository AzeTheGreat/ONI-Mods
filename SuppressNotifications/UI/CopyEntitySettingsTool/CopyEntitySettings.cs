using UnityEngine;
using STRINGS;

namespace SuppressNotifications
{
    class CopyEntitySettings : KMonoBehaviour
    {
        protected override void OnPrefabInit()
        {
            base.OnPrefabInit();
            Subscribe((int)GameHashes.RefreshUserMenu, OnRefreshUserMenuDelegate);
        }

        private void OnRefreshUserMenu(object data)
        {
            UserMenu userMenu = Game.Instance.userMenu;
            GameObject gameObject = base.gameObject;
            string iconName = "action_mirror";
            string text = UI.USERMENUACTIONS.COPY_BUILDING_SETTINGS.NAME;
            System.Action on_click = new System.Action(this.ActivateCopyTool);
            global::Action shortcutKey = global::Action.BuildingUtility1;
            string tooltipText = UI.USERMENUACTIONS.COPY_BUILDING_SETTINGS.TOOLTIP;
            userMenu.AddButton(gameObject, new KIconButtonMenu.ButtonInfo(iconName, text, on_click, shortcutKey, null, null, null, tooltipText, true), 1f);
        }

        private void ActivateCopyTool()
        {
            CopyEntitySettingsTool.instance.SetSourceObject(base.gameObject);
            CopyEntitySettingsTool.instance.Activate();
        }

        private static readonly EventSystem.IntraObjectHandler<CopyEntitySettings> OnRefreshUserMenuDelegate =
            new EventSystem.IntraObjectHandler<CopyEntitySettings>(Handler);

        private static void Handler(CopyEntitySettings component, object data)
        {
            component.OnRefreshUserMenu(data);
        }
    }
}
