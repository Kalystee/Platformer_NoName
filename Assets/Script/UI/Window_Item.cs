using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Window_Item : Window
{
    public static ItemBase item;
    public override string Header => item.Name;
    public override Sprite HeaderIcon => item.Sprite;
    public override bool DoXClose => false;
    public override bool CloseOnEscapeKey => false;
    public override bool Draggable => false;
    public override bool FollowMouse => true;

    public override Vector2 InitialSize => new Vector2(150f, 125f);

    public Window_Item(ItemBase _item)
    {
        item = _item;
    }

    public override void DoWindowContents(Rect inRect)
    {
        if(item is WeaponBase) {
            WeaponBase weapon = item as WeaponBase;
            
            //TODO : definir un offset

            Widgets.Label(new Rect(5, 5, inRect.width - 10, 25), weapon.Name);
            Widgets.Label(new Rect(5, 30, inRect.width - 10, 25), $"Range : {weapon.Ability.Range}");

            if (weapon.GetStat(Stats.Vitality) > 0) Widgets.Label(new Rect(5, 50, inRect.width - 10, 25), $"Vitality : {weapon.GetStat(Stats.Vitality)}");
            if (weapon.GetStat(Stats.Initiative) > 0) Widgets.Label(new Rect(5, 50, inRect.width - 10, 25), $"Initialive : {weapon.GetStat(Stats.Initiative)}");
            if (weapon.GetStat(Stats.Strength) > 0) Widgets.Label(new Rect(5, 50, inRect.width - 10, 25), $"Strength : {weapon.GetStat(Stats.Strength)}");
            if (weapon.GetStat(Stats.Precision) > 0) Widgets.Label(new Rect(5, 50, inRect.width - 10, 25), $"Precision : {weapon.GetStat(Stats.Precision)}");
            if (weapon.GetStat(Stats.Intelligence) > 0) Widgets.Label(new Rect(5, 50, inRect.width - 10, 25), $"Intelligence : {weapon.GetStat(Stats.Intelligence)}");
            if (weapon.GetStat(Stats.PA) > 0) Widgets.Label(new Rect(5, 50, inRect.width - 10, 25), $"PA : {weapon.GetStat(Stats.PA)}");

        } else if(item is EquipmentBase)
        {
           if(item is HelmetBase)
            {
                HelmetBase helmet = item as HelmetBase;
                Widgets.Label(new Rect(5, 5, inRect.width - 10, 25), helmet.Name);
                Widgets.Label(new Rect(5, 30, inRect.width - 10, 25), $"Armor : {helmet.Armor}");

                if (helmet.GetStat(Stats.Vitality) > 0) Widgets.Label(new Rect(5, 50, inRect.width - 10, 25), $"Vitality : {helmet.GetStat(Stats.Vitality)}");
                if (helmet.GetStat(Stats.Initiative) > 0) Widgets.Label(new Rect(5, 50, inRect.width - 10, 25), $"Initialive : {helmet.GetStat(Stats.Initiative)}");
                if (helmet.GetStat(Stats.Strength) > 0) Widgets.Label(new Rect(5, 50, inRect.width - 10, 25), $"Strength : {helmet.GetStat(Stats.Strength)}");
                if (helmet.GetStat(Stats.Precision) > 0) Widgets.Label(new Rect(5, 50, inRect.width - 10, 25), $"Precision : {helmet.GetStat(Stats.Precision)}");
                if (helmet.GetStat(Stats.Intelligence) > 0) Widgets.Label(new Rect(5, 50, inRect.width - 10, 25), $"Intelligence : {helmet.GetStat(Stats.Intelligence)}");
                if (helmet.GetStat(Stats.PA) > 0) Widgets.Label(new Rect(5, 50, inRect.width - 10, 25), $"PA : {helmet.GetStat(Stats.PA)}");
            }
            

        } else if (item is ItemBase)
        {
            Widgets.Label(new Rect(5, 5, inRect.width - 10, 25), item.Name);
            Widgets.Label(new Rect(5, 30, inRect.width - 10, 25), $"Price : {item.Price}");
            Widgets.Label(new Rect(5, 50, inRect.width - 10, 25), $"StackLimit : {item.stackLimit}");
        }
        

        
    }
}
