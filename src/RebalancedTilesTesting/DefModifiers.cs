using AzeLib.Extensions;
using Harmony;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;

namespace RebalancedTilesTesting
{
    [JsonObject(MemberSerialization.OptIn)]
    [JsonConverter(typeof(DefModifiersConverter))]
    public class DefModifiers
    {
        [JsonProperty] private Dictionary<string, Modifier> modifiers = new();

        public void SetModifier(string propertyId, Modifier mod) => modifiers[propertyId] = mod;

        public Modifier GetModifier(string propertyId)
        {
            modifiers.TryGetValue(propertyId, out var mod);
            return mod;
        }

        public void ApplyModifiers(BuildingDef def)
        {
            foreach (var kvp in modifiers)
            {
                var propertyId = kvp.Key;
                var mod = kvp.Value;

                mod.SetUnsavedValues(def, propertyId);

                var trav = Traverse.Create(def);
                if (trav.FieldOrProperty(propertyId) is Traverse propTrav)
                    propTrav.SetValue(mod.Value);
                else
                    Debug.Log("Property: '" + propertyId + "' not found on: '" + def.GetType() + "'; not set.");
            }
        }

        private class DefModifiersConverter : JsonConverter
        {
            public override bool CanConvert(Type objectType) => (objectType == typeof(DefModifiers));
            public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer) 
                => new DefModifiers() { modifiers = JToken.Load(reader).ToObject<Dictionary<string, Modifier>>() };

            public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
                => JToken.FromObject(((DefModifiers)value).modifiers).WriteTo(writer);
        }
    }
}

