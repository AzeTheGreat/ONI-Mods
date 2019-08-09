using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SuppressNotifications
{
    class StatusItemsSuppressed : KMonoBehaviour
    {
        public List<StatusItem> SuppressedStatusItems { get; private set; }
        private StatusItemGroup statusItemGroup;

        protected override void OnPrefabInit()
        {
            SuppressedStatusItems = new List<StatusItem>();
            statusItemGroup = gameObject.GetComponent<KSelectable>().GetStatusItemGroup();
        }

        public void SuppressStatusItems()
        {
            List<StatusItem> suppressableStatusItems = GetSuppressableStatusItems();

            SuppressedStatusItems.AddRange(suppressableStatusItems);
            RefreshStatusItems(suppressableStatusItems);
        }

        public void UnsuppressStatusItems()
        {
            var suppressedStatusItemsDuplicate = new List<StatusItem>(SuppressedStatusItems);
            SuppressedStatusItems.Clear();
            RefreshStatusItems(suppressedStatusItemsDuplicate);
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
                statusItemGroup.RemoveStatusItem(entry.id, true);
                statusItemGroup.AddStatusItem(entry.item, entry.data, entry.category);
            }
        }

        public List<StatusItem> GetSuppressableStatusItems()
        {
            List<StatusItem> suppressableStatusItems = new List<StatusItem>();

            var statEnumerator = statusItemGroup.GetEnumerator();

            using (statEnumerator)
            {
                while (statEnumerator.MoveNext())
                {
                    StatusItem statusItem = statEnumerator.Current.item;

                    if (ShouldShowIcon(statusItem))
                    {
                        suppressableStatusItems.Add(statusItem);
                    }
                }
            }

            return suppressableStatusItems;
        }

        public bool ShouldShowIcon(StatusItem statusItem)
        {
            return statusItem.ShouldShowIcon() && !SuppressedStatusItems.Contains(statusItem);
        }
    }
}
