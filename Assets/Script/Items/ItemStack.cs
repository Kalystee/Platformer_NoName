using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemStack
{
    public ItemBase Item { get; private set; }

    public int Quantity { get; private set; }

    public ItemStack(ItemBase item, int quantity = 1)
    {
        this.Item = item;
        this.Quantity = Mathf.Min(quantity, item.stackLimit);
    }

    public bool IsValid
    {
        get
        {
            return Quantity > 0;
        }
    }

    public bool HasFreeSpace
    {
        get
        {
            return RemainingSpace > 0;
        }
    }

    public int RemainingSpace
    {
        get
        {
            return Item.stackLimit - Quantity;
        }
    }

    public int IncreaseQuantity(int quantity)
    {
        if(RemainingSpace >= quantity)
        {
            Quantity += quantity;
            return quantity;
        }
        else
        {
            int modification = RemainingSpace;
            Quantity = Item.stackLimit;
            return modification;
        }
    }

    public int ReduceQuantity(int quantity)
    {
        if(Quantity >= quantity)
        {
            Quantity -= quantity;
            return quantity;
        }
        else
        {
            int modification = RemainingSpace;
            Quantity = 0;
            return modification;
        }
    }
}
