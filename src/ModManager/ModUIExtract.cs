namespace ModManager
{
    public class ModUIExtract
    {
        public KMod.Mod Mod => Global.Instance.modManager.mods[mod.mod_index];
        public LocText Title { get; private set; }
        public LocText Version { get; private set; }

        private ModsScreen.DisplayedMod mod;

        public ModUIExtract(ModsScreen.DisplayedMod mod)
        {
            this.mod = mod;
            var go = mod.rect_transform.gameObject;
            Title = go.transform.Find("Title").GetComponent<LocText>();
            Version = go.transform.Find("Version").GetComponent<LocText>();
        }
    }
}
