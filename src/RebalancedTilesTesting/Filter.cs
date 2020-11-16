using AzeLib.Extensions;
using RebalancedTilesTesting.CustomUIComponents;
using System;
using System.Collections.Generic;

namespace RebalancedTilesTesting
{
    static class Filter
    {
        private static readonly List<Property> buildingDefProperties = new List<Property>()
        {
            new Property(nameof(BuildingDef.BaseDecor), "Decor"),
            new Property(nameof(BuildingDef.BaseDecorRadius), "Decor Radius")
        };

        public static IEnumerable<DefaultIntOptionsEntry> GetOptionsForDef(BuildingDef def)
        {
            foreach (var prop in buildingDefProperties)
            {
                yield return new DefaultIntOptionsEntry(prop.displayName,
                    string.Empty,
                    Convert.ToInt32(Options.Opts.configOptions.GetDefaultValue(def.PrefabID, prop.propertyId)),
                    def.PrefabID,
                    prop.propertyId,
                    def.GetRawName());
            }
        }

        private struct Property
        {
            public string propertyId;
            public string displayName;

            public Property(string propertyId, string displayName)
            {
                this.propertyId = propertyId;
                this.displayName = displayName;
            }
        }
    }
}
