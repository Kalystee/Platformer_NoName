using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Window_Armory : Window
{
    public override bool DoXClose => false;
    public override bool Draggable => false;
    public override bool FollowMouse => false;

    private Character characterSelected;

    public override Vector2 InitialSize => new Vector2(ScreenUtility.width, ScreenUtility.height);

    public Window_Armory()
    {
        this.characterSelected = Find.Party.characterList[0];
    }

    public override void DoWindowContents(Rect inRect)
    {
        //----------------Fenetre Gauche----------------------------//
        //Equipe
        Widgets.DrawBox(new Rect(ScreenUtility.width / 40, ScreenUtility.height / 40, ScreenUtility.width / 2 - ScreenUtility.width / 20, ScreenUtility.height/ 8));

        
        for (int i = 0; i < Find.Party.characterList.Count; i++)
        {
            if(Widgets.ButtonImage(new Rect(ScreenUtility.width / 10 + i * ScreenUtility.width / 20 + i * ScreenUtility.width / 40, ScreenUtility.height / 25, ScreenUtility.width / 20, ScreenUtility.height / 12 + ScreenUtility.height / 100), Find.Party.characterList[i].Sprite))
            {
                this.characterSelected = Find.Party.characterList[i];
            }
        }
        //Fenetre Personnage
        Widgets.DrawBox(new Rect(ScreenUtility.width / 40, ScreenUtility.height / 20 + ScreenUtility.height / 8, ScreenUtility.width / 2 - ScreenUtility.width / 20, ScreenUtility.height/8));
        //Sprite Personnage
        Widgets.DrawSprite(new Rect(ScreenUtility.width / 20, ScreenUtility.height / 15 + ScreenUtility.height / 7 + ScreenUtility.height/60, ScreenUtility.width / 20, ScreenUtility.height /12 + ScreenUtility.height/100),this.characterSelected.Sprite);
        //Nom personnage
        Widgets.Label(new Rect(ScreenUtility.width / 15 + ScreenUtility.width / 20, ScreenUtility.height / 20 + ScreenUtility.height / 8 + ScreenUtility.height / 60, ScreenUtility.width / 2 - ScreenUtility.width / 20, ScreenUtility.height / 20),this.characterSelected.Name);
        //Inventaire
        Widgets.DrawBox(new Rect(ScreenUtility.width / 40, ScreenUtility.height / 15 + ScreenUtility.height/4, ScreenUtility.width / 2 - ScreenUtility.width / 20, ScreenUtility.height - ( ScreenUtility.height / 3 + ScreenUtility.height/20) ));

        Vector2 scroll = new Vector2();
        Widgets.DrawInventory(new Rect(ScreenUtility.width / 40, ScreenUtility.height / 15 + ScreenUtility.height / 4, ScreenUtility.width / 2 - ScreenUtility.width / 20, ScreenUtility.height - (ScreenUtility.height / 3 + ScreenUtility.height / 20)), Find.Party.CommualInventory,ref scroll);
        //----------------Fenetre Droite----------------------------//
        Widgets.DrawBox(new Rect(ScreenUtility.width / 2, ScreenUtility.height / 40, ScreenUtility.width / 2 - ScreenUtility.width / 20, ScreenUtility.height - ScreenUtility.height / 11));
        //Equipement
            //Head
        Widgets.DrawSprite(new Rect(ScreenUtility.width - (ScreenUtility.width / 3 + ScreenUtility.width / 30), ScreenUtility.height / 10, ScreenUtility.width / 20, ScreenUtility.height / 15), this.characterSelected.gears.Helmet.Sprite);
            //Chest
        Widgets.DrawSprite(new Rect(ScreenUtility.width - (ScreenUtility.width / 3 + ScreenUtility.width / 30), ScreenUtility.height / 5 + ScreenUtility.height / 7, ScreenUtility.width /20, ScreenUtility.height / 15), this.characterSelected.gears.Chestplate.Sprite);
            //Gloves
        Widgets.DrawSprite(new Rect(ScreenUtility.width - (ScreenUtility.width / 3 + ScreenUtility.width / 8), ScreenUtility.height / 5 + +ScreenUtility.height /6 , ScreenUtility.width / 20, ScreenUtility.height / 15), this.characterSelected.gears.Gloves.Sprite);
            //Boots
        Widgets.DrawSprite(new Rect(ScreenUtility.width - (ScreenUtility.width / 3 + ScreenUtility.width / 30), (2*ScreenUtility.height / 3) - ScreenUtility.height/40, ScreenUtility.width / 20, ScreenUtility.height / 15), this.characterSelected.gears.Boots.Sprite);

        //Weapon
        Widgets.DrawSprite(new Rect(ScreenUtility.width - (ScreenUtility.width /6 + ScreenUtility.width / 20), ScreenUtility.height / 3, ScreenUtility.width / 8, ScreenUtility.height / 6),this.characterSelected.gears.Weapon.Sprite);
        // Fenetre Gadget
        Widgets.DrawBox(new Rect(ScreenUtility.width / 2 + ScreenUtility.width / 17, ScreenUtility.height - ScreenUtility.height/4, ScreenUtility.width / 3, ScreenUtility.height/16));
        for (int j = 0; j < this.characterSelected.gears.ConsumableList.Count; j++)
        {
            Widgets.DrawSprite(new Rect(ScreenUtility.width / 2 + ScreenUtility.width / 17 + (j* ScreenUtility.width / 15) + ScreenUtility.width/30, ScreenUtility.height - ScreenUtility.height / 6 - ScreenUtility.height/15, ScreenUtility.width / 20, ScreenUtility.height / 20),this.characterSelected.gears.ConsumableList[j].Sprite);
        }
        //Fenetre Consomable
        Widgets.DrawBox(new Rect(ScreenUtility.width / 2 + ScreenUtility.width / 17, ScreenUtility.height - ScreenUtility.height /6, ScreenUtility.width / 3, ScreenUtility.height /16));
        for(int k = 0; k < this.characterSelected.gears.GadjetList.Count; k++)
        {
            Widgets.DrawSprite(new Rect(ScreenUtility.width / 2 + ScreenUtility.width / 17 + (k * ScreenUtility.width / 15) + ScreenUtility.width / 30, ScreenUtility.height - ScreenUtility.height / 6, ScreenUtility.width / 20, ScreenUtility.height / 20),this.characterSelected.gears.GadjetList[k].Sprite);
        }
    }
}
