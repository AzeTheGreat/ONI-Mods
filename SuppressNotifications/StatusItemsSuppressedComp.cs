using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SuppressNotifications
{
    class StatusItemsSuppressedComp : KMonoBehaviour
    {
        public List<string> SuppressedStatusItems { get; private set; }

        private StatusItemGroup statusItemGroup;

        protected override void OnPrefabInit()
        {
            SuppressedStatusItems = new List<string>();
            statusItemGroup = gameObject.GetComponent<KSelectable>().GetStatusItemGroup();
        }

        public void SuppressStatusItems()
        {
            List<StatusItem> suppressableStatusItems = GetSuppressableStatusItems();

            foreach (var item in suppressableStatusItems)
            {
                SuppressedStatusItems.Add(item.Name);
            }

            RefreshStatusItems(suppressableStatusItems);
        }

        public void UnsuppressStatusItems()
        {
            var suppressedStatusItems = GetSuppressedStatusItems();
            SuppressedStatusItems.Clear();
            RefreshStatusItems(suppressedStatusItems);
        }

        private void RefreshStatusItems(List<StatusItem> itemsToRefresh)
        {
            var statEnumerator = statusItemGroup.GetEnumerator();
            var entriesToRefresh = new List<StatusItemGroup.Entry>();

            using (statEnumerator)
            {
                while (statEnumerator.MoveNext())
                {
                    var entry = statEnumerator.Current;
                    if (itemsToRefresh.Contains(entry.item))
                    {
                        entriesToRefresh.Add(entry);
                    }
                }
            }

            foreach (var entry in entriesToRefresh)
            {
                statusItemGroup.SetStatusItem(entry.id, entry.category, entry.item, entry.data);
            }
        }
 
        public List<StatusItem> GetSuppressedStatusItems()
        {
            List<StatusItem> currentStatusItems = GetCurrentStatusItems();
            var suppressedStatusItems = new List<StatusItem>();

            foreach (var statusItem in currentStatusItems)
            {
                if (!ShouldShowIcon(statusItem))
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
            List<StatusItem> suppressableStatusItems = new List<StatusItem>();
            var statEnumerator = statusItemGroup.GetEnumerator();

            using (statEnumerator)
            {
                while (statEnumerator.MoveNext())
                {
                    StatusItem statusItem = statEnumerator.Current.item;
                    suppressableStatusItems.Add(statusItem);
                }
            }

            return suppressableStatusItems;
        }

        public bool ShouldShowIcon(StatusItem statusItem)
        {
            return statusItem.ShouldShowIcon() && !SuppressedStatusItems.Contains(statusItem.Name);
        }
    }
}
