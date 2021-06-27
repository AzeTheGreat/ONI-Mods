using AzeLib.Attributes;
using PeterHan.PLib.Database;
using STRINGS;

namespace DrinkTeaNotCoffee
{
    public class Tea
    {
        [OnLoad]
        private static void RegisterAndUpdateStrings()
        {
            new PLocalization().Register();

            BUILDINGS.PREFABS.ESPRESSOMACHINE.NAME = STRINGS.TEAMACHINE.NAME;
            BUILDINGS.PREFABS.ESPRESSOMACHINE.DESC = STRINGS.TEAMACHINE.DESC;

            DUPLICANTS.MODIFIERS.ESPRESSO.NAME = STRINGS.TEA.NAME;
        }
    }
}
