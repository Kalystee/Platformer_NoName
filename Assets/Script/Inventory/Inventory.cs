using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Inventory {

    public List<ItemStack> items { get; private set; }

    public override bool Equals(object obj)
    {
        if(obj is Inventory)
        {
            Inventory i = obj as Inventory;
            return i.items == items;
        }
        return false;
    }

    //Création d'un inventaire vide
    public Inventory()
    {
        this.items = new List<ItemStack>();
    }

    public int Size
    {
        get
        {
            return items.Count;
        }
    }

    public ItemStack this[int index]
    {
        get
        {
            return items[index];
        }
    }
    
    //Pour obtenir la liste des fenêtres (en lecture seule)
    public IList<ItemStack> Windows
    {
        get
        {
            return this.items.AsReadOnly();
        }
    }

    /// <summary>
    /// Permet de savoir si au moins 1 'item' est dans l'inventaire
    /// </summary>
    /// <param name="item">Item à comparer</param>
    /// <returns>Retourne true si l'inventaire possède au moins un dit 'item'</returns>
    public bool HasItem(ItemBase item)
    {
        if(item != null)
        {
            return items.Any(i => i.Item == item);
        }
        return false;
    }

    /// <summary>
    /// Regarde si il y a 'quantity' de 'item' dans l'inventaire
    /// </summary>
    /// <param name="item">Item à vérifier</param>
    /// <param name="quantity">Quantité à vérifier</param>
    /// <returns>Retourne true si il y a 'quantity' ou plus de 'item'</returns>
    public bool HasItemInQuantity(ItemBase item, int quantity)
    {
        if (item != null)
        {
            return items.Where(i => i.Item == item).Sum(i => i.Quantity) >= quantity;
        }
        return false;
    }

    /// <summary>
    /// Regarde si des ItemStack de 'item' ne sont pas plein
    /// </summary>
    /// <param name="item">Item à vérifier</param>
    /// <returns>Retourne true si un ItemStack de 'item' n'est pas plein</returns>
    public bool HasItemStackFree(ItemBase item)
    {
        return items.Any(i => i.Item == item && i.HasFreeSpace);
    }

    /// <summary>
    /// Ajoute un Item à l'inventaire
    /// </summary>
    /// <param name="newItems">ItemStacks à ajouter (scinde automatiquement avec les groupes existants</param>
    public void AddItems(params ItemStack[] newItems)
    {
        foreach (ItemStack item in newItems)
        {
            Debug.Log("INVENTORY : Item :" + item.Item.Name + " - Qty : " + item.Quantity);
            while (item.Quantity > 0)
            {
                if (HasItemStackFree(item.Item))
                {
                    ItemStack itemStack = items.First(i => i.Item == item.Item && i.HasFreeSpace);
                    int modif = itemStack.IncreaseQuantity(item.Quantity);
                    item.ReduceQuantity(modif);
                }
                else
                {
                    items.Add(item);
                    break;
                }
            }
        }
    }
    
    /// <summary>
    /// Retire des objets en quantité de l'inventaire
    /// </summary>
    /// <param name="item">Item à retirer</param>
    /// <param name="quantity">Quantité à retirer</param>
    public void RemoveItem(ItemBase item, int quantity)
    {
        while(quantity > 0)
        { 
            if (HasItemInQuantity(item, quantity))
            {
                ItemStack itemStack = items.Last(i => i.Item == item);
                int modif = itemStack.ReduceQuantity(quantity);
                quantity -= modif;
            }
            else
            {
                break;
            }
        }
    }   

    public override int GetHashCode()
    {
        return base.GetHashCode();
    }

    public override string ToString()
    {
        return base.ToString();
    }
}
