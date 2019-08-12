using KSerialization;
using System.Collections.Generic;
using UnityEngine;

namespace SuppressNotifications
{
    [SerializationConfig(MemberSerialization.OptIn)]
    class StatusItemsSuppressedComp : KMonoBehaviour, ISaveLoadable
    {
        // Would be a property with private set, but that breaks serialization loading
        [Serialize]
        public List<string> suppressedStatusItemTitles;

        private StatusItemGroup statusItemGroup;

        protected override void OnPrefabInit()
        {
            suppressedStatusItemTitles = new List<string>();
            statusItemGroup = gameObject.GetComponent<KSelectable>().GetStatusItemGroup();
            Subscribe(-905833192, OnCopySettingsDelegate);
        }

        public void SuppressStatusItems()
        {
            List<StatusItem> suppressableStatusItems = GetSuppressableStatusItems();
            List<StatusItem> suppressedStatusItems = GetSuppressedStatusItems();

            foreach (var item in suppressableStatusItems)
            {
                suppressedStatusItemTitles.Add(item.Name);
            }

            RefreshStatusItems(suppressableStatusItems, suppressedStatusItems);
        }

        public void UnsuppressStatusItems()
        {
            List<StatusItem> suppressableStatusItems = GetSuppressableStatusItems();
            List<StatusItem> suppressedStatusItems = GetSuppressedStatusItems();

            suppressedStatusItemTitles.Clear();

            RefreshStatusItems(suppressableStatusItems, suppressedStatusItems);
        }

        private void RefreshStatusItems(List<StatusItem> wasActive, List<StatusItem> wasSuppressed)
        {
            foreach (var item in wasActive)
            {
                if(!ShouldShowIcon(item))
                    Game.Instance.RemoveStatusItem(statusItemGroup.gameObject.transform, item);
            }

            foreach (var item in wasSuppressed)
            {
                if(ShouldShowIcon(item))
                    Game.Instance.AddStatusItem(statusItemGroup.gameObject.transform, item);
            }
        }

        public List<StatusItem> GetSuppressedStatusItems()
        {
            List<StatusItem> currentStatusItems = GetCurrentStatusItems();
            var suppressedStatusItems = new List<StatusItem>();

            foreach (var statusItem in currentStatusItems)
            {
                if (!ShouldShowIcon(statusItem) && statusItem.ShouldShowIcon())
                {
                    suppressedStatusItems.Add(statusItem);
                }
            }

            return suppressedStatusItems;
        }

        public List<StatusItem> GetSuppressableStatusItems()
        {
            List<StatusItem> currentStatusItems = GetCurrentStatusItems();
            var suppressableStatusItems = new List<StatusItem>();

            foreach (var statusItem in currentStatusItems)
            {
                if (ShouldShowIcon(statusItem))
                {
                    suppressableStatusItems.Add(statusItem);
                }
            }

            return suppressableStatusItems;
        }

        private List<StatusItem> GetCurrentStatusItems()
        {
            List<StatusItem> currentStatusItems = new List<StatusItem>();
            var statEnumerator = statusItemGroup.GetEnumerator();

            using (statEnumerator)
            {
                while (statEnumerator.MoveNext())
                {
                    StatusItem statusItem = statEnumerator.Current.item;
                    currentStatusItems.Add(statusItem);
                }
            }

            return currentStatusItems;
        }

        public bool ShouldShowIcon(StatusItem statusItem)
        {
            return statusItem.ShouldShowIcon() && !suppressedStatusItemTitles.Contains(statusItem.Name);
        }

        private void OnCopySettings(object data)
        {
            GameObject gameObject = (GameObject)data;
            StatusItemsSuppressedComp comp = gameObject.GetComponent<StatusItemsSuppressedComp>();

            if (comp != null)
            {
                List<StatusItem> suppressableStatusItems = GetSuppressableStatusItems();
                List<StatusItem> suppressedStatusItems = GetSuppressedStatusItems();

                suppressedStatusItemTitles = new List<string>(comp.suppressedStatusItemTitles);
                RefreshStatusItems(suppressableStatusItems, suppressedStatusItems);
            }
        }

        private static readonly EventSystem.IntraObjectHandler<StatusItemsSuppressedComp> OnCopySettingsDelegate = new EventSystem.IntraObjectHandler<StatusItemsSuppressedComp>(Handler);

        private static void Handler(StatusItemsSuppressedComp comp, object data)
        {
            comp.OnCopySettings(data);
        }
    }
}
