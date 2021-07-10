using AzeLib.Attributes;
using Newtonsoft.Json;
using PeterHan.PLib.Options;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AzeLib
{
    [ConfigFile("config.json", true)]
    [JsonObject(MemberSerialization.OptOut)]
    public abstract class BaseOptions<T> : IOptions where T : BaseOptions<T>, new()
    {
        private static T _opts;
        public static T Opts
        {
            get => _opts ??= Validate(POptions.ReadSettings<T>()) ?? new T();
            set => _opts = value;
        }

        void IOptions.OnOptionsChanged() => Opts = Validate((T)this);

        private static T Validate(T settings)
        {
            // If anything had to be changed again, save it back to file.
            if (!settings?.ValidateSettings() ?? false)
                POptions.WriteSettings(settings);

            return settings;
        }

        /// <summary>Called on the new settings object to check for validity. If the return value is false (settings not valid), the settings are rewritten.</summary>
        protected virtual bool ValidateSettings() => true;

        IEnumerable<IOptionsEntry> IOptions.CreateOptions() => CreateOptions();
        /// <inheritdoc cref="IOptions.CreateOptions"/>
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
