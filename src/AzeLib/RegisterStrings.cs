using Harmony;

namespace AzeLib
{
    public abstract class RegisterStrings
    {
        protected const string d = ".";
        protected const string parentPath = "STRINGS.AZE.";

        [HarmonyPatch(typeof(SetDefaults), nameof(SetDefaults.Initialize))]
        private class Patch
        {
            static void Postfix()
            {
                var childTypes = ReflectionHelpers.GetChildTypesOfType<RegisterStrings>();
                foreach (var type in childTypes)
                    LocString.CreateLocStringKeys(type, parentPath);
            }
        }
    }
}
