using System.Collections.Generic;
using UnityEngine;

namespace ModManager
{
    public class ModUIExtract
    {
        public KMod.Mod Mod => Global.Instance.modManager.mods[mod.mod_index];
        public LocText Title { get; private set; }
        public LocText Version { get; private set; }
        public List<GameObject> CustomGOs { get; private set; } = new();

        private ModsScreen.DisplayedMod mod;

        public ModUIExtract(ModsScreen.DisplayedMod mod, ModsScreen modsScreen)
        {
            this.mod = mod;
            var go = mod.rect_transform.gameObject;
            Title = go.transform.Find("Title").GetComponent<LocText>();
            Version = go.transform.Find("Version").GetComponent<LocText>();

            // Find custom GOs as child GOs whose name is not included on the prefab.
            foreach (Transform tf in go.transform)
                if (!modsScreen.entryPrefab.transform.Find(tf.name))
                {
                    tf.gameObject.AddComponent<ExtractedGO>();
                    CustomGOs.Add(tf.gameObject); 
                }
            // Must be a separate loop because changing the parent messes with the ienumerable.
            foreach (var cGo in CustomGOs)
                cGo.transform.SetParent(null);
        }
    }

    public class ExtractedGO : MonoBehaviour { }
}
