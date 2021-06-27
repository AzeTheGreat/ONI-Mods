using System;
//using CustomAnalyzers.Interface;

namespace AzeLib.Attributes
{
    [AttributeUsage(AttributeTargets.Field, Inherited = false, AllowMultiple = false)]
    //[SuppressNotAssigned]
    public sealed class MyIntGetAttribute : Attribute { }
}
