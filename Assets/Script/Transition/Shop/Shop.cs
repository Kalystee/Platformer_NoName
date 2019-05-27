using System.Collections.Generic;
using UnityEngine;

public class Shop {

    public Inventory listOfItem { get; }
    public Inventory rachat { get; }

    public Shop()
    {
        this.listOfItem = new Inventory();
        this.rachat = new Inventory();
    }

    public Shop(params ItemBase[] items)
    {
        this.listOfItem = new Inventory();
        foreach(ItemBase i in items)
        {
            this.listOfItem.AddItems(new ItemStack(i));
        }
    }

    public void AddItemToShop(ItemBase item,int qty=1)
    {
        this.listOfItem.AddItems(new ItemStack(item, qty));
    }

    public void AddItemToRachat(ItemBase item, int qty=1)
    {
        this.rachat.AddItems(new ItemStack(item, qty));
    }

    public void RemoveItemToShop(ItemBase item,int qty=1)
    {
        this.listOfItem.RemoveItem(item,qty);
    }

    public void RemoveItemToRachat(ItemBase item, int qty = 1)
    {
        this.rachat.RemoveItem(item, qty);
    }

}
