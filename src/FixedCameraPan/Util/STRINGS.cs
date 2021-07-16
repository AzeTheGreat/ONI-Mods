using AzeLib;

namespace FixedCameraPan
{
    class OPTIONS : RegisterStrings
    {
        public class PANSPEED
        {
            public static LocString NAME = "Pan Speed";
            public static LocString TOOLTIP = "Set to the framerate at which you find the speed comfortable.  At 60, the camera will always move at the same speed as it moves when the game is at 60 FPS.";
        }
    }
}
