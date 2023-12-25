using HarmonyLib;
using System;

namespace AzeLib
{
    /// <summary>Inherit from this class to implement singleton behavior.</summary>
    /// <remarks>Uses lazy instantiation.<br/>
    /// <see langword="private"/> constructors are supported since instantiation is executed via reflection.</remarks>
    /// <typeparam name="T">The type of the child class.  This is passed to <see cref="SingletonBase{T}"/> for instantiation.</typeparam>
    public abstract class SingletonBase<T> where T : SingletonBase<T>
    {
        /// <summary>Gets the current instance of the child class.</summary>
        public static T Instance => Lazy.Value;
        private static readonly Lazy<T> Lazy = new(() => (Activator.CreateInstance(typeof(T), true) as T)!);
    }

    static class SingletonHelper<T>
    {
        public static T GetInstance(Type type) => Traverse.Create(type).Property("Instance").GetValue<T>();
    }
}