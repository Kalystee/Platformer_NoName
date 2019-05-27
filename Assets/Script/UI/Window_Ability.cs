using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Window_Ability : Window
{
    public static AbilityBase ability;

    public override string Header => ability.Name;
    public override Sprite HeaderIcon => ability.Sprite;
    public override bool DoXClose => false;
    public override bool CloseOnEscapeKey => false;
    public override bool Draggable => false;
    public override bool FollowMouse => true;

    public override Vector2 InitialSize => new Vector2(150f, 125f);

    public Window_Ability(AbilityBase _ability)
    {
        ability = _ability;
    }

    public override void DoWindowContents(Rect inRect)
    {
        Widgets.Label(new Rect(5, 5, inRect.width - 10, 25), $"Range : {ability.Range}");
        Widgets.Label(new Rect(5, 30, inRect.width - 10, 25), $"Cost : {ability.Cost} PA");
    }
}
