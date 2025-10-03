using BetterInfoCards.Export;
using BetterInfoCards.Process;
using HarmonyLib;

namespace BetterInfoCards
{
    [HarmonyPatch(typeof(HoverTextDrawer), nameof(HoverTextDrawer.EndDrawing))]
    class ProcessHoverInfo
    {
        static void Prefix()
        {
            var infoCards = InterceptHoverDrawer.ConsumeInfoCards();
            var displayCards = new DisplayCards().UpdateData(infoCards);

            ModifyHits.Update(displayCards);

            InterceptHoverDrawer.IsInterceptMode = false;
            foreach (var card in displayCards)
                card.Draw();
            InterceptHoverDrawer.IsInterceptMode = true;

            var widgets = ExportWidgets.ConsumeWidgets();
            if (widgets.Count > 0)
            {
                var grid = new Grid(widgets, widgets[0].YMax);
                grid.MoveAndResizeInfoCards();
            }
        }
    }
}
