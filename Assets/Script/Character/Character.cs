using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;

[DataContract]
public class Character
{
    [DataMember]
    public string Name { get; protected set; }

    [DataMember]
    private string spriteName;
    public Sprite Sprite { get { return SpriteManager.GetSpriteOfItemNamed(spriteName); } }

    /*[DataMember]
    public Inventory inventory { get; protected set; }*/
    [DataMember]
    public Gears gears { get; protected set; }
    [DataMember]
    protected List<AbilityBase> learntAbilities;

    public List<AbilityBase> useableAbilities
    {
        get
        {
            List<AbilityBase> list = new List<AbilityBase>
            {
                gears.WeaponAttack
            };
            list.AddRange(learntAbilities); return list;
        }
    }

    //Caractéristiques
    [DataMember]
    private Dictionary<Stats, int> baseStatsValue;

    public int Vitality { get { return gears.GetBonusStatsOf(Stats.Vitality) + GetBaseStats(Stats.Vitality); } }
    public int Strength { get { return gears.GetBonusStatsOf(Stats.Strength) + GetBaseStats(Stats.Strength); } }
    public int Precision { get { return gears.GetBonusStatsOf(Stats.Precision) + GetBaseStats(Stats.Precision); } }
    public int Intelligence { get { return gears.GetBonusStatsOf(Stats.Intelligence) + GetBaseStats(Stats.Intelligence); } }
    public int Initiative { get { return gears.GetBonusStatsOf(Stats.Initiative) + GetBaseStats(Stats.Initiative); } }
    public int PA { get { return gears.GetBonusStatsOf(Stats.PA) + GetBaseStats(Stats.PA); } }

    public Character(string name, int vitality, int strength, int precision, int intelligence, int initiative, int pa)
    {
        this.Name = name;

        this.gears = new Gears();
       // this.inventory = new Inventory();
        this.learntAbilities = new List<AbilityBase>();

        //------------------------------------------------------------------TEST-----------------------------------------------------//
        this.learntAbilities.Add(AbilityDefOf.boom);
        this.learntAbilities.Add(AbilityDefOf.heal);
        this.learntAbilities.Add(AbilityDefOf.buffHP);
        this.learntAbilities.Add(AbilityDefOf.dot);

        //------------------------------------------------------------------TEST-----------------------------------------------------//

        this.baseStatsValue = new Dictionary<Stats, int>
        {
            { Stats.Initiative, initiative },
            { Stats.Intelligence, intelligence },
            { Stats.Precision, precision },
            { Stats.Strength, strength },
            { Stats.Vitality, vitality },
            { Stats.PA, pa }
        };
    }

    public Character(string name, int vitality, int strength, int precision, int intelligence, int initiative, int pa, string spriteName)
    {
        this.Name = name;
        this.spriteName = spriteName;

        this.gears = new Gears();
        //this.inventory = new Inventory();
        this.learntAbilities = new List<AbilityBase>();

        //------------------------------------------------------------------TEST-----------------------------------------------------//
        this.learntAbilities.Add(AbilityDefOf.boom);
        this.learntAbilities.Add(AbilityDefOf.heal);
        this.learntAbilities.Add(AbilityDefOf.buffHP);
        this.learntAbilities.Add(AbilityDefOf.dot);

        //------------------------------------------------------------------TEST-----------------------------------------------------//

        this.baseStatsValue = new Dictionary<Stats, int>
        {
            { Stats.Initiative, initiative },
            { Stats.Intelligence, intelligence },
            { Stats.Precision, precision },
            { Stats.Strength, strength },
            { Stats.Vitality, vitality },
            { Stats.PA, pa }
        };
    }

    public override bool Equals(object obj)
    {
        if (obj is Character)
        {
            Character c = obj as Character;
            return
                c.Name.Equals(Name) &&
                c.gears.Equals(gears) &&
                c.learntAbilities.Equals(learntAbilities) &&
              //  c.inventory.Equals(inventory) &&
                c.baseStatsValue.Equals(baseStatsValue);
        }
        return false;
    }

    /// <summary>
    /// Récupère une des stats de base du personnage
    /// </summary>
    /// <param name="stat">Stat à récuperer</param>
    /// <returns>Valeur de la stat</returns>
    public int GetBaseStats(Stats stat)
    {
        return baseStatsValue[stat];
    }

    /// <summary>
    /// Assigne une des stats de base du personnage
    /// </summary>
    /// <param name="stat">Stat à récuperer</param>
    /// <param name="value">Valeur à assigner</param>
    public void SetBaseStats(Stats stat, int value)
    {
        baseStatsValue[stat] = value;
    }

   /* public ItemStack[] PopInventory()
    {
        ItemStack[] stacks = inventory.items.ToArray();
        inventory = new Inventory();
        return stacks;
    }*/

    /// <summary>
    /// Apprend une nouvelle compétence
    /// </summary>
    /// <param name="abilitiesToLearn">Compétence à apprendre</param>
    public void LearnAbility(params AbilityBase[] abilitiesToLearn)
    {
        foreach (AbilityBase ab in abilitiesToLearn)
        {
            learntAbilities.Add(ab);
        }
    }

    /// <summary>
    /// Oublie une nouvelle compétence connue
    /// </summary>
    /// <param name="abilitiesToForgot">Compétence à oublier</param>
    public void ForgotAbility(params AbilityBase[] abilitiesToForgot)
    {
        foreach(AbilityBase ab in abilitiesToForgot)
        {
            learntAbilities.Remove(ab);
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
