using System;

namespace AzeLib.Attributes
{
    [AttributeUsage(AttributeTargets.Field, Inherited = false, AllowMultiple = false)]
    public sealed class MyIntGetAttribute : Attribute { }
}
