using PeterHan.PLib.Database;
using STRINGS;

namespace DrinkTeaNotCoffee
{
    public class Tea
    {
        public static void OnLoad()
        {
            var loc = new PLocalization();
            loc.Register();

            BUILDINGS.PREFABS.ESPRESSOMACHINE.NAME = STRINGS.TEAMACHINE.NAME;
            BUILDINGS.PREFABS.ESPRESSOMACHINE.DESC = STRINGS.TEAMACHINE.DESC;

            DUPLICANTS.MODIFIERS.ESPRESSO.NAME = STRINGS.TEA.NAME;
        }
    }
}
