using AzeLib.Attributes;
using STRINGS;

namespace DrinkTeaNotCoffee
{
    public class Tea
    {
        [OnLoad]
        private static void UpdateStrings()
        {
            BUILDINGS.PREFABS.ESPRESSOMACHINE.NAME = MYSTRINGS.TEAMACHINE.NAME;
            BUILDINGS.PREFABS.ESPRESSOMACHINE.DESC = MYSTRINGS.TEAMACHINE.DESC;

            DUPLICANTS.MODIFIERS.ESPRESSO.NAME = MYSTRINGS.TEA.NAME;
        }
    }
}
