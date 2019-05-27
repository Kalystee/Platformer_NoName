using System.Collections.Generic;

public class AbilityDefOf  {

    //Liste contenant toutes les abilities
    public static Dictionary<string, AbilityBase> abilityDefList = new Dictionary<string, AbilityBase>();

    public static AbilityBase GetAbilityBaseNamed(string name)
    {
        if (abilityDefList.ContainsKey(name))
        {
            return abilityDefList[name];
        }
        else
        {
            return null;
        }
    }

    //Déclaration de toute les abilities
    public static AbilityBase basicAttack = new AbilityDamage("Basic_Attack", "Build_Icon", 1, 1, new EffectZone(EffectZoneType.Radius,0), new Int2(-5, -10));
    public static AbilityBase boom = new AbilityDamage("Boom", "Damage_Icon", 5, 2, new EffectZone(EffectZoneType.Radius,2) ,new Int2(-10, -15));
    public static AbilityBase heal = new AbilityDamage("Heal", "Heart_Icon", 3, 3, new EffectZone(EffectZoneType.Radius, 0), new Int2(10, 15));
    public static AbilityBase buffHP = new AbilityBuff("BuffHP", "Bullet_Quantity_Icon", 1, 4, new EffectZone(EffectZoneType.Radius, 0), new Buff_Stats(2, Stats.Vitality, 5));
    public static AbilityBase dot = new AbilityBuff("Dot", "Shield_Icon", 4, 2, new EffectZone(EffectZoneType.Radius, 0), new Buff_OverTime(2, new Int2(-5,-10)));
}
