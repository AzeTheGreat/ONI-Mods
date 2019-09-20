using Harmony;

namespace SuppressNotifications
{
    public static class ExtensionMethods
    {
        // Stolen from Peter thanks Peter
        public static T GetField<T>(this Traverse root, string name)
        {
            return root.Field(name).GetValue<T>();
        }

        public static void SetField(this Traverse root, string name, object value)
        {
            root.Field(name).SetValue(value);
        }
    }
}
