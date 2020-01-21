using System.Reflection;

namespace AzeLib.Extensions
{
    public static class MethodInfoExt
    {
        public static bool IsOverride(MethodInfo m)
        {
            return m.GetBaseDefinition().DeclaringType != m.DeclaringType;
        }
    }
}
