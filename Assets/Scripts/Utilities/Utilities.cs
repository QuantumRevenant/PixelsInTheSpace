using UnityEngine;

namespace QuantumRevenant
{
    namespace Utilities
    {
        public static class Tools
        {
            public static float NormalizeAngle(float angle, bool equalLimits = false)
            {
                if (angle % 360 != 0 || equalLimits)
                    return angle %= 360;

                return angle == 0 ? 0 : 360;
            }

            public static bool DoesTagExist(string aTag)
            {
                try
                {
                    GameObject.FindGameObjectWithTag(aTag);
                    return true;
                }
                catch
                {
                    return false;
                }
            }
        }
    }
}

