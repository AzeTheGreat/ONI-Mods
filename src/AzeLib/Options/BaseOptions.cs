using PeterHan.PLib;
using PeterHan.PLib.Options;
using System.Collections;

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
        public IEnumerable CreateOptions() => null;
        public void OnOptionsChanged() => Opts = ReadAndValidateSettings();

        private static void OnOnLoad()
        {
            PUtil.InitLibrary(false);
            POptions.RegisterOptions(typeof(T));
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
    }
}
