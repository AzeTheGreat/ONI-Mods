using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BetterInfoCards
{
    class SelectableGroup
    {
        private List<KSelectable> selectables;

        public SelectableGroup(List<KSelectable> group)
        {
            selectables = group;
        }
    }
}
