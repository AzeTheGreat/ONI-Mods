using System;
//using CustomAnalyzers.Interface;

namespace AzeLib.Attributes
{
    [AttributeUsage(AttributeTargets.Field, Inherited = false, AllowMultiple = false)]
    //[SuppressNotAssigned]
    public sealed class MyIntGetAttribute : Attribute { }

    /// <summary>
    /// Mark method to be invoked when the mod is loaded.  Must be static void and have no parameters.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = false)]
    public sealed class OnLoadAttribute : Attribute { }
}
