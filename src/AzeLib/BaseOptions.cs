using Harmony;
using PeterHan.PLib;
using PeterHan.PLib.Options;
using System;
using System.Linq;
using System.Reflection;

namespace AzeLib
{
    public interface IOption
    {
        bool ValidateSettings();
    }

    [ConfigFile("config.json", true)]
    public abstract class BaseOptions<T> : IOption where T : class, IOption, new()
    {
        private static T _opts;
        public static T Opts
        {
            get => _opts ??= ReadAndValidateSettings() ?? new T();
            set { _opts = value; }
        }

        public virtual bool ValidateSettings() => true;

        private static T ReadAndValidateSettings()
        {
            T settings = POptions.ReadSettings<T>();
            if(!settings?.ValidateSettings() ?? false)
                POptions.WriteSettings(settings);

            return settings;
        }

        // Must re-read to update anything that was changed by the GUI
        private static void UpdateSettings() => Opts = ReadAndValidateSettings();

        private static void Load()
        {
            PUtil.InitLibrary(false);
            POptions.RegisterOptions(typeof(T));
        }
    }

    [HarmonyPatch(typeof(ModsScreen), "OnDeactivate")]
    class ReadSettings
    {
        private static MethodInfo readSettings;
        private static Type optionType;

        static bool Prepare()
        {
            // Assume only one Options per assembly
            optionType = ReflectionHelpers.GetChildTypesOfGenericType(typeof(BaseOptions<>)).FirstOrDefault();

            // If a restart is required, there's no need to reread the settings on screen close.
            if (optionType == null || optionType.IsDefined(typeof(RestartRequiredAttribute), true))
                return false;

            readSettings = AccessTools.Method(optionType, "UpdateSettings");
            return true;
        }

        // Can't just put OnLoad inside BaseOptions because it's generic.
        public static void OnLoad()
        {
            if (optionType != null)
                AccessTools.Method(optionType, "Load").Invoke(null, null);
        }

        static void Postfix() => readSettings.Invoke(null, null);
    }
}