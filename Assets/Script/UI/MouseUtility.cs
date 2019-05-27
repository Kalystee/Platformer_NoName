using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MouseUtility
{
    public static bool IsInputBlockedNow
    {
        get
        {
            WindowStack windowStack = Find.WindowStack;
            return windowStack.MouseNotOnWindow() || !windowStack.CurrentInFocus();
        }
    }

    public static Vector2 MousePos
    {
        get
        {
            return new Vector2(Input.mousePosition.x, Screen.height - Input.mousePosition.y);
        }
    }

    public static bool IsOver(Rect rect)
    {
        return rect.Contains(Event.current.mousePosition) && !MouseUtility.IsInputBlockedNow;
    }
}
