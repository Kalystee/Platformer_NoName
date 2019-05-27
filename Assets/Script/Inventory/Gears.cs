using System.Collections.Generic;

public class Gears
{
    public WeaponBase Weapon { get; set; }
    public HelmetBase Helmet { get; set; }
    public ChestPlateBase Chestplate { get; set; }
    public GlovesBase Gloves { get; set; }
    public BootsBase Boots { get; set; }
    public List<ConsumableBase> ConsumableList;
    public List<ItemBase> GadjetList;


    public Gears()
    {
        this.ConsumableList = new List<ConsumableBase>();
        this.GadjetList = new List<ItemBase>();
    }

    #region list
     public void AddConsumable(ConsumableBase c)
    {
        this.ConsumableList.Add(c);
    }

    public void RemoveConsumable(ConsumableBase c)
    {
        this.ConsumableList.Remove(c);
    }

    public void AddGadjet(ItemBase g)
    {
        this.GadjetList.Add(g);
    }

    public void RemoveGadget(ItemBase g)
    {
        this.GadjetList.Remove(g);
    }

    #endregion
    /// <summary>
    /// Equipe une arme (déséquipe si null)
    /// </summary>
    /// <param name="equipWeapon">Arme à équiper</param>
    /// <returns>Ancienne arme équipé</returns>
    public WeaponBase Equip(WeaponBase equipWeapon)
    {
        WeaponBase oldWeapon = Weapon;
        Weapon = equipWeapon;
        return oldWeapon;
    }

    /// <summary>
    /// Equipe un casque (déséquipe si null)
    /// </summary>
    /// <param name="equipHelmet">Casque à équiper</param>
    /// <returns>Ancien casque équipé</returns>
    public HelmetBase Equip(HelmetBase equipHelmet)
    {
        HelmetBase oldHelmet = Helmet;
        Helmet = equipHelmet;
        return oldHelmet;
    }
    /// <summary>
    /// Equipe un plastron (déséquipe si null)
    /// </summary>
    /// <param name="equipChestplate">Plastron à équiper</param>
    /// <returns>Ancien plastron équipé</returns>
    public ChestPlateBase Equip(ChestPlateBase equipChestplate)
    {
        ChestPlateBase oldHelmet = Chestplate;
        Chestplate = equipChestplate;
        return oldHelmet;
    }

    /// <summary>
    /// Equipe des gants (déséquipe si null)
    /// </summary>
    /// <param name="equipGloves">Gants à équiper</param>
    /// <returns>Anciens gants équipés</returns>
    public GlovesBase Equip(GlovesBase equipGloves)
    {
        GlovesBase oldHelmet = Gloves;
        Gloves = equipGloves;
        return oldHelmet;
    }

    /// <summary>
    /// Equipe des bottes (déséquipe si null)
    /// </summary>
    /// <param name="equipBoots">Bottes à équiper</param>
    /// <returns>Anciennes bottes équipés</returns>
    public BootsBase Equip(BootsBase equipBoots)
    {
        BootsBase oldHelmet = Boots;
        Boots = equipBoots;
        return oldHelmet;
    }

    /// <summary>
    /// Retourne la quantité bonus donnés par le Gear
    /// </summary>
    /// <param name="stat">Stat</param>
    /// <returns>Quantité donné par le Gear</returns>
    public int GetBonusStatsOf(Stats stat)
    {
        return  
            (Weapon != null ? Weapon.GetStat(stat) : 0) +
            (Helmet != null ? Helmet.GetStat(stat) : 0) +
            (Chestplate != null ? Chestplate.GetStat(stat) : 0) +
            (Gloves != null ? Gloves.GetStat(stat) : 0) +
            (Boots != null ? Boots.GetStat(stat) : 0);
    }
    
    public AbilityBase WeaponAttack { get { if(Weapon == null) { return AbilityDefOf.basicAttack; } else { return Weapon.Ability; } } }
}
