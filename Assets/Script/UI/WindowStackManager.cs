using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class WindowStackManager
{
	public static void Update ()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            Find.WindowStack.Add(new Window_Armory());
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            Find.WindowStack.Add(new Window_Shop());
        }
    }
}
