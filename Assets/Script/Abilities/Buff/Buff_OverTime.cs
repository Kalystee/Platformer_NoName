using UnityEngine;

public class Buff_OverTime : Buff
{
    public Int2 Value { get; }
    public override string Name
    {
        get
        {
            if (IsBuff)
                return "Regen";
            else
                return "D.O.T.";
        }
    }

    public override string Description
    {
        get
        {
            if (IsBuff)
                return $"Heal you {Value.Min}-{Value.Max} HP each turn";
            else
                return $"Deal {Value.Min}-{Value.Max} damage each turn";
        }
    }

    public override bool IsBuff
    {
        get
        {
            return Value.Min >= 0;
        }
    }

    public Buff_OverTime(int nbTours, Int2 value): base(nbTours)
    {
        this.Value = value;
    }

    public override Buff Copy()
    {
        return new Buff_OverTime(time, Value);
    }

    /// <summary>
    /// Retourne les dommages appliqué : (Négatif si dégats, Positifs si heal)
    /// </summary>
    /// <returns>Modification en HP basé sur le type d'attaque</returns>
    public int GetDamageApplied()
    {
        return Random.Range(this.Value.Min, this.Value.Max);
    }
}
