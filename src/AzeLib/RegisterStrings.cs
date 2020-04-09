using Harmony;

namespace AzeLib
{
    public abstract class RegisterStrings
    {
        protected const string d = ".";
        protected const string parentPath = "STRINGS.AZE.";

        [HarmonyPatch(typeof(GlobalAssets), "Awake")]
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
