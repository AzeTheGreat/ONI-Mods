using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KSerialization;
using STRINGS;

namespace SuppressNotifications
{
    public class BuildingNotificationButton: KMonoBehaviour, ISaveLoadable
    {
        protected override void OnPrefabInit()
        {
            base.Subscribe<BuildingNotificationButton>(493375141, BuildingNotificationButton.OnRefreshUserMenuDelegate);
        }

        private void OnRefreshUserMenu(object data)
        {
            KIconButtonMenu.ButtonInfo button;

            string iconName = "action_building_disabled";
            string text = "Test?";
            System.Action on_click = new System.Action(this.OnMenuToggle);
            //global::Action shortcutKey = global::Action.ToggleEnabled;
            button = new KIconButtonMenu.ButtonInfo(iconName, text, on_click);

            Game.Instance.userMenu.AddButton(base.gameObject, button);
            Debug.Log("Yes");
        }

        private void OnMenuToggle()
        {

        }

        private static readonly EventSystem.IntraObjectHandler<BuildingNotificationButton> OnRefreshUserMenuDelegate = new EventSystem.IntraObjectHandler<BuildingNotificationButton>(Handler);
           
        private static void Handler(BuildingNotificationButton component, object data)
        {
            component.OnRefreshUserMenu(data);
        }
    }
}
