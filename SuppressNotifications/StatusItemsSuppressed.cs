using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SuppressNotifications
{
    class StatusItemsSuppressed : KMonoBehaviour
    {
        List<StatusItem> suppressedStatusItems;

        protected override void OnPrefabInit()
        {
            suppressedStatusItems = new List<StatusItem>();
        }

        public void SuppressStatusItems()
        {
            List<StatusItem> suppressableStatusItems = GetSuppressableStatusItems();

            foreach (var item in suppressableStatusItems)
            {
                suppressedStatusItems.Add(item);
            }
        }

        public List<StatusItem> GetSuppressableStatusItems()
        {
            List<StatusItem> suppressableStatusItems = new List<StatusItem>();

            var statEnumerator = gameObject.GetComponent<KSelectable>()?.GetStatusItemGroup()?.GetEnumerator();
            if (statEnumerator == null)
                return suppressableStatusItems;

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
            return statusItem.ShouldShowIcon() && !suppressedStatusItems.Contains(statusItem);
        }
    }
}
