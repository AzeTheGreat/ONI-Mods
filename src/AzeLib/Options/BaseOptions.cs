using AzeLib.Attributes;
using PeterHan.PLib.Options;
using System;
using System.Collections.Generic;
using System.Linq;

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

        // TODO: Acording to IOptions docs, this may no longer be necessary to reread manually.
        void IOptions.OnOptionsChanged() => Opts = ReadAndValidateSettings();

        private static T ReadAndValidateSettings()
        {
            // Must re-read to update anything that was changed by the GUI.
            T settings = POptions.ReadSettings<T>();
            // If anything had to be changed again, save it back to file.
            if (!settings?.ValidateSettings() ?? false)
                POptions.WriteSettings(settings);

            return settings;
        }

        bool IValidatedOptions.ValidateSettings() => ValidateSettings();
        protected virtual bool ValidateSettings() => true;

        IEnumerable<IOptionsEntry> IOptions.CreateOptions() => CreateOptions();
        protected virtual IEnumerable<IOptionsEntry> CreateOptions() => new List<IOptionsEntry>();
    }

    // Can't be in BaseOptions since OnLoad invocation can't handle generics.
    class OptionsInit
    {
        [OnLoad]
        private static void RegisterOptions()
        {
            // Assume only one Options per assembly
            if (ReflectionHelpers.GetChildTypesOfGenericType(typeof(BaseOptions<>)).FirstOrDefault() is Type optionType)
                new POptions().RegisterOptions(AzeMod.UserMod, optionType);
        }
    }
}
