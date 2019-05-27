using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Party {
    
    #region Attribute
    public int Money { get; set; }
    public Inventory CommualInventory { get; }
    public List<Character> characterList;
    #endregion

    public Party()
    {
        this.Money = 0;
        this.CommualInventory = new Inventory();
        this.characterList = new List<Character>();
    }

    #region Gestion du groupe
    /// <summary>
    /// Méthode permettant d'ajouter un personnage au groupe 
    /// </summary>
    /// <param name="character">Personnage à ajouter</param>
    public void AddCharacterToParty(Character character)
    {
        this.characterList.Add(character);
    }

    /// <summary>
    /// Méthode permettant d'enlever un personnage du groupe
    /// </summary>
    /// <param name="character">Personnage à retirer du groupe</param>
    public void RemoveCharacterOfParty(Character character)
    {
        this.characterList.Remove(character);
    }
    #endregion

    #region Gestion de l'inventaire commun

    public void AddItemToCommunalInventory(ItemStack itemS)
    {
        this.CommualInventory.AddItems(itemS);
    }

    public void RemoveItemOfCommunalInventory(ItemBase item, int quantity)
    {
        this.CommualInventory.RemoveItem(item, quantity);
    }

    #endregion
}
