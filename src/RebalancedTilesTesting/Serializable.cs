using Newtonsoft.Json;
using PeterHan.PLib.Options;
using System;
using System.IO;
using System.Reflection;

namespace RebalancedTilesTesting
{
    public abstract class Serializable<T> where T : Serializable<T>, new()
    {
        protected static string ConfigPath => POptions.GetModDir(Assembly.GetExecutingAssembly()) + "/config.json";
        protected virtual void OnSerialize() { }

        public void Serialize()
        {
            OnSerialize();

            try
            {
                File.WriteAllText(ConfigPath, JsonConvert.SerializeObject(this, Formatting.Indented));
            }
            catch (Exception ex) when (ex is IOException || ex is UnauthorizedAccessException || ex is JsonException)
            {
                Debug.Log("Config could not be written for " + Assembly.GetExecutingAssembly() +
                    ", at: '" + ConfigPath + "', due to: " + ex + ". New values not saved to config");
            }
        }

        protected static T Deserialize()
        {
            if (!File.Exists(ConfigPath))
                return new T();

            try
            {
                return JsonConvert.DeserializeObject<T>(File.ReadAllText(ConfigPath));
            }
            catch (Exception ex) when (ex is IOException || ex is UnauthorizedAccessException || ex is JsonException)
            {
                Debug.Log("Config could not be loaded for " + Assembly.GetExecutingAssembly() +
                    ", at: '" + ConfigPath + "', due to: " + ex + ". Config reverted to default.");
                return new T();
            }
        }
    }
}
