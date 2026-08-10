using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utils
{
    public static string ToTimeString(float seconds)
    {
        int min = Mathf.FloorToInt(seconds / 60f);
        int sec = Mathf.FloorToInt(seconds % 60f);
        return $"{min:00}:{sec:00}";
    }
}
