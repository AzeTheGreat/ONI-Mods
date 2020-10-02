using Harmony;
using System;
using System.Collections.Generic;

namespace RebalancedTilesTesting
{
    public class ConfigOptions
    {
        private Dictionary<string, DefaultIntOptionsEntry> propertyOptions = new Dictionary<string, DefaultIntOptionsEntry>();

        public IEnumerable<DefaultIntOptionsEntry> GetOptions()
        {
            foreach (var option in propertyOptions)
                yield return option.Value;
        }

        public void AddOption(BuildingDef def, string property, string displayName)
        {
            if(Traverse.Create(def).Field(property).GetValue() is object defaultValue)
                propertyOptions.Add(property, new DefaultIntOptionsEntry(displayName, string.Empty, Convert.ToInt32(defaultValue), def.PrefabID, property, def.Name));    
        }

        public T GetValue<T>(string property)
        {
            if (!propertyOptions.TryGetValue(property, out var value))
                Debug.Log("No option was created for: " + property + ", it will not be modified");
            return (T)Convert.ChangeType(value.Value, typeof(T));
        }
    }
}

