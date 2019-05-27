using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Window_Inventory : Window
{
    public override string Header => "Test";
    private Inventory inv;

    public Window_Inventory(Inventory inv)
    {
        this.inv = inv;
    }

    Vector2 scrollInventory = default(Vector2);

    public override void DoWindowContents(Rect inRect)
    {
        Widgets.DrawInventory(inRect, inv, ref scrollInventory);
    }
}
