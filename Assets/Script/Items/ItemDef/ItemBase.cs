using System.Runtime.Serialization;
using UnityEngine;

[DataContract]
public class ItemBase {

    [DataMember]
    public string Name { get; protected set; }

    [DataMember]
    public int stackLimit { get; protected set; }

    [DataMember]
    private string spriteName;
    public Sprite Sprite { get { return SpriteManager.GetSpriteOfItemNamed(spriteName); } }
    public int Price { get; }

    public ItemBase(string name, string spriteName, int price, int maxStack = 999)
    {
        this.Name = name;
        this.spriteName = spriteName;
        this.stackLimit = Mathf.Min(maxStack,999);
        this.Price = price;
    }

    public override bool Equals(object obj)
    {
        bool res = false;
        if(this == obj)
        {
            return true;
        } else if( obj is ItemBase)
        {
            ItemBase i = obj as ItemBase;
            res = this.Name.Equals(i.Name) && this.spriteName.Equals(i.spriteName) && this.stackLimit == i.stackLimit && this.Price == i.Price;
        }

        return res;
    }
}
