using System.Runtime.Serialization;

[DataContract]
public class ConsumableBase : ItemBase{

    [DataMember]
    public bool IsDamage { get; protected set; }

    [DataMember]
    public int Ammount { get; protected set; }

    public ConsumableBase (string name, int cost, string spriteName, bool damage, int ammount, int maxStack = 999) : base(name, spriteName, cost, maxStack)
    {
        this.IsDamage = damage;
        this.Ammount = ammount;
    }
}
