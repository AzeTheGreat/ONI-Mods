using System;

namespace AzeLib.Attributes
{
    /// <summary>
    /// Marks a static method that should be invoked for each generated building prefab
    /// immediately after <see cref="global::GeneratedBuildings.LoadGeneratedBuildings"/> finishes.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true, Inherited = false)]
    public sealed class ApplyToBuildingPrefabsAttribute : Attribute
    {
        /// <summary>
        /// Initializes the attribute so that the decorated method applies to every building prefab.
        /// </summary>
        public ApplyToBuildingPrefabsAttribute()
            : this(Array.Empty<string>())
        {
        }

        /// <summary>
        /// Initializes the attribute so that the decorated method only targets the specified building IDs.
        /// </summary>
        /// <param name="buildingIds">Optional prefab IDs (tags) that gate invocation of the method.</param>
        public ApplyToBuildingPrefabsAttribute(params string[] buildingIds)
        {
            BuildingIds = buildingIds ?? Array.Empty<string>();
        }

        /// <summary>
        /// Gets the prefab IDs that restrict invocation of the decorated method.
        /// An empty collection indicates the method should run for every prefab.
        /// </summary>
        public string[] BuildingIds { get; }
    }
}
