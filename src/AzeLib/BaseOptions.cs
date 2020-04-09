using Harmony;
using PeterHan.PLib;
using PeterHan.PLib.Options;
using System;
using System.Linq;
using System.Reflection;

namespace AzeLib
{
    [ConfigFile("config.json", true)]
    public abstract class BaseOptions<T> where T : class, new() 
    {
        protected BaseOptions() { }

        private static T _opts;
        public static T Opts
        {
            get
            {
                if (_opts == null)
                    _opts = POptions.ReadSettings<T>() ?? new T();

                return _opts;
            }
            set { _opts = value; }
        }

        private static void ReadSettings<T2>() where T2: class, new()
        {
            Debug.Log("Settings read");
            Opts = POptions.ReadSettings<T2>() as T;
        }

        private static void Load()
        {
            PUtil.InitLibrary(false);
            POptions.RegisterOptions(typeof(T));
        }
    }

    [HarmonyPatch(typeof(ModsScreen), "Exit")]
    class ReadSettings
    {
        private static MethodInfo readSettings;
        private static Type optionType;

        static bool Prepare()
        {
            var inheritingTypes = ReflectionHelpers.GetChildTypesOfGenericType(typeof(BaseOptions<>));

            // Assume only one Options per assembly
            optionType = inheritingTypes.FirstOrDefault();

            // If a restart is required, there's no need to reread the settings on save load.
            if (optionType == null || optionType.IsDefined(typeof(RestartRequiredAttribute), true))
                return false;

            readSettings = AccessTools.Method(optionType, "ReadSettings").MakeGenericMethod(optionType);
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