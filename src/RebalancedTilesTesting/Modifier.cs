using AzeLib.Extensions;
using Harmony;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using System;

namespace RebalancedTilesTesting
{
    [JsonObject(MemberSerialization.OptIn)]
    [JsonConverter(typeof(ModifierConverter))]
    public class Modifier
    {
        [JsonProperty] private object _value;
        public object Value { 
            get => _value ?? GetDefaultValue();
            set { _value = value; defaultValue = GetDefaultValue(); }
        }

        private object defaultValue;

        private BuildingDef def;
        private string propertyId;

        public Modifier(BuildingDef def, string propertyId)
        {
            this.def = def;
            this.propertyId = propertyId;
        }

        public object GetDefaultValue() => defaultValue ?? Traverse.Create(def).FieldOrProperty(propertyId)?.GetValue();

        public void SetUnsavedValues(BuildingDef def, string propertyId)
        {
            this.def = def;
            this.propertyId = propertyId;
            defaultValue = GetDefaultValue();
        }

        private class ModifierConverter : JsonConverter
        {
            public override bool CanConvert(Type objectType) => (objectType == typeof(Modifier));
            public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer) 
                => new Modifier(default, default) { _value = JToken.Load(reader).ToObject<object>() };
            public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer) 
                => JToken.FromObject(((Modifier)value)._value).WriteTo(writer);
        }
    }
}

