using PeterHan.PLib.Core;
using PeterHan.PLib.Options;
using System.Collections.Generic;

namespace AzeLib
{
    [ConfigFile("config.json", true)]
    public abstract class BaseOptions<T> : IValidatedOptions, IOptions where T : class, IValidatedOptions, new()
    {
        private static T _opts;
        public static T Opts
        {
            get => _opts ??= ReadAndValidateSettings() ?? new T();
            set { _opts = value; }
        }

        public virtual bool ValidateSettings() => true;
        public virtual IEnumerable<IOptionsEntry> CreateOptions() => new List<IOptionsEntry>();
        public void OnOptionsChanged() => Opts = ReadAndValidateSettings();

        public static void OnLoad()
        {
            var opts = new POptions();
            PUtil.InitLibrary(false);
            opts.RegisterOptions(AzeMod.UserMod, typeof(T));
        }

        private static T ReadAndValidateSettings()
        {
            // Must re-read to update anything that was changed by the GUI.
            T settings = POptions.ReadSettings<T>();
            // If anything had to be changed again, save it back to file.
            if (!settings?.ValidateSettings() ?? false)
                POptions.WriteSettings(settings);

            return settings;
        }

        IEnumerable<IOptionsEntry> IOptions.CreateOptions()
        {
            return CreateOptions();
        }
    }
}
