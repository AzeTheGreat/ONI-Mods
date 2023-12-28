using System;

namespace BetterLogicOverlay
{
    public static class LabelUtil
    {
        public static string GetFormattedNum(float num) => GameUtil.FloatToString(num, GetFormatForNum(num));
        public static string GetFormatForNum(float num) => Math.Abs(num) >= 10f ? "#.;;0" : "#.#;;0";
    }
}
