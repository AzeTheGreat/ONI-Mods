using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Collections.Generic;
using System.Linq;

namespace RebalancedTilesTesting
{
    [JsonObject(MemberSerialization.OptIn)]
    public class SerializedConfigs
    {
        [JsonProperty] private Dictionary<string, Dictionary<string, object>> options = new Dictionary<string, Dictionary<string, object>>();

        public bool CleanUp()
        {
            options = options
                .Select(x => new KeyValuePair<string, Dictionary<string, object>>(x.Key, 
                    x.Value.Where(x => x.Value != null)
                    .ToDictionary(x => x.Key, x => x.Value)))
                .Where(x => x.Value.Count() > 0)
                .ToDictionary(x => x.Key, x => x.Value);
            return false;
        }

        public bool TryGetValue(string defId, string propertyId, out object value)
        {
            value = null;
            return options.TryGetValue(defId, out var option) && option.TryGetValue(propertyId, out value) && value != null;
        }

        public void SetValue(string defId, string propertyId, object newValue)
        {
            options.TryGetValue(defId, out var values);
            options[defId] = values ??= new Dictionary<string, object>();
            values[propertyId] = newValue;
        }
    }
}

