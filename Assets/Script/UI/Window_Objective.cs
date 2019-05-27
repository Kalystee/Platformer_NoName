using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Window_Objectives : Window
{
    public Objective objective;

    public override bool Draggable => false;
    public override string Header => objective?.Title;
    public override Vector2 InitialPosition => new Vector2(ScreenUtility.width - 200, 0);
    public override Vector2 InitialSize => new Vector2(200, 300);

    public Window_Objectives(Objective _obj)
    {
        objective = _obj;
    }

    public override void DoWindowContents(Rect inRect)
    {
        int y = 0;
        foreach (SubObjective so in objective.ListOfSub)
        {
            Widgets.Label(new Rect(0, y, inRect.width, 20), so.Title);
            Widgets.Label(new Rect(0, y + 15, inRect.width, inRect.height - 20), so.Description);
            y += 40;
        }
    }
}