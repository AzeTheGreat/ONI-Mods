using Harmony;
using System.Collections.Generic;
using System.Linq;

namespace RebalancedTilesTesting
{
    class Filter
    {
        private static readonly List<Property> buildingDefProperties = new List<Property>()
        {
            new Property(nameof(BuildingDef.BaseDecor), "Decor"),
            new Property(nameof(BuildingDef.BaseDecorRadius), "Decor Radius")
        };

        public static void SetDef(BuildingDef def)
        {
            if (!def.ShowInBuildMenu || def.Deprecated || !TUNING.BUILDINGS.PLANORDER.Any(x => ((List<string>)x.data).Contains(def.PrefabID)))
                return;

            if (!Options.Opts.UIConfigOptions.TryGetValue(def.PrefabID, out var options))
                Options.Opts.UIConfigOptions.Add(def.PrefabID, options = new UIConfigOptions());

            foreach (var property in buildingDefProperties)
            {
                var trav = Traverse.Create(def);
                if ((trav.Field(property.propertyId) ?? trav.Property(property.propertyId)) is Traverse propertyTrav)
                {
                    options.AddOption(def, property.propertyId, property.displayName);
                    propertyTrav.SetValue(options.GetValue(property.propertyId));
                }

                Debug.Log("Property: " + property + " not found on: " + def.GetType() + "; not set.");
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
