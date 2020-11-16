using Harmony;

namespace AzeLib.Extensions
{
    public static class TraverseExt
    {
        public static Traverse FieldOrProperty(this Traverse trav, string id)
        {
            Traverse propTrav = null;
            if (trav.Field(id).FieldExists())
                propTrav = trav.Field(id);
            else if (trav.Property(id).FieldExists())
                propTrav = trav.Property(id);
            return propTrav;
        }
    }
}
