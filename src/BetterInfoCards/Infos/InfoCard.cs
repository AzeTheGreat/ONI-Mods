using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace BetterInfoCards
{
    public class InfoCard
    {
        public bool isSelected;
        public KSelectable selectable;
        public List<DrawerInfo> infos = new();
        public Dictionary<string, DrawerInfo.Text> textInfos = new();

        public string GetTitleKey()
        {
            return textInfos.Values.FirstOrDefault()?.textInfo.Text.RemoveCountSuffix() ?? string.Empty;
        }

        public void Draw(List<InfoCard> cards)
        {
            foreach (var info in infos)
            {
                info.Draw(cards);
            }
        }
    }
}
