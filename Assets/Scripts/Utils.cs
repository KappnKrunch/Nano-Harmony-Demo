using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utils
{
    public static float Clamp(float a, float min, float max)
    {
        float output = a;

        if (a < min)
        {
            output = min;
        }
        else if (a > max)
        {
            output = max;
        }

        return output;
    }
}
