using KSerialization;
using System.Collections.Generic;

namespace SuppressNotifications
{
    [SerializationConfig(MemberSerialization.OptIn)]
    class StatusItemsSuppressedComp : KMonoBehaviour, ISaveLoadable
    {
        // Would be a property with private set, but that breaks serialization loading
        [Serialize]
        public List<string> suppressedStatusItems;

        private StatusItemGroup statusItemGroup;

        protected override void OnPrefabInit()
        {
            suppressedStatusItems = new List<string>();
            statusItemGroup = gameObject.GetComponent<KSelectable>().GetStatusItemGroup();
        }

        public void SuppressStatusItems()
        {
            List<StatusItem> suppressableStatusItems = GetSuppressableStatusItems();

            foreach (var item in suppressableStatusItems)
            {
                suppressedStatusItems.Add(item.Name);
            }

            RefreshStatusItems(suppressableStatusItems, false, true);
        }

        public void UnsuppressStatusItems()
        {
            List<StatusItem> suppressedStatusItems = GetSuppressedStatusItems();
            this.suppressedStatusItems.Clear();
            RefreshStatusItems(suppressedStatusItems, true, false);
        }

        private void RefreshStatusItems(List<StatusItem> itemsToRefresh, bool add, bool remove)
        {
            var statEnumerator = statusItemGroup.GetEnumerator();

            using (statEnumerator)
            {
                while (statEnumerator.MoveNext())
                {
                    var entry = statEnumerator.Current;

                    if (remove)
                        Game.Instance.RemoveStatusItem(statusItemGroup.gameObject.transform, entry.item);

                    if (add && ShouldShowIcon(entry.item))
                        Game.Instance.AddStatusItem(statusItemGroup.gameObject.transform, entry.item);

                    // Might be required to fix the offset visual bug, but is a pain to access
                    // And who knows
                    //Game.Instance.SetStatusItemOffset(statusItemGroup.gameObject.transform, statusItemGroup.)
                }
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
            return statusItem.ShouldShowIcon() && !suppressedStatusItems.Contains(statusItem.Name);
        }
    }
}
