using AzeLib.Attributes;
using AzeLib.Buildings;
using System.Collections.Generic;
using Xunit;

namespace AzeLib.Tests
{
    public sealed class BuildingPrefabAttributeHelperTests
    {
        [Fact]
        public void Process_InvokesAttributedMethodsAndRespectsFilters()
        {
            BuildingPrefabAttributeHelper.ResetForTests();
            TestHandlers.Clear();

            var matching = new StubBuildingDef("Filtered");
            var other = new StubBuildingDef("Different");

            BuildingPrefabAttributeHelper.Process(matching);
            BuildingPrefabAttributeHelper.Process(other);

            Assert.Equal(new[] { matching, other }, TestHandlers.AllInvocations);
            Assert.Equal(new[] { matching }, TestHandlers.FilteredInvocations);
        }

        private sealed class StubBuildingDef
        {
            public StubBuildingDef(string id)
            {
                PrefabID = new PrefabTag(id);
            }

            public PrefabTag PrefabID { get; }

            public object BuildingComplete { get; set; } = new();
        }

        private sealed class PrefabTag
        {
            public PrefabTag(string name)
            {
                Name = name;
            }

            public string Name { get; }

            public override string ToString() => Name;
        }

        private static class TestHandlers
        {
            private static readonly List<object> allInvocations = new();
            private static readonly List<object> filteredInvocations = new();

            public static IReadOnlyList<object> AllInvocations => allInvocations;
            public static IReadOnlyList<object> FilteredInvocations => filteredInvocations;

            [ApplyToBuildingPrefabs]
            public static void CaptureAll(object def)
            {
                allInvocations.Add(def);
            }

            [ApplyToBuildingPrefabs("Filtered")]
            public static void CaptureFiltered(object def)
            {
                filteredInvocations.Add(def);
            }

            public static void Clear()
            {
                allInvocations.Clear();
                filteredInvocations.Clear();
            }
        }
    }
}
