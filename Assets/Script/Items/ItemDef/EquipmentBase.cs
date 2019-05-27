using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;

[DataContract]
public abstract class EquipmentBase : ItemBase {

    [DataMember]
    private Dictionary<Stats, int> additionnalStats;

    protected EquipmentBase(string name, string spriteName, int cost) : base(name, spriteName, cost, 1)
    {
        additionnalStats = new Dictionary<Stats, int>();
    }

    protected EquipmentBase AddStat(Stats stat, int value)
    {
        if(additionnalStats.ContainsKey(stat))
        {
            Debug.LogWarning("Tentative d'ajout d'une stat déja existante sur un objet");
        }
        else
        {
            additionnalStats.Add(stat, value);
        }
        return this;
    }

    public int GetStat(Stats stat)
    {
        if (additionnalStats.ContainsKey(stat))
        {
            return additionnalStats[stat];
        }
        else
        {
            return 0;
        }
    }
}
