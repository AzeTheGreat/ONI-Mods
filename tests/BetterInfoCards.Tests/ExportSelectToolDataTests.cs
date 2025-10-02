using System.Reflection;
using BetterInfoCards;
using Xunit;

namespace BetterInfoCards.Tests
{
    public sealed class ExportSelectToolDataTests
    {
        [Fact]
        public void ExportGo_IgnoresMissingSelectable()
        {
            _ = ExportSelectToolData.ConsumeSelectable();
            _ = ExportSelectToolData.ConsumeTextInfo();

            var patchType = typeof(ExportSelectToolData).GetNestedType(
                "GetSelectInfo_Patch",
                BindingFlags.Public | BindingFlags.NonPublic);
            Assert.NotNull(patchType);

            var exportSelectable = patchType!.GetMethod(
                "ExportSelectable",
                BindingFlags.Static | BindingFlags.NonPublic);
            var exportGo = patchType.GetMethod(
                "ExportGO",
                BindingFlags.Static | BindingFlags.NonPublic);

            Assert.NotNull(exportSelectable);
            Assert.NotNull(exportGo);

            exportSelectable!.Invoke(null, new object?[] { null });

            var exception = Record.Exception(() => exportGo!.Invoke(null, new object?[] { "test" }));

            Assert.Null(exception);

            var (id, data) = ExportSelectToolData.ConsumeTextInfo();
            Assert.Equal(string.Empty, id);
            Assert.Null(data);
        }
    }
}
