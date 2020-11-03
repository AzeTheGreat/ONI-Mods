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

            if (!Options.Opts.UIConfigOptions.TryGetValue(def.Name, out var options))
                Options.Opts.UIConfigOptions.Add(def.Name, options = new UIConfigOptions());

            foreach (var property in buildingDefProperties)
            {
                var trav = Traverse.Create(def);
                Traverse propTrav = null;

                if (trav.Field(property.propertyId).FieldExists())
                    propTrav = trav.Field(property.propertyId);
                if (trav.Property(property.propertyId).FieldExists())
                    propTrav = trav.Property(property.propertyId);

                if (propTrav != null)
                {
                    options.AddOption(def, property.propertyId, property.displayName);
                    propTrav.SetValue(options.GetValue(property.propertyId));
                }
                else
                    Debug.Log("Property: '" + property.propertyId + "' not found on: '" + def.GetType() + "'; not set.");
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
