using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ScreenUtility
{
    private static float scale = 1f;
    public static float Scale
    {
        get
        {
            return scale;
        }
        set
        {
            scale = value;
            ApplyScale();
        }
    }

    public static int width;
    public static int height;

    public static void ApplyScale()
    {
        if(scale == 1f)
        {
            ScreenUtility.width = Screen.width;
            ScreenUtility.height = Screen.height;
        }
        else
        {
            ScreenUtility.width = Mathf.RoundToInt(Screen.width / scale);
            ScreenUtility.height = Mathf.RoundToInt(Screen.height / scale);
            GUI.matrix = Matrix4x4.TRS(Vector3.one, Quaternion.identity, new Vector3(ScreenUtility.width, ScreenUtility.height, 1f));
        }
    }
}
