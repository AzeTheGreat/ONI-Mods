using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SuppressNotifications
{
    class StatusItemsSuppressed : KMonoBehaviour
    {
        private List<StatusItem> suppressedStatusItems;
        private StatusItemGroup statusItemGroup;

        protected override void OnPrefabInit()
        {
            suppressedStatusItems = new List<StatusItem>();
            statusItemGroup = gameObject.GetComponent<KSelectable>().GetStatusItemGroup();
        }

        public void SuppressStatusItems()
        {
            List<StatusItemGroup.Entry> suppressableStatusItems = GetSuppressableStatusItems();

            foreach (var entry in suppressableStatusItems)
            {
                suppressedStatusItems.Add(entry.item);
                statusItemGroup.RemoveStatusItem(entry.id, true);
                statusItemGroup.AddStatusItem(entry.item);
            }
        }

        public List<StatusItemGroup.Entry> GetSuppressableStatusItems()
        {
            List<StatusItemGroup.Entry> suppressableStatusItems = new List<StatusItemGroup.Entry>();

            var statEnumerator = statusItemGroup.GetEnumerator();

            using (statEnumerator)
            {
                while (statEnumerator.MoveNext())
                {
                    StatusItemGroup.Entry statusItemEntry = statEnumerator.Current;

                    if (ShouldShowIcon(statusItemEntry.item))
                    {
                        suppressableStatusItems.Add(statusItemEntry);
                    }
                }
            }

            return suppressableStatusItems;
        }

        public bool ShouldShowIcon(StatusItem statusItem)
        {
            return statusItem.ShouldShowIcon() && !suppressedStatusItems.Contains(statusItem);
        }
    }
}
