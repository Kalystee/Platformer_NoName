public class LootBag
{
    //Propriété : Position
    public Int2 pos { get; private set; }
    //Propriété : Inventaire du LootBag
    public Inventory inventory { get; private set; }

    /// <summary>
    /// Créer un LootBag avec les objets listés
    /// </summary>
    /// <param name="_pos">Position</param>
    /// <param name="items">[Params] Liste des items</param>
    public LootBag(Int2 _pos, params ItemStack[] items)
    {
        pos = _pos;
        inventory = new Inventory();
        inventory.AddItems(items);
    }
}
