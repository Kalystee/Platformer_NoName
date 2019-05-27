
public class AbilityBuff : AbilityBase
{
    public Buff Buff { get; }

    public AbilityBuff(string nom, string spriteName, int range, int cost, EffectZone effectZone, Buff buff) : base(nom,spriteName,range,cost, effectZone)
    {
        this.Buff = buff;
    }
}
