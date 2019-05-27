using System.Collections.Generic;
using UnityEngine;

public class ItemDefOf {

    //Liste de tout les Item
    public static Dictionary<string, ItemBase> itemDefList = new Dictionary<string, ItemBase>();
    
    public static ItemBase GetItemBaseNamed(string name)
    {
        if (itemDefList.ContainsKey(name))
        {
            return itemDefList[name];
        }
        else
        {
            Debug.LogWarning($"L'Item '{name}' n'existe pas !");
            return null;
        }
    }

    //Déclaration de tout les Items fixe
    public static ConsumableBase firstAidKit = new ConsumableBase("FirstAid Kit",18, "Tomb" , false, 20);

    public static WeaponBase pistol = new WeaponBase("Pistol",50, "pistol", AbilityDefOf.boom);
    public static WeaponBase riffle = new WeaponBase("Pistol",200, "riffle", AbilityDefOf.boom);

    public static HelmetBase helmet = new HelmetBase("Helmet",20, "headIcon", 1);
    public static ChestPlateBase chest = new ChestPlateBase("Chest",45, "bodyIcon", 1);
    public static GlovesBase gloves = new GlovesBase("Gloves",15, "handIcon", 1);
    public static BootsBase boot = new BootsBase("Boots",25, "footIcon", 1);

    public static ItemBase gadjet1 = new ConsumableBase("gadjet1",5, "accesoriesIcon",true,1);
    public static ItemBase gadjet2 = new ConsumableBase("gadjet2",8, "accesoriesIcon", true, 1);
    public static ItemBase gadjet3 = new ConsumableBase("gadjet3",10, "accesoriesIcon", true, 1);

   
    public static ConsumableBase conso1 = new ConsumableBase("conso1",4 ,"blue_potion",false,1);
    public static ConsumableBase conso2 = new ConsumableBase("conso2",6 ,"green_potion",false, 1);
    public static ConsumableBase conso3 = new ConsumableBase("conso3",9 ,"red_potion",false, 1);
}
