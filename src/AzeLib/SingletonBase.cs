using HarmonyLib;
using System;

namespace AzeLib
{
    public abstract class SingletonBase<T> where T : SingletonBase<T>
    {
        public static T Instance => Lazy.Value;
        private static readonly Lazy<T> Lazy = new(() => (Activator.CreateInstance(typeof(T), true) as T)!);
    }

    static class SingletonHelper<T>
    {
        public static T GetInstance(Type type) => Traverse.Create(type).Property("Instance").GetValue<T>();
    }
}