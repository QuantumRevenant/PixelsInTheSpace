using UnityEngine;

public static class Utilities
{
    public static float NormalizeAngle(float angle,bool equalLimits=false)
    {
        if(angle%360==0)
        {
            if(!equalLimits)
            {
                if(angle==0)
                {
                    return 0;
                }else
                {
                    return 360;
                }
            }
        }
        return angle%=360;
    }
}