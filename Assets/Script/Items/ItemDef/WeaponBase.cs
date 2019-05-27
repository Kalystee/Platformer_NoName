using System.Runtime.Serialization;

[DataContract]
public class WeaponBase : EquipmentBase
{
    [DataMember]
    public AbilityBase Ability { get; protected set; }

    public WeaponBase(string name,int cost, string spriteName, AbilityBase ability) : base(name, spriteName,cost)
    {
        this.Ability = ability;
    }
}
