using UnityEngine;

public class Buff_Stats : Buff {

    public Stats AffectedStat { get; protected set; }
    public int Value { get; protected set; }

    public override string Name
    {
        get
        {
            if (IsBuff)
                return "Stat Bonus";
            else
                return "Stat Penalty";
        }
    }

    public override string Description
    {
        get
        {
            if (IsBuff)
                return $"Increase your {AffectedStat.ToString()} of {Value}";
            else
                return $"Reduce your {AffectedStat.ToString()} of {Value}";
        }
    }

    public override bool IsBuff
    {
        get
        {
            return Value >= 0;
        }
    }

    public Buff_Stats(int nbTours, Stats stat, int value) : base(nbTours)
    {
        this.AffectedStat = stat;
        this.Value = value;
    }

    public override Buff Copy()
    {
        return new Buff_Stats(time, AffectedStat, Value);
    }
}
