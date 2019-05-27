using System.Runtime.Serialization;

[DataContract]
public abstract class ArmorBase : EquipmentBase
{
    [DataMember]
    public int Armor { get; protected set; }

    protected ArmorBase(string name, int cost, string spriteName, int armor) : base(name, spriteName, cost)
    {
        Armor = armor;
    }
}
