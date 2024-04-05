using UnityEngine;

namespace UtilitiesNS
{
    public static class Utilities
    {
        public static float NormalizeAngle(float angle, bool equalLimits = false)
        {
            if (angle % 360 != 0 || equalLimits)
                return angle %= 360;

            return angle==0?0:360;
        }
    }
}
