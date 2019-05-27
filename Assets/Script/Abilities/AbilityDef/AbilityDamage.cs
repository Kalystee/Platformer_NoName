using UnityEngine;

public class AbilityDamage : AbilityBase {

    //Propriété : Dommage ou heal
    public bool IsDamage { get; protected set; }

    //Propriété : Quantité (dommage ou heal)
    public Int2 DamageAmount { get; protected set; }

    public AbilityDamage(string nom, string spriteName, int range, int cost, EffectZone zone, Int2 damageAmount ) : base(nom, spriteName, range,cost,zone)
    {
        this.DamageAmount = damageAmount;
        this.IsDamage = damageAmount.x < 0;
        AbilityDefOf.abilityDefList.Add(nom, this);
    }

    /// <summary>
    /// Retourne les dommages appliqué : (Négatif si dégats, Positifs si heal)
    /// </summary>
    /// <returns>Modification en HP basé sur le type d'attaque</returns>
    public int GetDamageApplied()
    {
        return Random.Range(this.DamageAmount.x, this.DamageAmount.y);
    }
}
