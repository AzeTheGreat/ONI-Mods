using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Collections.Generic;

namespace RebalancedTilesTesting
{
    [JsonObject(MemberSerialization.OptIn)]
    public class ConfigOptions
    {
        [JsonProperty] private readonly Dictionary<string, DefModifiers> modifiedDefs = new();

        public object GetModifierValue(string defId, string propertyId) => GetModifier(defId, propertyId).Value;
        public object GetDefaultValue(string defId, string propertyId) => GetModifier(defId, propertyId).GetDefaultValue();

        public void SetModifierValue(string defId, string propertyId, object newValue)
        {
            modifiedDefs.TryGetValue(defId, out var defModifiers);
            modifiedDefs[defId] = defModifiers ??= new();

            var mod = new Modifier(Assets.GetBuildingDef(defId), propertyId)
            {
                Value = newValue
            };

            defModifiers.SetModifier(propertyId, mod);
        }

        public void ApplyModifiersToDef(BuildingDef def)
        {
            if (modifiedDefs.TryGetValue(def.PrefabID, out var defModifiers))
                defModifiers.ApplyModifiers(def);
        }

        private Modifier GetModifier(string defId, string propertyId)
        {
            modifiedDefs.TryGetValue(defId, out var defMods);
            return defMods?.GetModifier(propertyId) ?? new Modifier(Assets.GetBuildingDef(defId), propertyId);
        }
    }
}

