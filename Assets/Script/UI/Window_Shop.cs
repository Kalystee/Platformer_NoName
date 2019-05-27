using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Window_Shop : Window
{
    public static Shop shop;

    public override bool DoXClose => false;
    public override bool Draggable => false;
    public override bool FollowMouse => false;

   // public override bool DoWindowBackground => false;
    public override Vector2 InitialSize => new Vector2(ScreenUtility.width,ScreenUtility.height);

    Inventory shopDraw;

    public Window_Shop()
    {
        shop = new Shop();
        shop.AddItemToShop(ItemDefOf.pistol);
        shop.AddItemToShop(ItemDefOf.helmet);
        shop.AddItemToShop(ItemDefOf.firstAidKit, 5);

        shop.AddItemToRachat(ItemDefOf.conso3);
        
        shopDraw = Window_Shop.shop.listOfItem;
    }
    public override void DoWindowContents(Rect inRect) {

        //----------------Fenetre Gauche----------------------------//
        Widgets.DrawBox(new Rect(ScreenUtility.width/40, ScreenUtility.height/40, ScreenUtility.width/2 - ScreenUtility.width / 20, ScreenUtility.height - ScreenUtility.height/11));

        //Fenetre Recherche
        string search = "";
        Widgets.InputField(new Rect(ScreenUtility.width / 40 + ScreenUtility.width/80, ScreenUtility.height / 25, ScreenUtility.width / 2 - (ScreenUtility.width / 20 + ScreenUtility.width / 40), ScreenUtility.height/ 20),ref search);
        //Fenetre Tag
        Widgets.DrawBox(new Rect(ScreenUtility.width / 40 + ScreenUtility.width / 30, ScreenUtility.height / 10, ScreenUtility.width / 2 - (ScreenUtility.width / 20 + ScreenUtility.width / 15), ScreenUtility.height / 20));
        //Fenetre Boutique
        Widgets.DrawBox(new Rect(ScreenUtility.width / 40 + ScreenUtility.width / 80, ScreenUtility.height / 6, ScreenUtility.width / 2 - (ScreenUtility.width / 20 + ScreenUtility.width / 40), (2*ScreenUtility.height) / 3));
        Vector2 s = new Vector2();

        //Bouton Achat
        if (Widgets.ButtonText(new Rect(ScreenUtility.width / 40 + ScreenUtility.width / 60, ScreenUtility.height / 5, ScreenUtility.width / 5, ScreenUtility.height / 20), "Achat"))
        {
            shopDraw = Window_Shop.shop.listOfItem;

        }
        //Bouton Rachat

        if (Widgets.ButtonText(new Rect(ScreenUtility.width / 17 + ScreenUtility.width / 5, ScreenUtility.height / 5, ScreenUtility.width / 5, ScreenUtility.height / 20), "Rachat"))
        {
            this.shopDraw = Window_Shop.shop.rachat;
        }

        Widgets.DrawShopInventory(new Rect(ScreenUtility.width / 40 + ScreenUtility.width / 80, ScreenUtility.height / 4, ScreenUtility.width / 2 - (ScreenUtility.width / 20 + ScreenUtility.width / 40), ScreenUtility.height / 2 + ScreenUtility.height / 13), this.shopDraw, ref s);


        //Argent Possédé
        Widgets.Label(new Rect(ScreenUtility.width / 15 + ScreenUtility.width / 3, ScreenUtility.height - ScreenUtility.height / 8, ScreenUtility.width / 10, ScreenUtility.height / 20), Find.Party.Money + " $");


        //----------------Fenetre Droite----------------------------//
        Widgets.DrawBox(new Rect(ScreenUtility.width / 2 , ScreenUtility.height / 40, ScreenUtility.width/2 - ScreenUtility.width/20, ScreenUtility.height - ScreenUtility.height / 11));
       
        //Fenetre Marchand
        Widgets.DrawBox(new Rect((ScreenUtility.width / 2) + ScreenUtility.width/80, ScreenUtility.height / 30, ScreenUtility.width / 2 - (ScreenUtility.width / 20 + ScreenUtility.width/40), ScreenUtility.height/3));
        //Fenetre Inventaire
        Widgets.DrawBox(new Rect((ScreenUtility.width / 2) + ScreenUtility.width / 80, ScreenUtility.height/3 + ScreenUtility.height / 20, ScreenUtility.width / 2 - (ScreenUtility.width / 20 + ScreenUtility.width / 40), ((2*ScreenUtility.height) / 3) - ScreenUtility.height/8 ));
        Vector2 scroll = new Vector2();
        Widgets.DrawInventory(new Rect((ScreenUtility.width / 2) + ScreenUtility.width / 80, ScreenUtility.height / 3 + ScreenUtility.height / 20, ScreenUtility.width / 2 - (ScreenUtility.width / 20 + ScreenUtility.width / 40), ((2 * ScreenUtility.height) / 3) - ScreenUtility.height / 8),Find.Party.CommualInventory, ref scroll);

    }

    
}
