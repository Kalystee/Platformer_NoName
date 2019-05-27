using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BattleCharacter
{
    //Propriété : Position en largeur
    public Int2 pos { get; private set; }

    public TeamTag teamTag { get; private set; }

    public Character character { get; private set; }

    public List<Buff> listOfBuffs { get; private set; }
    private List<Loot> listOfLoots;

    public int remainingHealth { get; private set; }
    public int useablePA { get; private set; }

    protected Action<Int2> onMove;
    protected Action onDeath;
    protected Action onAbilityUse;

    public BattleCharacter(Character character, TeamTag tag, int x, int y)
    {
        this.remainingHealth = character.Vitality;
        this.character = character;
        this.teamTag = tag;
        this.pos = new Int2(x, y);
        this.useablePA = character.PA;
        this.listOfBuffs = new List<Buff>();
        this.listOfLoots = new List<Loot>();

        Find.Party.CommualInventory.AddItems(new ItemStack(ItemDefOf.firstAidKit));

        RegisterOnDeathAction(DropStuffWithLoots);
    }

    #region Callback
    public void RegisterOnAbilityUseAction(Action action)
    {
        this.onAbilityUse += action;
    }

    public void RegisterOnMoveAction(Action<Int2> action)
    {
        this.onMove += action;
    }

    public void RegisterOnDeathAction(Action action)
    {
        this.onDeath += action;
    }
    #endregion

    #region Modifcaract
    public int Vitality { get { return character.Vitality + listOfBuffs.Where(b => b is Buff_Stats && (b as Buff_Stats).AffectedStat == Stats.Vitality).Sum(b => (b as Buff_Stats).Value); } }
    public int Strength { get { return character.Strength + listOfBuffs.Where(b => b is Buff_Stats && (b as Buff_Stats).AffectedStat == Stats.Strength).Sum(b => (b as Buff_Stats).Value); } }
    public int Initiative { get { return character.Initiative + listOfBuffs.Where(b => b is Buff_Stats && (b as Buff_Stats).AffectedStat == Stats.Initiative).Sum(b => (b as Buff_Stats).Value); } }
    public int Intelligence { get { return character.Intelligence + listOfBuffs.Where(b => b is Buff_Stats && (b as Buff_Stats).AffectedStat == Stats.Intelligence).Sum(b => (b as Buff_Stats).Value); } }
    public int Precision { get { return character.Precision + listOfBuffs.Where(b => b is Buff_Stats && (b as Buff_Stats).AffectedStat == Stats.Precision).Sum(b => (b as Buff_Stats).Value); } }
    
    /// <summary>
    /// Remet les PA au maximum
    /// </summary>
    public void ResetPA()
    {
        useablePA = character.PA;
    }

    /// <summary>
    /// Ajoute la valeur 'modify' à la vie du BattleCharacter (Clampé)
    /// </summary>
    /// <param name="modify">Vie ajouté</param>
    public void ModifyHealth(int modify)
    {
        remainingHealth = Mathf.Clamp(remainingHealth + modify, 0, Vitality);
        if(remainingHealth == 0)
        {
            onDeath?.Invoke();
        }
    }

    /// <summary>
    /// Ajoute la valeur 'modify' au PA du BattleCharacter (Clampé)
    /// </summary>
    /// <param name="modify">PA ajouté</param>
    public void ModifyPA(int modify)
    {
        useablePA = Mathf.Max(useablePA + modify, 0);
    }

    /// <summary>
    /// Ajoutes loots potentiels au BattleCharacter
    /// </summary>
    /// <param name="loots"></param>
    public void AddLootToCharacter(params Loot[] loots)
    {
        listOfLoots.AddRange(loots);
    }

    private void DropStuffWithLoots()
    {
        if (character.gears.Weapon != null)
        {
            Level.singleton.LootOnGround(pos, new ItemStack(character.gears.Weapon));
            character.gears.Weapon = null;
        }
        foreach(Loot l in listOfLoots.ToArray())
        {
            if(l.RollLoot())
            {
                Level.singleton.LootOnGround(pos, l.LootItemStack);
            }
        }
       /* --- WARNING----
        foreach (ItemStack item in character.PopInventory())
        {
            Level.singleton.LootOnGround(pos, item);
        }*/
    }
    #endregion

    #region Move
    /// <summary>
    /// Déplace le personnage en coordonnée fourni
    /// </summary>
    /// <param name="pos">Position</param>
    public void MoveToPosition(Int2 pos)
    {
        Int2 oldPos = this.pos;
        this.pos = pos;
        onMove?.Invoke(oldPos);
        onMove?.Invoke(this.pos);
    }

    /// <summary>
    /// Déplace le personnage en coordonnée fourni
    /// </summary>
    /// <param name="x">Position en largeur</param>
    /// <param name="y">Position en hauter</param>
    public void MoveToPosition(int x, int y)
    {
        MoveToPosition(new Int2(x, y));
    }
    #endregion

    #region Ability
    /// <summary>
    /// Utilise une Ability sur un points d'impact
    /// </summary>
    /// <param name="ability">Ability à utiliser</param>
    /// <param name="positionOfImpact">Point d'impact</param>
    public void UseAbility(AbilityBase ability, Int2 positionOfImpact)
    {
        if (CanUseAbility(ability) && WorldUtils.HasLineOfSightTo(pos, positionOfImpact))
        {
            ModifyPA(-ability.Cost);
            Level.singleton.AffectInZone(positionOfImpact, ability);
            onAbilityUse?.Invoke();
        }
        else
        {
            Debug.Log("Impossible d'utiliser cette compétence");
        }
    }

    /// <summary>
    /// Peut-on utiliser une Ability ?
    /// </summary>
    /// <param name="ability">Ability à utiliser</param>
    /// <returns>Les prérequis pour l'utilisation sont-ils valideés ?</returns>
    public bool CanUseAbility(AbilityBase ability)
    {
        return this.useablePA >= ability.Cost;
    }
    #endregion

    #region Buff
    /// <summary>
    /// Méthode permettant d'ajouter un buff(ou debuff) au BattleCharacter
    /// </summary>
    /// <param name="buff"> Buff à ajouter</param>
    public void AddBuff(Buff buff)
    {
        this.listOfBuffs.Add(buff);
        if(buff is Buff_Stats)
        {
            Buff_Stats buff_stat = buff as Buff_Stats;
            if(buff_stat.AffectedStat == Stats.Vitality)
            {
                ModifyHealth(buff_stat.Value);
            }
        }
    }

    /// <summary>
    /// Méthode permettant de retirer un buff(ou debuff) au BattleCharacter
    /// </summary>
    /// <param name="buff"> Buff à retirer</param>
    public void RemoveBuff(Buff buff)
    {
        this.listOfBuffs.Remove(buff);
        if (buff is Buff_Stats)
        {
            Buff_Stats buff_stat = buff as Buff_Stats;
            if (buff_stat.AffectedStat == Stats.Vitality)
            {
                //(On clamp la valeur des points de vie si elle est trop haute)
                ModifyHealth(0);
            }
        }
    }

    public void UpdateBuffs()
    {
        foreach(Buff b in listOfBuffs.ToArray())
        {
            b.ModifyTime(-1);
            if(b is Buff_OverTime)
            {
                Buff_OverTime ot = b as Buff_OverTime;
                ModifyHealth(ot.GetDamageApplied());
            }
            if(b.GetTime() <= 0)
            {
                RemoveBuff(b);
            }
        }
    }
    #endregion

    #region endTurn
    /// <summary>
    /// Permet de savoir si le tour est obligatoirement terminé
    /// </summary>
    /// <returns>Tout obligatoire terminé ?</returns>
    public bool IsTurnFinish()
    {
        if (this.character.PA <= 0)
        {
            return true;
        }
        return false;
    }
    #endregion

    #region Override
    public override int GetHashCode()
    {
        return base.GetHashCode();
    }

    public override string ToString()
    {
        return base.ToString();
    }

    public override bool Equals(object obj)
    {
        if (obj is BattleCharacter)
        {
            BattleCharacter bc = obj as BattleCharacter;
            return
                bc.pos.Equals(pos) &&
                bc.character.Equals(character) &&
                bc.teamTag.Equals(teamTag);
        }
        return false;
    }

    #endregion
}
